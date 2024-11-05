using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] public Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highscoreText;
    [SerializeField] private Text livesText;

    public AudioSource munch1;
    public AudioSource munch2;
    public int currentMunch = 0;

    public AudioSource death1;
    public AudioSource death2;
    public AudioSource death3;

    public AudioSource powerPeletEeaten;
    public AudioSource ghostEaten;


    private int ghostMultiplier = 1;
    private int Multiplier = 1;
    private int lives = 3;
    private int score = 0;

    public int Lives => lives;
    public int Score => score;

	private const string HighScoreKey = "HighScore";

	public static int HighScore
	{
		get
		{
			return PlayerPrefs.GetInt(HighScoreKey, 0);
		}
		set
		{
			PlayerPrefs.SetInt(HighScoreKey, value);
			PlayerPrefs.Save();
		}
	}

	private void Awake()
    {
		if (Instance != null && Instance != this) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }
    private void CheckLevel()
    {
		if (PlayerPrefs.GetString("GameMode") == "hard")
		{
			Multiplier = 2;
			pacman.movement.speedMultiplier = Multiplier;
			foreach (Ghost ghostObj in ghosts)
			{
				ghostObj.movement.speedMultiplier = Multiplier;
				ghostObj.points = 300;

			}
		}
	}
    private void Start()
    {
		Time.timeScale = 1f;
		NewGame();
		highscoreText.text = "High Score: " + HighScore.ToString();
	}

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown) {
            NewGame();
        }
        if (IsNewHighScore(score))
        {
            highscoreText.text="High Score: "+score.ToString();
            HighScore=score;
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
		gameOverText.enabled = false;

        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
	}

    private void ResetState()
    {
		for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
		CheckLevel();
	}

    private void GameOver()
    {
        gameOverText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }
	public static bool IsNewHighScore(int score)
	{
		return score > HighScore;
	}
	private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        if (lives == 3)
        {
            death3.Play();
        }
        else if (lives == 2)
        {
            death2.Play();
        }
        else
        {
            death1.Play();
        }
        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0) {
            Invoke(nameof(ResetState), 3f);
        } else {
            GameOver();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        ghostEaten.Play();
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(score + Multiplier*pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
        if (currentMunch == 1)
        {
            munch1.Play();
            currentMunch = 0;
        }
        else
        {
            munch2.Play();
            currentMunch = 1;
        }

    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
		// Bắt đầu phát audio
		powerPeletEeaten.Play();
		for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

	private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

}
