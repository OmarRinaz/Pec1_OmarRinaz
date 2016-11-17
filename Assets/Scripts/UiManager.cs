using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class UiManager: Object{

	private static UiManager instance = null;
	private GameObject winPanel;
	private GameObject winFighterPanel;
	private GameObject losePanel;
	private GameObject gameplayPanel;
	private GameObject[] buttonAwnser;
	private string awnserPath = "Canvas/GameplayPanel/AwnserGroup/AwnserButton";

	public Text currentQuestion;
	public static UiManager Instance{
		get {
			if (instance == null)
				UiManager.instance = new UiManager();
			return UiManager.instance;
		} 
	}

	public void Init(string[] awnsers){ 
		winPanel = GameObject.Find ("Canvas/WinPanel");
		winFighterPanel = GameObject.Find ("Canvas/WinFighterPanel");
		losePanel = GameObject.Find ("Canvas/LosePanel");
		gameplayPanel = GameObject.Find ("Canvas/GameplayPanel");
		currentQuestion = GameObject.Find ("Canvas/GameplayPanel/StoryGroup/MaskTextHistory/TextHistory").GetComponent<Text>();
		buttonAwnser = new GameObject[5];
		buttonAwnser[0] = GameObject.Find (awnserPath + "/AwnserTextButton");
		buttonAwnser[1] = GameObject.Find (awnserPath + " (1)" + "/AwnserTextButton (1)");
		buttonAwnser[2] = GameObject.Find (awnserPath + " (2)" + "/AwnserTextButton (2)");
		buttonAwnser[3] = GameObject.Find (awnserPath + " (3)" + "/AwnserTextButton (3)");
		winPanel.SetActive (false);
		losePanel.SetActive (false);
		winFighterPanel.SetActive (false);
		RenewUi (awnsers);
	}
	public void RenewUi(string[] awnsers, string currentCorrectAwnser=null ){
		for (int i = 0; i < 4; i++) {
			buttonAwnser[i].GetComponent<Text> ().text = awnsers[i];
		}
		bool noAwnser = false;
		for (int i = 0; i < buttonAwnser.Length-1; i++) {
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
		buttonAwnser [4] = buttonAwnser [0];
		buttonAwnser [0] = buttonAwnser [1];
		buttonAwnser [1] = buttonAwnser [4];
		buttonAwnser [4] = buttonAwnser [2];
		buttonAwnser [2] = buttonAwnser [3];
		buttonAwnser [3] = buttonAwnser [4];
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
		default:
			break;
		}
	}

}
