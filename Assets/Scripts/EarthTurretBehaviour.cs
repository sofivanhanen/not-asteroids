using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthTurretBehaviour : MonoBehaviour {

	// Arbitrary missile speed
	private const float S = 3;

	// Diameter of our danger zone (set for DangerZone game object)
	private const float dangerZoneDiameter = 3;
	// Radius
	private const float dangerZoneRadius = dangerZoneDiameter / 2;
	// Middle point of danger zone is (0,0,0)

	public GameObject asteroid;
	public GameObject[] spawning;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}

	// LateUpdate so AsteroidBehaviour has updated first
	void LateUpdate() {
		if (Input.GetMouseButtonUp (0)) {
			spawning = GameObject.FindGameObjectsWithTag ("Spawning");
			// TODO: fix the assumption here
			GameObject instance = spawning[0];
			AsteroidBehaviour asteroidScript = instance.GetComponent<AsteroidBehaviour>() as AsteroidBehaviour;
			bool willHit = TrajectoryWithinSafetyZone(asteroidScript.mousePositionAtTakeoff, asteroidScript.velocity);
			// TODO: Send missile
			instance.tag = "Untagged";
		}
	}

	// Calculates whether an object will enter the safety zone or not
	private bool TrajectoryWithinSafetyZone(Vector3 asteroidPosition, Vector3 asteroidVelocity) {
		// We approach this problem by calculating the perpendicular distance
		// between the path of the asteroid and the center of the danger zone.
		// If the distance is smaller than the radius, we know the asteroid will
		// enter the zone by more than half.
		// (The path is calculated through the center of the asteroid and 
		// this calculation does not take into account the size of the asteroid)
		// For the perpendicular distance, we use the equation for a line defined
		// by two points for its simplicity.

		// Define points
		float x1 = asteroidPosition.x;
		float z1 = asteroidPosition.z;
		// Next position
		// Since we added movespeed to the velocity, the velocity can be quite high.
		// This can create a bug where the 'next' spot is actually after Earth,
		// so we make the difference here small
		float x2 = x1 + (asteroidVelocity.x/1000);
		float z2 = z1 + (asteroidVelocity.z/1000);

		// Check that asteroid is moving
		if (x1 == x2 && z1 == z2)
			return false;

		// Equation derived on Wikipedia: https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
		// Since x0 = 0 and z0 = 0 (center of danger zone) equation is simplified:
		float distance = Mathf.Abs(x2*z1 - z2*x1) / Mathf.Sqrt(Mathf.Pow(z2-z1, 2) + Mathf.Pow(x2-x1, 2));

		if (distance > dangerZoneRadius)
			return false;

		// We still have to check if the asteroid is actually coming towards Earth.
		// Using Pythagorean theorem. x0 and z0 = 0, so equation is simplified.
		float firstDistance = Mathf.Sqrt(Mathf.Pow(x1, 2) + Mathf.Pow(z1, 2));
		float secondDistance = Mathf.Sqrt(Mathf.Pow(x2, 2) + Mathf.Pow(z2, 2));
		return secondDistance < firstDistance;
	}
}
