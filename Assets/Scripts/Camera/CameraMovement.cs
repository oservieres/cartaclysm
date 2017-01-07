using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	public GameObject Target;
	public float XOffset;
	public float ZOffset;

	private Rigidbody targetRigidbody;

	void Start() {
		targetRigidbody = Target.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		float vehicleVelocity = targetRigidbody.velocity.magnitude;
		transform.position = new Vector3 (
			Target.transform.position.x + XOffset + Mathf.Max(Mathf.Log(vehicleVelocity), 0.0001f),
			Target.transform.position.y + 10 + Mathf.Max(Mathf.Log(vehicleVelocity), 0.0001f),
			ZOffset
		);
	}
}
