using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour {

	private Challenge[] challenges;
	private Challenge currentChallenge;
	private Challenge nextChallenge;
	private float currentChallengeTimer = 0.0f;
	private float currentPauseTimer = 0.0f;

	public GameObject challengeNameDisplay;
	public GameObject challengeTimerDisplay;
	public GameObject mainCamera;
	public GameObject challengeEndCamera;
	public GameObject playerZone;
	public GameObject SceneInitManager;
	public GameObject flash;
	public TrafficTriggerZone trafficTriggerZone;
	public Text trafficPercentageDisplay;

	private SceneInitManager SceneInitManagerLogic;
	private Text challengeTimerDisplayText;

	void Start () {
		challenges = new Challenge[] {
			new StayInZone (),
			new PassTheGates (),
			new WrongWay(),
			new BarrelTruckSurvive()
		};
		challengeTimerDisplayText = challengeTimerDisplay.GetComponent<Text> ();
		SceneInitManagerLogic = SceneInitManager.GetComponent<SceneInitManager> ();
	}

	void StartChallenge() {
		currentChallenge = nextChallenge;
		nextChallenge = null;
		currentChallenge.Begin ();
		currentChallengeTimer = 0.0f;
		challengeNameDisplay.GetComponent<Text>().text = currentChallenge.Name ();
		challengeNameDisplay.SetActive (true);
		StartCoroutine (DisableChallengeDisplay(1.0f));
	}
		
	IEnumerator DisableChallengeDisplay(float delay)
	{
		float pauseEndTime = Time.realtimeSinceStartup + delay;
		while (Time.realtimeSinceStartup < pauseEndTime) {
			yield return 0;
		}
			
		challengeNameDisplay.SetActive (false);
	}

	void Update () {
		if (currentChallenge == null) {
			if (nextChallenge == null) {
				nextChallenge = challenges [Random.Range (0, challenges.Length)];
				int trafficPercentage = Random.Range(nextChallenge.TrafficLowerPercentage(), nextChallenge.TrafficHigherPercentage() + 1);
				trafficTriggerZone.SetTrafficPercentage (trafficPercentage);
				trafficPercentageDisplay.text = "Traffic : " + trafficPercentage + "%";
			}
			if (currentPauseTimer > 5) {
				StartChallenge ();
				currentPauseTimer = 0;
			} else {
				currentPauseTimer += Time.deltaTime;
			}
		} else {
			currentChallenge.Update ();
			if (currentChallengeTimer > currentChallenge.Duration ()) {
				currentChallenge.End ();
				currentChallenge = null;
				currentChallengeTimer = 0.0f;
				EventManager.TriggerEvent ("challenge_ended");
				challengeTimerDisplayText.text = "-";
				StartChallengeEndDisplay ();
				StartCoroutine (FinishChallengeEndDisplay(5.0f));
			} else {
				currentChallengeTimer += Time.deltaTime;
				challengeTimerDisplayText.text = (currentChallenge.Duration() - currentChallengeTimer).ToString();
			}
		}
	}

	void StartChallengeEndDisplay()
	{
		EventManager.TriggerEvent ("challenge_end_start");
		mainCamera.GetComponent<Camera> ().enabled = false;
		challengeEndCamera.GetComponent<Camera> ().enabled = true;
		playerZone.GetComponent<PlayersZone> ().isPlayerDeathActivated = false;
		GameObject winner = SceneInitManagerLogic.getCurrentChallengeWinner ();
		if (winner == null) {
			challengeNameDisplay.GetComponent<Text> ().text = "NO WINNER";
		} else {
			challengeNameDisplay.GetComponent<Text> ().text = winner.GetComponent<Player>().playerID.ToString() + " wins this round !";
			challengeNameDisplay.GetComponent<Text> ().color = winner.GetComponent<Player> ().carColor;
		}
		challengeNameDisplay.SetActive (true);
		flash.GetComponent<Flash> ().Launch ();
	}

	IEnumerator FinishChallengeEndDisplay(float delay) 
	{
		float pauseEndTime = Time.realtimeSinceStartup + delay;
		while (Time.realtimeSinceStartup < pauseEndTime) {
			yield return 0;
		}
			
		mainCamera.GetComponent<Camera> ().enabled = true;
		challengeEndCamera.GetComponent<Camera> ().enabled = false;
		challengeNameDisplay.SetActive (false);
		challengeNameDisplay.GetComponent<Text> ().color = Color.white;
		playerZone.GetComponent<PlayersZone> ().isPlayerDeathActivated = true;
		EventManager.TriggerEvent ("challenge_end_finish");
	}

}
