using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public GameObject asteroid;
	
	// Update is called once per frame
	void Update () {
		// If user clicked, spawn a new Asteroid.
		if (Input.GetMouseButtonDown(0)) {
			Vector3 position = new Vector3(-5, 0, 0);
			Instantiate(asteroid, position, Quaternion.identity);
		}
	}
}
