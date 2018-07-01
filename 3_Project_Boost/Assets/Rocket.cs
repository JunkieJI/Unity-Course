using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;

	enum State{ Alive, Dying, Transcending};
	State state = State.Alive;

	Rigidbody rigidBody;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.Alive) {
			Thrust();
			Rotate();
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (state != State.Alive) return;
		
		switch(collision.gameObject.tag) {
			case "Friendly":
				// Do nothing
				break;
			case "Finish":
				state = State.Transcending;
				Invoke("LoadNextLevel", 1f);
				break;
			default:
				state = State.Dying;
				Invoke("LoadFirstLevel", 1f);
				break;
		}
	}

	private void LoadNextLevel() {
		// TODO: Allow for more scenes
		SceneManager.LoadScene(1);
	}

	private void LoadFirstLevel() {
		SceneManager.LoadScene(0);
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
