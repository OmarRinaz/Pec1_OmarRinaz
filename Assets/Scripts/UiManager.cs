using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UiManager: MonoBehaviour{

	private static UiManager instance = null;
	private GameObject winPanel;		//win round panel and group
	private GameObject winFighterPanel; //Win Fight panel and group
	private GameObject losePanel;		//Lose Round panel and group
//	private GameObject gameplayPanel; //not used anymore
	private GameObject storyPanel;		//machine output panel and group
	private GameObject awnserPanel;		//player panel for awnsering/asking
	private GameObject pausePanel;		//menu pause
	private GameObject[] buttonAwnser;	//store the buttons from the player panel to asign them some listeners
	private string awnserPath = "Canvas/GameplayPanel/AwnserGroup/AwnserButton";
	private GameplayManager gameplayManager;

	public Text currentQuestion;
	public Text winScoreLabel;			//store the current wins we got
	public Text loseScoreLabel;		//store the current losses we got

	public static UiManager Instance {
		get {
			if (instance == null)
				Debug.Log ("Error, GameManager does not exist. ¡Attach GameManager Script to a game object named GameManager!");
			return instance;
		} 
	}

	void Awake(){
		instance = this;
		pausePanel = GameObject.Find ("Canvas/PausePanel");
		storyPanel =GameObject.Find ("Canvas/GameplayPanel/StoryGroup");
		awnserPanel =GameObject.Find ("Canvas/GameplayPanel/AwnserGroup");
		winPanel = GameObject.Find ("Canvas/WinPanel");
		winFighterPanel = GameObject.Find ("Canvas/WinFighterPanel");
		losePanel = GameObject.Find ("Canvas/LosePanel");
		winScoreLabel = GameObject.Find ("Canvas/WinScorePanel/WinScore").GetComponent<Text> ();
		loseScoreLabel = GameObject.Find ("Canvas/LoseScorePanel/LoseScore").GetComponent<Text> ();
//		gameplayPanel = GameObject.Find ("Canvas/GameplayPanel");
		currentQuestion = GameObject.Find ("Canvas/GameplayPanel/StoryGroup/MaskTextHistory/TextHistory").GetComponent<Text>();
		buttonAwnser = new GameObject[4];
		winPanel.SetActive (false);
		losePanel.SetActive (false);
		winFighterPanel.SetActive (false);
		pausePanel.SetActive (false);
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
				//Debug.Log ("changing"+"\t"+i);
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
	public void ShowScreenSM(string target,bool mode){
		//state machine to control the ui panels
		//gameplayPanel.SetActive (false);
		switch (target){
		case "activeWinPanel":  //1
			if (mode) {
					winPanel.SetActive (true); //if you win a round activate that
			} else {
					winPanel.SetActive (false); 
			}
			break;
		case "activeLosePanel": //2
			if (mode) {
					losePanel.SetActive (true);
			} else {
					losePanel.SetActive (false);
			}
			break;
		case "activeWinFighterPanel": //3
			if (mode) {
					winFighterPanel.SetActive (true);
			} else {
					winFighterPanel.SetActive (false);
			}
			break;
		case "activeStoryPanel": //4
			if (mode) {
				storyPanel.SetActive (true);
			} else {				
				storyPanel.SetActive (false);
			}
			break;
		case "activeAwnserPanel": //5
			if (mode) {
				awnserPanel.SetActive (true);
		
			} else {
				awnserPanel.SetActive (false);				
			}
			break;
		case "activePausePanel"://6
			if (mode) {
				pausePanel.SetActive (true);
			} else {
				pausePanel.SetActive (false);
			}
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