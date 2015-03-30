using UnityEngine;
using System.Collections;

public class LoginMenu : MonoBehaviour {

	string loginURL = "http://127.0.0.1/login.php";
	string registerURL = "http://127.0.0.1/register.php";

	string userName = "";
	string passWord = "" ;
	string problem ="";
	
	void OnGUI () {
		GUI.Window (0, new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 3 - 30, Screen.height / 2 - 30), LoginWindow, "Login"); 
	}

	void LoginWindow (int windowID){
		GUI.Label (new Rect (140, 40, 130, 100), "******UserName******");
		userName = GUI.TextField (new Rect (25, 60, 375, 30), userName);
		GUI.Label (new Rect (140, 92, 130, 100), "******Password******");
		passWord = GUI.TextField (new Rect (25, 115, 375, 30), passWord);

		if (GUI.Button (new Rect (25, 160, 175, 50), "Login"))
						StartCoroutine (handleLogin (userName, passWord));
		if (GUI.Button (new Rect (225, 160, 175, 50), "Register"))
			StartCoroutine (handleRegister (userName, passWord));

		GUI.Label(new Rect(55,222,250,100),problem);

	}
	IEnumerator handleLogin(string userNamez, string passWordz){
		problem = "Checking username and password...";
		string login_URL = loginURL + "?username=" + userNamez + "&password=" + passWordz;
		WWW loginReader = new WWW (login_URL);
		yield return loginReader;

		if (loginReader.error != null) {
						problem = "Could not locate page";
				} else {
			if(loginReader.text == "right"){
				problem="logged in";
			}else{
				problem="invalid user/pass";
			}
		}
	}
	IEnumerator handleRegister(string userNamez, string passWordz){
		problem = "Checking username and password...";
		string register_URL = registerURL + "?username=" + userNamez + "&password=" + passWordz;
		WWW registerReader = new WWW (register_URL);
		yield return registerReader;
		
		if (registerReader.error != null) {
			problem = "Could not locate page";
		} else {
			if(registerReader.text == "registered"){
				problem="Registered";
			 
			}else{
				problem="Did not register, try with another user name";
			}
		}
	}
}
