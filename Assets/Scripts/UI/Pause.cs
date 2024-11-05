using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
	public GameObject canvasPause;
	public GameObject canvasRUS;
	public GameObject blur;
	//[SerializeField] Button resume;
	//[SerializeField] Button mainMenu;
	//[SerializeField] Button pause;
	public void PauseGame()
	{
		Time.timeScale = 0f;
		canvasPause.SetActive(true);
		blur.SetActive(true);
	}

	public void ResumeGame()
	{
		Time.timeScale = 1f;
		canvasPause.SetActive(false);
		blur.SetActive(false);
	}
	public void MainMenu()
	{
		canvasPause.SetActive(false);
		canvasRUS.SetActive(true);
	}
	public void Back()
	{
		canvasPause.SetActive(true);
		canvasRUS.SetActive(false);
	}
	//private void Start()
	//{
	//	mainMenu.onClick.AddListener(MainMenu);
	//	resume.onClick.AddListener(ResumeGame);
	//	pause.onClick.AddListener(PauseGame);
	//}

	//private void MainMenu()
	//{
	//	UIManager.instance.LoadScene(UIManager.Scene.Menu);
	//}
}
