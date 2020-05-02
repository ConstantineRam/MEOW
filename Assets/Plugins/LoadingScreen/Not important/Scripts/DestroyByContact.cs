using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public int destroyCount = 0;

	void OnCollisionEnter (Collision c) {
		Destroy (c.gameObject);
		destroyCount++;
	}

	void OnTriggerEnter (Collider c) {
		Destroy (c.gameObject);
		destroyCount++;
	}
}
