using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
	[SerializeField] float rcsThrust = 100f;
	[SerializeField] float mainThrust = 100f;
	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip success;
	[SerializeField] AudioClip death;



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
			RespondToThrustInput();
			RespondToRotateInput();
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (state != State.Alive) return;

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
		state = State.Transcending;
		audioSource.Stop();
		audioSource.PlayOneShot(success);
		Invoke("LoadNextLevel", 1f);
	}

	private void StartDeathSequence() {
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(death);
		Invoke("LoadFirstLevel", 1f);
	}

	private void LoadNextLevel() {
		// TODO: Allow for more scenes
		SceneManager.LoadScene(1);
	}

	private void LoadFirstLevel() {
		SceneManager.LoadScene(0);
	}

	private void RespondToThrustInput() {
		// Handle thrust
		if (Input.GetKey(KeyCode.Space)) {
			ApplyThrust();
		} else {
			audioSource.Stop();
		}
	}

	private void ApplyThrust() {
		rigidBody.AddRelativeForce(Vector3.up * mainThrust);
		if (!audioSource.isPlaying) {
			audioSource.PlayOneShot(mainEngine);
		}
	}

	private void RespondToRotateInput() {
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
