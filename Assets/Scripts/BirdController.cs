using UnityEngine;
using System.Collections;
using System;

public class BirdController : MonoBehaviour {
    public float speed;

	private Rigidbody2D rb;

	private float rotationMax = 20f;

	private float rotationMin = -90f;

	private float radius = 0.01f;
	 	
	private float radian = 0f;

	private float perRadian = 0.15f;

	// 是否正式开始
	private Boolean isStart = false;

	// Use this for initialization
	void Awake()
	{
		rb = GetComponent<Rigidbody2D> ();
		rb.gravityScale = 0;
		GetComponent<Observer>().AddEventHandler("GameStart", OnGameStart);
	}

	public void OnGameStart(object sender, EventArgs e) {
		isStart = true;	
		rb.gravityScale = 2f;
		Animator animator = GetComponent<Animator>();
		animator.enabled = true;
		rb.velocity = new Vector2 (0, speed);
	}

	// Update is called once per frame
	void Update () {
		if (!isStart) {
			// 准备阶段做上下缓动
			radian += perRadian;
			float dy = Mathf.Cos(radian) * radius;
			//Debug.Log ("dy=" + dy);
			transform.localPosition += new Vector3(0, dy, 0);


			if (Input.GetButtonDown ("Jump")) {
				NotificationCenter.GetInstance().PostNotification("GameStart");
			}
		} else {
			if (Input.GetButtonDown ("Jump")) {
				rb.velocity = new Vector2 (0, speed);
				//Animator animator = GetComponent<Animator>();
				//animator.SetTrigger("jump");
			}

			// 处理下降和上升过程中的旋转
			Vector3 rotation = this.transform.localEulerAngles;
			float rotationZ = rotation.z;
			if (rotationZ >= 180) {
				rotationZ -= 360;
			}
			if (rb.velocity.y > 0) {
				rotationZ += rb.velocity.y * rb.velocity.y * 5f;	
			} else if (rb.velocity.y <= -2.5f) {
				rotationZ -= rb.velocity.y * rb.velocity.y * 0.5f;
			}
			rotationZ = Math.Min (rotationMax, rotationZ);
			rotationZ = Math.Max (rotationMin, rotationZ);
			this.transform.localEulerAngles = new Vector3 (rotation.x, rotation.y, rotationZ);
			//Debug.Log ("rotationZ=" + rotationZ);
		}

	}

    void OnTriggerExit2D(Collider2D col) {
        NotificationCenter.GetInstance().PostNotification("ScoreAdd");
    }


    void OnCollisionEnter2D(Collision2D col)
    {
		//Debug.Log ("collide");
 		rb.velocity = new Vector2(0, 0);
        NotificationCenter.GetInstance().PostNotification("GameOver");

		// 若撞柱而死，则旋转90度下落
		if (col.collider.tag == "Tube") {
			Vector3 rotation = this.transform.localEulerAngles;
			this.transform.localEulerAngles = new Vector3 (rotation.x, rotation.y, -90f);
		}

        this.enabled = false;
		Animator animator = GetComponent<Animator>();
		animator.enabled = false;
		//BoxCollider2D collider = GetComponent<BoxCollider2D>();
		//collider.enabled = false;
		int birdLayer = LayerMask.NameToLayer("Bird");
		int layerMask = Physics2D.GetLayerCollisionMask(birdLayer);
		int tubeLayer = LayerMask.NameToLayer ("Tube");
		int maskChange = ~(1 << tubeLayer);
		Physics2D.SetLayerCollisionMask(birdLayer, layerMask & maskChange);


	}



}