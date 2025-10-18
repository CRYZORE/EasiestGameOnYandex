using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Rotator : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotToPlayer = Quaternion.LookRotation(transform.position - playerCamera.transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotToPlayer, 1f);
    }
}
