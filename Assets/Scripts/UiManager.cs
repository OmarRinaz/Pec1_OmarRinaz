using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UiManager: MonoBehaviour{

	private static UiManager instance = null;
	private GameObject winPanel;
	private GameObject winFighterPanel;
	private GameObject losePanel;
	private GameObject gameplayPanel;
	private GameObject[] buttonAwnser;
	private string awnserPath = "Canvas/GameplayPanel/AwnserGroup/AwnserButton";
	private GameplayManager gameplayManager;
	public Text currentQuestion;

	public static UiManager Instance {
		get {
			if (instance == null)
				Debug.Log ("Error, GameManager does not exist. ¡Attach GameManager Script to a game object named GameManager!");
			return instance;
		} 
	}

	void Awake(){
		instance = this;

		winPanel = GameObject.Find ("Canvas/WinPanel");
		winFighterPanel = GameObject.Find ("Canvas/WinFighterPanel");
		losePanel = GameObject.Find ("Canvas/LosePanel");
		gameplayPanel = GameObject.Find ("Canvas/GameplayPanel");
		currentQuestion = GameObject.Find ("Canvas/GameplayPanel/StoryGroup/MaskTextHistory/TextHistory").GetComponent<Text>();
		buttonAwnser = new GameObject[4];

	}

	public void Init(){
		gameplayManager = GameplayManager.Instance;
		if (gameplayManager) {
			Debug.Log ("ui exist");
		} else {
			Debug.Log ("ui exist no exist");
		}

		for (int i = 0; i < buttonAwnser.Length; i++) {
			buttonAwnser [i] = GameObject.Find (awnserPath + " (" + i + ")" + "/AwnserTextButton (" + i + ")");
			Button button = buttonAwnser [i].transform.parent.GetComponent<Button> ();
			AddListener (button, gameplayManager.awnserParse[i],true);
		}

		winPanel.SetActive (false);
		losePanel.SetActive (false);
		winFighterPanel.SetActive (false);

	}

	void Test(){
		
		Debug.Log ("Hola roger");
	}

	void AddListener(Button b, string value, bool i) 
	{
		b.onClick.RemoveAllListeners();
		if (i) {
			b.onClick.AddListener (() => {
				gameplayManager.CheckAwnser (value);
			});
		} else {
			b.onClick.AddListener (() => {
				gameplayManager.CheckQuestion (value);
			});
		}
	}

	public void RenewUiAwnsering(string[] awnsers, string currentCorrectAwnser=null ){
		for (int i = 0; i < buttonAwnser.Length; i++) {
			buttonAwnser[i].GetComponent<Text> ().text = awnsers[i];
		}
		bool noAwnser = false;
		for (int i = 0; i < buttonAwnser.Length; i++) {
			if (currentCorrectAwnser != buttonAwnser [i].GetComponent<Text>().text) {
				noAwnser=true;
				Debug.Log ("changing"+"\t"+i);
			} else {
				noAwnser=false;
				break;
			}
		}
		if(noAwnser)
			buttonAwnser[Random.Range (0, 3)].GetComponent<Text> ().text = currentCorrectAwnser;
		Shuffle (ref buttonAwnser); //out

		for (int x = 0 ; x < buttonAwnser.Length ; x++){
			buttonAwnser [x].transform.parent.GetComponent<Button> ().onClick.RemoveAllListeners();
			AddListener (buttonAwnser [x].transform.parent.GetComponent<Button>(), buttonAwnser [x].GetComponent<Text> ().text, true);
		}
	}
	public void RenewUiAsking(string[] questions){
		//now put the questions in the answer buttons
		for (int i = 0; i < buttonAwnser.Length; i++) {
			buttonAwnser [i].GetComponent<Text>().text = questions[i];
			buttonAwnser [i].transform.parent.GetComponent<Button> ().onClick.RemoveAllListeners();
			AddListener (buttonAwnser [i].transform.parent.GetComponent<Button> (),buttonAwnser [i].GetComponent<Text> ().text, false);
		}
	}
	public void ShowScreenSM(int correct,GameObject activePanel=null){
		//state machine to control the ui panels
		gameplayPanel.SetActive (false);
		switch (correct){
		case 1:
			winPanel.SetActive (true);
			break;
		case 2:
			losePanel.SetActive (true);
			break;
		case 3:
			winFighterPanel.SetActive (true);
			break;
		case 4:
			activePanel.SetActive (false);
			gameplayPanel.SetActive (true);
			break;
		case 5:
			activePanel.SetActive (false);
			gameplayPanel.SetActive (true);
			gameplayPanel.transform.FindChild ("StoryGroup").gameObject.SetActive (false);
			break;
		default:
			break;
		}
	}
	private void Shuffle(ref GameObject[] toShuffle ){
		for (int i = 0; i < toShuffle.Length; i++) {
			GameObject temp = toShuffle [i];
			int x = Random.Range (i, toShuffle.Length);
			toShuffle [i] = toShuffle [x];
			toShuffle [x] = temp;
		}

	}
}
