using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

	#region public vars
	public static GameManager Instance {
		get {
			if (instance == null)
				Debug.Log ("Error, GameManager does not exist. ¡Attach GameManager Script to a game object named GameManager!");
			return instance;
		} 
	}
	#endregion

	#region private vars
	private static GameManager instance = null;
	#endregion

	#region Unity Methods
	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
		DontDestroyOnLoad (this.gameObject);
	}
	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {

	}
	#endregion

	#region Game Application Managment METHODS
	// Use this for exit / usar esto para salir
	public void ExitGame(){		
		Debug.Log ("Closing Application / Cerrando Aplicación");
		if (UnityEditor.EditorApplication.isPlaying) UnityEditor.EditorApplication.isPlaying = false; //use this for editor / usar esto para el editor
		Application.Quit ();
	}
	// Use this to start the game / usar esto para empezar el juego
	public void NewGame(){
		SceneManager.LoadScene ("NewGameScene");
	}
	// Use this to restart the current scene / usar esto para reiniciar la escena actual
	public void RestartScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
	public void Menu(){
		SceneManager.LoadScene ("MenuScene");
	}
	public void GameOver(){
		SceneManager.LoadScene ("EndGameScene");
	}
	#endregion
}
