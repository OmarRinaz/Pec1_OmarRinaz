using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	#region private vars
	#endregion
	#region public vars
	#endregion

	// Use this for initialization
	void Start () {
	
	}
	#region Game Application Managment METHODS

	// Use this for exit / usar esto para salir
	public void ExitGame(){		
		Debug.Log ("Closing Application / Cerrando Aplicación");
		if (UnityEditor.EditorApplication.isPlaying) UnityEditor.EditorApplication.isPlaying = false; //use this for editor / usar esto para el editor
		Application.Quit ();
	}

	#endregion

	// Update is called once per frame
	void Update () {
	
	}
}
