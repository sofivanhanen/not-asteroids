using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBehaviour : MonoBehaviour {

	public Vector3 velocity;
	public GameObject target;
	private float lastDistanceFromTarget;

	// Use this for initialization
	void Start () {
		lastDistanceFromTarget = Vector3.Distance (this.transform.position, target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {

		// Couldn't calculate direction as we can't hit the asteroid
		if (velocity.x == 0 && velocity.z == 0) {
			Destroy(this.gameObject);
		}

		transform.Translate(velocity * Time.deltaTime);

	}

	void LateUpdate() {
		float newDistance = Vector3.Distance (this.transform.position, target.transform.position);
		if (newDistance < lastDistanceFromTarget) {
			lastDistanceFromTarget = newDistance;
			return;
		} else {
			// Going away from our targer, which means we hit already!
			// TODO: We shouldn't have drawn the missile at this point
			Destroy(target);
			Destroy(this.gameObject);
		}
	}
}
