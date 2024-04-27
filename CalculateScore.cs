using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;

public class CalculateScore : MonoBehaviour
{

    // [SerializeField] private PlayfabManager playfabManager; // for leaderboard
    [HideInInspector] public int score; //to be sent to leaderboard in DictionaryAPI.cs script when word actually exists
    // and update score 
    [SerializeField] private TMP_Text[] tMP_Texts;
    [SerializeField] private GameObject[] enterBoxes;
    private GameObject enterBoxesParent;
    private int currentBoxImage = 0; // keeps track of the current box to display the letter entered
    // used in Update function
    [SerializeField] private TMP_Text yourScore; // for showing the total score visually
    private TMP_Text[] tMP_Texts_Boxes; // the components in the children of enterBoxesParent
    // used in Update function
    private TMP_Text[] tMP_Texts_Insert;// array for insert boxes in tMP_Texts_Boxes
    private TMP_Text[] tMP_Texts_Weights; //array for weight boxes in tMP_Texts_Boxes
    [HideInInspector] public int maxWordsForSubmit=0; // the maximum amount of words expected before sending API request
    [HideInInspector] public List<string> wordsEntered= new List<string>();// this list holds the current words that have been
    // entered by the player. used in Update function

    string[] arr8; // array that holds zip of all the letters with their weights
    int[] arr9;  //array for the weights of the insert boxes
    [SerializeField] private Audio audioS;
    void Start(){

        //randomly picking the insert boxes. whether they're 5 or 8 ...
        System.Random rand = new System.Random();
        enterBoxesParent = enterBoxes[rand.Next(0, enterBoxes.Length)];
        enterBoxesParent.SetActive(true);

        tMP_Texts_Boxes = enterBoxesParent.transform.GetComponentsInChildren<TMP_Text>(); 

        int tMP_Insert_MaxValue = 0; // the max value to assign tMP_Texts_Insert array to avoid null error
        int tMP_Weights_MaxValue = 0; // the max value to assign tMP_Texts_Weights array to avoid null error

        for(int i = 0;i<tMP_Texts_Boxes.Length;i++){
            if(i%2==0){
                tMP_Insert_MaxValue++; // value to be given to the array size 
            }else{
                tMP_Weights_MaxValue++;// value to be given to the array size 
            }
        }

        tMP_Texts_Insert = new TMP_Text[tMP_Insert_MaxValue];
        tMP_Texts_Weights = new TMP_Text[tMP_Weights_MaxValue];

        maxWordsForSubmit = tMP_Texts_Insert.Length;// this variable has to be the length of the insert boxes
        // or else if the user doesn't fill all boxes return must all fill error

        int tMP_TextsCount1 = 0; // this is for the for loop below. makes sure we insert elements in order
        // i.e., positions/indexes 0,1,2,3,4...
        int tMP_TextsCount2 = 0;

        // separating the insert boxes from the weight boxes in the fetched children in the above var        
        for(int i=0;i<tMP_Texts_Boxes.Length;i++){
            if(i%2==0){
                tMP_Texts_Insert[tMP_TextsCount1] = tMP_Texts_Boxes[i];
                tMP_TextsCount1++;

            }
            else{
                tMP_Texts_Weights[tMP_TextsCount2] = tMP_Texts_Boxes[i];
                tMP_TextsCount2++;
            }
        }
        // foreach(TMP_Text tMP in tMP_Texts_Boxes){
        //     Debug.Log(tMP.text);
        // }

        var random = new System.Random();

        // defining array of letters which will later be zipped with another array
        // containing numbers with each letter zipped with its weight
        string[] arr5 = {"A","B","C","D","E","F","G","H","I","J","K",
        "L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};
        


        // the array with weights to be zipped with the letters array
        int[] arr6 = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}; // 26 for 26 letters
        
        // making sure 5 letters have weights. we make sure a random number doesn't replace a weight twice
        int assignWeight = 0;
        
        while(assignWeight<5){
            int x = random.Next(0,26); // making sure that the same random number is used in if and 
            // else blocks

            if(arr6[x]>1){
                continue;
            }
            else{
                arr6[x] = random.Next(2,6);
                assignWeight++;
            }
        }


        // zipping letters with weights
        var arr7 = arr5.Zip(arr6,(first,second)=>first +" " +second);        
        arr8 = arr7.ToArray();

        // for(int z=0;z<arr8.Length;z++){
        //     Debug.Log(arr8[z]);
        // }

        // showing the exponents i.e., weights of each letter in game view
        for(int i = 0;i<26;i++){
            if(arr8[i].Split(" ")[1]=="1"){ // so that we don't have to show 1 if a letter is carrying 1 weight
                tMP_Texts[i].text = "";
            }else{
                tMP_Texts[i].text = arr8[i].Split(" ")[1];
            } 
            
        }

        //array for the weights of the insert boxes
        arr9 = new int[tMP_Texts_Weights.Length];

        // making sure all the values in the weights array are 1 so that next code for assigning weights
        // works well
        for(int x=0;x<arr9.Length;x++){
            arr9[x] = 1;
        }

        // int for keeping track if exactly three boxes have been given weights above 1
        int assignWeightForInsertBox = 0;
        
        // making sure that 3 boxes have weights above 1
        while(assignWeightForInsertBox<3){ 
            int x = random.Next(0,tMP_Texts_Weights.Length); // making sure that the same random number is used in if and 
            // else blocks

            if(arr9[x]>1){
                continue;
            }
            else{
                arr9[x] = random.Next(2,10);
                assignWeightForInsertBox++;
            }
        }
        // for(int x=0;x<arr9.Length;x++){
        //     Debug.Log(arr9[x]);
        // }

        for(int i = 0;i<tMP_Texts_Weights.Length;i++){
            if(arr9[i]==1){ // so that we don't have to show 1 if a letter is carrying 1 weight
                tMP_Texts_Weights[i].text = "";
            }else{
                tMP_Texts_Weights[i].text = arr9[i].ToString();
            } 
            
        }


        

        // // making sure only 4 numbers have numbers higher than 1

        // int[] arr1 = {2,3,4};
        // int[] arr2 = {2,3,4};
        
        // // zipping weights of letters with weights of boxes in game to calculate score by looping
        // // after converting it to an array
        // var arr3 = arr1.Zip(arr2,(first,second)=>first +" " +second);
        
        // // converting the zipped object above to an array for looping and multiplying to get the score
        // string[] arr4 = arr3.ToArray();

        // // looping through the array to calculate score
        // foreach(string item in arr4){

        //     Debug.Log(Int64.Parse(item.Split(" ")[0]) * Int64.Parse(item.Split(" ")[1])); 
            
        // }

    }

