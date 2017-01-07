using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class RoadManager : MonoBehaviour {

	public GameObject startRoad;
	public GameObject roadsContainer;

	private Queue roadsQueue = new Queue();
	private GameObject[] availableRoads;
	private GameObject currentRoad;
	private int roadsCount = 0;
	const int pregeneratedRoads = 3;

	void Start()
	{
		availableRoads = Resources.LoadAll<GameObject>("Roads/Freeway/Sections");
		roadsQueue.Enqueue (startRoad);
		currentRoad = startRoad;
		for (int i = 0; i < pregeneratedRoads; ++i) {
			BuildRoad ();
		}

	}

	void OnEnable ()
	{
		EventManager.StartListening ("virtual_checkpoint_pass", BuildRoad);
	}

	void OnDisable ()
	{
		EventManager.StopListening ("virtual_checkpoint_pass", BuildRoad);
	}

	void BuildRoad ()
	{
		//Get new road
		GameObject currentRoadEndPoint = currentRoad.transform.FindChild ("EndPoint").gameObject;

		//Instantiate object at some place, like "I'm doing my best"
		int randomOffset = Random.Range (0, availableRoads.Length);
		GameObject newRoad = availableRoads [randomOffset];
		Vector3 spawnPosition = currentRoadEndPoint.transform.position;
		Quaternion spawnRotation = Quaternion.identity;
		GameObject createdRoad = Instantiate (newRoad, spawnPosition, spawnRotation) as GameObject;

		//Put at right position
		GameObject createdRoadStartPoint = createdRoad.transform.FindChild ("StartPoint").gameObject;
		createdRoad.transform.rotation = currentRoadEndPoint.transform.rotation * Quaternion.Inverse(createdRoadStartPoint.transform.rotation);
		createdRoad.transform.position = currentRoadEndPoint.transform.position - (createdRoadStartPoint.transform.position - createdRoad.transform.position);

		//Connect waypoints
		string[] lanes = new string[] {"LeftLane", "MiddleLane", "RightLane"};
		foreach (string lane in lanes) {
			Waypoint currentRoadLastWaypoint = currentRoad.transform.FindChild("Waypoints").FindChild("PlayerDirection").FindChild(lane).gameObject.GetComponent<WaypointsLane>().last;
			Waypoint createdRoadFirstWaypoint = createdRoad.transform.FindChild("Waypoints").FindChild("PlayerDirection").FindChild(lane).gameObject.GetComponent<WaypointsLane>().first;
			currentRoadLastWaypoint.next = createdRoadFirstWaypoint;
		}
		foreach (string lane in lanes) {
			Waypoint currentRoadFirstWaypoint = currentRoad.transform.FindChild("Waypoints").FindChild("PlayerOppositeDirection").FindChild(lane).gameObject.GetComponent<WaypointsLane>().first;
			Waypoint createdRoadLastWaypoint = createdRoad.transform.FindChild("Waypoints").FindChild("PlayerOppositeDirection").FindChild(lane).gameObject.GetComponent<WaypointsLane>().last;
			createdRoadLastWaypoint.next = currentRoadFirstWaypoint;
		}

		//Connect camera waypoints
		Waypoint currentCameraLastWaypoint = currentRoad.transform.FindChild("Waypoints").FindChild("Camera").gameObject.GetComponent<WaypointsLane>().last;
		Waypoint createdCameraFirstWaypoint = createdRoad.transform.FindChild("Waypoints").FindChild("Camera").gameObject.GetComponent<WaypointsLane>().first;
		currentCameraLastWaypoint.next = createdCameraFirstWaypoint;

		//Store road
		createdRoad.transform.parent = roadsContainer.transform;
		roadsQueue.Enqueue (createdRoad);
		currentRoad = createdRoad;

		++roadsCount;
		if (roadsCount > pregeneratedRoads + 5) {
			Destroy(roadsQueue.Dequeue() as GameObject);
		}
	}

}