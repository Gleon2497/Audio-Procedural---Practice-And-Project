using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Método que se llama cuando el objeto entra en contacto con un Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Peligroso"))
        {
            Morir();
        }
    }

    // Método que maneja la muerte del jugador
    private void Morir()
    {
        Debug.Log("El jugador ha muerto");
        // Aquí puedes añadir la lógica que desees para manejar la muerte del jugador
        // Por ejemplo, destruir el objeto jugador:
        Destroy(gameObject);
    }
}