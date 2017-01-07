using UnityEngine;
using System.Collections;

public class ResourcesPreloader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Resources.LoadAll ("/");
	}

}
