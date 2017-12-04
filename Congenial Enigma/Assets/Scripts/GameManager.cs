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

	public bool GameStarted = true;

	public Text Score;
	public GameObject StartScreen;
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
		if (Time.time > _nextScoreTick && GameStarted)
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
		parentObject.layer = LayerMask.NameToLayer("Obstacles");
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
		GameStarted = false;
		
		// should only come after we're out of lives
		YouLose.gameObject.SetActive(true);

		TryAgainButton.gameObject.SetActive(true);
	}

	public void StartGame()
	{
		StartScreen.SetActive(false);
		Invoke("SetGameStart", 2);
	}

	void SetGameStart()
	{
		GameStarted = true;
	}
}
