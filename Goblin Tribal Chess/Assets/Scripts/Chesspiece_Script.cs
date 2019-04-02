using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesspiece_Script : MonoBehaviour {
	private Light Piece;
	private Light[] TileLight; //Array für Dame und so?
	private Move_Chesspiece Script;
	private gameboard_script Script2;
	private AI Script3;
	private float targetPosX = 0f;
	private float targetPosZ = 0f;
	private string PieceName;
	private int[] checkBorder;
	private int abziehWert = 0;
	private bool visited = false;
	private int i = 0;
	private int boardX = 0;
	private int boardY = 0;
	//private bool alreadyChecked = false; //Performancegründe beim nach Name suchen - irrelevant weil ButtonDown
										// eh nicht jedes Frame checkt
	private GameObject[] tile;
	//private RaycastHit hit;
	//private Ray strahl;

	// Use this for initialization
	void Start () {
		//Script2 = GetComponent (typeof(gameboard_script)) as gameboard_script;
		TileLight = new Light[28];
		tile = new GameObject[28];
		checkBorder = new int[8];
		Script2 = GameObject.Find ("Board").GetComponent<gameboard_script> ();
		Script3 = GameObject.Find ("AI_Object").GetComponent<AI> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			ShowPossibleTurns ();
		}else if(Input.GetMouseButtonDown (1) && Script3.Checkmate == false){
			MovePiece ();
		}
	}


	private void MovePiece(){

		RaycastHit hit;
		Ray strahl = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (strahl, out hit, 100.0f)) {
			if (hit.transform.name [0] == 'B') {
				boardX = Script2.GetCycleNameX (hit.transform.name);
				boardY = Script2.GetCycleNameY (hit.transform.name);
			}

			for(i = 0; i < 28; i++){
				if (tile[i] != null && hit.transform.name == tile [i].transform.name) {
					if (PieceName[2] == 'K') {
						Script3.kingSecurity (((int)tile [i].transform.name [5] - 49), ((int)tile [i].transform.name [7] - 49));
						if (Script3.protectTheKing == true) {
							Script3.protectTheKing = false;
							break;
						}
					}
					Script2.SetPieceOnTile (PieceName, ((int)tile[i].transform.name[5]-49), ((int)tile[i].transform.name[7]-49), (int)targetPosZ-1, (int)targetPosX-1, 'W');
					Script.moveTo (((int)tile[i].transform.name[7]-48), ((int)tile[i].transform.name[5]-48));
					if (TileLight [0] != null) {
						for (int i = 0; TileLight [i] != null && TileLight [i].enabled == true; i++) {
							TileLight [i].color = Color.green;
							TileLight [i].enabled = false;
							TileLight [i] = null;
							tile [i] = null;
							Piece.enabled = false;
						}
						abziehWert = 0;
					}
					break;
				}else if(hit.transform.name[0] == 'B'){
					if(tile[i]  != null && tile[i].transform.name == ("Tile_"+(boardX+1)+"/"+(boardY+1))){
						if (PieceName[2] == 'K') {
							Script3.kingSecurity (boardX, boardY);
							if (Script3.protectTheKing == true) {
								Script3.protectTheKing = false;
								break;
							}
						}
						//Debug.Log ("HEUREKA !!1111!1");
						Script2.SetPieceOnTile (PieceName, boardX, boardY, (int)targetPosZ-1, (int)targetPosX-1, 'W');
						Script.moveTo (boardY+1, boardX+1);
						if (TileLight [0] != null) {
							for (int i = 0; TileLight [i] != null && TileLight [i].enabled == true; i++) {
								TileLight [i].color = Color.green;
								TileLight [i].enabled = false;
								TileLight [i] = null;
								tile [i] = null;
								Piece.enabled = false;
							}
							abziehWert = 0;
						}
						break;
					}
				}
			}
		}
	}


	private void ShowPossibleTurns(){

		RaycastHit hit;
		Ray strahl = Camera.main.ScreenPointToRay (Input.mousePosition);

		if(Physics.Raycast (strahl,out hit,100.0f)){
			//hit.transform.GetComponentInChildren<Light> ().enabled = true;
			if (hit.transform.name [0] != 'T') {
				if (Piece != null) {
					Piece.enabled = false;
				}
				//Debug.Log ("Ausgewählt: <|" + hit.transform.name+"|>");

				Piece = hit.transform.GetComponentInChildren<Light> ();
				Piece.enabled = true;

				if (hit.transform.name[0] != 'B') {
					Script = hit.transform.GetComponent<Move_Chesspiece> ();

					targetPosX = Script.GetPositionX ();
					targetPosZ = Script.GetPositionZ ();
					PieceName = Script.GetName ();
				}
			}

			if (hit.transform.name[0] == 'B') {
				//Debug.Log ("TEST");
				//-------------------| LIGHT RESET |------------------------------------------------------------------
				if (TileLight [0] != null) {
					for (int i = 0; TileLight [i] != null && TileLight [i].enabled == true; i++) {
						TileLight [i].color = Color.green;
						TileLight [i].enabled = false;
						TileLight [i] = null;
						tile [i] = null;
					}
				}
			}else if (PieceName != null && PieceName [0] == 'W') {
				//-------------------| LIGHT RESET |------------------------------------------------------------------
				if (TileLight [0] != null) {
					for (int i = 0; TileLight [i] != null && TileLight [i].enabled == true; i++) {
						TileLight [i].color = Color.green;
						TileLight [i].enabled = false;
						TileLight [i] = null;
						tile [i] = null;
					}
					abziehWert = 0;
				}
				//-------------------| BAUER |------------------------------------------------------------------------
				if (PieceName [2] == 'B') {
					for (i = 0; i <= 4; i++) {
						if (Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX) [0] != 'W' && Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX) [0] != 'B' && i == 0) {
							if (targetPosX == 2) {
								tile [i] = GameObject.Find ("Tile_" + targetPosZ + "/" + (targetPosX + 1));
								TileLight [i] = tile [i].GetComponentInChildren<Light> ();
								TileLight [i].enabled = true;
								if (Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX + 1) [0] != 'W' && Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX + 1) [0] != 'B') {
									tile [i + 1] = GameObject.Find ("Tile_" + targetPosZ + "/" + (targetPosX + 2));
									TileLight [i + 1] = tile [i + 1].GetComponentInChildren<Light> ();
									TileLight [i + 1].enabled = true;
									i++;
								}
							} else {
								tile [i] = GameObject.Find ("Tile_" + targetPosZ + "/" + (targetPosX + 1));
								TileLight [i] = tile [i].GetComponentInChildren<Light> ();
								TileLight [i].enabled = true;
							}
						} else if (targetPosZ != 8 && Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX) [0] == 'B' && visited == false) {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ + 1) + "/" + (targetPosX + 1));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							TileLight [i].color = Color.red;

							visited = true;

						} else if (targetPosZ != 1 && Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX) [0] == 'B') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ - 1) + "/" + (targetPosX + 1));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							TileLight [i].color = Color.red;

							visited = false;
							break;
						} else {
							visited = false;
							break;
						}
					}
					//-------------------| TURM || DAME || KONIG |-----------------------------------------------------------
				} else if (PieceName [2] == 'T' || PieceName [2] == 'D' || PieceName [2] == 'K') {
					for (i = 1; i < 29; i++) {
						if (checkBorder [0] < 2 && targetPosX + 1 != 9 && Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + targetPosZ + "/" + (targetPosX + i));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX + i - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [0] += 2;
								abziehWert = i;
							}
							//Debug.Log ("This is the Output: " + Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX + i));
							if (targetPosX + i + 1 == 9 || Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX + i) [0] == 'W' || PieceName [2] == 'K') {
								checkBorder [0] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [1] < 2 && targetPosX - 1 != 0 && Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX - 2) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + targetPosZ + "/" + (targetPosX - (i - abziehWert)));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX - (i - abziehWert) - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [1] += 2;
								abziehWert = i;
							}
							if ((targetPosX - (i - abziehWert) - 1) == 0 || Script2.GetPieceOnTile ((int)targetPosZ - 1, (int)targetPosX - (i - abziehWert) - 2) [0] == 'W' || PieceName [2] == 'K') {
								checkBorder [1] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [2] < 2 && targetPosZ + 1 != 9 && Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX - 1) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ + (i - abziehWert)) + "/" + targetPosX);
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ + (i - abziehWert) - 1, (int)targetPosX - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [2] += 2;
								abziehWert = i;
							}
							if (targetPosZ + (i - abziehWert) + 1 == 9 || Script2.GetPieceOnTile ((int)targetPosZ + (i - abziehWert), (int)targetPosX - 1) [0] == 'W' || PieceName [2] == 'K') {
								checkBorder [2] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [3] < 2 && targetPosZ - 1 != 0 && Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX - 1) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ - (i - abziehWert)) + "/" + targetPosX);
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 1, (int)targetPosX - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [3] += 2;
								abziehWert = i;
							}
							if (targetPosZ - (i - abziehWert) - 1 == 0 || Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 2, (int)targetPosX - 1) [0] == 'W' || PieceName [2] == 'K') {
								checkBorder [3] += 2;
								abziehWert = i;
							}
						} else if (PieceName [2] == 'T') {
							checkBorder [0] = 0;
							checkBorder [1] = 0;
							checkBorder [2] = 0;
							checkBorder [3] = 0;

							break;

							//Diagonalfelder fuer Dame und Koenig:
						} else if (checkBorder [4] < 2 && targetPosX + 1 != 9 && targetPosZ + 1 != 9 && Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ + (i - abziehWert)) + "/" + (targetPosX + (i - abziehWert)));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ + (i - abziehWert) - 1, (int)targetPosX + (i - abziehWert) - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [4] += 2;
								abziehWert = i;
							}
							if (targetPosX + (i - abziehWert) + 1 == 9 || targetPosZ + (i - abziehWert) + 1 == 9 || Script2.GetPieceOnTile ((int)targetPosZ + (i - abziehWert), (int)targetPosX + (i - abziehWert)) [0] == 'W' || PieceName [2] == 'K') {
								checkBorder [4] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [5] < 2 && targetPosX - 1 != 0 && targetPosZ - 1 != 0 && Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX - 2) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ - (i - abziehWert)) + "/" + (targetPosX - (i - abziehWert)));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 1, (int)targetPosX - (i - abziehWert) - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [5] += 2;
								abziehWert = i;
							}
							if ((targetPosX - (i - abziehWert) - 1) == 0 || (targetPosZ - (i - abziehWert) - 1) == 0 || Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 2, (int)targetPosX - (i - abziehWert) - 2) [0] == 'W' || PieceName [2] == 'K') {
								checkBorder [5] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [6] < 2 && targetPosX - 1 != 0 && targetPosZ + 1 != 9 && Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX - 2) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ + (i - abziehWert)) + "/" + (targetPosX - (i - abziehWert)));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ + (i - abziehWert) - 1, (int)targetPosX - (i - abziehWert) - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [6] += 2;
								abziehWert = i;
							}
							if (targetPosZ + (i - abziehWert) + 1 == 9 || targetPosX - (i - abziehWert) - 1 == 0 || Script2.GetPieceOnTile ((int)targetPosZ + (i - abziehWert), (int)targetPosX - (i - abziehWert) - 2) [0] == 'W' || PieceName [2] == 'K') {
								checkBorder [6] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [7] < 2 && targetPosX + 1 != 9 && targetPosZ - 1 != 0 && Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ - (i - abziehWert)) + "/" + (targetPosX + (i - abziehWert)));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 1, (int)targetPosX + (i - abziehWert) - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [7] += 2;
								abziehWert = i;
							}
							if (targetPosZ - (i - abziehWert) - 1 == 0 || targetPosX + (i - abziehWert) + 1 == 9 || Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 2, (int)targetPosX + (i - abziehWert)) [0] == 'W' || PieceName [2] == 'K') {
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
					//-------------------| LAUFER |------------------------------------------------------------------------
				} else if (PieceName [2] == 'L') {
					for (i = 1; i <= 15; i++) {
						if (checkBorder [0] < 2 && targetPosX + 1 != 9 && targetPosZ + 1 != 9 && Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ + i) + "/" + (targetPosX + i));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ + i - 1, (int)targetPosX + i - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [0] += 2;
								abziehWert = i;
							}
							if (targetPosX + i + 1 == 9 || targetPosZ + i + 1 == 9 || Script2.GetPieceOnTile ((int)targetPosZ + i, (int)targetPosX + i) [0] == 'W') {
								checkBorder [0] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [1] < 2 && targetPosX - 1 != 0 && targetPosZ - 1 != 0 && Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX - 2) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ - (i - abziehWert)) + "/" + (targetPosX - (i - abziehWert)));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 1, (int)targetPosX - (i - abziehWert) - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [1] += 2;
								abziehWert = i;
							}
							if ((targetPosX - (i - abziehWert) - 1) == 0 || (targetPosZ - (i - abziehWert) - 1) == 0 || Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 2, (int)targetPosX - (i - abziehWert) - 2) [0] == 'W') {
								checkBorder [1] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [2] < 2 && targetPosX - 1 != 0 && targetPosZ + 1 != 9 && Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX - 2) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ + (i - abziehWert)) + "/" + (targetPosX - (i - abziehWert)));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ + (i - abziehWert) - 1, (int)targetPosX - (i - abziehWert) - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [2] += 2;
								abziehWert = i;
							}
							if (targetPosZ + (i - abziehWert) + 1 == 9 || targetPosX - (i - abziehWert) - 1 == 0 || Script2.GetPieceOnTile ((int)targetPosZ + (i - abziehWert), (int)targetPosX - (i - abziehWert) - 2) [0] == 'W') {
								checkBorder [2] += 2;
								abziehWert = i;
							}
						} else if (checkBorder [3] < 2 && targetPosX + 1 != 9 && targetPosZ - 1 != 0 && Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX) [0] != 'W') {
							tile [i-1] = GameObject.Find ("Tile_" + (targetPosZ - (i - abziehWert)) + "/" + (targetPosX + (i - abziehWert)));
							TileLight [i-1] = tile [i-1].GetComponentInChildren<Light> ();
							TileLight [i-1].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 1, (int)targetPosX + (i - abziehWert) - 1) [0] == 'B') {
								TileLight [i-1].color = Color.red;
								checkBorder [3] += 2;
								abziehWert = i;
							}
							if (targetPosZ - (i - abziehWert) - 1 == 0 || targetPosX + (i - abziehWert) + 1 == 9 || Script2.GetPieceOnTile ((int)targetPosZ - (i - abziehWert) - 2, (int)targetPosX + (i - abziehWert)) [0] == 'W') {
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
					//-------------------| SPRINGER |----------------------------------------------------------------------
				} else if (PieceName [2] == 'S') {
					for (i = 0; i <= 8; i++) {
						if (checkBorder [0] < 2 && targetPosX + 2 < 9 && targetPosZ + 1 < 9 && Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX + 1) [0] != 'W') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ + 1) + "/" + (targetPosX + 2));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX + 1) [0] == 'B') {
								TileLight [i].color = Color.red;
							}
							checkBorder [0] += 2;
						} else if (checkBorder [1] < 2 && targetPosX + 2 < 9 && targetPosZ - 1 > 0 && Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX + 1) [0] != 'W') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ - 1) + "/" + (targetPosX + 2));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX + 1) [0] == 'B') {
								TileLight [i].color = Color.red;
							}
							checkBorder [1] += 2;
						} else if (checkBorder [2] < 2 && targetPosX - 2 > 0 && targetPosZ + 1 < 9 && Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX - 3) [0] != 'W') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ + 1) + "/" + (targetPosX - 2));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ, (int)targetPosX - 3) [0] == 'B') {
								TileLight [i].color = Color.red;
							}
							checkBorder [2] += 2;
						} else if (checkBorder [3] < 2 && targetPosX - 2 > 0 && targetPosZ - 1 > 0 && Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX - 3) [0] != 'W') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ - 1) + "/" + (targetPosX - 2));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - 2, (int)targetPosX - 3) [0] == 'B') {
								TileLight [i].color = Color.red;
							}
							checkBorder [3] += 2;
						} else if (checkBorder [4] < 2 && targetPosX + 1 < 9 && targetPosZ + 2 < 9 && Script2.GetPieceOnTile ((int)targetPosZ + 1, (int)targetPosX) [0] != 'W') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ + 2) + "/" + (targetPosX + 1));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ + 1, (int)targetPosX) [0] == 'B') {
								TileLight [i].color = Color.red;
							}
							checkBorder [4] += 2;
						} else if (checkBorder [5] < 2 && targetPosX + 1 < 9 && targetPosZ - 2 > 0 && Script2.GetPieceOnTile ((int)targetPosZ - 3, (int)targetPosX) [0] != 'W') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ - 2) + "/" + (targetPosX + 1));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - 3, (int)targetPosX) [0] == 'B') {
								TileLight [i].color = Color.red;
							}
							checkBorder [5] += 2;
						} else if (checkBorder [6] < 2 && targetPosX - 1 > 0 && targetPosZ + 2 < 9 && Script2.GetPieceOnTile ((int)targetPosZ + 1, (int)targetPosX - 2) [0] != 'W') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ + 2) + "/" + (targetPosX - 1));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ + 1, (int)targetPosX - 2) [0] == 'B') {
								TileLight [i].color = Color.red;
							}
							checkBorder [6] += 2;
						} else if (checkBorder [7] < 2 && targetPosX - 1 > 0 && targetPosZ - 2 > 0 && Script2.GetPieceOnTile ((int)targetPosZ - 3, (int)targetPosX - 2) [0] != 'W') {
							tile [i] = GameObject.Find ("Tile_" + (targetPosZ - 2) + "/" + (targetPosX - 1));
							TileLight [i] = tile [i].GetComponentInChildren<Light> ();
							TileLight [i].enabled = true;
							if (Script2.GetPieceOnTile ((int)targetPosZ - 3, (int)targetPosX - 2) [0] == 'B') {
								TileLight [i].color = Color.red;
							}
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
				}
				//---------------------------------------------------------------------------------------------------------


				//Debug.Log ("X" + targetPosX + " Z" + targetPosZ + " " + PieceName);
			}
		}

	}
}
