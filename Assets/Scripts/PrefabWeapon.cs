using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private SoundShoot sound; // Referencia al script SoundShoot

    void Start()
    {
        sound = FindObjectOfType<SoundShoot>(); // Obtén la referencia al script SoundShoot
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        sound.GenerateGunshotSound(); // Llama al método para generar el sonido del disparo
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}


