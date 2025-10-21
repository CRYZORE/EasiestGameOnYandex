using System.Collections;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private EndingManager _endingManager; //ссылка на менеджер концовок
    private GameObject _player; //объект игрока
    private bool _isDead = false; //проверка на смерть

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void LooseHealth(int endingIndex)
    {
        if (!_isDead)
        {
            StartCoroutine(PlayDead(endingIndex));
        }
    }

    private IEnumerator PlayDead(int endingIndex)
    {
        _isDead = true;
        _player.GetComponent<PlayerMovement>().PlayDead(); //проигрывание визуальной смерти игрока через скрипт игрока
        yield return new WaitForSeconds(1f);
        _endingManager.EndingTrigger(endingIndex); //отображаем интерфейс концовки
    }
}
