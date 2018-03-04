using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour {

	private bool spawning;
	private double secondsAlive;
	private Camera cam;
	private Vector3 mousePositionBeforeTakeoff;
	public Vector3 mousePositionAtTakeoff;

	public Vector3 velocity;

	// Use this for initialization
	void Start () {
		spawning = true;
		secondsAlive = 0;
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {

		if (spawning) {
			// We get the position of the cursor in pixels,
			// and transform it into world coordinates
			Vector3 mousePosition = cam.ScreenToWorldPoint (Input.mousePosition);
			Vector3 position = new Vector3 (mousePosition.x, 0, mousePosition.z);
			if (Input.GetMouseButton(0)) { // Returns true if button is currently down
				transform.position = position;
				mousePositionBeforeTakeoff = position;
			} else {
				// Mouse is no longer down, asteroid has been thrown
				spawning = false;
				// Calculate new direction and velocity
				// Last position was saved in 'mousePositionBeforeTakeoff', and current position is in 'position'
				velocity = new Vector3(
					position.x - mousePositionBeforeTakeoff.x,
					position.y - mousePositionBeforeTakeoff.y,
					position.z - mousePositionBeforeTakeoff.z);
				// Velocity is counted with the Pythagorean theorem: speed is sqrt(a²+b²)/time
				// where a is difference of x and b is difference of z
				float aSquared = Mathf.Pow(position.x - mousePositionBeforeTakeoff.x, 2);
				float bSquared = Mathf.Pow(position.z - mousePositionBeforeTakeoff.z, 2);
				float movespeed = Mathf.Sqrt(aSquared + bSquared) / Time.deltaTime;
				velocity = velocity * movespeed;
				mousePositionAtTakeoff = position;
			}
			return;
		}

		secondsAlive += Time.deltaTime;

		transform.Translate(velocity * Time.deltaTime);
		//transfrom.Translate(direction*

		if (secondsAlive > 10) {
			// We delete old, redundant asteroids
			Destroy(this.gameObject);
		}
		
	}
}
