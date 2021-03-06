﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {

	[Tooltip("In seconds")][SerializeField] float levelLoadDelay = 1f;
	[Tooltip("FX ")][SerializeField] GameObject deathFX;
	
	void OnTriggerEnter(Collider other) {
		deathFX.SetActive(true);
		StartDeathSequence();
	}

	private void StartDeathSequence() {
		SendMessage("OnPlayerDeath");
		Invoke("LoadFirstLevel", levelLoadDelay);
	}

	void LoadFirstLevel() {
		SceneManager.LoadScene(1);
	}
}
