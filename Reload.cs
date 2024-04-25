using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    public void Retry(){
        int x = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(x);
    }
}
