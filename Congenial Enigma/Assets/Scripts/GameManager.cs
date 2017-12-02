﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance;

//	public bool gameStarted = false;
//
//	public Level currentLevel;
//
//	public Text Score;
//	public Button StartButton;
//	public Button TryAgainButton;
//	public Text YouLose;
//	public Text YouWin;
//	public Text WinText;
//	public Text Instructions;
//	public Text RoundLabel;
//	public Text Rounds;
//	public Level[] levels;
//
//	public GameObject gameOverSound;
//	
//	private int score;
//	private int featuresRemaining;
//	private GameObject[] emitters;
//	
//	
	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	
	}

	
//	void Start ()
//	{
//		AudioListener.volume = 0.5f;
//		YouLose.gameObject.SetActive(false);
//		TryAgainButton.gameObject.SetActive(false);
//		YouWin.gameObject.SetActive(false);
//		WinText.gameObject.SetActive(false);
//		
//		currentLevel = levels[0];
//		SetupNextLevel();
//
//		score = 0;
//	}
//
//	void SetupNextLevel()
//	{
//		Rounds.text = currentLevel.levelNum.ToString();
//		featuresRemaining = currentLevel.featureCount;
//		emitters = currentLevel.emitters;
//		foreach (var emitter in emitters)
//		{
//			Instantiate(emitter, emitter.transform.position, emitter.transform.rotation);
//		}
//	}
//	
//	public void IncreaseScore()
//	{
//		score++;
//		featuresRemaining--;
//		Score.text = score.ToString();
//		if (featuresRemaining == 0)
//		{
//			NextLevel();
//		}
//	}
//
//	public void RestartGame()
//	{
//		SceneManager.LoadScene("Main");
//	}
//	
	public void Lose()
	{
//		GameObject loseSound = Instantiate(gameOverSound, transform.position, Quaternion.identity);
//		Destroy(loseSound, 1f);
//		gameStarted = false;
//		YouLose.gameObject.SetActive(true);
//		TryAgainButton.gameObject.SetActive(true);
	}
//
//	void Win()
//	{
//		YouWin.gameObject.SetActive(true);
//		WinText.gameObject.SetActive(true);
//		TryAgainButton.gameObject.SetActive(true);
//	}
//	
//	public void StartGame()
//	{
//		gameStarted = true;
//		StartButton.gameObject.SetActive(false);
//		Instructions.gameObject.SetActive(false);
//		RoundLabel.gameObject.SetActive(false);
//
//	}
//
//	public void NextLevel()
//	{
//		int nextLevelNum = currentLevel.nextLevelNum;		
//		if (nextLevelNum != 0)
//		{
//			currentLevel = levels[nextLevelNum - 1];
//			SetupNextLevel();
//			StartCoroutine("ShowRound");
//			
//		}
//		else
//		{
//			Win();
//		}
//	}
//
//	IEnumerator ShowRound()
//	{
//		Rounds.gameObject.SetActive(true);
//		RoundLabel.gameObject.SetActive(true);
//		yield return new WaitForSeconds(2f);
//		HideRound();
//	}
//
//	void HideRound()
//	{
//		Rounds.gameObject.SetActive(false);
//		RoundLabel.gameObject.SetActive(false);
//	
//	}
}
