using UnityEngine;
using System.Collections;

public class ControlCameras : MonoBehaviour {

	public Camera camera1, camera2, camera3, camera4;
	public AudioListener micro1,micro2, micro3, micro4;
	int startCamera= 1;

	// Use this for initialization
	void Start () {
		camera1.enabled = true;
		camera2.enabled = false;
		camera3.enabled = false;
		camera4.enabled = false;
		micro1.enabled =true;
		micro2.enabled =false;
		micro3.enabled =false;
		micro4.enabled =false;
		
		startCamera = 1;

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("c") && (startCamera == 1))
		{
			startCamera = 2;
						
			camera1.enabled = false;
			camera2.enabled = true;
			camera3.enabled = false;
			camera4.enabled = false;
			micro1.enabled =false;
			micro2.enabled =true;
			micro3.enabled =false;
			micro4.enabled =false;
		}
		else if (Input.GetKeyDown ("c") && (startCamera == 2))
		{
			startCamera = 3;

			camera1.enabled = false;
			camera2.enabled = false;
			camera3.enabled = true;
			camera4.enabled = false;
			micro1.enabled =false;
			micro2.enabled =false;
			micro3.enabled =true;
			micro4.enabled =false;
		}
		else if (Input.GetKeyDown ("c") && (startCamera == 3))
		{
			startCamera = 4;
				
			camera1.enabled = false;
			camera2.enabled = false;
			camera3.enabled = false;
			camera4.enabled = true;
			micro1.enabled =false;
			micro2.enabled =false;
			micro3.enabled =false;
			micro4.enabled =true;
		}
		else if (Input.GetKeyDown ("c") && (startCamera == 4))
		{
			startCamera = 1;

			camera1.enabled = true;
			camera2.enabled = false;
			camera3.enabled = false;
			camera4.enabled = false;
			micro1.enabled =true;
			micro2.enabled =false;
			micro3.enabled =false;
			micro4.enabled =false;
		}
	}
}
