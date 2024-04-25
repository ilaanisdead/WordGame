using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class DictionaryAPI : MonoBehaviour
{
   
    [SerializeField] private TMP_InputField inputField;
    private string wordEntered="";
    [SerializeField] private CalculateScore calculateScore;
    [SerializeField] private TMP_Text fillAllBoxes; // for showing the errors visually
    [SerializeField] private TMP_Text yourScore; // for showing the total score visually. enables in this script

    public void Submit(){
        
        
        // if (inputField.text!=""){
            // Debug.Log(inputField.text);
        wordEntered = inputField.text;
        // string word = String.Join("",calculateScore.wordsEntered);
        // Debug.Log(word);
        // checking if user has filled all the boxes before submitting
        
        
        if(calculateScore.wordsEntered.Count==calculateScore.maxWordsForSubmit&&
        calculateScore.wordsEntered.Count!=0){
            StartCoroutine(CheckWordExists());
        }
        else{
            // Debug.Log("Please fill all the boxes");
            StartCoroutine(FillAll("Please fill all the boxes"));

        }
        calculateScore.CalculateForWordsEntered();
        
        // for(int y=0;y<calculateScore.wordsEntered.Count;y++){
        //     Debug.Log(calculateScore.wordsEntered[y]);
        // }
            
        // }
        
    }
    IEnumerator CheckWordExists(){

        // joining all the string characters in the list to make a word for submitting to API
        string word = String.Join("",calculateScore.wordsEntered);

        UnityWebRequest dictionaryAPI = UnityWebRequest.Get($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
        yield return dictionaryAPI.SendWebRequest();
        
        
        // Debug.Log();
        if (dictionaryAPI.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(dictionaryAPI.error);
            // Debug.Log("The word does not exist. Please try again with another word");

            // corouting for showing text if nothing has been entered yet
            // or the word is not correct
            StartCoroutine(FillAll("The word does not exist. Try again"));
        }
        else{
            // Request was successful, parse and print response
            string jsonResponse = dictionaryAPI.downloadHandler.text;
            Debug.Log(jsonResponse);

            yourScore.gameObject.SetActive(true);

            // string[] arr = wordEntered.Split('\n');
            
            // char[] arr = new char[wordEntered.Length];
            // int counter = 0;
            
            // while(counter<arr.Length){
                
                // arr[counter] = wordEntered[counter];  
                // counter++;
            // }


            
            // Debug.Log("[");
            // foreach(char c in arr){
                // Debug.Log($" \"{c}\" ");
            // }
            // Debug.Log("]");

            // foreach(char str in wordEntered){
            //     // Debug.Log(str);
            //     arr
            // }
            
            
            
            // Assuming jsonResponse contains the JSON response string
            // DictionaryEntryArray entryArray = JsonUtility.FromJson<DictionaryEntryArray>(jsonResponse);
            

            // Now you can access the data like this:
            
            // foreach (var entry in entryArray.entries)
            // {
                // Debug.Log($"Word: {entry.word}, Phonetic: {entry.phonetic}");

                // foreach (var phonetic in entry.phonetics)
                // {
                    // Debug.Log($"Phonetic Text: {phonetic.text}, Audio: {phonetic.audio}");
                // }

                // foreach (var meaning in entry.meanings)
                // {
                //     Debug.Log($"Part of Speech: {meaning.partOfSpeech}");

                //     foreach (var definition in meaning.definitions)
                //     {
                //         Debug.Log($"Definition: {definition.definition}, Example: {definition.example}");
                //     }
                // }
            // }
        }


    }
    // corouting for showing text if nothing has been entered yet
    // or the word is not correct 
    IEnumerator FillAll(string fillBox){
        fillAllBoxes.gameObject.SetActive(true);    
        fillAllBoxes.text = fillBox;
        yield return new WaitForSeconds(5f);
        fillAllBoxes.gameObject.SetActive(false);    

    }

}

// #region
// // [Serializable]
// public class Phonetic
// {
//     public string text;
//     public string audio;
// }

// // [Serializable]
// public class Definition
// {
//     public string definition;
//     public string example;
//     public List<string> synonyms;
//     public List<string> antonyms;
// }

// // [Serializable]
// public class Meaning
// {
//     public string partOfSpeech;
//     public List<Definition> definitions;
// }

// // [Serializable]
// public class DictionaryEntry
// {
//     public string word;
//     public string phonetic;
//     public List<Phonetic> phonetics;
//     public string origin;
//     public List<Meaning> meanings;
// }

// public class DictionaryEntryArray
// {
//     public List<DictionaryEntry> entries;
// }

// #endregion