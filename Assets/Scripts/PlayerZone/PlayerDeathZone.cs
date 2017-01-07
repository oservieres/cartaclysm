using UnityEngine;
using System.Collections;

public class PlayerDeathZone : MonoBehaviour {

	void OnTriggerEnter(Collider other) {

		if (other.tag != "Player") {
			return;
		}

		Destroy (other.gameObject);

		Debug.Log ("ATTENTIOn");
	}
}
