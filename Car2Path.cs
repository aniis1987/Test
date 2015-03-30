using UnityEngine;
using System.Collections;

public class Car2Path : MonoBehaviour {
	public Vector3 centerOfMass;
	public Transform[] path;
	public Transform pathGroup;
	public float maxSteer = 15.0f;
	public WheelCollider LF_WheelTransform;
	public WheelCollider RF_WheelTransform;
	public WheelCollider RL;
	public WheelCollider RR;
	public int currentPathObj;
	public float distFromPath = 20;
	public float maxTorque = 50;
	public float currentSpeed;
	public float topSpeed = 150;
	public float decellarationSpeed=10; 
	//Speedometer
	public GUIText mphDisplay;
	public Texture2D speedometerdial;
	public Texture2D speedometeraiguille;
	public Camera camera3;

	string lecturetext = "http://localhost/vitesse.txt";
	float[] floatArray;

	void  Start (){
		StartCoroutine (readfile (lecturetext));
		GetComponent<Rigidbody>().centerOfMass=centerOfMass;
		GetPath();
		
		
	}
	IEnumerator readfile(string url)
	{
		print("Loading");
		WWW w = new WWW(lecturetext);
		yield  return w;
		print(w.url);
		//System.IO.File.ReadAllText("C:\Users\anis\Desktop\bourse.txt");
		Debug.Log(w.text);
		
		/* create an array to store the floats we find in the file */

		int j = 0;		
		/* split the text file by newline characters */
		string[] lineArray = w.text.Split("\n"[0]);
		
		int tailtab = 0;
		//calcul lenght array
		foreach( string thisLine in lineArray ) {
			/* split each line by commas */
			string[] numberStrings =thisLine.Split(","[0]);
			/* loop over the numbers in this line */
			foreach( string thisNumber in numberStrings ) {
				tailtab++;
			}
		}
		floatArray= new float[tailtab];

		/* loop over each line in the file */
		foreach( string thisLine in lineArray ) {
			/* split each line by commas */
			string[] numberStrings =thisLine.Split(","[0]);
			/* loop over the numbers in this line */
			foreach( string thisNumber in numberStrings ) {
				/* parse the string into a float */
				float someFloat = float.Parse(thisNumber);
				print("Found this float: " + someFloat);
				/* put the float into an array you can use later */
				
				floatArray [j++]= someFloat  ;
			}
		}
		
		print("I found " + floatArray.Length + "numbers: ");
		
		for ( int i= 0; i < floatArray.Length ; i ++ ) {
			print(floatArray[i]);
		}
		
		/* convert the array to a builtin array for fun */
		
		//float[] fastFloatArray  = new float[floatArray.ToBuiltin(float)];    
	}



	void  GetPath (){
		Transform[] path_objs = pathGroup.GetComponentsInChildren<Transform>();
		path= new Transform[path_objs.Length-1];
		int i=0;
		foreach(Transform path_obj in path_objs)
		{
			if (path_obj != pathGroup)
				path [i++]= path_obj;
		}
		
	}
	void  Update (){
		GetSteer();
		StartCoroutine (Move());

		//Move();
	}
	void  GetSteer (){
		Vector3 steerVector = transform.InverseTransformPoint(new Vector3 (path[currentPathObj].position.x,transform.position.y,path[currentPathObj].position.z));
		float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
		LF_WheelTransform.steerAngle=newSteer;
		RF_WheelTransform.steerAngle=newSteer;
		
		if(steerVector.magnitude <= distFromPath)
		{	
			currentPathObj++;
			if(currentPathObj>=path.Length)
				currentPathObj=0;
		}
	}
	IEnumerator  Move (){
		for ( int i= 0; i < floatArray.Length-1 ; i ++ )
		{
			currentSpeed = floatArray[i];
			//currentSpeed = 2*(22/7)*RL.radius*RL.rpm * 60 /1000;
			//currentSpeed = Mathf.Round (currentSpeed);
			yield return new WaitForSeconds(1);
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
		
	}
	void  OnGUI (){	
		if(camera3.enabled == true)
		{
			
			GUI.DrawTexture( new Rect(Screen.width-300,Screen.height-300,300,300),speedometerdial,ScaleMode.ScaleToFit, true);
			float speedfactor = currentSpeed/240;
			float rotationangle= Mathf.Lerp(0,240,speedfactor);
			GUIUtility.RotateAroundPivot(rotationangle,new Vector2(Screen.width-150,Screen.height-150));
			GUI.DrawTexture( new Rect(Screen.width-300,Screen.height-300,300,300),speedometeraiguille,ScaleMode.ScaleToFit, true);
			//widthHeightRatio = Screen.width/Screen.height;
			//Transform. = new Vector3 (scaleOnRatio1.x, widthHeightRatio * scaleOnRatio1.y, 1);
		}
	}
	
}