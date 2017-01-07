using UnityEngine;
using System.Collections;

public class RoadCheckPoint : MonoBehaviour
{
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "RoadBuildTrigger") {
			EventManager.TriggerEvent ("virtual_checkpoint_pass");
			Destroy (this);
		}
	}
}
