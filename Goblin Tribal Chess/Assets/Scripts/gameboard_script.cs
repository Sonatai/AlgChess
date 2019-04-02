using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameboard_script : MonoBehaviour {
	private string[,] originalGameboard = new string[8,8]; 
	private GameObject ChessPiece;
	private AI Script;

	// Use this for initialization
	void Awake () {
		originalGameboard [0, 0] = "W_T1";
		originalGameboard [1, 0] = "W_S1";
		originalGameboard [2, 0] = "W_L1";
		originalGameboard [3, 0] = "W_K1";
		originalGameboard [4, 0] = "W_D1";
		originalGameboard [5, 0] = "W_L2";
		originalGameboard [6, 0] = "W_S2";
		originalGameboard [7, 0] = "W_T2";
		originalGameboard [0, 1] = "W_B1";
		originalGameboard [1, 1] = "W_B2";
		originalGameboard [2, 1] = "W_B3";
		originalGameboard [3, 1] = "W_B4";
		originalGameboard [4, 1] = "W_B5";
		originalGameboard [5, 1] = "W_B6";
		originalGameboard [6, 1] = "W_B7";
		originalGameboard [7, 1] = "W_B8";
		originalGameboard [0, 6] = "B_B1";
		originalGameboard [1, 6] = "B_B2";
		originalGameboard [2, 6] = "B_B3";
		originalGameboard [3, 6] = "B_B4";
		originalGameboard [4, 6] = "B_B5";
		originalGameboard [5, 6] = "B_B6";
		originalGameboard [6, 6] = "B_B7";
		originalGameboard [7, 6] = "B_B8";
		originalGameboard [0, 7] = "B_T1";
		originalGameboard [1, 7] = "B_S1";
		originalGameboard [2, 7] = "B_L1";
		originalGameboard [3, 7] = "B_K1";
		originalGameboard [4, 7] = "B_D1";
		originalGameboard [5, 7] = "B_L2";
		originalGameboard [6, 7] = "B_S2";
		originalGameboard [7, 7] = "B_T2";

	}

	void Start () {
		Script = GameObject.Find ("AI_Object").GetComponent<AI> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public string GetPieceOnTile(int Spalte, int Zeile){
		if (originalGameboard [Spalte, Zeile] != null) {
			return originalGameboard [Spalte, Zeile];
		} else {
			return "nothing";
		}
	}

	public void SetPieceOnTile(string piece, int X, int Y, int startX, int startY, char color){
		//Debug.Log ("piece: "+ piece + " X: "+X+" Y: "+Y+" startX: "+startX+" startY: "+startY );
		if(originalGameboard[X,Y] != null){
			ChessPiece = GameObject.Find (originalGameboard[X,Y]);
			if((color == 'W' && ChessPiece.name[0]=='B') || (color == 'B' && ChessPiece.name[0]=='W')){
				ChessPiece.SetActive(false);
			}
		}
		originalGameboard[startX, startY] = null;
		Script.SetBoard (startX, startY, null);
		originalGameboard [X, Y] = piece;
		Script.SetBoard (X, Y, piece);
	}

	public int GetCycleNameX(string bezeichnung){
		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){
				if(originalGameboard[i,j] != null && originalGameboard[i, j] == bezeichnung){
					return i;
				}
			}
		}
		return 0;
	}
	public int GetCycleNameY(string bezeichnung){
		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){
				if(originalGameboard[i,j] != null && originalGameboard[i, j] == bezeichnung){
					return j;
				}
			}
		}
		return 0;
	}

	public string GetGameboard(int i, int j){
		return originalGameboard [i, j];
	}
}
