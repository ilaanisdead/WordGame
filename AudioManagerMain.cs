using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManagerMain : MonoBehaviour
{
    public AudioSource audioSource;
    private static AudioManagerMain audioSingleton;
    [SerializeField] private AudioMixer audioMixer;

    // [Range(-80,10)] public float setValue=0f; // value to set audio 

    private void Awake()
    {
        if (audioSingleton == null)
        {
            audioSingleton = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static AudioManagerMain GetInstance()
    {
        if (audioSingleton == null)
        {
            // If AudioManager object was not created yet, find it in the scene
            audioSingleton = FindObjectOfType<AudioManagerMain>();

            if (audioSingleton == null)
            {
                // If AudioManager object doesn't exist in the scene, create a new one
                GameObject obj = new GameObject("AudioManager");
                audioSingleton = obj.AddComponent<AudioManagerMain>();
            }

            DontDestroyOnLoad(audioSingleton.gameObject);
        }

        return audioSingleton;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            // checking for key before setting the volume saved before starting
            audioMixer.SetFloat("Volume", PlayerPrefs.GetFloat("volume"));
        }
    }

    public void Play()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
