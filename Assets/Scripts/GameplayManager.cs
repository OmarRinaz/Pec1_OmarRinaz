using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameplayManager : MonoBehaviour {
	#region public vars
	public class QuestionAwnser {
		public string[] questions;																				//Posibles preguntas
		public string awnsers;																					//Respuestas posibles
	}
	;
	public string[] awnserParse;
	#endregion

	#region private vars
	private QuestionAwnser current;																				//Conjunto de preguntas/respuesta actual
	private List<GameplayManager.QuestionAwnser> qaList = new List<GameplayManager.QuestionAwnser> ();			//Listado de preguntas/respuesta restante
	private List<GameplayManager.QuestionAwnser> qaListFull = new List<GameplayManager.QuestionAwnser> ();		//backup del listado completo en caso de que se vuelva a empezar la partida.
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
	// Use this for initialization
	void Start () {
		//qaListFull = StoryFiller.FillList ();
		qaListFull = StoryFiller.FillListFromXml();
		qaList.AddRange (qaListFull);
		awnserParse = new string[qaListFull.Count + 1];
		uiManager = UiManager.Instance;
		for (int i = 0; i < qaListFull.Count; i++) {
			awnserParse[i] = qaListFull[i].awnsers;
		}
		uiManager.Init (awnserParse);
		FillCurrent ();
	}
	#endregion
	#region Private Methods
	private void FillCurrent(){
		try {
			int position = Random.Range (0, qaList.Count);
			Debug.Log (qaList.Count + "\t" + qaListFull.Count + "\t" + "wins"+wins+"   loses"+losses );
			current = qaList[position];
			qaList.RemoveAt (position);
			currentCorrectAwnser = current.awnsers;
			uiManager.currentQuestion.text = current.questions [Random.Range (0, 1)]; //send current question to renew question ui
			FillUI ();
		}
		catch(System.Exception e){
			Debug.LogError (e.Message); //what to do if try dosenot work
		}
	}
	private void FillUI(){
		//send current question to renew question ui
		uiManager.RenewUi(awnserParse, currentCorrectAwnser);
	}
	private void CheckRound(bool correct){
		if (correct) {
			wins++;
			if (wins == 2) {
				//you win that fighter charge the new one
				//also renew the list from the backup
				qaList.Clear();
				qaList.AddRange (qaListFull);
				//show new question.
				FillCurrent();
				//show the fighter win screen
				wins=0;
				uiManager.ShowScreenSM(3);
			} else {
				//show the win round screen.
				FillCurrent();
				uiManager.ShowScreenSM(1);
			}
		}else{
			losses++;
			if (losses == 2) {
				//you lose the game
				//charge the Game Over scene
				GameManager.Instance.GameOver();
			} else {
				FillCurrent();
				//show lose round screen
				uiManager.ShowScreenSM(2);
			
			}
		}
	}

	private void backupList(){

	}
	#endregion
	#region Public Methods
	public void CheckAwnser(Text text){ //Usar esto para verificar si el jugador acerto. !rellenar los valores en el editor de unity¡
		totalRounds++;
		if(text.text.Equals(currentCorrectAwnser)){
			Debug.Log("Correcto");
			CheckRound (true);
		}else{
			Debug.Log("Incorrecto");
			CheckRound (false);
		}
	}
	public void ContiuneButton(GameObject activePanel){ //!rellenar los valores en el editor de unity¡
		uiManager.ShowScreenSM(4,activePanel);
	}
	#endregion
}
