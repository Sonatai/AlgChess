using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour {
	private AI Script;
	private int[] checkBorder;
	private int abziehWert = 0;
	private bool visited = false;

	// Use this for initialization
	void Start () {
		Script = GameObject.Find ("AI_Object").GetComponent<AI> ();

		checkBorder = new int[8];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Speichert mögliche Züge in ein String Array (Figurname,z bewegung, x bewegung, geschlagener Name)
	public void ShowPossibleMoves(char b1, char b2){
		//Debug.Log ("Ich bin der Script.MoveCounter: " + Script.MoveCounter);
		Script.ResetArray ();
		//possible Moves for White:
		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){

				//------| BAUER WEISS |------
				if (Script.CopyGameboard[i,j] != null && Script.CopyGameboard [i, j] [0] == b1 && Script.CopyGameboard[i,j][2] == 'B' && b1 == 'W') {
					for (int k = 0; k < 4; k++) {
						if (j+1 != 8 && Script.CopyGameboard [i, j + 1]== null && k == 0) {
							//1//Debug.Log ("j | i | " + j + " | " + i + " | ");
							if (j == 1) {
								Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
								Script.PossibleMoves [Script.MoveCounter, 1] = "0";
								Script.PossibleMoves [Script.MoveCounter, 2] = "1";
								Script.MoveCounter++;

								if (j+2 < 8 && Script.CopyGameboard [i, j + 2]== null) {
									Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
									Script.PossibleMoves [Script.MoveCounter, 1] = "0";
									Script.PossibleMoves [Script.MoveCounter, 2] = "2";
									Script.MoveCounter++;
									k++;
								}
							} else {
								//Debug.Log ("Ich bin im normalen Modus ;)");
								Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
								Script.PossibleMoves [Script.MoveCounter, 1] = "0";
								Script.PossibleMoves [Script.MoveCounter, 2] = "1";
								Script.MoveCounter++;
							}
						} else if (i + 1 != 8 && j+1 != 8 && Script.CopyGameboard [i + 1, j + 1]!= null && Script.CopyGameboard [i + 1, j + 1] [0] == b2 && visited == false) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "1";
							Script.PossibleMoves [Script.MoveCounter, 2] = "1";
							Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + 1, j + 1];
							Script.MoveCounter++;

							visited = true;
						} else if (i - 1 >= 0 && j+1 != 8 && Script.CopyGameboard [i - 1, j + 1]!= null && Script.CopyGameboard [i - 1, j + 1] [0] == b2) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "-1";
							Script.PossibleMoves [Script.MoveCounter, 2] = "1";
							Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - 1, j + 1];
							Script.MoveCounter++;

							visited = false;
							break;
						} else {
							visited = false;
							break;
						}
					} 
					//Debug.Log ("I've got the Bauer!");

					//------| BAUER SCHWARZ |------
				}else if (Script.CopyGameboard[i,j] != null && Script.CopyGameboard [i, j] [0] == b1 && Script.CopyGameboard[i,j][2] == 'B' && b1 == 'B') {
					for (int k = 0; k < 4; k++) {
						if (j-1 >= 0 && Script.CopyGameboard [i, j - 1]== null && k == 0) {
							if (j == 6) {
								Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
								Script.PossibleMoves [Script.MoveCounter, 1] = "0";
								Script.PossibleMoves [Script.MoveCounter, 2] = "-1";
								Script.MoveCounter++;

								if (j-2 >= 0 && Script.CopyGameboard [i, j - 2]== null) {
									Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
									Script.PossibleMoves [Script.MoveCounter, 1] = "0";
									Script.PossibleMoves [Script.MoveCounter, 2] = "-2";
									Script.MoveCounter++;
									k++;
								}
							} else {
								Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
								Script.PossibleMoves [Script.MoveCounter, 1] = "0";
								Script.PossibleMoves [Script.MoveCounter, 2] = "-1";
								Script.MoveCounter++;
							}
						} else if (i + 1 != 8 && j-1 >= 0 && Script.CopyGameboard [i + 1, j - 1]!= null && Script.CopyGameboard [i + 1, j - 1] [0] == b2 && visited == false) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "1";
							Script.PossibleMoves [Script.MoveCounter, 2] = "-1";
							Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + 1, j - 1];
							Script.MoveCounter++;

							visited = true;
						} else if (i - 1 >= 0 && j-1 >= 0 && Script.CopyGameboard [i - 1, j - 1]!= null && Script.CopyGameboard [i - 1, j - 1] [0] == b2) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "-1";
							Script.PossibleMoves [Script.MoveCounter, 2] = "-1";
							Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - 1, j - 1];
							Script.MoveCounter++;

							visited = false;
							break;
						} else {
							visited = false;
							break;
						}
					}

					//------| TURM || DAME || KONIG |------
				}else if (Script.CopyGameboard[i,j] != null && Script.CopyGameboard [i, j] [0] == b1 && (Script.CopyGameboard[i,j][2] == 'T' || Script.CopyGameboard[i,j][2] == 'D' || Script.CopyGameboard[i,j][2] == 'K')) {
					abziehWert = 0;
					for (int k = 1; k < 29; k++) {
						if (checkBorder [0] < 2 && j + 1 != 8 && (Script.CopyGameboard [i,  j+1] == null || (Script.CopyGameboard[i, j+1] != null && Script.CopyGameboard[i, j+1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "0";
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + k;

							if (Script.CopyGameboard[i, j+k] != null && Script.CopyGameboard[i, j +k] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i, j + k];
								checkBorder [0] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if (j + k + 1 == 8 || (Script.CopyGameboard[i, j+k+1] != null && Script.CopyGameboard[i, j+k+1][0] != b2 || Script.CopyGameboard[i,j][2] =='K')) {
								checkBorder [0] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [1] < 2 && j - 1 >= 0 && (Script.CopyGameboard [i,  j-1] == null || (Script.CopyGameboard[i, j-1] != null && Script.CopyGameboard[i, j-1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "0";
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + (-(k - abziehWert));

							if (Script.CopyGameboard[i, j-(k - abziehWert)] != null && Script.CopyGameboard[i, j - (k - abziehWert)] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i, j - (k - abziehWert)];
								checkBorder [1] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if ((j - (k - abziehWert) - 1) < 0 || (Script.CopyGameboard[i, j-(k - abziehWert)-1] != null && Script.CopyGameboard[i, j-(k - abziehWert)-1][0] != b2 || Script.CopyGameboard[i,j][2] =='K')){
								checkBorder [1] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [2] < 2 && i + 1 != 8 && (Script.CopyGameboard [i+1,  j] == null || (Script.CopyGameboard[i+1, j] != null && Script.CopyGameboard[i+1, j][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" +(k - abziehWert);
							Script.PossibleMoves [Script.MoveCounter, 2] = "0";

							if (Script.CopyGameboard[i + (k - abziehWert), j] != null && Script.CopyGameboard[i + (k - abziehWert), j] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + (k - abziehWert), j];
								checkBorder [2] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if (i + (k - abziehWert) + 1 == 8 || (Script.CopyGameboard[i+ (k - abziehWert) + 1, j] != null && Script.CopyGameboard[i+ (k - abziehWert) + 1, j][0] != b2 || Script.CopyGameboard[i,j][2] =='K')) {
								checkBorder [2] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [3] < 2 && i - 1 >= 0 && (Script.CopyGameboard [i-1,  j] == null || (Script.CopyGameboard[i-1, j] != null && Script.CopyGameboard[i-1, j][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + (-(k - abziehWert));
							Script.PossibleMoves [Script.MoveCounter, 2] = "0";

							if (Script.CopyGameboard[i - (k - abziehWert), j] != null && Script.CopyGameboard[i - (k - abziehWert), j] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - (k - abziehWert), j];
								checkBorder [3] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if (i - (k - abziehWert) - 1 < 0 || (Script.CopyGameboard[i- (k - abziehWert) - 1, j] != null && Script.CopyGameboard[i- (k - abziehWert) - 1, j][0] != b2 || Script.CopyGameboard[i,j][2] =='K')) {
								checkBorder [3] += 2;
								abziehWert = k;
							}
						} else if (Script.CopyGameboard[i,j][2] =='T') {
							checkBorder [0] = 0;
							checkBorder [1] = 0;
							checkBorder [2] = 0;
							checkBorder [3] = 0;

							break;

							//Diagonalfelder fuer Dame und Koenig:
						} else if (checkBorder [4] < 2 && j + 1 != 8 && i + 1 != 8 && (Script.CopyGameboard [i+1,  j+1] == null || (Script.CopyGameboard[i+1, j+1] != null && Script.CopyGameboard[i+1, j+1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + (k - abziehWert);
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + (k - abziehWert);

							if (Script.CopyGameboard[i + (k - abziehWert), j + (k - abziehWert)] != null && Script.CopyGameboard[i + (k - abziehWert), j + (k - abziehWert)] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + (k - abziehWert), j + (k - abziehWert)];
								checkBorder [4] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if (j + (k - abziehWert) + 1 == 8 || i + (k - abziehWert) + 1 == 8 || (Script.CopyGameboard[i+ (k - abziehWert) + 1, j+ (k - abziehWert) + 1] != null && Script.CopyGameboard[i+ (k - abziehWert) + 1, j+ (k - abziehWert) + 1][0] != b2 || Script.CopyGameboard[i,j][2] =='K')) {
								checkBorder [4] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [5] < 2 && j - 1 >= 0 && i - 1 >= 0 && (Script.CopyGameboard [i-1,  j-1] == null || (Script.CopyGameboard[i-1, j-1] != null && Script.CopyGameboard[i-1, j-1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + (-(k - abziehWert));
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + (-(k - abziehWert));

							if (Script.CopyGameboard[i - (k - abziehWert), j - (k - abziehWert)] != null && Script.CopyGameboard[i - (k - abziehWert), j - (k - abziehWert)] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - (k - abziehWert), j - (k - abziehWert)];
								checkBorder [5] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if ((j - (k - abziehWert) - 1) < 0 || (i - (k - abziehWert) - 1) < 0 || (Script.CopyGameboard[i- (k - abziehWert) - 1, j- (k - abziehWert) - 1] != null && Script.CopyGameboard[i- (k - abziehWert) - 1, j- (k - abziehWert) - 1][0] != b2 || Script.CopyGameboard[i,j][2] =='K')) {
								checkBorder [5] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [6] < 2 && j - 1 >= 0 && i + 1 != 8 && (Script.CopyGameboard [i+1,  j-1] == null || (Script.CopyGameboard[i+1, j-1] != null && Script.CopyGameboard[i+1, j-1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + (k - abziehWert);
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + (-(k - abziehWert));

							if (Script.CopyGameboard[i + (k - abziehWert), j - (k - abziehWert)] != null && Script.CopyGameboard[i + (k - abziehWert), j - (k - abziehWert)] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + (k - abziehWert), j - (k - abziehWert)];
								checkBorder [6] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if (i + (k - abziehWert) + 1 == 8 || j - (k - abziehWert) - 1 < 0 || (Script.CopyGameboard[i+ (k - abziehWert) + 1, j- (k - abziehWert) - 1] != null && Script.CopyGameboard[i+ (k - abziehWert) + 1, j- (k - abziehWert) - 1][0] != b2 || Script.CopyGameboard[i,j][2] =='K')) {
								checkBorder [6] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [7] < 2 && j + 1 != 8 && i - 1 >= 0 && (Script.CopyGameboard [i-1,  j+1] == null || (Script.CopyGameboard[i-1, j+1] != null && Script.CopyGameboard[i-1, j+1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + (-(k - abziehWert));
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + (k - abziehWert);

							if (Script.CopyGameboard[i - (k - abziehWert), j + (k - abziehWert)] != null && Script.CopyGameboard[i - (k - abziehWert), j + (k - abziehWert)] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - (k - abziehWert), j + (k - abziehWert)];
								checkBorder [7] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if (i - (k - abziehWert) - 1 < 0 || j + (k - abziehWert) + 1 == 8 || (Script.CopyGameboard[i- (k - abziehWert) - 1, j+ (k - abziehWert) + 1] != null && Script.CopyGameboard[i- (k - abziehWert) - 1, j+ (k - abziehWert) + 1][0] != b2 || Script.CopyGameboard[i,j][2] =='K')) {
								checkBorder [7] += 2;
							}
						} else {
							checkBorder [0] = 0;
							checkBorder [1] = 0;
							checkBorder [2] = 0;
							checkBorder [3] = 0;
							//----
							checkBorder [4] = 0;
							checkBorder [5] = 0;
							checkBorder [6] = 0;
							checkBorder [7] = 0;

							break;
						}
					}

					//------| SPRINGER |------
				}else if (Script.CopyGameboard[i,j] != null && Script.CopyGameboard [i, j] [0] == b1 && Script.CopyGameboard[i,j][2] == 'S') {
					for (int k = 0; k < 8; k++) {
						if (checkBorder [0] < 2 && j + 2 < 8 && i + 1 < 8 &&  (Script.CopyGameboard [ i + 1,  j + 2] == null || (Script.CopyGameboard[i+1,j+2] != null && Script.CopyGameboard[i+1,j+2][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "1";
							Script.PossibleMoves [Script.MoveCounter, 2] = "2";
							if (Script.CopyGameboard[i+1,j+2] != null && Script.CopyGameboard [ i + 1,  j + 2] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + 1, j + 2];
							}
							Script.MoveCounter++;
							checkBorder [0] += 2;
						} else if (checkBorder [1] < 2 && j + 2 < 8 && i - 1 >= 0 &&  (Script.CopyGameboard [ i - 1,  j + 2] == null || (Script.CopyGameboard[i-1,j+2] != null && Script.CopyGameboard[i-1,j+2][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "-1";
							Script.PossibleMoves [Script.MoveCounter, 2] = "2";
							if (Script.CopyGameboard[i-1,j+2] != null && Script.CopyGameboard [ i - 1,  j + 2] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - 1, j + 2];
							}
							Script.MoveCounter++;
							checkBorder [1] += 2;
						} else if (checkBorder [2] < 2 && j - 2 >= 0 && i + 1 < 8 &&  (Script.CopyGameboard [ i + 1,  j - 2] == null || (Script.CopyGameboard[i+1,j-2] != null && Script.CopyGameboard[i+1,j-2][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "1";
							Script.PossibleMoves [Script.MoveCounter, 2] = "-2";
							if (Script.CopyGameboard[i+1,j-2] != null && Script.CopyGameboard [ i + 1,  j - 2] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + 1, j - 2];
							}
							Script.MoveCounter++;
							checkBorder [2] += 2;
						} else if (checkBorder [3] < 2 && j - 2 >= 0 && i - 1 >= 0 &&  (Script.CopyGameboard [ i - 1,  j - 2] == null || (Script.CopyGameboard[i-1,j-2] != null && Script.CopyGameboard[i-1,j-2][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "-1";
							Script.PossibleMoves [Script.MoveCounter, 2] = "-2";
							if (Script.CopyGameboard[i-1,j-2] != null && Script.CopyGameboard [ i - 1,  j - 2] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - 1, j - 2];
							}
							Script.MoveCounter++;
							checkBorder [3] += 2;
						} else if (checkBorder [4] < 2 && j + 1 < 8 && i + 2 < 8 &&  (Script.CopyGameboard [ i + 2,  j + 1] == null || (Script.CopyGameboard[i+2,j+1] != null && Script.CopyGameboard[i+2,j+1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "2";
							Script.PossibleMoves [Script.MoveCounter, 2] = "1";
							if (Script.CopyGameboard[i+2,j+1] != null && Script.CopyGameboard [ i + 2,  j + 1] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + 2, j + 1];
							}
							Script.MoveCounter++;
							checkBorder [4] += 2;
						} else if (checkBorder [5] < 2 && j + 1 < 8 && i - 2 >= 0 &&  (Script.CopyGameboard [ i - 2,  j + 1] == null || (Script.CopyGameboard[i-2,j+1] != null && Script.CopyGameboard[i-2,j+1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "-2";
							Script.PossibleMoves [Script.MoveCounter, 2] = "1";
							if (Script.CopyGameboard[i-2,j+1] != null && Script.CopyGameboard [ i - 2,  j + 1] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - 2, j + 1];
							}
							Script.MoveCounter++;
							checkBorder [5] += 2;
						} else if (checkBorder [6] < 2 && j - 1 >= 0 && i + 2 < 8 &&  (Script.CopyGameboard [ i + 2,  j - 1] == null || (Script.CopyGameboard[i+2,j-1] != null && Script.CopyGameboard[i+2,j-1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "2";
							Script.PossibleMoves [Script.MoveCounter, 2] = "-1";
							if (Script.CopyGameboard[i+2,j-1] != null && Script.CopyGameboard [ i + 2,  j - 1] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + 2, j - 1];
							}
							Script.MoveCounter++;
							checkBorder [6] += 2;
						} else if (checkBorder [7] < 2 && j - 1 >= 0 && i - 2 >= 0 &&  (Script.CopyGameboard [ i - 2,  j - 1] == null || (Script.CopyGameboard[i-2,j-1] != null && Script.CopyGameboard[i-2,j-1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "-2";
							Script.PossibleMoves [Script.MoveCounter, 2] = "-1";
							if (Script.CopyGameboard[i-2,j-1] != null && Script.CopyGameboard [ i - 2,  j - 1] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - 2, j - 1];
							}
							Script.MoveCounter++;
							checkBorder [7] += 2;
						} else {
							checkBorder [0] = 0;
							checkBorder [1] = 0;
							checkBorder [2] = 0;
							checkBorder [3] = 0;
							checkBorder [4] = 0;
							checkBorder [5] = 0;
							checkBorder [6] = 0;
							checkBorder [7] = 0;
							break;
						}
					}

					//------| LAUFER |------
				}else if (Script.CopyGameboard[i,j] != null && Script.CopyGameboard [i, j] [0] == b1 && Script.CopyGameboard[i,j][2] == 'L' ) {
					abziehWert = 0;
					for (int k = 1; k < 15; k++) {
						if (checkBorder [0] < 2 && j+1 != 8 && i+1 != 8 && (Script.CopyGameboard [i+1,  j+1] == null || (Script.CopyGameboard[i+1, j+1] != null && Script.CopyGameboard[i+1, j+1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + k;
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + k;
							if (Script.CopyGameboard[i+k, j+k] != null && Script.CopyGameboard[ i + k, j + k] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + k, j + k];
								checkBorder [0] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if (j + k + 1 == 8 ||  i + k + 1 == 8 || (Script.CopyGameboard[i+k+1, j+k+1] != null && Script.CopyGameboard[i+k+1, j+k+1][0] != b2)) {
								checkBorder [0] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [1] < 2 && j-1 >= 0 && i-1 >= 0 && (Script.CopyGameboard [i-1,  j-1] == null || (Script.CopyGameboard[i-1, j-1] != null && Script.CopyGameboard[i-1, j-1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + (-(k - abziehWert));
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + (-(k - abziehWert));
							if (Script.CopyGameboard[i-(k - abziehWert), j-(k - abziehWert)] != null && Script.CopyGameboard[ i - (k - abziehWert), j - (k - abziehWert)] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - (k - abziehWert), j - (k - abziehWert)];
								checkBorder [1] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if ((j - (k - abziehWert) - 1) < 0 || ( i - (k - abziehWert) - 1) < 0 || (Script.CopyGameboard[i-(k - abziehWert)-1, j-(k - abziehWert)-1] != null && Script.CopyGameboard[i-(k - abziehWert)-1, j-(k - abziehWert)-1][0] != b2)) {
								checkBorder [1] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [2] < 2 && j-1 >= 0 && i+1 != 8 && (Script.CopyGameboard [i+1,  j-1] == null || (Script.CopyGameboard[i+1, j-1] != null && Script.CopyGameboard[i+1, j-1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + (k - abziehWert);
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + (-(k - abziehWert));
							if (Script.CopyGameboard[i+(k - abziehWert), j-(k - abziehWert)] != null && Script.CopyGameboard[ i + (k - abziehWert), j - (k - abziehWert)] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i + (k - abziehWert), j - (k - abziehWert)];
								checkBorder [2] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if ( i + (k - abziehWert) + 1 == 8 || j - (k - abziehWert) - 1 < 0 || (Script.CopyGameboard[i+(k - abziehWert)+1, j-(k - abziehWert)-1] != null && Script.CopyGameboard[i+(k - abziehWert)+1, j-(k - abziehWert)-1][0] != b2)) {
								checkBorder [2] += 2;
								abziehWert = k;
							}
						} else if (checkBorder [3] < 2 && j+1 != 8 && i-1 >= 0 && (Script.CopyGameboard [i-1,  j+1] == null || (Script.CopyGameboard[i-1, j+1] != null && Script.CopyGameboard[i-1, j+1][0] == b2))) {
							Script.PossibleMoves [Script.MoveCounter, 0] = Script.CopyGameboard[i,j];
							Script.PossibleMoves [Script.MoveCounter, 1] = "" + (-(k - abziehWert));
							Script.PossibleMoves [Script.MoveCounter, 2] = "" + (k - abziehWert);
							if (Script.CopyGameboard[i-(k - abziehWert), j+(k - abziehWert)] != null && Script.CopyGameboard[ i - (k - abziehWert), j + (k - abziehWert)] [0] == b2) {
								Script.PossibleMoves [Script.MoveCounter, 3] = Script.CopyGameboard[i - (k - abziehWert), j + (k - abziehWert)];
								checkBorder [3] += 2;
								abziehWert = k;
							}
							Script.MoveCounter++;
							if ( i - (k - abziehWert) - 1 < 0 || j + (k - abziehWert) + 1 == 8 || (Script.CopyGameboard[i-(k - abziehWert)-1, j+(k - abziehWert)+1] != null && Script.CopyGameboard[i-(k - abziehWert)-1, j+(k - abziehWert)+1][0] != b2)) {
								checkBorder [3] += 2;
							}
						} else {
							checkBorder [0] = 0;
							checkBorder [1] = 0;
							checkBorder [2] = 0;
							checkBorder [3] = 0;

							break;
						}
					}
				}
			}
		}



		/*if (b1 == 'W') {
			//Debug.Log ("/----------------------------------------| ZUG " + zugCounter + " |----------------------------------------\\");
			for (int i = 0; Script.PossibleMoves [i, 0] != null; i++) {
				//Debug.Log ("moves bis " + (i + 5) + ">     " + Script.PossibleMoves [i, 0] + " " + Script.PossibleMoves [i, 1] + "/" + Script.PossibleMoves [i, 2] + Script.PossibleMoves [i, 3] + " | " + Script.PossibleMoves [i + 1, 0] + " " + Script.PossibleMoves [i + 1, 1] + "/" + Script.PossibleMoves [i + 1, 2] + Script.PossibleMoves [i + 1, 3] + " | "
				+ Script.PossibleMoves [i + 2, 0] + " " + Script.PossibleMoves [i + 2, 1] + "/" + Script.PossibleMoves [i + 2, 2] + Script.PossibleMoves [i + 2, 3] + " | " + Script.PossibleMoves [i + 3, 0] + " " + Script.PossibleMoves [i + 3, 1] + "/" + Script.PossibleMoves [i + 3, 2] + Script.PossibleMoves [i + 3, 3] + " | "
				+ Script.PossibleMoves [i + 4, 0] + " " + Script.PossibleMoves [i + 4, 1] + "/" + Script.PossibleMoves [i + 4, 2] + Script.PossibleMoves [i + 4, 3]);
				i += 4;
			}
		}*/



	}
}
