using UnityEngine;
using System.Collections;

public class WallRock : MonoBehaviour {

	private float delay = 0.0f;

	private bool isFalling = false;
	private float speed = 200.0f;

	void Start () {
		delay = ((float)Random.Range(0, 400)) / 1000;
		StartCoroutine (StartFall(delay));
	}

	IEnumerator StartFall(float delay)
	{
		float pauseEndTime = Time.realtimeSinceStartup + delay;
		while (Time.realtimeSinceStartup < pauseEndTime) {
			yield return 0;
		}

		isFalling = true;
	}

	void FixedUpdate () {
		if (isFalling) {
			transform.Translate (Vector3.down * Time.deltaTime * speed, Space.World);
		}
	}
		
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Scenery") {
			isFalling = false;
		}
	}
}
