﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	[Header("General")]
	[Tooltip("In ms^-1")][SerializeField] float controlSpeed = 20f;
	[Tooltip("In m")][SerializeField] float xRange = 5f;
	[Tooltip("In m")][SerializeField] float yRange = 3f;

	[SerializeField] GameObject[] guns;


	[Header("Screen-position based")]
	[SerializeField] float positionPitchFactor = -5f;
	[SerializeField] float positionYawFactor = 5f;


	[Header("Control-throw based")]
	[SerializeField] float controlPitchFactor = -20f;

	[SerializeField] float controlRollFactor = -20f;

	

	float xThrow, yThrow;
	bool isControlEnabled = true;
	
	// Update is called once per frame
	void Update () {
		if (isControlEnabled) {
			ProcessTranslation();
			ProcessRotation();
			ProcessFiring();	
		}
	}

	void OnPlayerDeath() { // Called by string reference
		isControlEnabled = false;
	}

	private void ProcessTranslation() {
		xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
		float xOffset = xThrow * controlSpeed * Time.deltaTime;
		float rawXPos = transform.localPosition.x + xOffset;
		float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

		yThrow = CrossPlatformInputManager.GetAxis("Vertical");
		float yOffset = yThrow * controlSpeed * Time.deltaTime;
		float rawYPos = transform.localPosition.y + yOffset;
		float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);
		
		transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
	}

	private void ProcessRotation() {
		float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
		float pitchDueToControlThrow = yThrow * controlPitchFactor;
		float pitch = pitchDueToPosition + pitchDueToControlThrow;
		float yaw = transform.localPosition.x * positionYawFactor;
		float roll = xThrow * controlRollFactor;
		transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
	}

	private void ProcessFiring() {
		if(CrossPlatformInputManager.GetButton("Fire")) {
			ActivateGuns();
		} else {
			DeactivateGuns();
		}
	}

	private void ActivateGuns() {
		foreach(GameObject gun in guns) {
			gun.SetActive(true);
		}
	}

	private void DeactivateGuns() {
		foreach(GameObject gun in guns) {
			gun.SetActive(false);
		}
	}
}
