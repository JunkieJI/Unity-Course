﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;
	[SerializeField] float levelLoadDelay = 2f;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip success;
	[SerializeField] AudioClip death;
	[SerializeField] ParticleSystem mainEngineParticles;
	[SerializeField] ParticleSystem successParticles;
	[SerializeField] ParticleSystem deathParticles;



	bool isTransitioning = false;

	Rigidbody rigidBody;
	AudioSource audioSource;

	bool allowCollisions = true;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isTransitioning) {
			RespondToThrustInput();
			RespondToRotateInput();
		}
		if (Debug.isDebugBuild) {
			RespondToDebugKeys();
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (isTransitioning || !allowCollisions) return;

		switch(collision.gameObject.tag) {
			case "Friendly":
				// Do nothing
				break;
			case "Finish":
				StartSuccessSequence();
				break;
			default:
				StartDeathSequence();
				break;
		}
	}

	private void StartSuccessSequence() {
		isTransitioning = true;
		audioSource.Stop();
		audioSource.PlayOneShot(success);
		successParticles.Play();
		Invoke("LoadNextLevel", levelLoadDelay);
	}

	private void StartDeathSequence() {
		isTransitioning = true;
		audioSource.Stop();
		audioSource.PlayOneShot(death);
		deathParticles.Play();
		Invoke("LoadFirstLevel", levelLoadDelay);
	}

	private void LoadNextLevel() {
		// TODO: Allow for more scenes
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		int nextSceneIndex = currentSceneIndex + 1;
		if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
			nextSceneIndex = 0;
		}
		SceneManager.LoadScene(nextSceneIndex);
	}

	private void LoadFirstLevel() {
		SceneManager.LoadScene(0);
	}

	private void RespondToThrustInput() {
		// Handle thrust
		if (Input.GetKey(KeyCode.Space)) {
			ApplyThrust();
		} else {
			StopApplyingThrust();
		}
	}

	private void ApplyThrust() {
		rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot(mainEngine);
		}
		mainEngineParticles.Play();
	}

	private void StopApplyingThrust() {
		mainEngineParticles.Stop();
		audioSource.Stop();
	}

	private void RespondToRotateInput() {
		rigidBody.angularVelocity = Vector3.zero;

		float rotationThisFrame = rcsThrust * Time.deltaTime;

		// Handle rotation
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Vector3.forward * rotationThisFrame);
		} else if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}
	}

	private void RespondToDebugKeys() {
		if (Input.GetKeyDown(KeyCode.L)) {
			LoadNextLevel();
		} 

		if (Input.GetKeyDown(KeyCode.C)) {
			allowCollisions = !allowCollisions;
		}
	}
}
