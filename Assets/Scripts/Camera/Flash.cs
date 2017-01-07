using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Flash : MonoBehaviour {

	private Image imageComponent;

	// Use this for initialization
	void Start () {
		imageComponent = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Color color = imageComponent.color;
		if (color.a > 0) {
			color.a -= 0.01f;
			imageComponent.color = color;
		}
	}

	public void Launch() {
		Color color = imageComponent.color;
		color.a = 1;
		imageComponent.color = color;
	}
}
