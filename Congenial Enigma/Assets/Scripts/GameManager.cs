﻿using System;
 using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
 using JetBrains.Annotations;
 using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

	public static GameManager Instance;

//	public bool gameStarted = false;

	public Text Score;
//	public Button StartButton;
	public Button TryAgainButton;
	public Text YouLose;
	public GameObject[] ParentSpawners;
	public GameObject[] ParentPrefabs;
	[Range(1,6)]
	public int NumParentsToSpawn;

	public float InitialParentRespawn;

	public float ParentRespawnRate;
//	public Text Instructions;
//
//	public GameObject gameOverSound;

	private Transform[] _kidOrigins;
	private KidController[] _kids;
	private int _score;
	private float _tick = 0.1f;
	private float _nextScoreTick = 0.0f;
	private GameObject[] _parents;
	
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

	
	void Start ()
	{
//		AudioListener.volume = 0.5f;
		YouLose.gameObject.SetActive(false);
		TryAgainButton.gameObject.SetActive(false);
		_score = 0;

		SpawnParents();
		InvokeRepeating("SpawnParent", InitialParentRespawn, ParentRespawnRate);
	}

	void Update()
	{
		if (Time.time > _nextScoreTick)
		{
			_nextScoreTick += _tick;
			_score += 1;
			Score.text = _score.ToString().PadLeft(5, '0');
		}
	}

	void SpawnParent()
	{
		var prefab = ParentPrefabs[Random.Range(0, ParentPrefabs.Length)];
		var spawnPoint = GetFreeSpawnPoint();

		var parentObject = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity);
		parentObject.transform.parent = spawnPoint.transform;			
	
	}
	
	void SpawnParents()
	{
		for (int i = 0; i < NumParentsToSpawn; i++)
		{
			SpawnParent();
		}

	}

	GameObject GetFreeSpawnPoint()
	{
		List<GameObject> freeSpawners = new List<GameObject>();
		
		for (var i = 0; i < ParentSpawners.Length; i++)
		{
			if (ParentSpawners[i].GetComponentInChildren<AdultController>() == null)
			{
				freeSpawners.Add(ParentSpawners[i]);
			}
		}
		
		return freeSpawners[Random.Range(0, freeSpawners.Count)];
	}
	
	public void RestartGame()
	{
		SceneManager.LoadScene("Main");
	}
	
	public void Lose()
	{
		//		GameObject loseSound = Instantiate(gameOverSound, transform.position, Quaternion.identity);
//		Destroy(loseSound, 1f);
//		gameStarted = false;\
		
		// should only come after we're out of lives
		YouLose.gameObject.SetActive(true);

		TryAgainButton.gameObject.SetActive(true);
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
