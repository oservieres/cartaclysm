using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {

	public Waypoint next;

	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, 0.5F);
		Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
		if (next != null) {
			Gizmos.DrawLine(transform.position, next.transform.position);
		}
	}

}
