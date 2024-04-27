using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;


public class PlayfabManager : MonoBehaviour
{
    [SerializeField]private Transform parentEntry;
    [SerializeField]private GameObject entry,nameWindow,mainmenu,retryOnline;
    [SerializeField]private TMP_InputField nameInput;
    [SerializeField]private GameObject leaderBoardGameObject;
    // Start is called before the first frame update
    void Start()
    {
        Login();

    }
    public void Login(){
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams{
                GetPlayerProfile = true
            }

        };
        PlayFabClientAPI.LoginWithCustomID(request,OnSuccess,OnError);

    }

    void OnSuccess(LoginResult result){
        Debug.Log("Successful login/account create!");
        string name = null;
        
        if(result.InfoResultPayload.PlayerProfile!=null){
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        if(name==null){
            nameWindow.SetActive(true);
        }
        // Debug.Log(name);
        // mainmenu.SetActive(true);
    }

    public void SubmitNameButton(){
        var request = new UpdateUserTitleDisplayNameRequest{
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,OnDisplayNameUpdate,OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result){
        Debug.Log("Updated display name!");
        nameWindow.SetActive(false);
        // mainmenu.SetActive(true);

    }

    void OnError(PlayFabError error){
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }
    void OnErrorRetry(PlayFabError error){
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
        retryOnline.SetActive(true);
    }

    public void SendLeaderboard(int score){
        var request = new UpdatePlayerStatisticsRequest{
            Statistics=new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = "PlatformScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request,OnLeaderboardUpdate,OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("Successful leaderboard sent");
    }

    public void GetLeaderboard(){
        var request = new GetLeaderboardRequest{
            StatisticName = "PlatformScore",
            StartPosition = 0,
            MaxResultsCount = 10,
            
        };
        PlayFabClientAPI.GetLeaderboard(request,OnLeaderboardGet,OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result){
        
        leaderBoardGameObject.gameObject.SetActive(true);// seeing the leaderboard

        foreach(Transform item in parentEntry){
            Destroy(item.gameObject);
        }
        foreach(var item in result.Leaderboard)
        {
            GameObject prefEntry = Instantiate(entry,parentEntry); // instantiating the gameobject with the stats
            // to display position, name and score 
            TMP_Text[] texts = prefEntry.GetComponentsInChildren<TMP_Text>();

            // visually setting the texts 
            texts[0].SetText((item.Position + 1).ToString());
            texts[1].SetText(item.DisplayName);
            texts[2].SetText(item.StatValue.ToString());
            
            Debug.Log(item.Position + " "+item.PlayFabId+" "+item.StatValue);
        }    
    }
}
