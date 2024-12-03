using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunJump : MonoBehaviour
{
    float LaunchForce = 30;
    bool onCoolDown = false;
    bool needsReloading = false;
    float reloadCooldown = 1.0f;
    [NonSerialized]
    public Rigidbody attached_rb;

    AudioClip firingNoise;

    AudioClip reloadNoise;
    private void Awake()
    {
        firingNoise = Resources.Load<AudioClip>("SFX/ShotGunFiring");
        reloadNoise = Resources.Load<AudioClip>("SFX/ShotGunReload");
    }

    public void Launch() 
    {
        if (!onCoolDown && !needsReloading)
        {
            attached_rb.AddForce(-FindObjectOfType<Camera>().transform.forward * LaunchForce, ForceMode.Impulse);
            GetComponent<AudioSource>().PlayOneShot(firingNoise, 1.0f);
            onCoolDown = true;
            needsReloading = true;
            StartCoroutine(weaponCooldown());
        }
    }

    IEnumerator weaponCooldown() 
    {
        yield return new WaitForSeconds(reloadCooldown + firingNoise.length);
        onCoolDown = false;
    }
    public void Reload() 
    {
        if (!onCoolDown && needsReloading && !GetComponent<AudioSource>().isPlaying)
        {
            StartCoroutine(weaponReload());
        }
    }

    IEnumerator weaponReload() 
    {
        GetComponent<AudioSource>().PlayOneShot(reloadNoise, 1.0f);
        yield return new WaitForSeconds(reloadNoise.length);
        needsReloading = false;
    }
}
