using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour {

	public AudioSource audioPlayer;
	public AudioClip[] clip ;
	private Button firstBut;
	private Button secondBut;
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
		audioPlayer = instance.GetComponent<AudioSource> ();
		//clip = new AudioClip[3]; create the array in the editor
		audioPlayer.volume = 0.25f;
		audioPlayer.loop = true;
		audioPlayer.clip = clip[0];
		audioPlayer.Play ();
		firstBut = GameObject.Find ("NewGameButton").GetComponent<Button> ();

		AddListener ();
	}
	void start(){
//		string filePath =Application.dataPath+"Song1";             //CANT LOAD OGG FILES
//		clip[0] = (AudioClip)Resources.Load<AudioClip>("Song1") ;
//		clip[0] = Resources.Load<AudioClip>("Song1") ;
//		clip[0] = Resources.Load("Song1")as AudioClip ;

		Debug.Log (clip);
	}
	void Init(){
//		secondBut = GameObject.Find ("SalirButton").GetComponent<Button> ();
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
		audioPlayer.clip = clip[1];
		audioPlayer.Play ();
	}
	// Use this to restart the current scene / usar esto para reiniciar la escena actual
	public void RestartScene(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		audioPlayer.clip = clip[1];
		audioPlayer.Play ();
	}
	public void Menu(){
		SceneManager.LoadScene ("MenuScene");
		firstBut = GameObject.Find ("NewGameButton").GetComponent<Button> ();

		Init ();

	}
	public void GameOver(){

		SceneManager.LoadScene ("EndGameScene");

		audioPlayer.clip = clip[2];
		audioPlayer.Play ();
		GameManager.Instance.audioPlayer.PlayOneShot (GameManager.Instance.clip [4]);
	
	}
	void AddListener() 
	{
		secondBut = GameObject.Find ("SalirButton").GetComponent<Button> ();
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