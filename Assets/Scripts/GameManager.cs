using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {

	public AudioSource audioPlayer; // to play things use this
	public AudioClip[] clip ;		//store clips on editor with that array
	public Button firstBut;			//start game butn
	public Button secondBut;		//Exit game butn
	#region private vars
	private static GameManager instance = null; 
	#endregion

	#region public vars
	public static GameManager Instance {
		get {
			if (instance == null)
				Debug.Log ("Error, GameManager does not exist. ¡Attach GameManager Script to a game object named GameManager!");
			return instance;
		} 
	}
	#endregion

	#region Unity Methods
	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
		DontDestroyOnLoad (this.gameObject);
		//get some components
		audioPlayer = instance.GetComponent<AudioSource> ();
		//set the values for audio source
		//clip = new AudioClip[3]; create the array in the editor
		audioPlayer.volume = 0.25f;
		audioPlayer.loop = true;
		audioPlayer.clip = clip[0];
		audioPlayer.Play ();
		//find the butons and add the functions to them
		firstBut = GameObject.Find ("NewGameButton").GetComponent<Button> ();
		secondBut = GameObject.Find ("SalirButton").GetComponent<Button> ();
		AddListener ();
	}
	void start(){
		Debug.Log (clip);
	}
	void Update(){
		if (!firstBut && SceneManager.GetActiveScene ().name == "MenuScene") { //bcause i was geting lots of errors on charging the menu i decided to do this checking, if so i charge some vars that are null after switching scenes
			Init ();
		}
	}
		
	void Init(){ //find some components and add stuff to them
//		secondBut = GameObject.Find ("SalirButton").GetComponent<Button> ();
		firstBut = GameObject.Find ("NewGameButton").GetComponent<Button> ();
		secondBut = GameObject.Find ("SalirButton").GetComponent<Button> ();
		AddListener ();
	}
	#endregion

	#region Game Application Managment METHODS
	// Use this for exit / usar esto para salir
	public void ExitGame(){
		Debug.Log ("Closing Application / Cerrando Aplicación");
//		if (UnityEditor.EditorApplication.isPlaying) UnityEditor.EditorApplication.isPlaying = false; //use this for editor / usar esto para el editor
		Application.Quit ();
	}
	// Use this to start the game / usar esto para empezar el juego
	public void NewGame(){
		GameManager.Instance.audioPlayer.PlayOneShot (GameManager.Instance.clip [5]);
		SceneManager.LoadSceneAsync ("NewGameScene");
		if (SceneManager.GetActiveScene ().name == "MenuScene") {
			audioPlayer.clip = clip [1];
			audioPlayer.Play ();
		}
	}
	// Use this to restart the current scene / usar esto para reiniciar la escena actual
	public void RestartScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		audioPlayer.clip = clip[1];
		audioPlayer.Play ();
	}
	//use this to go back to the menu
	public void Menu(){
		SceneManager.LoadScene ("MenuScene");
		//Init ();
		Debug.Log (SceneManager.GetActiveScene ().name);
	}
	//use this to point the end of the game
	public void GameOver(){
		SceneManager.LoadScene ("EndGameScene");
		audioPlayer.clip = clip[2];
		audioPlayer.Play ();
		GameManager.Instance.audioPlayer.PlayOneShot (GameManager.Instance.clip [4]);
	}
	//ad the functions to some buttons
	void AddListener() 
	{
		
		firstBut.onClick.RemoveAllListeners ();
		firstBut.onClick.AddListener (() => {
			NewGame();
		});
		secondBut.onClick.RemoveAllListeners ();
		secondBut.onClick.AddListener (() => {
			ExitGame();
		});
	}

	#endregion

}