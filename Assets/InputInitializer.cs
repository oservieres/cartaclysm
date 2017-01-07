using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class InputInitializer : MonoBehaviour {

	void Start () {
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.tvOS) {
			InputManager inputManager = GetComponent<InputManager> ();
			inputManager.playerOneDefault = "Xbox_360_OSX_P1";
			inputManager.playerTwoDefault = "Xbox_360_OSX_P2";
			inputManager.playerThreeDefault = "Xbox_360_OSX_P3";
			inputManager.playerFourDefault = "Xbox_360_OSX_P4";
		}
	}

}
