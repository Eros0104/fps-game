using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;

    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;


    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
    }


    void Update()
    {
        if (playerInput.OnFoot.Shoot.IsPressed() && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        bool hitSomething = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range);

        if (hitSomething)
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-(hit.normal * impactForce));
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);
        }
    }
}
