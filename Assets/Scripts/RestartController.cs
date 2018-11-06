using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class RestartController : MonoBehaviour {

	public SpriteRenderer restartRenderer;
	
	void Start() {
		GetComponent<Observer>().AddEventHandler("GameReset", OnGameReset);
	}

	void Update() {
		if (restartRenderer.enabled &&
				Input.GetButtonDown ("Jump")) {
			SceneManager.LoadScene ("Game");
			NotificationCenter.GetInstance().PostNotification("GameReset");
		}	
	}


	void OnMouseDown() {
		SceneManager.LoadScene ("Game");
		NotificationCenter.GetInstance().PostNotification("GameReset");
	}

	void OnGameReset(object sender, EventArgs e) {
		int birdLayer = LayerMask.NameToLayer("Bird");
		int layerMask = Physics2D.GetLayerCollisionMask(birdLayer);
		int tubeLayer = LayerMask.NameToLayer ("Tube");
		int maskChange = (1 << tubeLayer);
		Physics2D.SetLayerCollisionMask(birdLayer, layerMask | maskChange);
	}



}

