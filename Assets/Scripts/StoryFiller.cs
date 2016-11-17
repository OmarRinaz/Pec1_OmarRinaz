using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class StoryFiller {
	
	public static List<GameplayManager.QuestionAwnser> FillList(){		//usar esto para rellenar la lista con las preguntas y su respuesta correcta.
		List<GameplayManager.QuestionAwnser> qaList = new List<GameplayManager.QuestionAwnser> ();
		qaList.Add(CreateQA(new string[]{"¡No hay palabras para describir lo asqueroso que eres!","Ya no hay técnicas que te puedan salvar."},"Sí que las hay, sólo que nunca las has aprendido."));
		qaList.Add(CreateQA(new string[]{"¡Obtuve esta cicatriz en mi cara en una lucha a muerte! ","En mi última pelea terminé con las manos llenas de sangre."},"Espero que ya hayas aprendido a no tocarte la nariz."));
		qaList.Add(CreateQA(new string[]{"¡Una vez tuve un perro más listo que tú!","Sólo he conocido a uno tan cobarde como tú."},"Te habrá enseñado todo lo que sabes."));
		qaList.Add(CreateQA(new string[]{"¡La gente cae a mis pies al verme llegar!","Mis enemigos más sabios corren al verme llegar."},"¿Incluso antes de que huelan tu aliento?"));
		return qaList;
	}
	private static GameplayManager.QuestionAwnser CreateQA(string[] questions, string awnser){ //constructor de obj tipo QuestionAwnser.
		GameplayManager.QuestionAwnser newQA = new GameplayManager.QuestionAwnser ();
		newQA.questions = questions;
		newQA.awnsers = awnser;
		return newQA;
	}
}