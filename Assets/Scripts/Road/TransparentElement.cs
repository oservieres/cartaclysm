using UnityEngine;
using System.Collections;

public class TransparentElement : MonoBehaviour {

	private Material material;
	private bool isFading = false;

	// Use this for initialization
	void Start () {
		material = GetComponent<MeshRenderer> ().materials [0];
	}

	void Update() {
		Color color = material.color;
		if (isFading) {
			if (color.a > 0.3f) {
				color.a = color.a - (2f * Time.deltaTime);
			}
		} else {
			color.a = 1;
		}
		material.color = color;
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "TransparencyZone") {
			isFading = true;
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (other.tag == "TransparencyZone") {
			isFading = false;
		}
	}
}
