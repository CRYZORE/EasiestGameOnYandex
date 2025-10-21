using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallSpeak : MonoBehaviour
{
    [Header("Cloud Window")]
    [SerializeField] private GameObject _cloudCanvas;
    [SerializeField] private TextMeshProUGUI _mainDialogText;
    [SerializeField] private float _onePhraseTime = 2f;
    private bool _isSpeaking = false;
    [Header("Dialog")]
    [SerializeField] private List<string> _phrasesToSpeak = new();

    private void Start()
    {
        _cloudCanvas.SetActive(false);
    }

    public void DropPhrase()
    {
        if (!_isSpeaking)
        {
            StartCoroutine(SpeakOnePhrase());
        }
    }

    private IEnumerator SpeakOnePhrase()
    {
        _isSpeaking = true;
        _cloudCanvas.SetActive(true);
        _mainDialogText.text = _phrasesToSpeak[Random.Range(0, _phrasesToSpeak.Count)];
        yield return new WaitForSeconds(_onePhraseTime);
        _isSpeaking = false;
        yield return new WaitForSeconds(.5f);
        if (!_isSpeaking)
        {
            _cloudCanvas.SetActive(false);
        }
    }
}
