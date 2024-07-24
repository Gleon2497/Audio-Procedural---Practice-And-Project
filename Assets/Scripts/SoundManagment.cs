using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Oscilador : MonoBehaviour
{
    public float FM = 44100f;

    int TM, inst;

    [Range(20, 2000)]
    public float frecuencia = 100; 

    public float TiempoSegundos = 2.0f;

    AudioSource audioSource;

    public Slider selector, level;

    int Octava=1;

    bool isPlaying=true, isPlaying1=true, isPlaying2=true;

    public TextMeshProUGUI  textonivel;

    CharacterController2D controlador;

    PlayerMovement controlador1;

    Enemy controlador2;


    public enum WaveformType 
    {
        Sine,
        Square,
        Sawtooth,
        Triangle
    }

    public WaveformType waveformType = WaveformType.Sine;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        controlador = GameObject.FindGameObjectWithTag("player").GetComponent<CharacterController2D>();
        controlador1 = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerMovement>();
        controlador2 = GameObject.FindGameObjectWithTag("enemy").GetComponent<Enemy>();
        // Configuramos el AudioSource
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0;
        audioSource.Stop();

        GenerateWavetable();
    }

    private void Update()
    {
        /*
        if (controlador1.crouch == true)
        {
            if (check == false)
            {
                StopAllCoroutines();
                //PlaySong();
                Debug.Log("Funciona");
                check1 = true;
                check2 = true;
            }
            else
            {
                check = false;
            }
        }

        if (controlador1.jump == true)
        {
            if (check1 == false)
            {
                StopAllCoroutines();
                //PlaySong1();
                Debug.Log("Funciona");
                check = true;
                check2 = true;
            } 
            else
            {
                audioSource.Play();
                check1 = false;
            }
        }

        if (controlador1.horizontalMove != 0f)
        {
            if (check2 == false)
            {
                StopAllCoroutines();
                //PlaySong2();
                Debug.Log("Funciona");
                check = true;
                check1 = true;
            }
            else
            {
                check2 = false;
            }
        }*/
    }
    public int funcion = 0;

    public float CreateSeno(int timeIndex, float frecuencia)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frecuencia / FM);
    }

    public float[] wavetable;
    public int wavetableSize = 2048;
    private void GenerateWavetable() 
    {
        wavetable = new float[wavetableSize];
        float f = FM / wavetableSize;
        for(int i = 0; i < wavetableSize; i++) 
        {
            switch (waveformType)
            {
                case WaveformType.Sine:
                    wavetable[i] += CreateSeno(i, f);
                    break;
            }
        }
    }

    public void KeyboardDown(float f)
    {
        frecuencia = f * Mathf.Pow(2, Octava);
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
            funcion = 0;
            ADSRindex = 0;
        }
    }

    public void KeyboardUp()
    {
        frecuencia = 0;
        ADSRindex = 0;
    }

    float DBStoLinear(float dbfs)
    {
        return Mathf.Pow(10f, dbfs / 20f);
    }

    float nivel = 1;

    public void amplitud()
    {
        nivel = DBStoLinear(level.value);
        textonivel.SetText(Mathf.Round(level.value).ToString());
    }

    [Range(0, 1)]
    public float[] Amplitudes = new float[10] { 1, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.3f, 0.2f };

    [Range(5, 11600)]
    public float A = 100;

    [Range(10, 4000)]
    public float D = 100;

    [Range(100, 6000)]
    public float S = 100;

    [Range(0.001f, 1f)]
    public float Slevel = 0.7f;

    [Range(10, 1500)]
    public float R = 100;

    int ADSRindex = 0;

    float phaseM;
    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {

            float currentsample = 0f;
            try 
            {
                currentsample += wavetable[(int)(phaseM * wavetableSize)];
                data[i] = currentsample * nivel ;
                if (channels == 2) data[i+1] = currentsample * nivel;  
            } 
            catch(System.IndexOutOfRangeException ex) 
            {
                Debug.LogError("An IndexOutOfRangeException occured.");
                Debug.LogError("error message" + ex.Message); 
            }
            try 
            {
                phaseM += frecuencia / FM;
                if(phaseM > 1f) phaseM -= 1f;
            }
            catch (System.IndexOutOfRangeException ex)
            {
                Debug.LogError("An IndexOutOfRangeException occured.");
                Debug.LogError("error message" + ex.Message);
            }
            ADSRindex++;
        }
    }

    public void PlaySong() 
    {
        if (isPlaying == true)
        {
            StartCoroutine(Song());
        }
 
    }

    public void PlaySong1()
    {
        if (isPlaying1 == true)
        {
            StartCoroutine(Song1());
        }
    }

    public void PlaySong2()
    {
        if (isPlaying2 == true)
        {
            StartCoroutine(Song2());
        }
    }

    IEnumerator Song()
    {
            isPlaying=true;
            float tempo = 395f;
            float timePerNote = 60 / tempo;
            Octava= 0;

    

            KeyboardDown(523.251f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(587.330f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(622.254f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(523.251f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(587.330f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();


            KeyboardDown(622.254f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(622.254f);
            yield return new WaitForSeconds(timePerNote );
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(622.254f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote );
            KeyboardUp();

            KeyboardDown(880.000f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(880.000f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(830.609f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(932.328f);
            yield return new WaitForSeconds(timePerNote );
            KeyboardUp();

            KeyboardDown(1046.50f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(830.609f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(932.328f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1046.50f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1046.50f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1046.50f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1046.50f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1046.50f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1046.50f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(698.456f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(987.767f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(987.767f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(987.767f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(987.767f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(987.767f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(987.767f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(987.767f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(311.127f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(391.995f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(466.164f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(587.330f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(622.254f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(783.991f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(932.328f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();

            KeyboardDown(1567.98f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
    
            KeyboardDown(1174.66f);
            yield return new WaitForSeconds(timePerNote);
            KeyboardUp();
        
    }

    IEnumerator Song1()
    {
        float tempo = 160f;
        float timePerNote = 60 / tempo;
        Octava = 0;
        KeyboardDown(783.989f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();
        KeyboardDown(783.989f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();
        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.989f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();


        KeyboardDown(880f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.989f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(493.883f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(440f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(440f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(880f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();


        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(493.883f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 7);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote * 7);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(1046.50f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(987.767f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();


        KeyboardDown(880f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * (1 / 2));
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * (1 / 2));
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * (1 / 2));
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * (1 / 2));
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.395f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(349.228f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(349.228f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(311.127f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(349.228f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote * 5);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote * 6);
        KeyboardUp();
    }

    IEnumerator Song2()
    {
        float tempo = 260f;
        float timePerNote = 60 / tempo;
        Octava = 0;
        KeyboardDown(783.989f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();
        KeyboardDown(783.989f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();
        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.989f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();


        KeyboardDown(880f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.989f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(493.883f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(440f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(440f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(880f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();


        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(493.883f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 7);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote * 7);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(1046.50f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(987.767f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();


        KeyboardDown(880f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(659.255f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * (1 / 2));
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * (1 / 2));
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * (1 / 2));
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * (1 / 2));
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.395f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(349.228f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(349.228f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(311.127f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(349.228f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote * 4);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(415.305f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote * 5);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(391.995f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote * 3);
        KeyboardUp();

        KeyboardDown(466.164f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(587.330f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(523.251f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote * 2);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(622.254f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(698.456f);
        yield return new WaitForSeconds(timePerNote);
        KeyboardUp();

        KeyboardDown(783.991f);
        yield return new WaitForSeconds(timePerNote * 6);
        KeyboardUp();
    }

}




    
 