using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreText;

    private float timer;
    public static int score;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.1f)
        {

            score += 5;

            //We only need to update the text if the score changed. 
            ScoreText.text = score.ToString();

            //Reset the timer to 0. 
            timer = 0;
        }
    }

}
