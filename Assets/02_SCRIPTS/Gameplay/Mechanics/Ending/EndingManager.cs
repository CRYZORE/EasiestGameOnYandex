using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private GameObject _endingScreen; //объект экрана концовки
    [SerializeField] private TextMeshProUGUI _endingNameText, _endingDescriptionText; //объект текста названия концовки
    [SerializeField] private Image _endingIconImage; //объект изображения иконки концовки
    [Header("Connections")]
    [SerializeField] private EndingsBase _endingsBase; //ссылка на базу концовок
    [SerializeField] private EndingSaves _endingSaves; //ссылка на сохранения

    public void EndingTrigger(int endingIndex)
    {
        Ending ending = _endingsBase.GetEnding(endingIndex);

        _endingNameText.text = ending.title;
        _endingDescriptionText.text = ending.description;
        _endingIconImage.sprite = ending.icon;

        _endingSaves.UnlockEnding(endingIndex); //сохраняем открытие концовки

        _endingScreen.SetActive(true); //включаем экран концовки
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
