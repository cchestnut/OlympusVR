using UnityEngine;
using System.Collections;

public class DetermineHeadset : MonoBehaviour {

	GameObject instance;
	// Use this for initialization
	void Start () {
		Debug.Log ("happening");
		Object[] obj = FindObjectsOfType (typeof(SteamVR_ControllerManager));
		if (obj.Length >= 1) {
			return;
			//instance = Instantiate (Resources.Load ("AltFlyingPrefab", typeof(GameObject))) as GameObject;
		} else {
			instance = Instantiate (Resources.Load ("FlyingPrefab", typeof(GameObject))) as GameObject;
		}
		gameObject.transform.parent = instance.transform;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
