using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using PlayFab.Json;


public class DictionaryAPI : MonoBehaviour
{   
    [SerializeField] private PlayfabManager playfabManager;
    // [SerializeField] private TMP_InputField inputField;
    // private string wordEntered="";
    [SerializeField] private CalculateScore calculateScore;
    [SerializeField] private GameObject fillAllBoxesParent; // parent for text for showing the errors visually
    private TMP_Text fillAllBoxes;// text for showing the errors visually
    [SerializeField] private TMP_Text yourScore; // for showing the total score visually. enables in this script

    void Awake(){
        fillAllBoxes = fillAllBoxesParent.transform.GetChild(0).GetComponent<TMP_Text>(); 
        
    }

    public void Submit(){
        
        
        // if (inputField.text!=""){
            // Debug.Log(inputField.text);
        // wordEntered = inputField.text;
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
        
        // for(int y=0;y<calculateScore.wordsEntered.Count;y++){
        //     Debug.Log(calculateScore.wordsEntered[y]);
        // }
            
        // }
        
    }
    IEnumerator CheckWordExists(){

        // joining all the string characters in the list to make a word for submitting to API
        string word = String.Join("",calculateScore.wordsEntered);

        // UnityWebRequest dictionaryAPI = UnityWebRequest.Get($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");
        UnityWebRequest dictionaryAPI = UnityWebRequest.Get($"https://meanings.onrender.com/api/{word}");

        yield return dictionaryAPI.SendWebRequest();
        
        // checking with Contains since this api returns json object when error occurs and not an error

        if (dictionaryAPI.result != UnityWebRequest.Result.Success||
        dictionaryAPI.downloadHandler.text.Contains("err")){
        // if (dictionaryAPI.result == UnityWebRequest.Result.ProtocolError){

            // Debug.Log("Error run");
            Debug.LogError(dictionaryAPI.error);
            // Debug.Log("The word does not exist. Please try again with another word");

            // corouting for showing text if nothing has been entered yet
            // or the word is not correct
            StartCoroutine(FillAll("The word does not exist. Try again"));
        }
        else{

            // calculate score only after the word exists
            calculateScore.CalculateForWordsEntered();

            // Request was successful, parse and print response
            string jsonResponse = dictionaryAPI.downloadHandler.text;
            Debug.Log(jsonResponse);
            // DictionaryEntryArray wrapper = new DictionaryEntryArray();  
            // wrapper.entries = JsonUtility.FromJson<List<DictionaryEntry>>(jsonResponse); 
            
            
            yourScore.gameObject.SetActive(true);

            playfabManager.SendLeaderboard(calculateScore.score); // send score to leaderboard if word exists
            

            // DictionaryEntryArray entryArray = new DictionaryEntryArray();

            // Remove square brackets from the JSON string to separate each object
            // string[] jsonObjects = jsonResponse.Substring(1, jsonResponse.Length - 2).Split(',');

            // string trimmedJson = jsonResponse.Replace("[", "").Replace("]", "");

            // // Split the string into individual JSON objects
            // List<string> jsonObjects = SplitJsonObjects(trimmedJson);

            // foreach (string jsonObject in jsonObjects) {
            //     // Deserialize each object individually
            //     DictionaryEntry entry = JsonUtility.FromJson<DictionaryEntry>(jsonObject);
            //     entryArray.entries.Add(entry);
            // }

           // Find the index of the first "[" and the last "]"
            int firstBracketIndex = jsonResponse.IndexOf("[");
            int lastBracketIndex = jsonResponse.LastIndexOf("]");

            // Check if both indices are found
            if (firstBracketIndex != -1 && lastBracketIndex != -1)
            {
                // Replace the first "[" and the last "]" with an empty string
                jsonResponse = jsonResponse.Remove(lastBracketIndex, 1).Remove(firstBracketIndex, 1);
            }
            

            DictionaryEntry entryArray = JsonUtility.FromJson<DictionaryEntry>(jsonResponse);
            
            int definitionIndex = jsonResponse.IndexOf("\"definition\"");
            string definition ="";
            // Check if "definition" substring is found
            if (definitionIndex != -1)
            {
                // Locate the index of the starting quote after "definition"
                int startQuoteIndex = jsonResponse.IndexOf('"', definitionIndex + "\"definition\"".Length);

                // Check if the starting quote is found
                if (startQuoteIndex != -1)
                {
                    // Locate the index of the ending quote after the starting quote
                    int endQuoteIndex = jsonResponse.IndexOf('"', startQuoteIndex + 1);

                    // Check if the ending quote is found
                    if (endQuoteIndex != -1)
                    {
                        // Extract the definition substring
                        definition = jsonResponse.Substring(startQuoteIndex + 1, endQuoteIndex - startQuoteIndex - 1);
                        Debug.Log("Definition " + definition);

                    }
                }
            }

            // visually setting word, def, antonyms, and synonyms for entered word of play when in game view

            GameAssets.i.word.text = $"<color=#0af515>{entryArray.WORD}</color> ";//  definition
            GameAssets.i.definition.text = $"<color=#ff8700>Definition\n</color>{definition}";// showing the typed in word and definition
            GameAssets.i.antonyms.text = "<color=#ff8700>Antonyms</color> \n";
            GameAssets.i.synonyms.text = "<color=#ff8700>Synonyms</color> \n";

            // printing out all the synonyms and anonyms in the array for the word the user entered
            for(int x=0;x<entryArray.ANTONYMS.Count;x++){
    
                // using the conditions to add a comma or end without a comma
                if(x==entryArray.ANTONYMS.Count-1){
                    GameAssets.i.antonyms.text += $"{entryArray.ANTONYMS[x]}";

                }
                else{
                    GameAssets.i.antonyms.text += $"{entryArray.ANTONYMS[x]}, ";

                }

            }
            for(int y=0;y<entryArray.SYNONYMS.Count;y++){

                if(y==entryArray.SYNONYMS.Count-1){
                    GameAssets.i.synonyms.text += $"{entryArray.SYNONYMS[y]}";

                }else{
                    GameAssets.i.synonyms.text += $"{entryArray.SYNONYMS[y]}, ";
                    
                }

            }

            // string meaningsJson = jsonResponse.Contains("MEANINGS") 
            //             ? jsonResponse.Substring(jsonResponse.IndexOf("MEANINGS") + 11)
            //             : null;

            // // Check if MEANINGS array JSON string is not null
            // if (!string.IsNullOrEmpty(meaningsJson))
            // {
            //     // Add back the square brackets to form a valid JSON array
            //     // meaningsJson = "[" + meaningsJson + "]";
            //     Debug.Log(meaningsJson);
            //     // Deserialize the MEANINGS array JSON string into a list of Meaning objects
            //     Meaning meanings = JsonUtility.FromJson<Meaning>(meaningsJson);
            //     Debug.Log(meanings.partsOfSpeech);
            //     // Assign the deserialized meanings list to the MEANINGS property of entryArray
            //     // entryArray.MEANINGS = meanings;
            // }
            
            // Debug.Log(entryArray.MEANINGS.);

            // foreach (DictionaryEntry entry in entryArray) {
            // Debug.Log($"Word: {entryArray.WORD}");
            
            // foreach (Meaning meaning in entryArray.MEANINGS) {
                // Debug.Log($"Part of Speech: {meaning.partsOfSpeech}");
                // Debug.Log($"Definition: {meaning.definition}");

                // foreach (string relatedWord in meaning.relatedWords) {
                //     Debug.Log($"Related Word: {relatedWord}");
                // }

                // foreach (string example in meaning.exampleSentence) {
                //     Debug.Log($"Example Sentence: {example}");
                // }
            // }

            // foreach (string antonym in entryArray.ANTONYMS) {
            //     Debug.Log($"Antonym: {antonym}");
            // }

            // foreach (string synonym in entry.SYNONYMS) {
            //     Debug.Log($"Synonym: {synonym}");
            // }
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
        fillAllBoxesParent.gameObject.SetActive(true);    
        fillAllBoxes.text = fillBox;
        yield return new WaitForSeconds(5f); // time for text to show before it disappears
        fillAllBoxesParent.gameObject.SetActive(false);    

    }


    // List<string> SplitJsonObjects(string json) {
    //     List<string> objects = new List<string>();
    //     int braceCounter = 0;
    //     int startIndex = 0;

    //     for (int i = 0; i < json.Length; i++) {
    //         if (json[i] == '{') {
    //             braceCounter++;
    //         } else if (json[i] == '}') {
    //             braceCounter--;
    //         }

    //         // Check if we've reached the end of an object
    //         if (braceCounter == 0 && json[i] == ',') {
    //             objects.Add(json.Substring(startIndex, i - startIndex + 1).Trim());
    //             startIndex = i + 1;
    //         }
    //     }

    //     // Add the last object
    //     if (startIndex < json.Length) {
    //         objects.Add(json.Substring(startIndex).Trim());
    //     }

    //     return objects;
    // }

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









public class DictionaryEntryArray
{
    public List<DictionaryEntry> entries;
}

// [System.Serializable]
public class DictionaryEntry {
    public string WORD;
    public List<Meaning> MEANINGS;
    public List<string> ANTONYMS;
    public List<string> SYNONYMS;
}

// [System.Serializable]
public class Meaning {
    public string partsOfSpeech;
    public string definition;
    public List<string> relatedWords;
    public List<string> exampleSentence;
}


// #endregion