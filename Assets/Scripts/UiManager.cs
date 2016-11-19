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
	private GameObject storyPanel;
	private GameObject awnserPanel;
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
		storyPanel =GameObject.Find ("Canvas/GameplayPanel/StoryGroup");
		awnserPanel =GameObject.Find ("Canvas/GameplayPanel/AwnserGroup");
		winPanel = GameObject.Find ("Canvas/WinPanel");
		winFighterPanel = GameObject.Find ("Canvas/WinFighterPanel");
		losePanel = GameObject.Find ("Canvas/LosePanel");
		gameplayPanel = GameObject.Find ("Canvas/GameplayPanel");
		currentQuestion = GameObject.Find ("Canvas/GameplayPanel/StoryGroup/MaskTextHistory/TextHistory").GetComponent<Text>();
		buttonAwnser = new GameObject[4];
		winPanel.SetActive (false);
		losePanel.SetActive (false);
		winFighterPanel.SetActive (false);
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
				if (!winPanel.activeInHierarchy){
					winPanel.SetActive (true); //if you win a round activate that
				}
			} else {
				if (winPanel.activeInHierarchy) {
					winPanel.SetActive (false); 
				}
			}
			break;
		case "activeLosePanel": //2
			if (mode) {
				if (!losePanel.activeInHierarchy){
					losePanel.SetActive (true);
				}
			} else {
				if (losePanel.activeInHierarchy){
					losePanel.SetActive (false);
				}
			}
			break;
		case "activeWinFighterPanel": //3
			if (mode) {
				if (!winFighterPanel.activeInHierarchy){
					winFighterPanel.SetActive (true);
				}
			} else {
				if (winFighterPanel.activeInHierarchy) {
					winFighterPanel.SetActive (false);
				}
			}
			break;
		case "activeStoryPanel": //4
			if (mode) {
				if (!storyPanel.activeSelf) {
					storyPanel.SetActive (true);
				}
			} else {
				
					storyPanel.SetActive (false);

			}
			break;
		case "activeAwnserPanel": //5
			if (mode) {
				if (!awnserPanel.activeInHierarchy){
					awnserPanel.SetActive (true);
				}
			} else {
				if (awnserPanel.activeInHierarchy){
					awnserPanel.SetActive (false);
				}
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
//break;
//case 2:
//losePanel.SetActive (true);//if you lose a round no mater what trigger that
//break;
//case 3:
//winFighterPanel.SetActive (true);//if you finaly win the fighter fire that one
//break;
//case 4:
//activePanel.SetActive (false);//triger that when 
//gameplayPanel.SetActive (true);
//break;
//case 5:
//activePanel.SetActive (false);
//gameplayPanel.SetActive (true);
//gameplayPanel.transform.FindChild ("StoryGroup").gameObject.SetActive (false);
//break;
//case 6:
////activePanel.SetActive (false);
//if(winPanel.activeInHierarchy)winPanel.SetActive (true);//if you finaly win the fighter fire that one)
//gameplayPanel.SetActive (true);
//gameplayPanel.transform.FindChild ("AwnserGroup").gameObject.SetActive (false);
//gameplayPanel.transform.FindChild ("StoryGroup").gameObject.SetActive (true);
//break;
//case 7:
//gameplayPanel.transform.FindChild ("StoryGroup").gameObject.SetActive (true);
//gameplayPanel.transform.FindChild ("AwnserGroup").gameObject.SetActive (true);
//break;
//case 8:
//gameplayPanel.transform.FindChild ("StoryGroup").gameObject.SetActive (false);
//gameplayPanel.transform.FindChild ("AwnserGroup").gameObject.SetActive (false);
//break;