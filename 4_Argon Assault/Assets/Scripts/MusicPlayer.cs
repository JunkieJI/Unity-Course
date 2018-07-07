using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	void Awake () {
		int numMusicPlayers = FindObjectsOfType<MusicPlayer>().Length;
		if (numMusicPlayers > 0) {
			Destroy(gameObject);
		} else {
			DontDestroyOnLoad(gameObject);
		}
	}
}
