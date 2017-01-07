using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using TeamUtility.IO;

namespace Cartaclysm.Car
{
	[RequireComponent(typeof (CarController))]
	public class CarUserControl : MonoBehaviour
	{
		private CarController m_Car; // the car controller we want to use

		public PlayerID playerId;


		public float speed = 90f;
		public float turnSpeed = 5f;
		public float hoverForce = 65f;
		public float hoverHeight = 3.5f;
		private float powerInput;
		private float turnInput;
		private Rigidbody carRigidbody;


		private void Awake()
		{
			// get the car controller
			m_Car = GetComponent<CarController>();
			carRigidbody = GetComponent <Rigidbody>();
		}

		void Update () 
		{
			turnInput = InputManager.GetAxis("Direction", playerId);

			float thrust = InputManager.GetAxis("Thrust", playerId);
			float brake = InputManager.GetAxis("Brake", playerId);
			powerInput = thrust - brake;
		}


		private void FixedUpdate()
		{
			Ray ray = new Ray (transform.position, -transform.up);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, hoverHeight))
			{
				float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
				Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
				carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
			}
			carRigidbody.AddRelativeForce(0f, 0f, powerInput * speed * 400);
			carRigidbody.AddRelativeForce( turnInput * turnSpeed * 4000, 0f, 0f);

			/*
			// pass the input to the car!
			float h = InputManager.GetAxis("Direction", playerId);
			// get a "forward vector" for each rotation
			Vector3 forwardA = new Quaternion (0.0f, 1.0f, 0.0f, 0.0f) * Vector3.forward;
			Vector3 forwardB = transform.rotation * Vector3.forward;
			// get a numeric angle for each vector, on the X-Z plane (relative to world forward)
			var angleA = Mathf.Atan2 (forwardA.x, forwardA.z) * Mathf.Rad2Deg;
			var angleB = Mathf.Atan2 (forwardB.x, forwardB.z) * Mathf.Rad2Deg;
			// get the signed difference in these angles
			var angleDiff = Mathf.DeltaAngle (angleB, angleA);
			if (Mathf.Approximately (h, 0.0f)) {
				h = Mathf.Clamp (angleDiff / 20, -1, 1);
			}
			float thrust = InputManager.GetAxis("Thrust", playerId);
			float brake = InputManager.GetAxis("Brake", playerId);
			float v = thrust - brake;
			#if !MOBILE_INPUT
			float handbrake = TeamUtility.IO.InputManager.GetAxis("Jump", playerId);
			m_Car.Move(h, v, v, handbrake);
			#else
			m_Car.Move(h, v, v, 0f);
			#endif*/
		}
	}
}