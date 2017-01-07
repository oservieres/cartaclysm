using UnityEngine;
using System.Collections;

public class CarWreck : MonoBehaviour {

	private int explosionRadius = 30;

	void Start() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach(Collider collider in colliders) {
			Rigidbody rigidBody = collider.gameObject.GetComponent<Rigidbody> ();
			if (rigidBody) {
				rigidBody.AddExplosionForce (500000, transform.position, explosionRadius, 1);
			}
		}

		Destroy (this.gameObject, 10);
	}


}
