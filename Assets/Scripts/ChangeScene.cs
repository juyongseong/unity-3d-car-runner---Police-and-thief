using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private bool isPause = true;

    public GameObject pauseCanvas;

    public void ChangeGameScene()
    {
        SceneManager.LoadScene("game");
        Time.timeScale = 1;
    }

    public void ChangeMenuScene()
    {
        Time.timeScale = 0;
        Score.score = 0;
        SceneManager.LoadScene("Menu");
    }
    public void ChangeRankScene()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene("Rank");
    }
    
    public void ChangePause()
    {

        if (isPause)
        {
            pauseCanvas.SetActive(true);
            Time.timeScale = 0;
            isPause = false;
        }
        else
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            isPause = true;
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
