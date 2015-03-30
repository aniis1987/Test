public var camera1 : Camera;
public var camera2 : Camera;
public var camera3 : Camera;
public var camera4 : Camera;

public var micro1 : AudioListener;
public var micro2 : AudioListener;
public var micro3 : AudioListener;
public var micro4 : AudioListener;

public var startCamera:int=1;
public var startAudioListener:int=1;
// affichage du temps	
var timer : GUIText;
function Start () 
{
camera1.enabled = true;
camera2.enabled = false;
camera3.enabled = false;
camera4.enabled = false;
micro1.enabled =true;
micro2.enabled =false;
micro3.enabled =false;
micro4.enabled =false;

startCamera = 1;
startAudioListener = 1;
}
function Update () 
{
// affichage du temps	
timer.text = Time.time.ToString();


if (Input.GetKeyDown ("c") && (startCamera == 1))
{
startCamera = 2;
startAudioListener = 2;

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
startAudioListener = 3;
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
startAudioListener = 4;

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
startAudioListener = 1;

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



