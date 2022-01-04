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
    [SerializeField] public ParticleSystem shotEffect;
    [SerializeField] public ParticleSystem impactEffect;
    private List<ParticleSystem> particleSystemStorage;

    private void Start()
    {
        List<ParticleSystem> list = new List<ParticleSystem>(20);
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
        shotEffect.Play();
        if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out var hit, range))
        {
            Debug.Log(hit.transform.name);
            var targetComponent = hit.transform.GetComponent<TargetBehaviour>();

            if (targetComponent != null) targetComponent.Hit();

            ParticleSystem impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObj.gameObject, 2f);
        }
    }
}
