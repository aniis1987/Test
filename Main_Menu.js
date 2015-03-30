#pragma strict
var state=0;
var LonguitudeAccident:String;
var LatitudeAccident:String;
var LonguitudeVehicule1:String;
var LatitudeVehicule1:String;
var LonguitudeVehicule2:String;
var LatitudeVehicule2:String;
var vehicule1: String;
var vehicule2: String;
function Start () {

}

function Update () {
	if (state==3)
		{
		Application.Quit();
		}
	
}
function OnMouseEnter () {
	GetComponent.<Renderer>().material.color=Color.green;
}
function OnMouseExit () {
	GetComponent.<Renderer>().material.color=Color.red;
}

function OnGUI () 
{	// Menu Play
	if(state == 0)
	{
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-100,100,30),"PLAY"))
		{
		state=1;
		Application.LoadLevel("VTADS");
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-60,100,30),"Configuration"))
		{
		state=2;
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-20,100,30),"EXIT"))
		{
		state=3;
		Application.Quit();
		}
	}
	
	// Sous menu 1 "Configuration"
	if(state == 2)
	{
		if(GUI.Button(new Rect(Screen.width/2-160, Screen.height/2-180,320,30),"The geographical coordinates of the first vehicle"))
		{
		state=21;
		}
		if(GUI.Button(new Rect(Screen.width/2-160, Screen.height/2-140,320,30),"The geographical coordinates of the second vehicle"))
		{
		state=22;
		}
		if(GUI.Button(new Rect(Screen.width/2-160, Screen.height/2-100,320,30),"The Geographical coordinates of the accident"))
		{
		state=23;
		}
		if(GUI.Button(new Rect(Screen.width/2-160, Screen.height/2-60,320,30),"Type of Vehicle 1"))
		{
		state=24;
		}
		if(GUI.Button(new Rect(Screen.width/2-160, Screen.height/2-20,320,30),"Type of Vehicle 2"))
		{
		state=25;
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2+20,100,30),"BACK"))
		{
		state=0;
		}
	}
		// Sous menu 2 "Coordonnées géographique de la premiere vehicule 
	if(state == 21)
	{
		GUI.Label (new Rect (Screen.width/2-125, Screen.height/2-100,250,30), "******Longuitude Vehicule1******");
		LonguitudeVehicule1 = GUI.TextField (new Rect (Screen.width/2+100, Screen.height/2-100,150,30), LonguitudeVehicule1);
		GUI.Label (new Rect (Screen.width/2-125, Screen.height/2-70,250,30), "******Latitude Vehicule1******");
		LatitudeVehicule1 = GUI.TextField (new Rect (Screen.width/2+100, Screen.height/2-70,150,30), LatitudeVehicule1);
		if (GUI.Button(new Rect(Screen.width/2-75, Screen.height/2-40,100,30),"BACK"))
			{
			state=2;
			}
		if (GUI.Button(new Rect(Screen.width/2+75, Screen.height/2-40,100,30),"OK"))
			{
			state=0;
			}
	}
	
		// Sous menu 2 "Coordonnées géographique de la deuxieme vehicule 
	if(state == 22)
	{
		GUI.Label (new Rect (Screen.width/2-125, Screen.height/2-100,250,30), "******Longuitude Vehicule2******");
		LonguitudeVehicule2 = GUI.TextField (new Rect (Screen.width/2+100, Screen.height/2-100,150,30), LonguitudeVehicule2);
		GUI.Label (new Rect (Screen.width/2-125, Screen.height/2-70,250,30), "******Latitude Vehicule2******");
		LatitudeVehicule2 = GUI.TextField (new Rect (Screen.width/2+100, Screen.height/2-70,150,30), LatitudeVehicule2);
		if (GUI.Button(new Rect(Screen.width/2-75, Screen.height/2-40,100,30),"BACK"))
			{
			state=2;
			}
		if (GUI.Button(new Rect(Screen.width/2+75, Screen.height/2-40,100,30),"OK"))
			{
			state=0;
			}
	}
	// Sous menu 2 "Coordonnées géographique de l'accident"
	if(state == 23)
	{
		GUI.Label (new Rect (Screen.width/2-125, Screen.height/2-100,250,30), "******Longuitude Accident******");
		LonguitudeAccident = GUI.TextField (new Rect (Screen.width/2+100, Screen.height/2-100,150,30), LonguitudeAccident);
		GUI.Label (new Rect (Screen.width/2-125, Screen.height/2-70,250,30), "******Latitude Accident******");
		LatitudeAccident = GUI.TextField (new Rect (Screen.width/2+100, Screen.height/2-70,150,30), LatitudeAccident);
		if (GUI.Button(new Rect(Screen.width/2-75, Screen.height/2-40,100,30),"BACK"))
			{
			state=2;
			}
		if (GUI.Button(new Rect(Screen.width/2+75, Screen.height/2-40,100,30),"OK"))
			{
			state=0;
			}
	}
	// Sous menu 2 "Type de véhicule"
	if(state == 24)
	{
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-100,100,30),"Truck"))
		{
		vehicule1="Truck";
		state=0;
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-60,100,30),"Car"))
		{
		vehicule1="Car";
		state=0;
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-20,100,30),"BACK"))
		{
		state=2;
		}
		
	}
	if(state == 25)
	{
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-100,100,30),"Truck"))
		{
		vehicule2="Truck";
		state=0;
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-60,100,30),"Car"))
		{
		vehicule2="Car";
		state=0;
		}
		if(GUI.Button(new Rect(Screen.width/2-50, Screen.height/2-20,100,30),"BACK"))
		{
		state=2;
		}
	}






}