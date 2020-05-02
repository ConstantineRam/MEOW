using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerAudio : MonoBehaviour {

	public List <AudioClip> audioList;

	private AudioSource player;

	private static ManagerAudio instance { get; set; } 

	void Awake () {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		player = GetComponent <AudioSource>();

		StartCoroutine (PlayMusicBackground ());
	}

	private IEnumerator PlayMusicBackground () {
		while (true) {
			int n = Mathf.RoundToInt(Random.Range (0, audioList.Count));

			player.PlayOneShot (audioList[n]);

			yield return new WaitForSeconds (audioList[n].length);
		}
	}
}
