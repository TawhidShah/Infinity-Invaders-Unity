using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This class is responsible for managing the game state.
public class GameManager : MonoBehaviour
{
    //References to the player, invaders, and boss invaders.
    private Player player;
    private Invaders invaders;
    private BossInvaders bossInvaders;

    //References to the score, lives, and high score text.
    public Text scoreText;
    public Text livesText;
    public Text highScoreText;

    //Properties for the score and lives.
    public int score { get; private set; }
    public int lives { get; private set; }

    //Constants for the initial score and lives.
    private const int INITIAL_SCORE = 0;
    private const int INITIAL_LIVES = 3;


    //Awake is called when the script instance is being loaded.
    private void Awake()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        bossInvaders = FindObjectOfType<BossInvaders>();
    }

    //Start is called before the first frame update.
    private void Start()
    {
        //Set up event handlers.
        player.playerKilledEvent += OnPlayerKilled;
        invaders.invaderKilledEvent += OnInvaderKilled;
        bossInvaders.bossInvaderKilledEvent += BossInvaderKilled;
        StartGame();
    }

    //This method initializes the game. It sets the high score, 
    //resets the score and lives to their initial values, 
    //and starts a new round of the game.
    private void StartGame()
    {
        SetHighScore();
        SetScore(INITIAL_SCORE);
        SetLives(INITIAL_LIVES);
        NewRound();
    }

    //This method starts a new regular round by spawning the invaders and player.
    private void NewRound()
    {
        invaders.SpawnInvaders();
        player.SpawnPlayer();
    }

    //This method starts a new boss round by rspawning the boss invaders and player.
    private void NewBossRound()
    {
        bossInvaders.SpawnBossInvaders();
        player.SpawnPlayer();
    }

    //This method sets the high score text.
    private void SetHighScore()
    {
        highScoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("HighScore", 0).ToString().PadLeft(4, '0');
    }

    //This method saves the high score.
    private void SaveHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    //This method sets the score text.
    private void SetScore(int _score)
    {
        score = _score;
        scoreText.text = "SCORE: " + score.ToString().PadLeft(4, '0');
    }

    //This method sets the lives text.
    private void SetLives(int _lives)
    {
        if (_lives < 0)
        {
            _lives = 0;
        }
        lives = _lives;
        livesText.text = "LIVES: " + lives.ToString();
    }

    //This method is called when the player is killed.
    private void OnPlayerKilled()
    {
        //Decrement the number of lives.
        SetLives(lives - 1);

        //Deactivate the player game object.
        player.gameObject.SetActive(false);

        //If the player has lives remaining, spawn the player.
        if (lives > 0)
        {
            player.SpawnPlayer();
        }
        else
        {
            //If there are no lives left, save the high score and load the game over menu.
            SaveHighScore();
            LoadGameOverMenu();
        }

    }

    //This method is called when an invader is killed.s
    private void OnInvaderKilled(Invader invader)
    {
        //Increase the score by the invader's score.
        SetScore(score + invader.invaderScore);

        //If all invaders have been killed, start a new round with the boss invaders.
        if (invaders.numOfInvadersKilled == invaders.totalInvaders)
        {
            invaders.numOfInvadersKilled = 0;
            Invoke("NewBossRound", 1.0f);
        }
    }

    //This method is called when a boss invader is killed.
    private void BossInvaderKilled(BossInvader bossInvader)
    {
        //Increase the score by the boss invader's score.
        SetScore(score + bossInvader.bossInvaderScore);

        //If all boss invaders have been killed, start a new round with the regular invaders.
        if (bossInvaders.numOfBossesKilled == bossInvaders.totalNumOfBosses)
        {
            bossInvaders.numOfBossesKilled = 0;
            Invoke("NewRound", 1.0f);
        }
    }

    //This method loads the game over menu.
    private void LoadGameOverMenu()
    {
        SceneManager.LoadScene("GAME OVER MENU");
    }

}
