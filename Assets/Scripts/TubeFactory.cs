using UnityEngine;
using System.IO;
using System.Collections;
using LitJson;

public class TubeFactory : MonoBehaviour {
    public Rigidbody2D tube;
	public Rigidbody2D[] grounds;
	public Rigidbody2D[] backgrounds;
	public GameObject bird;
    public float waitTime;
    public float minCreatePosY;
    public float maxCreatePosY;
    public float finishPosX;
    public float speed;
	private int groundIndex = 1;
	private int backgroundIndex = 1;
	private int maxOrderInLayer = 5;

	// Use this for initialization
	void Start () {
		//Random.state = System.DateTime.Today.Millisecond;
        Random.seed = System.DateTime.Today.Millisecond;

        GetComponent<Observer>().AddEventHandler("GameOver", OnGameOver);
		GetComponent<Observer>().AddEventHandler("GameStart", OnGameStart);  




		//ground.velocity = new Vector2(speed, 0);
		//ground1.velocity = new Vector2(speed, 0);
	}

	void Update() {
		int prevIndex = 0;
		int aftIndex = 0;
		if (grounds [groundIndex].transform.position.x <= bird.transform.position.x) {
			prevIndex = groundIndex - 1;
			aftIndex = groundIndex + 1;
			if (prevIndex < 0)
				prevIndex += 3;
			if (aftIndex >= 3)
				aftIndex -= 3;
			grounds[prevIndex].transform.position = grounds[aftIndex].transform.position + new Vector3(3.08f, 0f, 0f);
			groundIndex = aftIndex;
		}



		if (backgrounds [backgroundIndex].transform.position.x <= bird.transform.position.x) {
			prevIndex = backgroundIndex - 2;
			aftIndex = backgroundIndex + 2;
			if (prevIndex < 0)
				prevIndex += 5;
			if (aftIndex >= 5)
				aftIndex -= 5;

			backgrounds[prevIndex].transform.position = backgrounds[aftIndex].transform.position + new Vector3(2.39f, 0f, 0f);
			backgroundIndex++;
			if (backgroundIndex >= 5)
				backgroundIndex -= 5;
			maxOrderInLayer++;
			backgrounds[prevIndex].transform.GetComponent<SpriteRenderer>().sortingOrder = maxOrderInLayer;
		}
	}



    IEnumerator CreateTube() {
        yield return new WaitForSeconds(waitTime);

        float posY = Random.Range(minCreatePosY, maxCreatePosY);
        Vector3 createPos = new Vector3(0, posY, transform.position.z);
        Rigidbody2D instance = Instantiate(tube, createPos, Quaternion.identity) as Rigidbody2D;
        instance.velocity = new Vector2(speed, 0);

        StartCoroutine(CreateTube());

        while(instance.transform.position.x >= finishPosX)
            yield return null;
        Destroy(instance.gameObject);
    }

    public void OnGameOver(object sender, System.EventArgs e)
    {
        StopAllCoroutines();

		foreach (Rigidbody2D ground in grounds) {
			ground.velocity = new Vector2(0, 0);
		}

		int order = 1;
		foreach (Rigidbody2D background in backgrounds) {
			background.velocity = new Vector2(0, 0);
		}
    }

	public void OnGameStart(object sender, System.EventArgs e)
	{
		StartCoroutine(CreateTube());

		foreach (Rigidbody2D ground in grounds) {
			ground.velocity = new Vector2(speed, 0);
		}

		int order = 1;
		foreach (Rigidbody2D background in backgrounds) {
			background.velocity = new Vector2(speed, 0);
			background.gameObject.GetComponent<SpriteRenderer>().sortingOrder = order;
			order++;
		}
	}
}