using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;


	Rigidbody rigidBody;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		Thrust();
		Rotate();
	}

	void OnCollisionEnter(Collision collision) {
		switch(collision.gameObject.tag) {
			case "Friendly":
				print("Safu");
				break;
			default:
				print("Dead");
				break;
		}
	}

	private void Thrust() {
		// Handle thrust
		if (Input.GetKey(KeyCode.Space)) {
			rigidBody.AddRelativeForce(Vector3.up * mainThrust);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
		} else {
			audioSource.Stop();
		}
	}

	private void Rotate() {
		// Take manual control of rotation
		rigidBody.freezeRotation = true;

		float rotationThisFrame = rcsThrust * Time.deltaTime;

		// Handle rotation
		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Vector3.forward * rotationThisFrame);
		} else if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(-Vector3.forward * rotationThisFrame);
		}
		// Resume physics control of rotation
		rigidBody.freezeRotation = false;
	}
}
