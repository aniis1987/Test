using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Configuration;

public class ReadFileServer : MonoBehaviour {
		private string url = "http://www.textfiles.com/100/adventur.txt";
	// Use this for initialization
	void Start () {
		StartCoroutine (readfile (url));
	}
	
	// Update is called once per frame
	void Update () {

	}
	IEnumerator readfile(string url1)
	{
		print("Loading");
		WWW w = new WWW(url);
		yield  return w;
		print(w.url);
		//System.IO.File.ReadAllText("C:\Users\anis\Desktop\bourse.txt");
		Debug.Log(w.text);
	}
}
