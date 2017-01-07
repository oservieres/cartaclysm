using UnityEngine;
using System.Collections;

public class TrafficGarbageCollector : MonoBehaviour {

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Traffic") {
			Destroy (other.gameObject);
		}
	}
}
