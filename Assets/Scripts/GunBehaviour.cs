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
    [SerializeField] private float fireRate = 15f;
    [SerializeField] private float gunSoundStart = 0.25f;

    [SerializeField] public Camera firstPersonCamera;
    [SerializeField] public ParticleSystem shotEffect;
    [SerializeField] public ParticleSystem impactEffect;
    [SerializeField] ScoreCounter scoreCounter;

    [SerializeField] private AudioClip gunSound;

    private float nextTimeToFire = 0f;

    private void Update()
    {
        if (Input.GetMouseButton(LEFTMOUSE) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        scoreCounter.Shot();
        shotEffect.Play();
        PlayGunSound();

        if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.forward, out var hit, range))
        {
            var targetComponent = hit.transform.GetComponent<TargetBehaviour>();

            if (targetComponent != null) { 
                targetComponent.Hit();
            }

            ParticleSystem impactObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactObj.gameObject, 2f);
        }
    }

    private void PlayGunSound()
    {
        var go = new GameObject("gunSound");
        var audioSource = go.AddComponent<AudioSource>();
        audioSource.time = gunSoundStart;
        audioSource.PlayOneShot(gunSound);
        Destroy(go, 2f);
    }
}
