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

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}

	// Calculates whether an object will enter the safety zone or not
	private bool TrajectoryWithinSafetyZone(Vector3 asteroidPosition, Vector3 asteroiVelocity) {
		// We approach this problem by calculating the perpendicular distance
		// between the path of the asteroid and the center of the danger zone.
		// If the distance is smaller than the radius, we know the asteroid will
		// enter the zone by more than half.
		// (The path is calculated through the center of the asteroid and 
		// this calculation does not take into account the size of the asteroid)
		// For the perpendicular distance, we use the equation for a line defined
		// by two points for its simplicity.
		// Slope
		float slope = asteroiVelocity.z / asteroiVelocity.x;
		// TODO: Special case: slope = 0
		// TODO: Check if actually going towards
		// Define points
		float x1 = asteroidPosition.x;
		float z1 = asteroidPosition.z;
		float x2 = x1 + 5; // arbitrary difference / delta
		float z2 = z1 + (slope * 5); // slope = deltaZ / deltaX -> deltaZ = slope * deltaX

		// Equation derived on Wikipedia: https://en.wikipedia.org/wiki/Distance_from_a_point_to_a_line
		// Since x0 = 0 and z0 = 0 (center of danger zone) equation is simplified:
		float distance = Mathf.Abs(x2*z1 - z2*x1) / Mathf.Sqrt(Mathf.Pow(z2-z1, 2) + Mathf.Pow(x2-x1, 2));

		return distance <= dangerZoneRadius;
	}
}
