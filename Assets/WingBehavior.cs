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
    GameObject bludgerSpawn;
	Transform tranny;
	public double health = 500;
	public int fuel = 1000;
	bool flightConfig = false;
    //string[] highScoreNames = new string[10];
    //int[] highScoreVals = new int[10];
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
		Debug.Log (Application.platform);
		controllers = gameObject.GetComponentInParent<SteamVR_ControllerManager> ();
		rb = gameObject.GetComponent<Rigidbody> ();
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		//future bewarned when tags became variables for MINE CONVENIENCE!
		//cloudText = (Text) GameObject..FindObjectsOfTag();
		//for(int 
		cloudSpawn = Resources.Load ("Cloud", typeof(GameObject)) as GameObject;
        bludgerSpawn = Resources.Load("Bludger", typeof(GameObject)) as GameObject;
 		tranny = gameObject.transform;
        //LoadHighScores();

	}

   /* void LoadHighScores()
    {
        for(int i =0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey(i + "HScore") && PlayerPrefs.HasKey(i + "HScoreName"))
            {
                highScoreNames[i] = PlayerPrefs.GetString(i + "HScoreName");
                highScoreVals[i] = PlayerPrefs.GetInt(i + "HScore");
            }
        }
    }*/
	
	// Update is called once per frame
	void Update () {
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
		if (flightConfig && fuel > 0) {
			rb.drag = 0;
			Thrust ();
		} else if (fuel <= 0) {
			rb.velocity = new Vector3 (
				rb.velocity.x,
				rb.velocity.y - 1,
				rb.velocity.z
			);
		} else {
			rb.drag = .5f;
			//Invoke ("Gravity", .5f);
		}
		if (DeathCheck ()) {
			Invoke ("Death", 3f);
		}
	}

	void Gravity () {
		//rb.drag = 0;
		//rb.velocity.y -= camera.transform.up.y;
	}

	void Thrust () {
		//GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
		//Vector3 vel = new Vector3(gameObject.transform.forward.x, gameObject.transform
		//gameObject.GetComponent<Rigidbody> ().AddForce(camera.gameObject.transform.forward * thrust);
		
		//transform.position += camera.transform.forward * thrust;
		//if(rb.velocity.magnitude <= maxThrust)
			rb.velocity += camera.transform.forward * thrust;
		fuel--;
		fuelText.text = "Fuel: " + fuel;
	}

	void OnCollisionEnter (Collision col) {
		//if(col.gameObject.
		GameObject gobj = col.gameObject;

		if (gobj.name.Contains("Cloud")) {
			GetComponent<AudioSource> ().Play ();
			CloudManager share = gobj.GetComponent<CloudManager>() as CloudManager;
			if (share == null || !share.getClean ()) {
				return;
			}
			AudioSource muteButton = gobj.GetComponent<AudioSource> () as AudioSource;
			muteButton.mute = true;
			cloudLevel++;
			Debug.Log (cloudLevel);
			share.taint (); 
			if (cloudText != null)
				cloudText.text = "Cloud: " + cloudLevel;

            
			GameObject obj = Instantiate (cloudSpawn,
				spawnHigher(transform.position), transform.rotation) as GameObject;
			obj.transform.localScale = shrink (obj.transform.localScale);
            if (cloudLevel >= 3)
            {
                (Instantiate(bludgerSpawn,
                    new Vector3(camera.transform.position.x, camera.transform.position.y,
                    camera.transform.position.z + 10), 
                    camera.transform.rotation) 
                    as GameObject).GetComponent<BludgerScript>()
                    .setHomeCloud(gobj);
            }
        } else if (gobj.name.Contains ("Terrain")) {
			Debug.Log (col.relativeVelocity.y);
			ReduceHealth (Mathf.Abs (col.relativeVelocity.y));
		}

	}

	Vector3 shrink (Vector3 oldPos) {
		return new Vector3 (
			oldPos.x / (cloudLevel+1),
			oldPos.y,
			oldPos.z / (cloudLevel+1)
		);
	}


	void OnCollisionStay (Collision col) {
		GameObject gobj = col.gameObject;
		if (gobj.name.Contains ("Cloud")) {
			fuel++;
			fuelText.text = "Fuel: " + fuel;
		} else if (gobj.name.Contains ("Terrain")) {
			StartCoroutine (ReduceHealth(1));
		} else if (gobj.name.Contains("Bludger"))
        {
            StartCoroutine(ReduceHealth(5));
        }
	}

	IEnumerator ReduceHealth(double damage) {
		health -= damage;
		healthText.text = "Health: " + health;
		yield return new WaitForSeconds(3f);
	}

	void Death() {
        //CheckHighScores ();
        //DisplayHighScores ();
        PlayerPrefs.SetInt("playerScore", cloudLevel);
		Application.LoadLevel ("HighScore");
	}

	/*void CheckHighScores () {
        //do check
        if (isNewHighScore(cloudLevel))
        {
           // Application.LoadLevel(Application.)
           //load high score level
        }
	}

    bool isNewHighScore(int cloudLevel)
    {
        int newScore, oldScore;
        string newName, oldName;
        bool result = false;
        newScore = cloudLevel;
        newName = "BOB";
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey(i + "HScore"))
            {
                if (PlayerPrefs.GetInt(i + "HScore") < newScore)
                {
                    // new score is higher than the stored score
                    oldScore = PlayerPrefs.GetInt(i + "HScore");
                    oldName = PlayerPrefs.GetString(i + "HScoreName");
                    PlayerPrefs.SetInt(i + "HScore", newScore);
                    PlayerPrefs.SetString(i + "HScoreName", newName);
                    newScore = oldScore;
                    newName = oldName;
                    result = true;
                }
            }
            else
            {
                PlayerPrefs.SetInt(i + "HScore", newScore);
                PlayerPrefs.SetString(i + "HScoreName", newName);
                newScore = 0;
                newName = "";
            }
        }
        return result;
    }

	void DisplayHighScores(){
	}*/

	bool DeathCheck() {
		if (this.health <= 0 || tranny.position.y <= -10) {
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
