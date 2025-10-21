using UnityEngine;
using UnityEngine.Events;

public class Collision : MonoBehaviour
{
    [SerializeField] private UnityEvent _collisionEnterEvent; //список действий для входа в коллизию
    [SerializeField] private UnityEvent _collisionExitEvent; //список действий для выхода из коллизии

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collisionEnterEvent.Invoke();
        }
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collisionExitEvent.Invoke();
        }
    }
}
