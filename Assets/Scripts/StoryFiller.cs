using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Xml;

public class StoryFiller {
	//old and antifaishon way not used anymore
	public static List<GameplayManager.QuestionAwnser> FillList(){		//usar esto para rellenar la lista con las preguntas y su respuesta correcta.
		List<GameplayManager.QuestionAwnser> qaList = new List<GameplayManager.QuestionAwnser> ();
		qaList.Add(CreateQA(new string[]{"¡No hay palabras para describir lo asqueroso que eres!","Ya no hay técnicas que te puedan salvar."},"Sí que las hay, sólo que nunca las has aprendido."));
		qaList.Add(CreateQA(new string[]{"¡Obtuve esta cicatriz en mi cara en una lucha a muerte!","En mi última pelea terminé con las manos llenas de sangre."},"Espero que ya hayas aprendido a no tocarte la nariz."));
		qaList.Add(CreateQA(new string[]{"¡Una vez tuve un perro más listo que tú!","Sólo he conocido a uno tan cobarde como tú."},"Te habrá enseñado todo lo que sabes."));
		qaList.Add(CreateQA(new string[]{"¡La gente cae a mis pies al verme llegar!","Mis enemigos más sabios corren al verme llegar."},"¿Incluso antes de que huelan tu aliento?"));
		return qaList;
	}
	//new faishon way search for xml with all the text and charge it to the data struct QuestionAwnser
	public static List<GameplayManager.QuestionAwnser> FillListFromXml(){
		string filePath = Application.dataPath+"/Resources/QaTextList.xml";
		TextAsset textAsset = (TextAsset)Resources.Load ("QaTextList");
		//string fileName = "QaTextList.xml";
		XmlDocument xmlFile = new XmlDocument ();
		List<GameplayManager.QuestionAwnser> qaList = new List<GameplayManager.QuestionAwnser> ();	
		if (File.Exists (filePath)) {
			xmlFile.LoadXml (textAsset.text);
			XmlNodeList questionsList = xmlFile.GetElementsByTagName ("questions");
//			Debug.Log (questionsList.Item(1).Name.ToString());
			foreach (XmlNode question in questionsList) {
				XmlNodeList qa = question.ChildNodes;
				for (int i = 0; i<qa.Count; i++) {
					XmlNode qaParse = qa.Item (i);
					qaList.Add (CreateQA (new string[]{ qaParse.ChildNodes.Item (0).FirstChild.Value.ToString(), qaParse.ChildNodes.Item (1).FirstChild.Value.ToString()}, qaParse.ChildNodes.Item (2).FirstChild.Value.ToString()));
				}
			}
		} else {
			//No file was found prompt error and get the default qalist
			Debug.Log("No file was found! charging the default qalist");
			qaList = FillList ();
		}
		return qaList;
	}
	//use this to initialize the list
	private static GameplayManager.QuestionAwnser CreateQA(string[] questions, string awnser){ //constructor de obj tipo QuestionAwnser.
		GameplayManager.QuestionAwnser newQA = new GameplayManager.QuestionAwnser ();
		newQA.questions = questions;
		newQA.awnsers = awnser;
		return newQA;
	}
}