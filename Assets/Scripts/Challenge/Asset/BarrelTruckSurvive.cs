using UnityEngine;
using System.Collections;

public class BarrelTruckSurvive : Challenge {

	public GameObject barrelTruckSetup;

	public override string Name () {
		return "Stay in the zone !";
	}

	public override void Begin () {
		GameObject playersZone = GameObject.Find ("PlayersZone");

		barrelTruckSetup = GameObject.Instantiate (
			Resources.Load("Challenges/BarrelTruckSetup", typeof(GameObject)),
			playersZone.transform.position,
			playersZone.transform.rotation) as GameObject;
		barrelTruckSetup.transform.parent = playersZone.transform;
		barrelTruckSetup.transform.localPosition = new Vector3 (0, 0, 0);
	}
		
	public override void Update() {

	}

	public override void End() {
		if (barrelTruckSetup) {
			GameObject.Destroy (barrelTruckSetup, 0.15f);
		}
	}

	public override int TrafficLowerPercentage() {
		return 70;
	}

	public override int TrafficHigherPercentage() {
		return 100;
	}
}