    void Update(){
        // Debug.Log(wordsEntered.Count);
        // Iterate over each letter
        for (char letter = 'a'; letter <= 'z'; letter++)
        {
            // preventing indexoutofrange
            // Check if the corresponding key is pressed
            if (Input.GetKeyDown(letter.ToString()))
            {
                if(currentBoxImage<tMP_Texts_Insert.Length){
                    // setting the box to the letter pressed
                    tMP_Texts_Insert[currentBoxImage].text = letter.ToString();
                    
                    wordsEntered.Add(letter.ToString()); // keeping track of the words that have been
                    // currently entered by the user
                    
                    // condition to prevent it from reaching same number as length of array which would 
                    // interfere with backspacing since the index would be out of range and there would
                    // not be a item in that int
                    
                    currentBoxImage++;

                    // playing audio 
                    audioS.Play();
                }
                // Debug.Log("Letter " + letter + " pressed.");
                // Do whatever you want when the letter is pressed
            
            }
        }

        // handling erasing of letters
        if(Input.GetKeyDown(KeyCode.Backspace)){
            
            if(currentBoxImage>0){
                currentBoxImage--; 
            }

            // since currentBoxImage is incremented regardless, we have to remove the value after
            // and after reducing currentBoxImage because a user might at first enter 'a' and then the 
            // the variable goes from 0 to 1 but then might not enter a value again. so you have to remove
            // 'a' only after you've decremented the variable to where it corresponds to the UI holding 'a'.
            // value 
            // This eliminates double backspacing

            tMP_Texts_Insert[currentBoxImage].text = ""; // removing letters with backspace
            
            if(wordsEntered.Count>0){
                wordsEntered.RemoveAt(wordsEntered.Count-1);// removing the latest character entered in list   
            }
            
        }
    }

    public void CalculateForWordsEntered(){

        score = 0;

        // the visuals of the calculation of the score
        string toPrintOut = "";

        // array to keep the weights of the entered words
        int[] wordsEnteredWeights = new int[wordsEntered.Count]; 

        int countWordsEntered = 0; // keeps track of the number of words in the wordsEntered array

        
        // the foreach block makes sure to start back from the start checking the alphabet for each
        // letter in the wordsEntered array otherwise it will give it a 0 if we don't have this block
        foreach(string j in wordsEntered){

        // looping through whole alphabet to check for the weights of the entered words
        // in the wordsEntered list
            for(int i=0;i<26;i++){
                
                // if(countWordsEntered<wordsEntered.Count){ // making sure loop doesn't go on after all strings
                // in the wordsEntered have already been checked

                    if(wordsEntered[countWordsEntered].ToUpper() == 
                    arr8[i].Split(" ")[0]){
                        
                        // storing the weights of each letter in the wordsEntered array in the wordsEnteredWeights
                        // array which will later on be zipped with the array with the insert box weights
                        wordsEnteredWeights[countWordsEntered] = (int)Int64.Parse(arr8[i].Split(" ")[1]);
                        countWordsEntered++;
                        break;
                    }
                    
                // }
                // else{
                    // break; // breaking loop to make sure loop doesn't go on after all strings
                    // in the wordsEntered have already been checked
                // }
            }
        }
        

        // combining the weights of those of the entered words and 
        // those of the insert boxes into one array

        var zippedWeights = wordsEnteredWeights.Zip(arr9,(first,second)=>first +" " +second);        
        string[] zippedWeightsArray= zippedWeights.ToArray(); 

        // creating a string of the calculation of the score to print out to the player
        for(int i=0;i<zippedWeightsArray.Length;i++){
            toPrintOut += zippedWeightsArray[i].Split(" ")[0]+"x"+zippedWeightsArray[i].Split(" ")[0];
            
            if(i==zippedWeightsArray.Length-1){
                toPrintOut += "";
            }
            else{
                toPrintOut += "+";
            }
        }


        // multiplying the weights in zippedWeightsArray to get total score
        for(int z=0;z<zippedWeightsArray.Length;z++){
            
            int y = (int)Int64.Parse(zippedWeightsArray[z].Split(" ")[0]) *
            (int)Int64.Parse(zippedWeightsArray[z].Split(" ")[1]);
            
            score+=y;
        }

        // Debug.Log($"Your score \n {toPrintOut} = {score}");
        yourScore.text = $"Your score: \n {toPrintOut} = {score}"; // visually showing the score to players

        // send score to leaderboard

        // for(int y=0;y<wordsEnteredWeights.Length;y++){
        //     Debug.Log(wordsEnteredWeights[y]);
        // }

        // weights for entered words
        // int[] enteredwordsweights = 
         
        

    }

}
