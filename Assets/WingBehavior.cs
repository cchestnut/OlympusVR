using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class WingBehavior : MonoBehaviour {

	SteamVR_ControllerManager controllers;
	int leftIndex, rightIndex;
	float thrust = 1.5f;
	public float maxThrust = 50f;
	SteamVR_Controller.Device leftDevice, rightDevice;
	int cloudLevel = 0;
	public Text cloudText, fuelText, healthText;
	Rigidbody rb;
	GameObject camera;
	GameObject cloudSpawn;
	public int health;
	bool flightConfig = false;
	// Use this for initialization
/*	void Awake () {
		Debug.Log ("Made it!");
		Object[] obj = FindObjectsOfType (typeof(SteamVR_ControllerManager));
		if (obj.Length > 1) {
			Destroy (gameObject);
			Destroy (this);
		}
	}*/
	void Start () {
		controllers = gameObject.GetComponentInParent<SteamVR_ControllerManager> ();
		rb = gameObject.GetComponent<Rigidbody> ();
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		//future bewarned when tags became variables for MINE CONVENIENCE!
		//cloudText = (Text) GameObject..FindObjectsOfTag();
		//for(int 
		cloudSpawn = Resources.Load ("Cloud", typeof(GameObject)) as GameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (leftDevice == null || rightDevice == null) {
			leftIndex = (int) controllers.left.gameObject.GetComponent<SteamVR_TrackedObject> ().index;
			rightIndex = (int) controllers.right.gameObject.GetComponent<SteamVR_TrackedObject> ().index;
			if (leftIndex < 0 || rightIndex < 0)
				return;
			leftDevice = SteamVR_Controller.Input (leftIndex);
			rightDevice = SteamVR_Controller.Input (rightIndex);
			flightConfig = leftDevice.GetPress (SteamVR_Controller.ButtonMask.Trigger) &&
			rightDevice.GetPress (SteamVR_Controller.ButtonMask.Trigger);
			return;
		}
		flightConfig = leftDevice.GetPress (SteamVR_Controller.ButtonMask.Trigger) &&
			rightDevice.GetPress (SteamVR_Controller.ButtonMask.Trigger);
		if (flightConfig ) {
			rb.drag = 0;
			Thrust ();
		} else {
			rb.drag = .5f;
		}
	}

	void Thrust () {
		//GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		//Vector3 vel = new Vector3(gameObject.transform.forward.x, gameObject.transform
		//gameObject.GetComponent<Rigidbody> ().AddForce(camera.gameObject.transform.forward * thrust);
		
		//transform.position += camera.transform.forward * thrust;
		//if(rb.velocity.magnitude <= maxThrust)
			rb.velocity += camera.transform.forward * thrust;
	}

	void OnCollisionEnter (Collision col) {
		//if(col.gameObject.
		GameObject gobj = col.gameObject;

		if (gobj.name.Contains("Cloud")) {
			CloudManager share = gobj.GetComponent<CloudManager>() as CloudManager;
			if (share == null || !share.getClean ()) {
				return;
			}
			cloudLevel++;
			Debug.Log (cloudLevel);
			share.taint ();
			if (cloudText != null)
				cloudText.text = "Cloud: " + cloudLevel;
			GameObject obj = Instantiate (cloudSpawn,
				spawnHigher(transform.position), transform.rotation) as GameObject;
			/*obj.transform.localScale = new Vector3 (
				gameObject.transform.localScale.x / 1.2f,
				gameObject.transform.localScale.y,
				gameObject.transform.localScale.z / 1.2f);*/
			Debug.Log ("complete");
		} else {
			DeathCheck ();
		}

	}

	bool DeathCheck() {
		if (this.health < 0 || gameObject.transform.position.y <= -10) {
			return true;
		}
		return false;
	}

	Vector3 spawnHigher(Vector3 pos) {
		float x = pos.x + Random.value  * 10 + 40;
		float z = pos.z + Random.value  * 10 + 40;
		float y = pos.y + Random.value  * 10 + 100 * cloudLevel;
		return new Vector3 (x, y, z);
	}
}
