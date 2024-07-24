using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundShoot : MonoBehaviour
{
    public float FM = 44100f;

    [Range(20, 2000)]
    public float frecuencia = 100;

    public float frecuenciaVariacion = 5.0f; // Variación de la frecuencia

    AudioSource audioSource;

    float nivel = 1;

    public float[] wavetable;
    public int wavetableSize = 2048;

    float phaseM;

    public enum WaveformType
    {
        Sine,
        Square,
        Sawtooth,
        Triangle
    }

    public WaveformType waveformType = WaveformType.Triangle;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        audioSource.Stop();

        GenerateWavetable();
    }

    private void GenerateWavetable()
    {
        wavetable = new float[wavetableSize];
        float f = FM / wavetableSize;
        for (int i = 0; i < wavetableSize; i++)
        {
            switch (waveformType)
            {
                case WaveformType.Sine:
                    wavetable[i] = CreateSeno(i, f);
                    break;
                case WaveformType.Square:
                    wavetable[i] = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * i * f / FM));
                    break;
                case WaveformType.Sawtooth:
                    wavetable[i] = 2f * (i / (float)wavetableSize) - 1f;
                    break;
                case WaveformType.Triangle:
                    wavetable[i] = Mathf.PingPong(i / (float)wavetableSize, 1f) * 2f - 1f;
                    break;
            }
        }
    }

    public float CreateSeno(int timeIndex, float frecuencia)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frecuencia / FM);
    }

    float DBStoLinear(float dbfs)
    {
        return Mathf.Pow(10f, dbfs / 20f);
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            float currentSample = 0f;
            try
            {
                currentSample = wavetable[(int)(phaseM * wavetableSize)];
                data[i] = currentSample * nivel;
                if (channels == 2) data[i + 1] = currentSample * nivel;
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.LogError("An IndexOutOfRangeException occured.");
                Debug.LogError("Error message: " + ex.Message);
            }

            phaseM += frecuencia / FM;
            if (phaseM > 1f) phaseM -= 1f;
        }
    }

    public void GenerateGunshotSound()
    {
        StartCoroutine(GunshotCoroutine());
    }

    private IEnumerator GunshotCoroutine()
    {
        float[] gunshotWavetable = new float[wavetableSize];
        float duration = 0.3f; // Duración del sonido del disparo
        int numSamples = (int)(FM * duration);

        // Variar la frecuencia
        float randomFrecuencia = frecuencia + Random.Range(-frecuenciaVariacion, frecuenciaVariacion);

        for (int i = 0; i < numSamples; i++)
        {
            float sample = Mathf.Sin(2 * Mathf.PI * i * (randomFrecuencia * 10) / FM); // Frecuencia alta para el disparo
            sample *= Mathf.Exp(-5.0f * i / numSamples); // Decaimiento exponencial para el sonido
            gunshotWavetable[i % wavetableSize] = sample;
        }

        wavetable = gunshotWavetable;
        phaseM = 0; // Reset phase
        audioSource.Play();
        yield return new WaitForSeconds(duration);
        audioSource.Stop();

        GenerateWavetable(); // Regenerate the original wavetable
    }
}




