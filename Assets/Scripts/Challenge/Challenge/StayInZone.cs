using UnityEngine;
using System.Collections;

namespace Cartaclysm.Challenge.Challenge {
	public class MovingZone : BaseChallenge {

		private GameObject zone;

		public override string Name () {
			return "Stay in the zone !";
		}

		public override void Begin () {
			GameObject playersZone = GameObject.Find ("PlayersZone");

			zone = GameObject.Instantiate (Resources.Load("Challenges/Zone", typeof(GameObject)), playersZone.transform.position, playersZone.transform.rotation) as GameObject;
			zone.transform.parent = playersZone.transform;
			zone.transform.localPosition = new Vector3 (0, 0, 40);
		}

		public override void End() {
			GameObject.Destroy (zone);
		}

		public override void Update () {

		}

		public override int TrafficLowerPercentage() {
			return 25;
		}

		public override int TrafficHigherPercentage() {
			return 75;
		}
	}
}