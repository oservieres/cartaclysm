using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TeamUtility.IO;
using Cartaclysm.Car;

public class SpawnHelicopter : MonoBehaviour {

	public GameObject attachPoint;
	public GameObject CarPrefab;

	private PlayerID currentRespawningPlayerID;
	private GameObject currentRespawningPlayer;
	private GameObject currentRespawningCar;
	private float speed = 20;
	private Queue<PlayerID> playersIDsToRespawn;
	private float currentRespawnTimer = 0.0f;
	private float maxRespawnLimit = 3.0f;

	void Awake() {
		playersIDsToRespawn = new Queue<PlayerID> ();
	}

	// Update is called once per frame
	void Update () {

		if (currentRespawningPlayer && TeamUtility.IO.InputManager.GetButtonDown ("Jump", currentRespawningPlayerID)) {
			DropCar ();
		}
		if (currentRespawningPlayer) {
			currentRespawnTimer += Time.deltaTime;
			if (currentRespawnTimer >= maxRespawnLimit) {
				DropCar ();
			}
		}
	}

	private void DropCar()
	{
		currentRespawningCar.transform.parent = currentRespawningPlayer.transform;
		Vector3 velocity = currentRespawningCar.transform.forward * 80;
		currentRespawningCar.GetComponent<Rigidbody> ().velocity = velocity;
		PlayerCar playerCar = currentRespawningCar.GetComponent<PlayerCar> ();
		Rigidbody rigidbody = currentRespawningCar.GetComponent<Rigidbody> ();
		rigidbody.useGravity = true;
		rigidbody.isKinematic = false;
		playerCar.isActive = true;
		currentRespawningPlayer = null;
		currentRespawnTimer = 0.0f;
	}

	public void FixedUpdate(){
		if (currentRespawningPlayer && !Mathf.Approximately(transform.localPosition.y, 6)) {
			float step = speed * 2 * Time.deltaTime;
			Vector3 target = transform.localPosition;
			target.y = 6;
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, step);
		} else if (!currentRespawningPlayer) {
			if (!Mathf.Approximately (transform.localPosition.y, 23)) {
				float step = speed * 2 * Time.deltaTime;
				Vector3 target = transform.localPosition;
				target.y = 23;
				transform.localPosition = Vector3.MoveTowards (transform.localPosition, target, step);
			} else {
				if (playersIDsToRespawn.Count > 0) {
					Respawn(playersIDsToRespawn.Dequeue());
				}
			}
		}
			
		if (currentRespawningPlayer) {
			float moveHorizontal = InputManager.GetAxis ("Direction", currentRespawningPlayerID);

			float step = speed * Time.deltaTime * Mathf.Abs(moveHorizontal);
			Vector3 target = transform.localPosition;
			target.x = moveHorizontal > 0 ? 20 : -20;
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, step);
			transform.localRotation = Quaternion.Euler (0.0f, 0.0f, moveHorizontal * -10);
		}
	}

	public void InitRespawn(PlayerID playerID) {
		if (currentRespawningPlayerID != null) {
			playersIDsToRespawn.Enqueue (playerID);
			return;
		}
		Respawn(playerID);
	}

	public void Respawn(PlayerID playerID) {
		currentRespawningPlayerID = playerID;
		currentRespawningPlayer = GameObject.Find ("Player_" + currentRespawningPlayerID.ToString ());
		Vector3 spawnPosition = attachPoint.transform.position;
		Quaternion spawnRotation = attachPoint.transform.rotation;
		currentRespawningCar = Instantiate (CarPrefab, spawnPosition, spawnRotation) as GameObject;
		Rigidbody rigidbody = currentRespawningCar.GetComponent<Rigidbody> ();
		rigidbody.useGravity = false;
		rigidbody.isKinematic = true;
		currentRespawningCar.transform.parent = attachPoint.transform;
		currentRespawningCar.GetComponent<PlayerCar> ().playerId = currentRespawningPlayerID;
		currentRespawningCar.tag = "PlayerCar";

		GameObject body = currentRespawningCar.transform.Find ("Body").gameObject;
		body.GetComponent<Renderer> ().material.color = currentRespawningPlayer.GetComponent<Player>().carColor;
	}
}
