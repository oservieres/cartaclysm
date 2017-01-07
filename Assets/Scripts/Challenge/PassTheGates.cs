using UnityEngine;
using System.Collections;

public class PassTheGates : Challenge {

	private float timer = 0.0f;
	private GameObject playersZone;

	private GameObject currentWall;

	public override string Name () {
		return "Pass the gates !";
	}

	public override void Begin () {
		playersZone = GameObject.Find ("PlayersZone");
		CreateWall ();
	}

	public override void Update() {
		timer += Time.deltaTime;
		if (timer > 8.0f) {
			timer = 0.0f;
			CreateWall ();
		}
	}

	private void CreateWall()
	{
		if (currentWall) {
			GameObject.Destroy (currentWall);
		}
		int sideMultiplier = Random.Range (0, 2);
		if (sideMultiplier == 0) {
			sideMultiplier = -1;
		}
		int position = Random.Range (8 * sideMultiplier, 20 * sideMultiplier);
		currentWall = GameObject.Instantiate (
			Resources.Load("Challenges/Wall", typeof(GameObject)), 
			playersZone.transform.position - new Vector3(position, 0, 300), 
			playersZone.transform.rotation
		) as GameObject;
	}

	public override void End() {
		timer = 0.0f;
		if (currentWall) {
			GameObject.Destroy (currentWall);
		}
	}

	public override int TrafficLowerPercentage() {
		return 10;
	}

	public override int TrafficHigherPercentage() {
		return 30;
	}
}
