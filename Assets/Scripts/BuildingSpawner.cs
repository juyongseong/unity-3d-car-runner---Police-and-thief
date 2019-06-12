using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour {

	public GameObject[] buildings;

	public int howManySpawned = 100;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < howManySpawned; ++i) {
			GenerateRandom ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void GenerateRandom () {
		GameObject spawnObject = buildings [Random.Range (0, buildings.Length - 1)];

		float randomScale = Random.Range (1.0f, 3.0f);
		spawnObject.transform.localScale = new Vector3 (randomScale, randomScale, randomScale);

		// Vector3 area = transform.localScale;
		// Vector3 pos = new Vector3 (Random.value * area.x, Random.value * area.y, Random.value * area.z);
		Vector3 pos = RandomPointInBox(transform.position, transform.GetComponent<Collider>().bounds.size);

		GameObject obj = Instantiate (spawnObject, pos, transform.rotation) as GameObject;
		obj.transform.SetParent (transform);
	}

	private static Vector3 RandomPointInBox(Vector3 center, Vector3 size){
		return center + new Vector3 (
			(Random.value - 0.5f) * size.x,
			(Random.value - 0.5f) * size.y,
			(Random.value - 0.5f) * size.z
		);
	}
}
