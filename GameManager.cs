using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // private int scene;
    [SerializeField] private Animator animator;
    [SerializeField] private float timeForLoading=1f;

    void Start(){
        // for android phones
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void StartGame(){
        animator.SetTrigger("close");
        // int scene = SceneManager.GetActiveScene().buildIndex+1;
        // SceneManager.LoadSceneAsync(scene);
        StartCoroutine(LoadMainGame());
    }
    public void MainMenu(){
        animator.SetTrigger("close");
        // int scene = SceneManager.GetActiveScene().buildIndex+1;
        // SceneManager.LoadSceneAsync(scene);
        StartCoroutine(LoadMainMenu());
    }
    
    public void Quit(){
        Application.Quit();   
    }

    // give time for animations to play during loading scenes
    IEnumerator LoadMainGame(){

        // yield return new WaitForSeconds(timeForLoading);
        yield return SceneManager.LoadSceneAsync(1);
    }
    IEnumerator LoadMainMenu(){

        yield return new WaitForSeconds(timeForLoading);
        yield return SceneManager.LoadSceneAsync(0);
    }
}
