using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cartaclysm.Car;

public class PlayersZone : MonoBehaviour {

	public Camera mainCamera;
	public Vector3 averageVelocity;
	public Waypoint target;

	public bool isPlayerDeathActivated = true;

	// Use this for initialization
	void Start () {
	
	}

	void walk(){
		int speed = 70;
		// rotate towards the target
		Waypoint targetAfter = target.GetComponent<Waypoint>().next;
		transform.forward = Vector3.RotateTowards(transform.forward, targetAfter.transform.position - target.transform.position, 0.1f * Time.deltaTime, 0.0f);
		//transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 0.05f * Time.deltaTime, 0.0f);

		// move towards the target
		transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
	} 

	void OnTriggerEnter(Collider other) {
		if (other.tag == "CameraWaypoint") {
			target = other.gameObject.GetComponent<Waypoint>().next;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		walk ();

		if (!isPlayerDeathActivated) {
			return;
		}

		GameObject[] playersCars = GameObject.FindGameObjectsWithTag("PlayerCar");
		int playersCarsCount = playersCars.Length;

		// Check for lost players
		for (int i = 0; i < playersCarsCount; ++i) {
			GameObject playerCar = playersCars [i];
			Vector3 screenPoint = mainCamera.WorldToViewportPoint (playerCar.transform.position);
			if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1) {
				continue;
			}
			PlayerCar carController = playerCar.GetComponent<PlayerCar> ();
			if (carController.isActive) {
				carController.Die (true);
			}
		}

	}
}
