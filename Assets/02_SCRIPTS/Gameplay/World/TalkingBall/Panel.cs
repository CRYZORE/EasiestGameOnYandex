using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    void Start()
    {
        if (_target == null)
        {
            _target = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        gameObject.transform.LookAt(_target.transform);
    }
}
