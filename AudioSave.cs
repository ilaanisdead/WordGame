using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class AudioSave : MonoBehaviour
{   
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    
    // making sure if we're returning to main menu, slider is referenced since game scene shouldn't have slider

    void Start(){
    
        // Start is called before the first frame update
        if(PlayerPrefs.HasKey("volume")){ // checking for key before setting the volume saved b4 at start
            audioMixer.SetFloat("Volume",PlayerPrefs.GetFloat("volume"));

        slider.value = PlayerPrefs.GetFloat("volume");

        }
    }
    
    public void SetVolume(float volume){ // slider function
        audioMixer.SetFloat("Volume", volume);// setting the mixer volume
        PlayerPrefs.SetFloat("volume",volume);// saving the current set volume

    }   
}
