using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	public GameObject player;

	public float offsetX = 0f;
	public float offsetY = 3f;
	public float offsetZ = -5f;
	public float followSpeed = 2.5f;

	Vector3 position;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LateUpdate () {
		position.x = player.transform.position.x + offsetX;
		position.y = player.transform.position.y + offsetY;
		position.z = player.transform.position.z + offsetZ;

        //transform.position = Vector3.Lerp (transform.position, position, followSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, position, 3.5f*Time.deltaTime);
    }
}
