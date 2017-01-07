using UnityEngine;
using System.Collections;
using TeamUtility.IO;
using System.Collections.Generic;

public class PlayerRespawnManager : MonoBehaviour {

	private Dictionary<PlayerID, float> playersToRespawn = new Dictionary<PlayerID, float>();
	public GameObject car;
	public GameObject cameraTarget;
	
	void OnEnable ()
	{
		EventManager.StartListening ("player_die_One", delegate() { InitRespawnPlayer(PlayerID.One); });
		EventManager.StartListening ("player_die_Two", delegate() { InitRespawnPlayer(PlayerID.Two); });
		EventManager.StartListening ("player_die_Three", delegate() { InitRespawnPlayer(PlayerID.Three); });
		EventManager.StartListening ("player_die_Four", delegate() { InitRespawnPlayer(PlayerID.Four); });
	}

	void OnDisable ()
	{
		EventManager.StopListening ("player_die_One", delegate() { InitRespawnPlayer(PlayerID.One); });
		EventManager.StopListening ("player_die_Two", delegate() { InitRespawnPlayer(PlayerID.Two); });
		EventManager.StopListening ("player_die_Three", delegate() { InitRespawnPlayer(PlayerID.Three); });
		EventManager.StopListening ("player_die_Four", delegate() { InitRespawnPlayer(PlayerID.Four); });
	}

	void InitRespawnPlayer (PlayerID playerId)
	{
		playersToRespawn.Add(playerId, 0.0f);
	}

	void Update()
	{
		List<PlayerID> keys = new List<PlayerID> (playersToRespawn.Keys);
		foreach(PlayerID key in keys)
		{
			playersToRespawn[key] += Time.deltaTime;
			if (playersToRespawn[key] > 2) {
				playersToRespawn.Remove (key);
				SpawnPlayer (key);
			}
		}
	}

	void SpawnPlayer(PlayerID playerId)
	{
		GameObject player = GameObject.Find ("Player_" + playerId.ToString());
		if (!player) {
			return;
		}

		SpawnHelicopter helicopter = GameObject.Find("SpawnShip").GetComponent<SpawnHelicopter>() as SpawnHelicopter;
		helicopter.InitRespawn (playerId);
	}
}
