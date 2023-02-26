using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private string playScene = "PlayScene";
    //LoadScene
    public void Play()
    {
        SceneManager.LoadScene(playScene);
    }
    
    //Quit Game
    public void Quit()
    {
        Application.Quit();
    }
}
