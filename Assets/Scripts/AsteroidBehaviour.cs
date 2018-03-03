using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {

	private bool spawning;
	private double secondsAlive;
	private Camera cam;

	// Use this for initialization
	void Start () {
		spawning = true;
		secondsAlive = 0;
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

		if (spawning) {
			// If mouse is down, player is 'throwing'
			if (Input.GetMouseButton(0)) { // Returns true, if button is currently down
				// We get the position of the cursor in pixels,
				// and transform it into world coordinates
				Vector3 mousePosition = cam.ScreenToWorldPoint (Input.mousePosition);
				Vector3 position = new Vector3 (mousePosition.x, 0, mousePosition.z);
				transform.position = position;
			} else {
				// Mouse is no longer down, asteroid has been thrown
				spawning = false;
			}
			return;
		}

		secondsAlive += Time.deltaTime;

		if (secondsAlive > 10) {
			// We delete old, redundant asteroids
			Destroy(this.gameObject);
		}
		
	}
}
