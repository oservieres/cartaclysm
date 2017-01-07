using UnityEngine;
using System.Collections;

public class TrafficTriggerZone : MonoBehaviour {

	public GameObject[] carsModels;
	public GameObject trafficCarsContainer;
	private int trafficPercentage = 50;

	public void SetTrafficPercentage(int newValue) {
		trafficPercentage = Mathf.Clamp (newValue, 0, 100);
	}

	void OnTriggerEnter(Collider other) {
		if (Random.Range (0, 101) > trafficPercentage) {
			return;
		}

		if (other.tag != "Waypoint") {
			return;
		}

		Waypoint currentWaypoint = other.gameObject.GetComponent<Waypoint> ();
		//Instantiate
		Waypoint nextWaypoint = currentWaypoint.next;
		if (!nextWaypoint) {
			return;
		}
		Vector3 spawnPosition = other.gameObject.transform.position;
		Quaternion spawnRotation = Quaternion.identity;

		//Only match traffic
		var layerMask = 1 << 8;
		if (Physics.OverlapSphere(spawnPosition, 3, layerMask).Length != 0) {
			return;
		}

		GameObject createdCar = Instantiate (carsModels [Random.Range(0, carsModels.Length)], spawnPosition, spawnRotation) as GameObject;

		//Setup correct alignment to ground
		RaycastHit hit;
		if (Physics.Raycast(createdCar.transform.position, Vector3.down, out hit)) {
			createdCar.transform.position = hit.point;
			createdCar.transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * createdCar.transform.rotation;
			Vector3 createdCarVelocity = nextWaypoint.transform.position - currentWaypoint.transform.position;
			createdCarVelocity.Normalize ();
			createdCar.GetComponent<Rigidbody> ().velocity = createdCarVelocity * 50;
		}

		//Setup car
		createdCar.GetComponent<Cartaclysm.Car.CarTrafficControl>().SetTarget(nextWaypoint);
		createdCar.transform.LookAt (nextWaypoint.transform.position);
		createdCar.transform.parent = trafficCarsContainer.transform;
	}
}
