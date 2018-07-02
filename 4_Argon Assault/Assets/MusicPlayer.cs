using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {

	void Awake () {
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Use this for initialization
	void Start () {
		Invoke("LoadNextScene", 2f);
	}
	
	private void LoadNextScene() {
		SceneManager.LoadScene(1);
	}
}
