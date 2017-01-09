using UnityEngine;
using System.Collections;

public class LeadingTruck : MonoBehaviour {

	public Waypoint target;
	private int speed = 30;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.gameObject.transform.localPosition, speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "HazardWaypoint") {
			target = other.gameObject.GetComponent<Waypoint> ().next.GetComponent<Waypoint> ();
			speed = Random.Range (1, 5);
		}
	}
}
