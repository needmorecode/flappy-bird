using UnityEngine;
using System.Collections;
using System;

public class ScoreFactory : MonoBehaviour {

    public Sprite[] scores;
    public SpriteRenderer scoreRenderer_0;
    public SpriteRenderer scoreRenderer_1;
	public SpriteRenderer currentRenderer_0;
	public SpriteRenderer currentRenderer_1;
	public SpriteRenderer bestRenderer_0;
	public SpriteRenderer bestRenderer_1;
	public SpriteRenderer resultRenderer;
	public SpriteRenderer restartRenderer;
	public SpriteRenderer splashRenderer;
	public Camera camera;
	private int oldCullingMask = 0;
	private bool isSplash = false;
	private float radian = 0f;

    private int currentScore;

    void Start()
    {
        GetComponent<Observer>().AddEventHandler("ScoreAdd", OnScoreAdd);
		GetComponent<Observer>().AddEventHandler("GameOver", OnGameOver);
		GetComponent<Observer>().AddEventHandler("GameStart", OnGameStart);
        currentScore = 0;
        scoreRenderer_1.sprite = scores[0];
		Vector3 pos = scoreRenderer_1.transform.localPosition;
		scoreRenderer_1.transform.localPosition = new Vector3 (0, pos.y, pos.z);
        
    }

    void OnScoreAdd(object sender, EventArgs e) {
        currentScore += 1;
        scoreRenderer_1.sprite = scores[currentScore % 10];
        scoreRenderer_0.sprite = scores[currentScore / 10];
		if (currentScore >= 10) {
			Vector3 pos = scoreRenderer_1.transform.localPosition;
			scoreRenderer_1.transform.localPosition = new Vector3 (0.17f, pos.y, pos.z);
			scoreRenderer_0.enabled = true;
		}
    }

	void OnGameStart(object sender, EventArgs e){
		scoreRenderer_1.enabled = true;
	}

	void Update() {
		if (isSplash) {
			radian += 0.25f;
			if (radian >= Mathf.PI) {
				radian = Mathf.PI;
				isSplash = false;
			}
			float splashAlpha = Mathf.Sin (radian) * 0.5f;
			Color color = splashRenderer.color;
			splashRenderer.color = new Color(color.r, color.g, color.b, splashAlpha);
		}
	}

	void OnGameOver(object sender, EventArgs e)
	{
		//StartCoroutine(BlackScreen());
		isSplash = true;

		scoreRenderer_0.enabled = false;
		scoreRenderer_1.enabled = false;

		resultRenderer.enabled = true;
		restartRenderer.enabled = true;

		// 显示分数;
		currentRenderer_1.sprite = scores[currentScore % 10];
		currentRenderer_0.sprite = scores[currentScore / 10];
		currentRenderer_1.enabled = true;
		if (currentScore >= 10) {
			currentRenderer_0.enabled = true;
		} else {
			Vector3 pos = currentRenderer_1.transform.localPosition;
			currentRenderer_1.transform.localPosition = new Vector3 (0, pos.y, pos.z);
		}

		// 存储最佳分数
		int bestScore = PlayerPrefs.GetInt ("bestScore");
		if (currentScore > bestScore) {
			bestScore = currentScore;
			PlayerPrefs.SetInt ("bestScore", bestScore);
		}

		// 显示最佳分数
		bestRenderer_1.sprite = scores[bestScore % 10];
		bestRenderer_0.sprite = scores[bestScore / 10];
		bestRenderer_1.enabled = true;
		if (bestScore >= 10) {
			bestRenderer_0.enabled = true;
		} else {
			Vector3 pos = bestRenderer_1.transform.localPosition;
			bestRenderer_1.transform.localPosition = new Vector3 (0, pos.y, pos.z);
		}
	}
}
