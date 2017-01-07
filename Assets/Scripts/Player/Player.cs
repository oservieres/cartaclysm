using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class Player : MonoBehaviour {

	public PlayerID playerID;
	public bool isActive = false;
	public Color carColor;
	public int totalScore = 0;
	public int currentChallengeScore = 0;

	public void FlushScore() {
		totalScore += currentChallengeScore;
		currentChallengeScore = 0;
	}

	public void AddPoints(int count) {
		currentChallengeScore += count;
	}

	public void changeColor() {
		if (playerID == PlayerID.One) {
			carColor = Color.red;
		} else if (playerID == PlayerID.Two) {
			carColor = Color.blue;
		} else if (playerID == PlayerID.Three) {
			carColor = Color.green;
		} else if (playerID == PlayerID.Four) {
			carColor = Color.yellow;
		}
	}
}
