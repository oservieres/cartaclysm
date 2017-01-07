using UnityEngine;
using System.Collections;

public class LimitZone : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag != "PlayerCar") {
			return;
		}
		other.gameObject.GetComponent<PlayerCar> ().isAbletoThrust = false;
	}

	void OnTriggerExit(Collider other) {
		if (other.tag != "PlayerCar") {
			return;
		}
		other.gameObject.GetComponent<PlayerCar> ().isAbletoThrust = true;
	}
}
