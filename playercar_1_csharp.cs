using UnityEngine;
using System.Collections;

public class playercar_1_csharp : MonoBehaviour {

	//variables declaration used in  carPhysicsUpdate function
	private Vector3 myRight;
	private Vector3 velo;
	private Vector3 flatVelo;
	private Vector3 relativeVelocity;
	private Vector3 dir;
	private Vector3 flatDir;
	private Vector3 carUp;
	private Transform carTransform;
	private Rigidbody carRigidbody;
	private Vector3 engineForce;
	private float slideSpeed;
	public float mySpeed;
	private float rev;
	private float actualTurn;
	public float turnSpeed=0.30f;
	private float carMass;
	private Vector3 tempVEC;
	public float power= 300;
	public float throttle;
	public float horizontal;
	private Vector3 turnVec;
	public float actualGrip;
	public float carGrip=70;
	private Vector3 imp;
	private Vector3 carFwd;

	//variables declaration used in  checkinput function
	private Vector3 accel;
	private float  deviceAccelerometerSensitivity= 2f;
	private float deadZone =0.001f;
	public WheelCollider frontleftcol;
	public WheelCollider frontrightcol;

	//the phisical transforms for the car s wheel
	public Transform frontLeftWheel;
	public Transform frontRightWheel;
	public Transform rearLeftWheel;
	public Transform rearRightWheel;
	private Transform[] wheelTransform = new Transform [4];

	//these transform parents will allow wheels to turn for steering / separates steering turn from acceleration
	public Transform LFWheelTransform ;
	public Transform RFWheelTransform ;

	//Speedometer
	public GUIText mphDisplay;
	public Texture2D speedometerdial;
	public Texture2D speedometeraiguille;
	public Camera camera1 ;

	private Vector3 carRight;
	private Vector3  rotationAmount ;
	public float maxSpeed =5f;
	private float maxSpeedToTurn=0.02f;
	private float	rotationangle;


	// Use this for initialization
	void Start () {
		//Cache reference to our car s transform 
		carTransform =transform;
		//Cache rigidbody for our car
		carRigidbody = GetComponent<Rigidbody>() ;
		//Cache our vector up direction
		carUp=carTransform.up;
		//Cache the mass of our car
		carMass = GetComponent<Rigidbody>().mass;
		//Cache the Forward World Vector for our car
		carFwd =  Vector3.forward;
		//Cache the Word Right Vector for our car
		carRight = Vector3.right;
		//Call to set up our wheels array
		setUpWheels();
		// we set a COG here and lower the center of mass to a 
		//negative value in Y axis to prevent car from flipping over
		carRigidbody.centerOfMass= new Vector3(0,-0.7f,.35f);
		}
	
	// Update is called once per frame
	void Update () 
		{
		//call the fonction to start processsing all vehicle physics
		carPhysicsUpdate();
		//call the function to see what input we are using and apply it
		checkInput();
		// Affichage numérique de la vitesse 
		/*if(camera1.enabled == true)
		{
			var mph = rigidbody.velocity.magnitude * 2.237;
			mphDisplay.text = mph + " MPH";
		}*/
		}


	public void LateUpdate()
	{
		// this function makes the visual 3d wheels rotate and turn
		rotateVisualWheels();
		
		//this is where we send to a function to do engine sounds
		engineSound();
	}
	public void setUpWheels()
	{
		if ((null==frontLeftWheel || null==frontRightWheel || null==rearLeftWheel || null==rearRightWheel))
		{
			Debug.LogError("One or more of the wheel transforms have not been plugged in on the car ");
			Debug.Break();
		}
		else
		{
			//set up the car s wheel transforms
			wheelTransform[0] = frontLeftWheel;
			wheelTransform[1] = rearLeftWheel;
			wheelTransform[2] = frontRightWheel;
			wheelTransform[3] = rearRightWheel;	
		}
	}

	/*/public void tableau () {
		GUI.Window (0, new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 3 - 30, Screen.height / 2 - 30), LoginWindow, "Login"); 
	}*/
	
	public void rotateVisualWheels()
	{
		//front wheels visual rotation while steering the car 

		Vector3 temp0 = LFWheelTransform.localEulerAngles;
		Vector3 temp1 = RFWheelTransform.localEulerAngles;

		temp0.y = horizontal * 30.0f;
		temp1.y = horizontal * 30.0f;
		LFWheelTransform.localEulerAngles = temp0;
		RFWheelTransform.localEulerAngles = temp1;

		rotationAmount = carRight *(relativeVelocity.z * 1.6f * Time.deltaTime * Mathf.Rad2Deg);
		
		wheelTransform[0].Rotate(rotationAmount);
		wheelTransform[1].Rotate(rotationAmount);
		wheelTransform[2].Rotate(rotationAmount);
		wheelTransform[3].Rotate(rotationAmount);
	}


