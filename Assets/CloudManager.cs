using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CloudManager : MonoBehaviour {
	public int cloudLevel;
	public int share = 1;
	string message = "Cloud: ";
	string cloudName;
	public bool isClean = true;
	Vector3 rig;
	// Use this for initialization
	void Start () {
		/*rig = GameObject.Find ("Player").transform.position;
		if (rig != null && untainted)
			SpawnRandomHigher ( rig );*/
		//untainted = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FallFromSky() {
		GetComponent<Rigidbody> ().useGravity = true;
	}

	public void taint () {
		isClean = false;
		Debug.Log("tainted " + isClean);
	}

	public bool getClean() {
		return isClean;
	}

	void SpawnRandomHigher(Vector3 pos) {
		float x = pos.x + Random.value  * 10 + 40;
		float z = pos.z + Random.value  * 10 + 40;
		float y = pos.y + Random.value  * 10 + 100 * cloudLevel;
		gameObject.transform.position = new Vector3 (x, y, z);

	}


}
