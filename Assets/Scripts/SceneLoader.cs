using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	[SerializeField]
	GameObject victoryScreen;
	[SerializeField]
	GameObject lossScreen;

	public void Load_Scene(string name) {
		SceneManager.LoadScene(name);
	}

	public void Game_Over() {
		lossScreen.SetActive(true);
	}

	public void Victory() {
		victoryScreen.SetActive(true);
	}

}