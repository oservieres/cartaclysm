using UnityEngine;
using System.Collections;

public class CameraTarget : MonoBehaviour {

	private int speed = 70;

	// Update is called once per frame
	void FixedUpdate () {
		
		GameObject[] playersCars = GameObject.FindGameObjectsWithTag("PlayerCar");
		int playersCarsCount = playersCars.Length;
		Vector3 localPosition = transform.localPosition;
		if (playersCarsCount == 0) {
			localPosition.x = 0;
		} else {
			float positionX = 0;

			for (int i = 0; i < playersCarsCount; ++i) {
				positionX += transform.InverseTransformDirection(playersCars[i].transform.position).x;
			}
			positionX /= playersCarsCount;
			localPosition.x = positionX;
		}
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, localPosition, speed * Time.deltaTime);

	}
}
