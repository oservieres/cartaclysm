using UnityEngine;
using System.Collections;
using TeamUtility.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	public GameObject menu;

	protected PlayerID[] playersIDs = new PlayerID[] { PlayerID.One, PlayerID.Two, PlayerID.Three, PlayerID.Four };
	protected PlayerID currentPlayerID;

	void Awake() {
		menu.SetActive (false);
	}

	void Update () {
		foreach (PlayerID playerID in playersIDs) {
			if (TeamUtility.IO.InputManager.GetButtonDown ("Start", playerID)) {
				if (!menu.activeInHierarchy && GameObject.Find("Player_" + playerID.ToString())) {
					currentPlayerID = playerID;
					EnablePause (playerID);
				} else {
					DisablePause (playerID);
				}
			}
		}
	}

	void EnablePause(PlayerID PlayerID) {
		menu.SetActive (true);
		GameObject.Find ("PauseTitleText").GetComponent<Text> ().text = "Pause for player " + PlayerID.ToString();
		GameObject.Find ("GameRestart").GetComponent<Button> ().Select();
		Time.timeScale = 0.01f;
	}

	void DisablePause(PlayerID PlayerID) {
		menu.SetActive (false);
		Time.timeScale = 1;
	}

	public void RestartScene()
	{
		menu.SetActive (false);
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void QuitGame()
	{
		menu.SetActive (false);
		Time.timeScale = 1;
		Application.Quit ();
	}

	public void QuitPlayer()
	{
		GameObject.Find ("SceneInitManager").GetComponent<SceneInitManager>().RemovePlayer(currentPlayerID);
		StartCoroutine (QuitAfterPause(1.0f));
	}

	IEnumerator QuitAfterPause(float delay)
	{
		float pauseEndTime = Time.realtimeSinceStartup + 1;
		while (Time.realtimeSinceStartup < pauseEndTime) {
			yield return 0;
		}
		
		Time.timeScale = 1;
		menu.SetActive (false);
	}
}
