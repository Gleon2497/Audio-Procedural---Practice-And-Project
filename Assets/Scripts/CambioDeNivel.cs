using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambiodenivel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Tocando algo");

        if(collision.gameObject.tag =="cambio")
        {
             Debug.Log("Tocando algo");
             
             int nivelActual=SceneManager.GetActiveScene().buildIndex;
             SceneManager.LoadScene(nivelActual+1);
        }
    }
}
