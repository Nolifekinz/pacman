using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public GameObject canvasLevel;
	public GameObject canvas;
	public GameObject canvasHighScore;
	public Text highScore;
	//public static UIManager instance;

	//private void Awake()
	//{
	//	instance = this;
	//}
	//public enum Scene
	//{
	//	Menu,
	//	Pacman
	//}
	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
	public void Play()
	{
		canvasLevel.SetActive(true);
		canvas.SetActive(false);
	}
	public void Back()
	{
		canvas.SetActive(true);
		canvasLevel.SetActive(false);
		canvasHighScore.SetActive(false);
	}
	public void Hard()
	{
		PlayerPrefs.SetString("GameMode", "hard");
		LoadScene("Pacman");
		
	}
	public void Easy()
	{
		PlayerPrefs.SetString("GameMode", "easy");
		LoadScene("Pacman");
	}
	public void HighScore()
	{
		canvas.SetActive(false);
		canvasHighScore.SetActive(true);
		highScore.text = GameManager.HighScore.ToString();
	}
	public void QuitGame()
	{
		Application.Quit();
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
