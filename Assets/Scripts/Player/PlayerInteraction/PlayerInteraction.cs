using UnityEngine;
using TMPro; 

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Camera mainCam; 
    [SerializeField] float interactionDistance; 

    [SerializeField] GameObject interactionUI; 
    [SerializeField] TMP_Text interactionText; 

    [SerializeField] KeyCode interactionKey = KeyCode.E; 

    // Update is called once per frame
    void Update()
    {
        InteractionRay(); 
    }
    void InteractionRay() {
        Ray ray = mainCam.ViewportPointToRay(Vector3.one / 2f); 
        RaycastHit hit; 

        bool hitSomething = false; 
        if(Physics.Raycast(ray, out hit, interactionDistance)) {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>(); 

            if(interactable != null && hit.collider.gameObject.activeSelf) {
                hitSomething = true; 
                interactionText.text = interactable.GetDescription(); 

                if(Input.GetKeyDown(interactionKey)) {
                    interactable.Interact(); 
                }
 
            }
        }
        interactionUI.SetActive(hitSomething);
    }
}
