using UnityEngine;
using System.Collections;

namespace Cartaclysm.Challenge.Challenge {
		
	abstract public class BaseChallenge {

		abstract public string Name ();

		abstract public void Begin ();

		public float Duration() {
			return 62f;
		}

		public virtual int TrafficLowerPercentage() {
			return 45;
		}

		public virtual int TrafficHigherPercentage() {
			return 55;
		}

		abstract public void Update ();

		abstract public void End ();
	}
}