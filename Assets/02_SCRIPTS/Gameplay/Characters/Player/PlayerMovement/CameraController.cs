using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player, orient, playerObj, target;
    public float sensitivity = 100f;
    public float zoomSpeed = 2f;
    public float minZoomDistance = 1f;
    public float maxZoomDistance = 10f;
    public float smoothTime = 0.1f;
    public float characterRotSpeed = 10f; 

    [Header("First Person Settings")]
    public bool showCursorInFirstPerson = false;

    private float mouseX;
    private float mouseY;
    private float currentZoom;
    private bool isFirstPerson;
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;
    private bool isRightMousePressed = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        if (target != null)
        {
            currentZoom = Vector3.Distance(transform.position, target.position);
        }
        
        // Изначально определяем режим камеры
        isFirstPerson = currentZoom <= minZoomDistance * 1.1f;
    }

    private void Update()
    {
        if (target == null) return;

        HandleInput();
        HandleZoom();
        HandleRotation();
        UpdateCameraMode();
        UpdateCursorState();
    }

    private void HandleInput()
    {
        // Отслеживаем состояние правой кнопки мыши
        isRightMousePressed = Input.GetMouseButton(1);

        if (isFirstPerson)
        {
            // В режиме 1 лица - вращение камеры всегда
            mouseX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            mouseY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
            mouseY = Mathf.Clamp(mouseY, -90f, 90f);
        }
        else
        {
            // В режиме 3 лица - вращение камеры только при зажатой правой кнопке мыши
            if (isRightMousePressed)
            {
                mouseX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
                mouseY -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
                mouseY = Mathf.Clamp(mouseY, -90f, 90f);
            }
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (isFirstPerson)
        {
            // В режиме 1 лица - можно ТОЛЬКО отдаляться (положительный scroll)
            if (scroll < 0)
            {
                currentZoom -= scroll * zoomSpeed;
                currentZoom = Mathf.Clamp(currentZoom, minZoomDistance, maxZoomDistance);
                
                // Если начали отдаляться - переключаемся в режим 3 лица
                if (currentZoom > minZoomDistance * 1.1f)
                {
                    isFirstPerson = false;
                }
            }
            // Отрицательный scroll (приближение) в режиме 1 лица игнорируется
        }
        else
        {
            // В режиме 3 лица - обычный зум в обе стороны
            currentZoom -= scroll * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoomDistance, maxZoomDistance);
            
            // Автоматическое переключение в режим 1 лица при сильном приближении
            if (currentZoom <= minZoomDistance * 1.1f)
            {
                isFirstPerson = true;
                currentZoom = minZoomDistance; // Фиксируем на минимальном расстоянии
            }
        }
    }

    private void UpdateCameraMode()
    {
        if (isFirstPerson)
        {
            // Режим от первого лица
            transform.position = target.position;
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX, 0), ref rotationSmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
        }
        else
        {
            // Режим от третьего лица
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(mouseY, mouseX, 0), ref rotationSmoothVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);

            // Позиция камеры с учетом currentZoom
            Vector3 desiredPosition = target.position - transform.forward * currentZoom;
            
            // Проверка на коллизии (опционально)
            RaycastHit hit;
            if (Physics.Linecast(target.position, desiredPosition, out hit))
            {
                desiredPosition = hit.point - transform.forward * 0.1f;
            }
            
            transform.position = desiredPosition;
        }
    }

    private void UpdateCursorState()
    {
        if (isFirstPerson)
        {
            // В режиме 1 лица
            if (showCursorInFirstPerson)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            // В режиме 3 лица
            if (isRightMousePressed)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    public void HandleRotation() {
        if(isFirstPerson) {
            Quaternion targetRot = transform.rotation; 
            Quaternion sourceRot = orient.rotation; 

            float targetYAngle = targetRot.eulerAngles.y; 
            orient.rotation = Quaternion.Slerp(orient.rotation, Quaternion.Euler(sourceRot.eulerAngles.x, targetYAngle, sourceRot.eulerAngles.z), characterRotSpeed*Time.deltaTime); 
            playerObj.rotation = Quaternion.Slerp(playerObj.rotation, Quaternion.Euler(sourceRot.eulerAngles.x, targetYAngle, sourceRot.eulerAngles.z), characterRotSpeed*Time.deltaTime); 
        }
        else {
            Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z); 
            orient.forward = viewDir.normalized;

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical"); 
            Vector3 inputDir = orient.forward * verticalInput + orient.right * horizontalInput; 

            if(inputDir != Vector3.zero) playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * characterRotSpeed);  
        }
    }

    // Метод для принудительного переключения режима (можно использовать из других скриптов)
    public void SetFirstPerson(bool firstPerson)
    {
        isFirstPerson = firstPerson;
        if (!firstPerson && currentZoom <= minZoomDistance)
        {
            currentZoom = minZoomDistance * 1.5f; // Устанавливаем небольшое отдаление
        }
    }

    // Метод для получения текущего режима
    public bool IsFirstPerson()
    {
        return isFirstPerson;
    }
}