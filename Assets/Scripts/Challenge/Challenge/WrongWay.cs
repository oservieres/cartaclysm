using UnityEngine;
using System.Collections;

namespace Cartaclysm.Challenge.Challenge {

	public class WrongWay : BaseChallenge {

		private GameObject zone;

		public override string Name () {
			return "Drive against traffic !";
		}

		public override void Begin () {
			GameObject playersZone = GameObject.Find ("PlayersZone");

			zone = GameObject.Instantiate (Resources.Load("Challenges/WrongWayZone", typeof(GameObject)), playersZone.transform.position, playersZone.transform.rotation) as GameObject;
			zone.transform.parent = playersZone.transform;
			zone.transform.localPosition = new Vector3 (-15, -1, 490);
		}

		public override void End() {
			GameObject.Destroy (zone);
		}

		public override void Update () {

		}
	}

}