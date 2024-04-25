using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrintText : MonoBehaviour
{
    private TMP_Text tMP_Text;
    private int charIndex;
    private float timer;
    private float timePerChar = 0.1f;
    private string text;
    // private string text = "Kick Buttowski is dominating Widow Maker's Peak";
    private bool invisibleChar=true;

    void Start(){
        text = gameObject.GetComponent<TMP_Text>().text;
        tMP_Text=gameObject.GetComponent<TMP_Text>();
    }
    void Update(){
        if(tMP_Text!=null){
            timer-=Time.deltaTime;
            while(timer<0f){
                
                timer += timePerChar;
                charIndex++;

                string tex = text.Substring(0, charIndex);
                if(invisibleChar){
                    tex += "<color=#00000000>" + text.Substring(0, charIndex)+"</color>";
                }
                tMP_Text.text = tex;

                if(charIndex>=text.Length){
                    tMP_Text = null; // so that in the next frame code doesn't run
                    // and throw error
                    return;
                }
            }
        }        
    }
    

}