	public void checkInput()
	{
		//Mobile platform turning input ... testing to see if we are on a mobile device.
		if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			//we give the acceleration a little boost to make turning more sensitive
			accel = Input.acceleration * deviceAccelerometerSensitivity;
			
			if(accel.x > deadZone || accel.x < -deadZone ){
				horizontal = accel.x ;
			}
			else
			{
				horizontal = 0 ;
			}
			throttle = 0 ;
			
			foreach (Touch touch in Input.touches){
				if (touch.position.x > Screen.width -Screen.width/3 && touch.position.y < Screen.height/3)
				{
					throttle =1;
				}
				else if (touch.position.x > Screen.width/3 && touch.position.y < Screen.height/3)
				{
					throttle =-1;
				}
			}
		}
		else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.WindowsPlayer )//this input is for the unity editor
			//use the Keybord for all car input
		{
			horizontal= Input.GetAxis("Horizontal");
			throttle = Input.GetAxis("Vertical");
			if (Input.GetKey("space"))
			{
				frontleftcol.brakeTorque = frontrightcol.brakeTorque=100;
			}
			else 
			{
			frontleftcol.brakeTorque = frontrightcol.brakeTorque=0;
			}
		}
	}	


	public void carPhysicsUpdate()
		{
		//grab all the physics info we need to calc everything
		myRight = carTransform.right;
		
		//find our velocity
		velo = carRigidbody.velocity;
		
		tempVEC= new Vector3(velo.x,0,velo.z);
		
		//figure out our velocity without y movement - our flat velocity
		flatVelo = tempVEC;
		
		//find out which direction we are moving in
		dir = transform.TransformDirection (carFwd);
		
		tempVEC = new Vector3(dir.x,0,dir.z);
		
		//calculate our direction removing y movement - our flat direction
		flatDir = Vector3.Normalize(tempVEC);
		
		//calculate relative Velocity
		relativeVelocity = carTransform.InverseTransformDirection(flatVelo);
		
		//calculate how much we are sliding (find out movement along our x axis)
		slideSpeed = Vector3.Dot(myRight,flatVelo);
		
		//calculate current speed (the magnitude of flat velocity)
		mySpeed = flatVelo.magnitude;
		
		//check to see if we are moving in reverse
		rev= Mathf.Sign(Vector3.Dot(flatVelo,flatDir));
		
		//calculate engine force with our flat diection vector and acceleration
		engineForce = (flatDir * (power * throttle) * carMass);
		
		//do turning
		actualTurn = horizontal;
		
		//if we are reverse we reverse the turning direction too
		if(rev < 0.1f)
		{
			actualTurn = -actualTurn;
		}
		
		//calculate torque for applying to our rigidbody
		turnVec = (((carUp * turnSpeed) * actualTurn ) * carMass) * 800;
		
		//calculate impulses to simulate grip by taking our right vector, reversing the slidespeed and
		//multiplying that by our mass, to give us a completely " corrected" force that would completely
		//stop sliding. we the nmultiply that by our grip amount ( Wich is,technically, a slide amount) which
		//reduces the corrected force so that it only helps to reduce sliding rather than completly
		//Stop it
		
		actualGrip = Mathf.Lerp(100,carGrip, mySpeed*0.02f);
		imp = myRight * (- slideSpeed * carMass*actualGrip);
		
	}
	public void slowVelocity()
	{
		carRigidbody.AddForce (-flatVelo * 0.8f);
	}
	//this controls the sounds of the engine audio bi adjusting the pitch of our sound file
	public void engineSound()
	{
		GetComponent<AudioSource>().pitch = 0.30f + mySpeed * 0.025f;
		if (mySpeed > 30 )
		{
			GetComponent<AudioSource>().pitch = 0.25f + mySpeed *0.015f;
		}
		if (mySpeed > 40 )
		{
			GetComponent<AudioSource>().pitch = 0.20f + mySpeed *0.013f;
		}
		if (mySpeed > 49 )
		{
			GetComponent<AudioSource>().pitch = 0.15f + mySpeed *0.011f;
		}
		//ensure we dont exceed to crazy of a pitch by resetting it back to default 2
		if (GetComponent<AudioSource>().pitch > 2.0f )
		{
			GetComponent<AudioSource>().pitch = 2.0f;
		}
		
		
	}	
	public void FixedUpdate()
	{
		if (mySpeed < maxSpeed)
		{
			//apply the engine force to rigidbody
			carRigidbody.AddForce (engineForce * Time.deltaTime);
		}
		//if we are going to slow to allow kart to rotate around 
		if(mySpeed > maxSpeedToTurn)
		{
			//apply torque to our rigidbody
			carRigidbody.AddTorque (turnVec * Time.deltaTime);
		}
		else if (mySpeed < maxSpeedToTurn)
		{
			return;
		}
		//apply forces to our rigidbody for grip 
		carRigidbody.AddForce(imp * Time.deltaTime);
		
	}
	
	/*IEnumerator  coordonnees () 
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break ;
		// Start service before querying location
		Input.location.Start ();
		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			print ("Timed out");
			yield break ;
		}
		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			print ("Unable to determine device location");
			yield break ;
		}
		// Access granted and location value could be retrieved
		else {
			print ("Location: " + Input.location.lastData.latitude + " " +
			       Input.location.lastData.longitude + " " +
			       Input.location.lastData.altitude + " " +
			       Input.location.lastData.horizontalAccuracy + " " +
			       Input.location.lastData.timestamp);
		}
		// Stop service if there is no need to query location updates continuously
		Input.location.Stop ();
	}*/
	
	// Affichage de la vitesse dur un Compteur (Speedomater)
	void  OnGUI () 
	{	
		if(camera1.enabled == true)
		{
			
			GUI.DrawTexture(new Rect(Screen.width-300,Screen.height-300,300,300),speedometerdial,ScaleMode.ScaleToFit, true);
			if(rotationAmount.x >= 0.0f)
			{
			float speedfactor = mySpeed/240;
			rotationangle = Mathf.Lerp(0,240,speedfactor);
			GUIUtility.RotateAroundPivot(rotationangle,new Vector2(Screen.width-150,Screen.height-150));
			GUI.DrawTexture(new Rect(Screen.width-300,Screen.height-300,300,300),speedometeraiguille,ScaleMode.ScaleToFit, true);
			}
			else
			{
			GUI.DrawTexture(new Rect(Screen.width-300,Screen.height-300,300,300),speedometeraiguille,ScaleMode.ScaleToFit, true);
			}


			//widthHeightRatio = Screen.width/Screen.height;
			//Transform. = new Vector3 (scaleOnRatio1.x, widthHeightRatio * scaleOnRatio1.y, 1);
		}
	}
}
