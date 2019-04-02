using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Chesspiece : MonoBehaviour {

	private bool move = false;
	private float tempX = 0f;
	private float tempZ = 0f;
	private float targetPosX = 0f;
	private float targetPosZ = 0f;
	private float posX = 0f;
	private float posZ = 0f;
	private float rundungsHelfer = 0f;
	private bool checkX = false;
	private bool checkZ = false;
	private int directionX = 0;
	private int directionZ = 0;
	private AI Script;



	// Use this for initialization
	void Start () {
		//Debug.Log("Dieser Bauer ist auf Feld: "+ (((transform.position.x-1)/2)+1)+" | " + ((transform.position.z/-2+1)) );
		Script = GameObject.Find("AI_Object").GetComponent<AI>();
	}
	
	// Update is called once per frame
	void Update () {
		if(move == true){

			if (directionX < 0f && checkX == false) {
				if (transform.position.x > targetPosX + .05f) {
					tempX = transform.position.x - Time.deltaTime * 1.6f;
				} else {
					rundungsHelfer = transform.position.x % 1;
					tempX = transform.position.x - rundungsHelfer;
					checkX = true;
					//Debug.Log (rundungsHelfer+" rundA!");
				}
			} else if(checkX == false) {
				if (transform.position.x < targetPosX + .05f) {
					tempX = transform.position.x + Time.deltaTime * 1.6f;
				} else {
					rundungsHelfer = transform.position.x % 1;
					tempX = transform.position.x - rundungsHelfer;
					checkX = true;
					//Debug.Log (rundungsHelfer+" rundB");
				}
			}

			if (directionZ < 0f && checkZ == false) {
				if (transform.position.z > targetPosZ - .05f) {
					tempZ = transform.position.z - Time.deltaTime * 1.6f;				
				} else {
					rundungsHelfer = transform.position.z % 1;
					tempZ = transform.position.z - rundungsHelfer;
					checkZ = true;
					//Debug.Log (rundungsHelfer+" rundC");
				}
			} else if(checkZ == false) {
				if (transform.position.z < targetPosZ - .05f) {
					tempZ = transform.position.z + Time.deltaTime * 1.6f;				
				} else {
					rundungsHelfer = transform.position.z % 1;
					tempZ = transform.position.z - rundungsHelfer;
					checkZ = true;
					//Debug.Log (rundungsHelfer+" rundD+");
				}
			}

			transform.position = new Vector3 (tempX, transform.position.y, tempZ);
			if (checkX == true && checkZ == true) {
				transform.position = new Vector3 (targetPosX, transform.position.y, targetPosZ);
				move = false;
				checkX = !checkX;
				checkZ = !checkZ;
				directionX = 0;
				directionZ = 0;
				if(this.transform.name[0] == 'W'){
					Script.SetChecked ();
				}
			}
		}
	}

	//-----------| moveTo übermittelt die zielkoordinaten des Zuges |----------------------------
	public void moveTo(int Value1, int Value2){
		move = true;
		targetPosX = (float)(((Value1 - 1) * 2) + 1);
		targetPosZ = (float)((Value2 - 1) * -2);
		//Debug.Log (Value1 + " | " + Value2);


		if(targetPosX < transform.position.x){
			directionX--;
		}
		if(targetPosZ < transform.position.z){
			directionZ--;
		}
	}
	//-------------------------------------------------------
	public float GetPositionX(){
		posX = (((transform.position.x-1)/2)+1);

		return this.posX;
	}

	public float GetPositionZ(){
		posZ = ((transform.position.z/-2)+1);

		return this.posZ;
	}

	public string GetName(){
		return this.transform.name;
	}

}
