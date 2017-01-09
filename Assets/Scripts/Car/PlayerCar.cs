using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class PlayerCar : MonoBehaviour {

	[SerializeField] private GameObject[] wheelMeshes = new GameObject[4];
	public PlayerID playerId;
	public GameObject Wreck;
	public GameObject carBody;
	public GameObject sparks;
	public bool isAbletoThrust = true;
	public AudioSource engineSlowSound;
	public AudioSource skidSound;
	public AudioSource hoverSound;

	public GameObject tireSmokeLeft;
	public GameObject tireSmokeRight;
	private float smokeTimer = 0.0f;
	public bool isActive = false;
	private bool isDead = false;
	private bool isHover = false;

	private Rigidbody carRigidbody;

	private float maxTorque = 200000;
	private float maxSteer = 100000;

	public float hoverForce = 100f;
	public float hoverHeight = 35f;

	private float thrustInput;
	private float brakeInput;
	private float steerInput;

	private float hoverMultiplier = 2;

	private bool isGrounded = false;

	private Quaternion initialBodyRotation;
	private Quaternion initialRotation;

	void Start () {
		carRigidbody = GetComponent <Rigidbody>();
		initialRotation = transform.rotation;
		initialBodyRotation = carBody.transform.localRotation;
	}

	public void Activate() {
		isActive = true;
		engineSlowSound.Play ();
	}

	void Update () {
		thrustInput = InputManager.GetAxis ("Thrust", playerId);
		if (InputManager.GetInputConfiguration (playerId).name == "KeyboardAndMouse_P1") {
			brakeInput = InputManager.GetAxis ("Brake", playerId);
			thrustInput -= brakeInput;
		} else {
			thrustInput *= -1;
		}
		steerInput = InputManager.GetAxis ("Direction", playerId);
		if (TeamUtility.IO.InputManager.GetButtonDown ("Switch", playerId)) {
			isHover = !isHover;
			if (!isHover) {
				carRigidbody.drag = 0f;
				carRigidbody.angularVelocity = Vector3.zero;
				carRigidbody.AddRelativeForce (
					Vector3.down * 40000,
					ForceMode.Impulse
				);
				carRigidbody.AddRelativeForce (
					Vector3.forward * 40000,
					ForceMode.Impulse
				);
				hoverSound.Stop();
			} else {
				tireSmokeLeft.GetComponent<ParticleSystem> ().Stop ();
				tireSmokeRight.GetComponent<ParticleSystem> ().Stop ();
				skidSound.Stop();
				transform.rotation = initialRotation;
				carBody.transform.localRotation = initialBodyRotation;
				carRigidbody.drag = 1.5f;
				hoverSound.Play();
			}
		}
	}

	void FixedUpdate () {
		rotateWheelMeshes ();
		if (isHover) {
			manageHoverModeDriving ();
		} else {
			manageGroundModeDriving ();
		}
	}

	private void rotateWheelMeshes()
	{
		Quaternion leftWheelsRotation;
		Quaternion rightWheelsRotation;
		if (isHover) {
			leftWheelsRotation = Quaternion.RotateTowards (wheelMeshes [0].transform.localRotation, Quaternion.identity * Quaternion.Euler (90, 0, 0), 5.0f);
			rightWheelsRotation = Quaternion.RotateTowards (wheelMeshes [1].transform.localRotation, Quaternion.identity * Quaternion.Euler (-90, 0, 0), 5.0f);
		} else {
			leftWheelsRotation = Quaternion.RotateTowards (wheelMeshes [0].transform.localRotation, Quaternion.identity * Quaternion.Euler (0.0f, -90f, 0), 5.0f);
			rightWheelsRotation = Quaternion.RotateTowards (wheelMeshes [1].transform.localRotation, Quaternion.identity * Quaternion.Euler (0.0f, -90f, 0), 5.0f);
		}
		wheelMeshes [0].transform.localRotation = leftWheelsRotation;
		wheelMeshes [2].transform.localRotation = leftWheelsRotation;
		wheelMeshes [1].transform.localRotation = rightWheelsRotation;
		wheelMeshes [3].transform.localRotation = rightWheelsRotation;
	}

	private void manageGroundModeDriving()
	{
		// Wheels representation
		for (int i = 0; i < 4; i++) {
			wheelMeshes [i].transform.Rotate (new Vector3(200, 0, 0) * Time.deltaTime);
		}

		if (!isGrounded) {
			return;
		}

		//Thrust
		if (carRigidbody.velocity.magnitude >= 85 || !isAbletoThrust) {
			thrustInput = 0;
		}
		carRigidbody.AddRelativeForce(0f, 0f, thrustInput * maxTorque);
		if (Mathf.Approximately (thrustInput, 0.0f) && carRigidbody.velocity.magnitude > 10) {
			carRigidbody.AddRelativeForce(0f, 0f, -100);
		}

		//Turn
		carRigidbody.AddRelativeForce(steerInput * maxSteer, 0f, 0f);
		carBody.transform.localRotation = Quaternion.RotateTowards (carBody.transform.localRotation, initialBodyRotation * Quaternion.Euler(steerInput * -8, 0, 0), 5.0f);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, initialRotation * Quaternion.Euler(0, steerInput * 8, 0), 15.0f);

		//Stop turning
		if (Mathf.Approximately (steerInput, 0.0f)) {
			carRigidbody.velocity = new Vector3(carRigidbody.velocity.x * 0.95f, carRigidbody.velocity.y, carRigidbody.velocity.z);
		}

		//Skidding: Smoke and sound
		if (!Mathf.Approximately (steerInput, 0.0f)) {
			smokeTimer = 0.2f;
		}
		if (smokeTimer > 0) {
			if (!tireSmokeLeft.GetComponent<ParticleSystem> ().isPlaying) {
				tireSmokeLeft.GetComponent<ParticleSystem> ().Play ();
				tireSmokeRight.GetComponent<ParticleSystem> ().Play ();
				skidSound.Play ();
			}
		} else {
			tireSmokeLeft.GetComponent<ParticleSystem> ().Stop ();
			tireSmokeRight.GetComponent<ParticleSystem> ().Stop ();
			skidSound.Stop();
		}
		smokeTimer -= Time.fixedDeltaTime;
	}

	private void manageHoverModeDriving()
	{
		//Hover
		RaycastHit hit;
		bool intersects = Physics.Raycast (
			                  transform.position, 
			                  -Vector3.up, out hit,
			                  hoverHeight
		                  );
		if (intersects) {
			carRigidbody.AddRelativeForce (
				Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight))
			);
		}

		//Default Thrust
		carRigidbody.AddRelativeForce(0f, 0f, 200000f);

		//Thrust
		if (carRigidbody.velocity.magnitude >= 100 || !isAbletoThrust) {
			thrustInput = 0;
		}
		carRigidbody.AddRelativeForce(0f, 0f, thrustInput * maxTorque * hoverMultiplier * 2);

		//Turn
		carRigidbody.AddForce(-steerInput * maxSteer * hoverMultiplier, 0f, 0f);

		transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.identity * Quaternion.Euler(0.0f, 180.0f, steerInput * -8), 2.0f);
	}

	public void Die(bool byCamera = false)
	{
		GameObject createdWreck = Instantiate(
			Wreck, 
			transform.position, 
			transform.rotation
		) as GameObject;
		if (byCamera) {
			createdWreck.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity * 10;
		} else {
			createdWreck.GetComponent<Rigidbody> ().velocity = GetComponent<Rigidbody> ().velocity;
		}
			
		Destroy (this.gameObject);

		EventManager.TriggerEvent ("player_die_" + playerId.ToString());
	}

	void OnCollisionEnter(Collision col) {
		if (col.relativeVelocity.magnitude > 50 && col.gameObject.tag == "ChallengeHazard" && !isDead) {
			isDead = true;
			EventManager.TriggerEvent ("big_collision");
			Die();
		}
		if (col.relativeVelocity.magnitude > 80 && col.gameObject.tag == "Traffic" && !isDead) {
			isDead = true;
			EventManager.TriggerEvent ("big_collision");
			Die();
		}
		if (col.gameObject.tag == "Scenery") {
			isGrounded = true;
		}
	}

	void OnCollisionStay(Collision col) {
		if (col.gameObject.tag == "Scenery") {
			isGrounded = true;
		} else {
			ContactPoint contact = col.contacts[0];
			GameObject createdSparks = Instantiate(
				sparks, 
				contact.point, 
				Quaternion.identity
			) as GameObject;
			createdSparks.transform.parent = transform;
			createdSparks.transform.localScale = new Vector3(0.3f,0.3f,0.3f);
			GameObject.Destroy (createdSparks, 0.5f);
		}
	}

	void OnCollisionExit(Collision col) {
		if (col.gameObject.tag == "Scenery") {
			isGrounded = false;
		}
	}

	void OnDrawGizmos()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, 
			-Vector3.up, out hit,
			hoverHeight))
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, hit.point);
			Gizmos.DrawSphere(hit.point, 0.5f);
		} else
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, 
				transform.position - Vector3.up * hoverHeight);
		}
	}

}
