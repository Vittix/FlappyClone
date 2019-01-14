using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Frustum Constant size defined at awake
    float height;
    float width;

    //Score vars
    int score = 0, bestScore = 0;

    //singleton pattern variable
    public static GameManager _Instance;

    //GameOverScreen
    public GameObject gameOverScreen;

    //Player reference
    public PlayerController pController;

    //Obstacle Handler class
    public ObstacleHandler obstacleHandler;

    //ui score and best
    public TextMeshProUGUI ScoreCounter, BestScoreText;

    
    public float FrustumHeight
    {
        get
        {
            return height;
        }
    }

    public float FrustumWidth
    {
        get
        {
            return width;
        }
    }

    private void Awake()
    {
        //destroy current gameobject if gamemanager instance has been already assigned
        if (GameManager._Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //get best  score from playerpref if any
        if (PlayerPrefs.HasKey("Best"))
        {
            bestScore = PlayerPrefs.GetInt("Best");
            BestScoreText.text = "Best:" + bestScore.ToString();
        }
        
    }

  
    public void OnDeath()
    {
        //stop playing 
        obstacleHandler.isPlaying = false;
        StartCoroutine(DeathRoutine());
    }

    public void increaseScore()
    {
        //increase Score and show animation 
        ScoreCounter.gameObject.SetActive(false);
        score++;     
        ScoreCounter.text = score.ToString();
        ScoreCounter.gameObject.SetActive(true);
    }

    IEnumerator DeathRoutine()
    {
        //check if new score higher than best score if so replace best with new
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("Best", bestScore);

            BestScoreText.text = "Best:" + bestScore.ToString();
        }

        yield return new WaitForSeconds(1.5f);

        ShowGameOver();
    }

    private void ShowGameOver()
    {
        //show gameover screen
        gameOverScreen.SetActive(true);
    }

    public void OnPlay()
    {
       //Reset score and play newgame
        score = 0;
        pController.ResetAndPlay();       
    }

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        //Get Frustum Height
        height = 2f * cam.orthographicSize;
        //Get Frustum Width
        width = height * cam.aspect;
    }
}
