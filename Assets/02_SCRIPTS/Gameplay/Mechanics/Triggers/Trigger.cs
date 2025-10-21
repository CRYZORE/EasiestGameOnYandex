using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _triggerEnterEvent; //список действий для входа в триггер
    [SerializeField] private UnityEvent _triggerExitEvent; //список действий для выхода из триггера

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _triggerEnterEvent.Invoke(); //проигрывание всех действий входа
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _triggerExitEvent.Invoke(); //проигрывание всех действий выхода
        }
    }
}
