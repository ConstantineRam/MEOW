using UnityEngine;
using System.Collections.Generic;

public class CapsulseSpawn : MonoBehaviour {

	public GameObject goToSpawn;
	public Vector3 v3Velo, v3SpawnPosition;

	public List<Material> listOfMaterials;

//	public Transform tContainer;

	public float interval = 0.1f;
	float lastSpawn = -60f;

	void Update () {
		if (interval + lastSpawn < Time.time) {
			lastSpawn = Time.time;
			Spawn (goToSpawn);
		}
	}

	void Spawn (GameObject go) {
		var g = (GameObject) Instantiate (go, transform.position + v3SpawnPosition, Quaternion.identity);
		AddRandomVelocity (g.GetComponent<Rigidbody> (), v3Velo);
		ChangeMaterial (g.GetComponent<MeshRenderer> ());
		g.transform.SetParent (transform.parent);
	}

	void AddRandomVelocity (Rigidbody r, Vector3 velo) {
		var v = new Vector3 (Random.Range (-velo.x, velo.x), Random.Range (-velo.y, velo.y), Random.Range (-velo.z, velo.z));
		r.velocity = v;
	}

	void ChangeMaterial (MeshRenderer mr) {
		mr.material = listOfMaterials [Random.Range (0, listOfMaterials.Count)];
	}

}
