using UnityEngine;
using System.Collections;

public class SlowMotionManager : MonoBehaviour {

	public GameObject pauseMenu;
	private float defaultFixedDeltaTime;

	void OnEnable ()
	{
		EventManager.StartListening ("challenge_end_start", StartChallengeEnd);
		EventManager.StartListening ("challenge_end_finish", FinishChallengeEnd);
	}

	void OnDisable ()
	{
		EventManager.StartListening ("challenge_end_start", StartChallengeEnd);
		EventManager.StartListening ("challenge_end_finish", FinishChallengeEnd);
	}

	private void StartChallengeEnd() {
		Time.timeScale = 0.01f;
		defaultFixedDeltaTime = Time.fixedDeltaTime;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
	}

	private void FinishChallengeEnd() {
		Time.timeScale = 1;
		Time.fixedDeltaTime = defaultFixedDeltaTime;
	}

}
