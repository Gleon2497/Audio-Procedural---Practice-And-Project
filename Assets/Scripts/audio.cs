using UnityEngine;
using System.Collections;

public class SongGenerator : MonoBehaviour
{
    public int sampleRate = 44100; // 
    public float frequency = 440f; // 
    public float duration = 5f; // 

    private AudioSource audioSource;

    void Start()
    {
        // Obtener el componente AudioSource del objeto actual
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(GenerateSong());
    }

    IEnumerator GenerateSong()
    {
        int numSamples = (int)(sampleRate * duration);
        float[] samples = new float[numSamples];

        for (int i = 0; i < numSamples; i++)
        {
            float t = i / (float)sampleRate;
            samples[i] = Mathf.Sin(2 * Mathf.PI * frequency * t);

            // Actualizar el AudioSource con los samples generados hasta ahora
            audioSource.clip = AudioClip.Create("GeneratedSong", i + 1, 1, sampleRate, false);
            audioSource.clip.SetData(samples, 0);

            
            yield return null;
        }

      
        audioSource.Play();
    }
}