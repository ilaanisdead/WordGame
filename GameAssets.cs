using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameAssets : MonoBehaviour
{
    
    public static GameAssets i;

    void Awake ()   {
        if(i == null){
            i = this;
        }else{
            Destroy(gameObject);
        }
    }

    public TMP_Text[] tMP_Texts;
    public GameObject[] Applied; // for showing applied in menu modes
    public TMP_Text word; // text for word for the word typed

    public TMP_Text definition; // text for definition for the word typed
    public TMP_Text antonyms; // text for antonyms for the word typed
    public TMP_Text synonyms; // text for synonyms for the word typed

}
