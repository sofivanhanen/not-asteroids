using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthTurretBehaviour : MonoBehaviour {

	// Arbitrary missile speed
	private const float S = 8;

	// Diameter of our danger zone (set for DangerZone game object)
	private const float dangerZoneDiameter = 3;
	// Radius
	private const float dangerZoneRadius = dangerZoneDiameter / 2;
	// Middle point of danger zone is (0,0,0)

	public GameObject asteroid;
	public GameObject[] spawning;

	public GameObject missile;

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
			if (TrajectoryWithinSafetyZone(asteroidScript.mousePositionAtTakeoff, asteroidScript.velocity)) {
				fireMissile(asteroidScript, instance);
			}
			instance.tag = "Untagged";
		}
	}

	private void fireMissile(AsteroidBehaviour asteroidScript, GameObject asteroidInstance) {
		GameObject instance = Instantiate(missile, new Vector3 (0, 0, 0), Quaternion.identity);
		MissileBehaviour missileScript = instance.GetComponent<MissileBehaviour>() as MissileBehaviour;
		missileScript.velocity = CalculateMissileVelocity(asteroidScript.mousePositionAtTakeoff, asteroidScript.velocity);
		missileScript.target = asteroidInstance;
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

	// Calculate velocity vector for missile
	private Vector3 CalculateMissileVelocity(Vector3 asteroidPosition, Vector3 asteroidVelocity) {
		// As per https://stackoverflow.com/questions/17204513/how-to-find-the-interception-coordinates-of-a-moving-target-in-3d-space?noredirect=1&lq=1

		// Helpers - simplified by a lot because missile origin is at (0,0,0)
		// Also, we're not using asteroid's speed as it's own variable, it's a part of the velocity vector already
		float a = Mathf.Pow(asteroidVelocity.x, 2) + Mathf.Pow(asteroidVelocity.z, 2) - Mathf.Pow(S, 2);
		float b = 2 * ((asteroidPosition.x * asteroidVelocity.x) + (asteroidPosition.z * asteroidVelocity.z));
		float c = Mathf.Pow (asteroidPosition.x, 2) + Mathf.Pow (asteroidPosition.z, 2);

		// Time calculated
		float t1 = (-b + Mathf.Sqrt(Mathf.Pow(b, 2) - (4 * a * c))) / (2 * a);
		float t2 = (-b - Mathf.Sqrt(Mathf.Pow (b, 2) - (4 * a * c))) / (2 * a);

		// Final time
		float t = smallestUsableTime(t1, t2);

		if (t == -1) { // Can't hit! Asteroid is probably too fast
			return new Vector3(0,0,0);
		}

		// Final missile velocity
		Vector3 missileVelocity = (asteroidPosition + (t * asteroidVelocity)) / (t * S);
		return missileVelocity * S;
	}

	// Check to make sure the time we use is the smallest available and usable (not NaN or <0)
	private float smallestUsableTime(float t1, float t2) {
		if (float.IsNaN(t1) || t1 < 0) {
			if (float.IsNaN(t2) || t2 < 0) {
				return -1;
			} else {
				return t2;
			}
		} else {
			if (float.IsNaN(t2) || t2 < 0 || t1 < t2) {
				return t1;
			} else {
				return t2;
			}
		}
	}
}
