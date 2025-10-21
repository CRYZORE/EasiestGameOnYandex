using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingSaves : MonoBehaviour
{
    [SerializeField] private EndingsBase _endingsBase; //связь с EndingsBase
    [SerializeField] private List<bool> _endingsUnlocked = new(); //массив открытых концовок
    [Header("Endings Unlocked Menu")]
    [SerializeField] private Transform _endingSlotsParent; //родительский объект с классом Grid Layout Group
    [SerializeField] private GameObject _endingSlotPrefab; //префаб слота с информацией о концовке
    [SerializeField] private Sprite _lockedEndingSprite;
    [SerializeField] private string _lockedEndingMessage = "Ending locked";
    private List<GameObject> _endingSlots = new();

    private void Start()
    {
        ///настроить под яндекс, сделав подгрузку массива из облачных сохранений, если сохранений нет или кол-во концовок меньше, то: UpdateArray();
    }

    /*private*/ public void UpdateArray()
    {
        if (_endingsUnlocked.Count != _endingsBase.GetEndingsCount())
        {
            for (int i = _endingsUnlocked.Count; i < _endingsBase.GetEndingsCount(); i++)
            {
                _endingsUnlocked.Add(false);
            }
        }
    }

    public void LoadEndingsMenu()
    {
        if (_endingSlots.Count < _endingsBase.GetEndingsCount()) //спавн слотов и загрузка данных концовок
        {
            _endingSlots.Clear();
            for (int i = 0; i < _endingsBase.GetEndingsCount(); i++)
            {
                GameObject slot = Instantiate(_endingSlotPrefab, _endingSlotsParent);
                _endingSlots.Add(slot);
                LoadSlotData(slot, i);
            }
        }
        else
        {
            for (int i = 0; i < _endingsBase.GetEndingsCount(); i++)
            {
                GameObject slot = _endingSlots[i];
                LoadSlotData(slot, i);
            }
        }
    }

    private void LoadSlotData(GameObject slot, int endingIndex)
    {
        TextMeshProUGUI[] texts = slot.GetComponentsInChildren<TextMeshProUGUI>();
        if (_endingsUnlocked[endingIndex])
        {
            texts[0].text = _endingsBase.GetEnding(endingIndex).title;
            texts[1].text = _endingsBase.GetEnding(endingIndex).description;
            slot.GetComponentInChildren<Image>().sprite = _endingsBase.GetEnding(endingIndex).icon;
        }
        else
        {
            texts[0].text = _endingsBase.GetEnding(endingIndex).title;
            texts[1].text = _lockedEndingMessage;
            slot.GetComponentInChildren<Image>().sprite = _lockedEndingSprite;
        }
    }

    public void UnlockEnding(int endingIndex)
    {
        _endingsUnlocked[endingIndex] = true;

        ///прописать логику сохранения в яндексе в будущем
    }
}
