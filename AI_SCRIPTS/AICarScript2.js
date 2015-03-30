
var centerOfMass: Vector3;
var path: Array;
var pathGroup: Transform;
var maxSteer: float = 15.0;
var CC_ME_Wheel_FL : WheelCollider;
var CC_ME_Wheel_FR : WheelCollider;
var CC_ME_Wheel_BL : WheelCollider;
var CC_ME_Wheel_BR : WheelCollider;
var currentPathObj: int;
var distFromPath:float = 20;
var maxTorque:float = 50;
var currentSpeed:float;
var topSpeed : float = 150;
var decellarationSpeed : float=10; 

//Speedometer
var mphDisplay : GUIText;
var speedometerdial : Texture2D;
var speedometeraiguille : Texture2D;
public var camera3 : Camera;

function Start () {
GetComponent.<Rigidbody>().centerOfMass=centerOfMass;
GetPath();


}
function GetPath()
{
var path_objs :Array = pathGroup.GetComponentsInChildren(Transform);
path= new Array ();
	
for(var path_obj: Transform in path_objs)
{
	if (path_obj != pathGroup)
		path [path.length]= path_obj;
}

}
function Update () {
GetSteer();
Move();
}
function GetSteer(){
var steerVector : Vector3 = transform.InverseTransformPoint(Vector3 (path[currentPathObj].position.x,transform.position.y,path[currentPathObj].position.z));
var newSteer : float = maxSteer * (steerVector.x / steerVector.magnitude);
CC_ME_Wheel_FL.steerAngle=newSteer;
CC_ME_Wheel_FR.steerAngle=newSteer;

if(steerVector.magnitude <= distFromPath)
{	
currentPathObj++;
if(currentPathObj>=path.length)
	currentPathObj=0;
}
}
function Move(){
currentSpeed = 2*(22/7)*CC_ME_Wheel_BL.radius*CC_ME_Wheel_BL.rpm * 60 /1000;
currentSpeed = Mathf.Round (currentSpeed);
if(currentSpeed <=topSpeed)
{
CC_ME_Wheel_BL.motorTorque = CC_ME_Wheel_BR.motorTorque = maxTorque;
CC_ME_Wheel_BL.brakeTorque = CC_ME_Wheel_BR.brakeTorque = 0;

}
else
{
CC_ME_Wheel_BL.motorTorque = CC_ME_Wheel_BR.motorTorque = 0;
CC_ME_Wheel_BL.brakeTorque = CC_ME_Wheel_BR.brakeTorque = decellarationSpeed;
}

}
function OnGUI () 
{	
if(camera3.enabled == true)
{

GUI.DrawTexture(Rect(Screen.width-300,Screen.height-300,300,300),speedometerdial,ScaleMode.ScaleToFit, true);
var speedfactor:float = currentSpeed/topSpeed;
var rotationangle = Mathf.Lerp(0,240,speedfactor);
GUIUtility.RotateAroundPivot(rotationangle,Vector2(Screen.width-150,Screen.height-150));
GUI.DrawTexture(Rect(Screen.width-300,Screen.height-300,300,300),speedometeraiguille,ScaleMode.ScaleToFit, true);
//widthHeightRatio = Screen.width/Screen.height;
//Transform. = new Vector3 (scaleOnRatio1.x, widthHeightRatio * scaleOnRatio1.y, 1);
}
}