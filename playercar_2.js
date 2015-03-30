//car physics calculations / input stuff
private var accel : Vector3;
public var throttle: float ;
private var deadZone : float =.001;
private var myRight : Vector3;
private var velo : Vector3;
private var flatVelo : Vector3;
private var relativeVelocity : Vector3;
private var dir : Vector3;
private var flatDir : Vector3;
private var carUp : Vector3;
private var carTransform : Transform;
private var carRigidbody :  Rigidbody;
private var engineForce : Vector3;

private var turnVec : Vector3;
private var imp : Vector3 ;
private var rev : float;
private var actualTurn : float;
private var carMass : float;
private var wheelTransform : Transform[] = new Transform [4];
public var actualGrip: float;
public var horizontal:float;
private var maxSpeedToTurn:float=.2;

//the phisical transforms for the car s wheel
public var frontLeftWheel: Transform;
public var frontRightWheel: Transform;
public var rearLeftWheel: Transform;
public var rearRightWheel: Transform;

public var frontleftcol: WheelCollider;
public var frontrightcol: WheelCollider;

//these transform parents will allow wheels to turn for steering / separates steering turn from acceleration
public var LFWheelTransform : Transform;
public var RFWheelTransform : Transform;

// car physics adjustments
public var power : float = 300;
public var maxSpeed: float=50;
public var carGrip : float=70;
public var turnSpeed : float=3.0;

private var slideSpeed:float;
public var mySpeed:float;

private var carRight: Vector3;
private var carFwd: Vector3;
private var tempVEC: Vector3;



//Speedometer
var mphDisplay : GUIText;
var speedometerdial : Texture2D;
var speedometeraiguille : Texture2D;
public var camera2 : Camera;
//Ajustement du speedometer suivant la taille de la fenetre
/*public var scaleOnRatio1 = new Vector2(0.1f, 0.1f);
private var myTrans:Transform;
private var widthHeightRatio:float;*/

function Start () 
{
initialize();
}

function initialize () 
{
//Cache reference to our car s transform 
carTransform =transform;
//Cache rigidbody for our car
carRigidbody = GetComponent.<Rigidbody>() ;
//Cache our vector up direction
carUp=carTransform.up;
//Cache the mass of our car
carMass = GetComponent.<Rigidbody>().mass;
//Cache the Forward World Vector for our car
carFwd =  Vector3.forward;
//Cache the Word Right Vector for our car
carRight = Vector3.right;
//Call to set up our wheels array
setUpWheels();
// we set a COG here and lower the center of mass to a 
//negative value in Y axis to prevent car from flipping over
carRigidbody.centerOfMass= Vector3(0,-0.7,.35);
}

function Update () 
{
//call the fonction to start processsing all vehicle physics
carPhysicsUpdate();

//call the function to see what input we are using and apply it
checkInput();

// Affichage numérique de la vitesse 
if(camera2.enabled == true)
{
var mph = GetComponent.<Rigidbody>().velocity.magnitude * 2.237;
mphDisplay.text = mph + " MPH";
}
}

function LateUpdate()
{
// this function makes the visual 3d wheels rotate and turn
rotateVisualWheels();

//this is where we send to a function to do engine sounds
engineSound();
}

function setUpWheels()
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

private var  rotationAmount : Vector3;

function rotateVisualWheels()
{
//front wheels visual rotation while steering the car 
LFWheelTransform.localEulerAngles.y = horizontal * 30;
RFWheelTransform.localEulerAngles.y = horizontal * 30;

rotationAmount = carRight * (relativeVelocity.z * 1.6 * Time.deltaTime * Mathf.Rad2Deg);

wheelTransform[0].Rotate(rotationAmount);
wheelTransform[1].Rotate(rotationAmount);
wheelTransform[2].Rotate(rotationAmount);
wheelTransform[3].Rotate(rotationAmount);
}

private var  deviceAccelerometerSensitivity : float = 2;//how sensitive our mobile acceleromter will be

function checkInput()
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
	
	for (var touch : Touch in Input.touches){
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
else if (Application.platform == RuntimePlatform.WindowsEditor || RuntimePlatform.WindowsWebPlayer || RuntimePlatform.WindowsPlayer )//this input is for the unity editor
//use the Keybord for all car input
	{
	horizontal= Input.GetAxis("Horizontal_2");
	throttle = Input.GetAxis("Vertical_2");
	if (Input.GetKey("z"))
	{
	frontleftcol.brakeTorque = frontrightcol.brakeTorque=100;
	}
	else
	{
	frontleftcol.brakeTorque = frontrightcol.brakeTorque=0;
	}
	}
}

function carPhysicsUpdate()
{
//grab all the physics info we need to calc everything
	myRight = carTransform.right;
	
	//find our velocity
	velo = carRigidbody.velocity;
	
	tempVEC= Vector3(velo.x,0,velo.z);
	
	//figure out our velocity without y movement - our flat velocity
	flatVelo = tempVEC;
	
	//find out which direction we are moving in
	dir = transform.TransformDirection (carFwd);
	
	tempVEC = Vector3(dir.x,0,dir.z);
	
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
	
	actualGrip = Mathf.Lerp(100,carGrip, mySpeed*0.02);
	imp = myRight * (- slideSpeed * carMass*actualGrip);
	
}

function slowVelocity()
{
	carRigidbody.AddForce (-flatVelo * 0.8);
}
//this controls the sounds of the engine audio bi adjusting the pitch of our sound file
function engineSound()
{
	GetComponent.<AudioSource>().pitch = 0.30 + mySpeed * 0.025;
	if (mySpeed > 30 )
	{
	GetComponent.<AudioSource>().pitch = 0.25 + mySpeed *0.015;
	}
	if (mySpeed > 40 )
	{
	GetComponent.<AudioSource>().pitch = 0.20 + mySpeed *0.013;
	}
	if (mySpeed > 49 )
	{
	GetComponent.<AudioSource>().pitch = 0.15 + mySpeed *0.011;
	}
	//ensure we dont exceed to crazy of a pitch by resetting it back to default 2
	if (GetComponent.<AudioSource>().pitch > 2.0 )
	{
	GetComponent.<AudioSource>().pitch = 2.0;
	}
}		

function FixedUpdate()
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

function OnGUI () 
{	
if(camera2.enabled == true)
{
GUI.DrawTexture(Rect(Screen.width-300,Screen.height-300,300,300),speedometerdial,ScaleMode.ScaleToFit, true);
var speedfactor:float = mySpeed/maxSpeed;
var rotationangle = Mathf.Lerp(0,240,speedfactor);
GUIUtility.RotateAroundPivot(rotationangle,Vector2(Screen.width-150,Screen.height-150));
GUI.DrawTexture(Rect(Screen.width-300,Screen.height-300,300,300),speedometeraiguille,ScaleMode.ScaleToFit, true);
}
}



