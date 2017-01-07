using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TeamUtility.IO;
using Cartaclysm.Car;

public class StayInZoneChallengeZone : Zone {

	public GameObject[] targets;

	private GameObject currentTarget;
	private float speed = 2.0f;
	private float timer = 0.0f;

	void Update () {
		if (!currentTarget || timer > 8) {
			int randomOffset = Random.Range (0, targets.Length);
			currentTarget = targets [randomOffset];
			timer = 0;
		} else {
			transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, speed * Time.deltaTime);
			timer += Time.deltaTime;
		}
	}
		
}
