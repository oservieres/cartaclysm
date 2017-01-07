using UnityEngine;
using System.Collections;
using Cartaclysm.Car;

public class WallCheckPoint : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "PlayerCar" && other.gameObject.GetComponent<PlayerCar>().isActive) {
			Player player = other.gameObject.GetComponentInParent<Player> ();
			player.AddPoints (50);
		}
	}

}
