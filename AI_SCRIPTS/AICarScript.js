
var centerOfMass: Vector3;
public var path: Array;
var pathGroup: Transform;
var maxSteer: float = 15.0;
var LF_WheelTransform : WheelCollider;
var RF_WheelTransform : WheelCollider;
var RL : WheelCollider;
var RR : WheelCollider;
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
public var camera4 : Camera;
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
LF_WheelTransform.steerAngle=newSteer;
RF_WheelTransform.steerAngle=newSteer;

if(steerVector.magnitude <= distFromPath)
{	
currentPathObj++;
if(currentPathObj>=path.length)
	currentPathObj=0;
}
}
function Move(){
currentSpeed = 2*(22/7)*RL.radius*RL.rpm * 60 /1000;
currentSpeed = Mathf.Round (currentSpeed);
if(currentSpeed <=topSpeed)
{
RL.motorTorque = RR.motorTorque =maxTorque;
RL.brakeTorque = RR.brakeTorque = 0;
}
else
{
RL.motorTorque = RR.motorTorque = 0;
RL.brakeTorque = RR.brakeTorque = decellarationSpeed;
}

}
function OnGUI () 
{	
if(camera4.enabled == true)
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
