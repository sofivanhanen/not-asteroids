using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickListener : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public GameObject asteroid;
	public Camera camera;
	
	// Update is called once per frame
	void Update () {
		// If user clicked, spawn a new Asteroid.
		if (Input.GetMouseButtonDown(0)) { // Returns true if button was recently pushed down
			// We get the position of the cursor in pixels,
			// and transform it into world coordinates
			Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
			Vector3 position = new Vector3(mousePosition.x, 0, mousePosition.z);
			Instantiate(asteroid, position, Quaternion.identity);
		}
	}
}
