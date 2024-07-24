using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSounds : MonoBehaviour
{
    public AudioSource audioSource, audioSource1, audioSource2;
    public PlayerMovement controlador1;
    bool isWalking = false;
    public AudioClip sonidoDeSalto, sonidoCaminado, sonidoDano;

    void Start()
    {
        audioSource.clip = sonidoDeSalto;
        audioSource1.clip = sonidoCaminado;
        audioSource2.clip = sonidoDano;
    }

    void Update()
    {
        if (controlador1.jump == true)
        {
            audioSource.clip = sonidoDeSalto;
            audioSource.Play();
            Debug.Log("Funciona");
        }

        if (Mathf.Abs(controlador1.horizontalMove) > 0f && !isWalking)
        {
            isWalking = true;
            audioSource1.clip = sonidoCaminado;
            audioSource1.Play();
            Debug.Log("Funciona");
        }
        else if(Mathf.Abs(controlador1.horizontalMove) == 0f && isWalking)
        {
            isWalking = false;
            audioSource1.Play();
        }

    }
}
