using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour {

	public Vector3 velocity;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Couldn't calculate direction as we can't hit the asteroid
		if (velocity.x == 0 && velocity.z == 0) {
			Destroy(this.gameObject);
		}

		transform.Translate(velocity * Time.deltaTime);

	}
}
