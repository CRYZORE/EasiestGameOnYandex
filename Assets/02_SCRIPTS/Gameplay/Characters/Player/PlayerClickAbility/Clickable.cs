using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    [Header("Настройки кликов")]
    public int clicksToActivate = 1; // Через сколько кликов произойдет событие
    public bool resetAfterActivation = false; // Сбросить счетчик после активации

    [Header("События")]
    public UnityEvent OnClickEvent; // Событие при обычном клике
    public UnityEvent OnActivateEvent; // Событие при достижении нужного количества кликов

    private int currentClickCount;

    // Обработчик клика (может быть вызван извне)
    public virtual void HandleClick()
    {
        // Увеличиваем счетчик кликов
        currentClickCount++;
        
        // Вызываем событие клика
        OnClickEvent?.Invoke();

        // Проверяем условие активации
        if (currentClickCount >= clicksToActivate)
        {
            // Вызываем основное событие
            OnActivateEvent?.Invoke();

            // Сброс счетчика если нужно
            if (resetAfterActivation)
            {
                currentClickCount = 0;
            }
        }
    }

    // Метод для сброса счетчика
    public void ResetClicks()
    {
        currentClickCount = 0;
    }

    // Автоматическая обработка кликов мышью
    private void OnMouseDown()
    {
        HandleClick();
    }
}