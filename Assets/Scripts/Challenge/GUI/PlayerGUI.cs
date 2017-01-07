using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {

	public GameObject activeSection;
	public GameObject inactiveSection;

	public GameObject totalScoreText;
	public GameObject currentChallengeScoreText;

	public Player player;

	void Update() {
		if (player == null) {
			return;
		}
		totalScoreText.GetComponent<Text> ().text = "Total score : " + player.totalScore.ToString();
		currentChallengeScoreText.GetComponent<Text> ().text = "Challenge Score : " + player.currentChallengeScore.ToString();
	}

	public void Enable(Player player) {
		activeSection.SetActive (true);
		inactiveSection.SetActive (false);
		this.player = player;
	}

	public void Disable() {
		activeSection.SetActive (false);
		inactiveSection.SetActive (true);
	}
}
