//using System;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
public struct Ending //структура концовки 
{
    public string title;
    public string description;
    public Sprite icon;
}

public class EndingsBase : MonoBehaviour
{
    [SerializeField] private EndingSaves _endingSaves; ///удалить посде добавления загрузки через яндекс
    [SerializeField] private List<Ending> _endings = new(); //массив объектов концовок
    //[SerializeField] private string _iconsFolder = "Sprites"; //путь до папки с иконками концовок (/Assets/Resources/Sprites)
    [SerializeField] private List<Sprite> _endingSprites = new();
    private List<string> _endingTitles = new List<string>
    {
        "END1",
        "END2"
    };
    private List<string> _endingDescriptions = new List<string>
    {
        "END description 1",
        "END description 2"
    };

    private void Start()
    {
        _endings.Clear();
        //Sprite[] loaded = Resources.LoadAll<Sprite>(_iconsFolder);

        int index = 0;
        foreach (var sprite in _endingSprites)
        {
            _endings.Add(new Ending
            {
                title = _endingTitles[index],
                description = _endingDescriptions[index],
                icon = sprite
            });
            index++;
        }

        _endingSaves.UpdateArray(); ///удалить позле создания загрузки из яндекса и сделать приватным метод
    }

    public Ending GetEnding(int endingIndex) => _endings[endingIndex];

    public int GetEndingsCount() => _endings.Count;
}
