using UnityEngine;
using System.Collections;

public class Readfile : MonoBehaviour {
	string lecturetext = "http://localhost/vitesse.txt";
	// Use this for initialization
	void Start () {
		StartCoroutine (readfile (lecturetext));
	}

	// Update is called once per frame
	void Update () {
	
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
		float[] floatArray = new float[1000] ;
		int j = 0;		
		/* split the text file by newline characters */
		string[] lineArray = w.text.Split("\n"[0]);
		
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
}
