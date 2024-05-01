using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChangeMode : MonoBehaviour
{
     
    void Start(){
        
        // showing applied image on the selected mode in menu for modes when game starts 
        string currentMode = PlayerPrefs.GetString("mode", "Normal");
        switch(currentMode){
            default:
                case "Normal":GameAssets.i.Applied[0].SetActive(true);break;
                case "NormalNeg":GameAssets.i.Applied[1].SetActive(true);break;
                case "Inversed":GameAssets.i.Applied[2].SetActive(true);break;
                case "InversedNeg":GameAssets.i.Applied[3].SetActive(true);break;


        }
    }
    public void NormalMode(){
        PlayerPrefs.SetString("mode","Normal"); 
    
    }
    public void NormalNeg(){
        PlayerPrefs.SetString("mode","NormalNeg"); 

    }
    public void Inversed(){
        PlayerPrefs.SetString("mode","Inversed"); 

    }
    public void InversedNeg(){
        PlayerPrefs.SetString("mode","InversedNeg"); 

    }
}
