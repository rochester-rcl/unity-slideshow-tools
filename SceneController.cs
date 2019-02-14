﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour {
	private int currentSceneIndex = 1;
	// Use this for initialization
	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	void OnEnable() {
		FullscreenFade.OnFadeOutEnds += loadNext;
	}

	void OnDisable() {
		FullscreenFade.OnFadeOutEnds -= loadNext;
	}

	void Start () {
		SceneManager.LoadSceneAsync(currentSceneIndex);
	}

	void loadNext() {
		currentSceneIndex++;
		if (SceneManager.sceneCountInBuildSettings > currentSceneIndex) {
			SceneManager.LoadSceneAsync(currentSceneIndex);
		} else {
			currentSceneIndex = 1;
			SceneManager.LoadSceneAsync(currentSceneIndex);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}