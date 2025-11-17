using UnityEngine; 

public class MyClickableObject : Clickable
{
    public override void HandleClick()
    {
        // Дополнительная логика перед обработкой
        Debug.Log("Объект был кликнут!");
        
        // Вызов базовой реализации
        base.HandleClick();
    }
}