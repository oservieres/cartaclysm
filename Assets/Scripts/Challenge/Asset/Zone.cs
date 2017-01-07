using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TeamUtility.IO;
using Cartaclysm.Car;

public class Zone : MonoBehaviour {

	protected Dictionary<PlayerID, float> playersTimers = new Dictionary<PlayerID, float>();

	protected int period = 1;
	protected int pointsPerPeriod = 10;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "PlayerCar" && other.gameObject.GetComponent<PlayerCar>().isActive) {
			Player player = other.gameObject.GetComponentInParent<Player> ();
			playersTimers [player.playerID] = 0.0f;
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.gameObject.tag == "PlayerCar" && other.gameObject.GetComponent<PlayerCar>().isActive) {
			Player player = other.gameObject.GetComponentInParent<Player> ();
			if (!playersTimers.ContainsKey (player.playerID)) {
				playersTimers [player.playerID] = 0.0f;
			} else {
				playersTimers [player.playerID] += Time.deltaTime;
			}
			if (playersTimers [player.playerID] > period) {
				playersTimers [player.playerID] = 0;
				player.AddPoints (pointsPerPeriod);
			}
		}
	}
}
