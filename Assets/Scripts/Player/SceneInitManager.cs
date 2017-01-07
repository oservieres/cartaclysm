using UnityEngine;
using System.Collections;
using TeamUtility.IO;
using System.Collections.Generic;
using Cartaclysm.Car;

public class SceneInitManager : MonoBehaviour {

	public GameObject PlayerPrefab;
	public GameObject CarPrefab;
	public GameObject PlayersCarsContainer;
	public GameObject pauseMenu;
	public GameObject challengeManager;

	public GameObject GUI_Player_One;
	public GameObject GUI_Player_Two;
	public GameObject GUI_Player_Three;
	public GameObject GUI_Player_Four;

	protected PlayerID[] playersIDs = new PlayerID[] { PlayerID.One, PlayerID.Two, PlayerID.Three, PlayerID.Four };
	public Dictionary<PlayerID, GameObject> players = new Dictionary<PlayerID, GameObject>();
	protected Dictionary<PlayerID, PlayerGUI> playersGUIs = new Dictionary<PlayerID, PlayerGUI>();

	void Awake() {
		playersGUIs.Add (PlayerID.One, GUI_Player_One.GetComponent<PlayerGUI>());
		playersGUIs.Add (PlayerID.Two, GUI_Player_Two.GetComponent<PlayerGUI>());
		playersGUIs.Add (PlayerID.Three, GUI_Player_Three.GetComponent<PlayerGUI>());
		playersGUIs.Add (PlayerID.Four, GUI_Player_Four.GetComponent<PlayerGUI>());

		foreach (KeyValuePair<PlayerID, PlayerGUI> playerGUI in playersGUIs) {
			playerGUI.Value.Disable ();
		}
	}

	void Update() {
		foreach(PlayerID playerId in playersIDs) {
			if (TeamUtility.IO.InputManager.GetButtonDown("Jump", playerId)) {
				if (!players.ContainsKey(playerId) && !pauseMenu.activeInHierarchy) {
					activatePlayer(playerId);
				}
			}
		}
	}

	public GameObject getCurrentChallengeWinner() {
		GameObject bestPlayer = null;
		foreach (KeyValuePair<PlayerID, GameObject> player in players) {
			if (bestPlayer == null || bestPlayer.GetComponent<Player> ().currentChallengeScore < player.Value.GetComponent<Player> ().currentChallengeScore) {
				bestPlayer = player.Value;
			}
		}

		return bestPlayer;
	}

	void OnEnable ()
	{
		EventManager.StartListening ("challenge_ended", EndChallenge);
	}

	void OnDisable ()
	{
		EventManager.StopListening ("challenge_ended", EndChallenge);
	}

	private void EndChallenge()
	{
		foreach (KeyValuePair<PlayerID, GameObject> player in players) {
			player.Value.GetComponent<Player> ().FlushScore ();
		}
	}

	private void activatePlayer(PlayerID playerID)
	{
		//Add player
		GameObject newPlayer = Instantiate (PlayerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		Player playerComponent = newPlayer.GetComponent<Player> ();
		newPlayer.transform.parent = PlayersCarsContainer.transform;
		playerComponent.playerID = playerID;
		playerComponent.changeColor();
		newPlayer.name = "Player_" + playerID.ToString();
		players.Add (playerID, newPlayer);

		//Add car
		GameObject.Find("SpawnShip").GetComponent<SpawnHelicopter>().InitRespawn(playerID);

		//Activate Challenges
		challengeManager.SetActive (true);

		//Enable GUI
		playersGUIs [playerID].Enable (playerComponent);
	}

	public void RemovePlayer(PlayerID playerID)
	{
		playersGUIs [playerID].Disable ();
		players.Remove (playerID);
		GameObject player = GameObject.Find("Player_" + playerID.ToString());
		if (player) {
			Destroy (player);
		}
	}

}
