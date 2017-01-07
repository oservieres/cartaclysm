using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	public GameObject mainPosition;
	public GameObject challengeEndPosition;

	public bool isAtMainPosition = true;

	private int translateSpeed = 10;
	private int rotationSpeed = 5;
	private Camera cameraComponent;

	void Start() {
		cameraComponent = GetComponent<Camera> ();
	}
	
	void Update () {
		if (isAtMainPosition) {
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, mainPosition.transform.localPosition, translateSpeed);
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, mainPosition.transform.localRotation, rotationSpeed);
			if (cameraComponent.fieldOfView < 30) {
				cameraComponent.fieldOfView += 0.7f;
			}
		} else {
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, challengeEndPosition.transform.localPosition, translateSpeed);
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, challengeEndPosition.transform.localRotation, rotationSpeed);
			if (cameraComponent.fieldOfView > 4) {
				cameraComponent.fieldOfView -= 0.7f;
			}
		}
	}

	public void SetAtMainPosition() {
		isAtMainPosition = true;
	}

	public void SetAtChallengeEndPosition() {
		isAtMainPosition = false;
	}
}
