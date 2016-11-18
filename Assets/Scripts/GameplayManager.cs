using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameplayManager : MonoBehaviour {

	private static GameplayManager instance = null;

	#region public vars

	public static GameplayManager Instance {
		get {
			if (instance == null)
				Debug.Log ("Error, GamePlay Manager does not exist. ¡Attach GameManager Script to a game object named GameManager!");
			return instance;
		} 
	}

	public class QuestionAwnser {
		public string[] questions;																				//Posibles preguntas
		public string awnsers;																					//Respuestas posibles
	}

	public string[] awnserParse;
	#endregion

	#region private vars
	private QuestionAwnser current;																				//Conjunto de preguntas/respuesta actual
	private List<GameplayManager.QuestionAwnser> qaListAwnsering = new List<GameplayManager.QuestionAwnser> ();			//Listado de preguntas/respuesta restante
	private List<GameplayManager.QuestionAwnser> qaListAsking = new List<GameplayManager.QuestionAwnser> ();
	public List<GameplayManager.QuestionAwnser> qaListFull = new List<GameplayManager.QuestionAwnser> ();		//backup del listado completo en caso de que se vuelva a empezar la partida.
	private string currentCorrectAwnser;
	private int totalRounds=0;
	private int wins=0;
	private int losses=0;
	private UiManager uiManager;
	private GameObject winPanel;
	private GameObject winFighterPanel;
	private GameObject losePanel;
	private GameObject gameplayPanel;
	#endregion

	#region Unity Methods
	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);

		qaListFull = StoryFiller.FillListFromXml();
		qaListAwnsering.AddRange (qaListFull);
		qaListAsking.AddRange (qaListFull);

		awnserParse = new string[qaListFull.Count];

		uiManager = UiManager.Instance;

		for (int i = 0; i < qaListFull.Count; i++) {
			awnserParse[i] = qaListFull[i].awnsers;
		}
	
	}
	// Use this for initialization
	void Start () {
		//qaListFull = StoryFiller.FillList ();
		uiManager.Init ();
		FillCurrentAwnser ();

	}
	#endregion
	#region Private Methods
	private void FillCurrentAwnser(){

		int position = Random.Range (0, qaListAwnsering.Count);
		Debug.Log (qaListAwnsering.Count + "\t" + qaListFull.Count + "\t" + "wins"+wins+"   loses"+losses );
		current = qaListAwnsering[position];
		qaListAwnsering.RemoveAt (position);
		currentCorrectAwnser = current.awnsers;
		uiManager.currentQuestion.text = current.questions [Random.Range (0, 1)]; //send current question to renew question ui
		uiManager.RenewUiAwnsering(awnserParse, currentCorrectAwnser);
		try {

		}
		catch(System.Exception e){
			Debug.LogError (e.Message); //what to do if try dosenot work
		}
	}
	private void CheckRound(bool correct){
		if (correct) {
			wins++;
			if (wins == 2) {
				//you win that fighter charge the new one
				//also renew the list from the backup
				qaListAwnsering.Clear();
				qaListAwnsering.AddRange (qaListFull);
				//show new question.
				FillCurrentAwnser();
				//show the fighter win screen
				wins=0;
				uiManager.ShowScreenSM(3);
			} else {
				//show the win round screen.
				FillCurrentAwnser();
				uiManager.ShowScreenSM(1);
			}
		}else{
			losses++;
			if (losses == 2) {
				//you lose the game
				//charge the Game Over scene
				GameManager.Instance.GameOver();
			} else {
				FillCurrentAwnser();
				//show lose round screen
				uiManager.ShowScreenSM(2);
			
			}
		}
	}
	private void RoundWin(){
		//the machine starts aasking if you do correct then you ask, you allways score point for awnsering correct ot not being correctly awnsered
		//here now we need to charge varius questions, to put it in the anwsers buttons we have on ui manager

		//after the win round screen we actualize the ui
		uiManager.ShowScreenSM(1);
		string[] asking = new string[4];
		for(int i =0 ; i <asking.Length;i++){  //FLAG
			asking [i] = qaListFull [i].questions [Random.Range (0, 1)];
		}
		uiManager.RenewUiAsking (asking);


	}
	private void RoundLose(){
		//if you dont awnser correct, then the machine scores a point and asks again, if the machine gots it correct, then its his turn to ask, we go back to what we were doing
		FillCurrentAwnser();
	}
	private void backupList(){

	}
	#endregion
	#region Public Methods
	public void CheckAwnser(string text){ //Usar esto para verificar si el jugador acerto.
		if(text.Equals(currentCorrectAwnser)){
			Debug.Log("Correcto"+ text);
			//CheckRound (true); // WIP ROUNDWIN AND ROUNDLOSE SWITCH
			wins++;
			RoundWin ();
		}else{
			losses++;
			Debug.Log("Incorrecto"+ text);
			CheckRound (false);
		}
	}
	public void CheckAwnser(Text text){ //Usar esto para verificar si el jugador acerto. !rellenar los valores en el editor de unity¡
		totalRounds++;
		if(text.text.Equals(currentCorrectAwnser)){
			Debug.Log("Correcto"+ text.text);
			//CheckRound (true); // WIP ROUNDWIN AND ROUNDLOSE SWITCH
			RoundWin ();
		}else{
			Debug.Log("Incorrecto"+ text.text);
			CheckRound (false);
		}
	}
	public void CheckQuestion(string text){
		// track the correct awnser for the player question choice.
		int x = 0;
		for (int i=0; i < qaListFull.Count; i++){
			Debug.Log (i);
			if (qaListFull [i].questions [0] == text||qaListFull [i].questions [1] == text) {
				current = qaListFull [i];
				Debug.Log ("found it");
				x = Random.Range(0,3);
				break;
			} else {
				if (i == 9)
					Debug.Log ("wrong");
			}
		}
		//randomize a reponse for the question
		if (qaListFull [x].questions [0] == text||qaListFull [x].questions [1] == text) {
			//MACHINE SCORESSSS
			Debug.Log("MAchine Scoreeees");
			RoundLose ();
		} else {
			//WHAT A FAIL FROM THE MACHINE OMG!
			Debug.Log("MAchine faiiils"+"\t"+x+"\t"+text);
			RoundWin ();

		}
	}
	public void ContiuneButton(GameObject activePanel){ //!rellenar los valores en el editor de unity¡
		if (activePanel.name == "WinPanel") {
			uiManager.ShowScreenSM (5, activePanel);
		}else{
			uiManager.ShowScreenSM(4,activePanel);
		}

	}
	#endregion
}
