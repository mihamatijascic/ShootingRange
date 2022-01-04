using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    private const int LEFTMOUSE = 0;
    private const int RIGHTMOUSE = 1;
    private const int MIDDLEMOUSE = 2;

    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100000f;

    [SerializeField] public Camera firstPersonCamera;
    
    private void Start()
    {
        
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(LEFTMOUSE))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out var hit, range))
        {
            Debug.Log(hit.transform.name);
            var targetComponent = hit.transform.GetComponent<TargetBehaviour>();

            if (targetComponent != null) targetComponent.Hit();
        }
    }
}
