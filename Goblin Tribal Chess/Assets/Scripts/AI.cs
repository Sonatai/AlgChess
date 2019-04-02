using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class AI : MonoBehaviour {
	public string[,] CopyGameboard = new string [8,8]; //a copy of the origin gameboard
	private gameboard_script Script;
	private Move_Chesspiece Script2;
	private Engine Script3;
	private bool alreadyChecked = true;
	public int MoveCounter = 0;
	private int gegnerZugCounter = 0;
	private GameObject ChessPiece;
	int ubergangsWert = 0;
	public string[,] PossibleMoves = new string[122,4];
	private int pieceCounter = 0;
	private int lastPieceCounter = 0;
	private bool[] checkNow = new bool[2];
	private bool[] check = new bool[2];
	public bool Checkmate = false;
	private GameObject[] king;
	private Light[] checkLight;
	private GameObject victroy;
	private Text victoryText;
	public bool protectTheKing = false;

	//--- BewertungsArrays -----------


	//---- AI Variablen -----------
	[SerializeField]
	private int depth; //tiefe des Baums
	private int score; //Score pro Move
	private string[] bestMove = new string[4];
	Hashtable ranking = new Hashtable();

	//-----------------------------


	//***************************************************************
	//------------------------- START -------------------------------
	//***************************************************************

	void Start () {
		king = new GameObject[2];
		checkLight = new Light[2];
		checkNow[0] = false;
		checkNow[1] = false;
		check[0] = false;
		check[1] = false;
		//TODO:rewrite the code :)

		Script = GameObject.Find ("Board").GetComponent<gameboard_script> ();
		Script3 = GameObject.Find ("Engine_Object").GetComponent<Engine> ();

		for(int i = 0; i < 8; i++){
			for(int j = 0; j < 8; j++){
				
				CopyGameboard [i, j] = Script.GetGameboard (i, j);

			}
		}

		king[0] = GameObject.Find ("W_K1");
		king[1] = GameObject.Find ("B_K1");
		checkLight[0] = king[0].GetComponentInChildren<Light> ();
		checkLight[1] = king[1].GetComponentInChildren<Light> ();
		victroy = GameObject.Find ("UI_Object");
		victoryText = victroy.GetComponentInChildren<Text> ();

		//TODO: Implement Opening Book
		//TODO: Implement Endgame
		//TODO: Optimizing weight of moves (HashMap?)	
		//Value for Sorting

		ranking.Add ("BD",'Q' );
		ranking.Add ("BT",'M' );
		ranking.Add ("BS",'I' );
		ranking.Add ("BL",'I' );
		ranking.Add ("BB",'D' );

		ranking.Add ("LD",'P' );
		ranking.Add ("LT",'L' );
		ranking.Add ("LS",'H' );
		ranking.Add ("LL",'H' );
		ranking.Add ("LB",'C' );

		ranking.Add ("SD",'P' );
		ranking.Add ("ST",'L' );
		ranking.Add ("SS",'G' );
		ranking.Add ("SL",'G' );
		ranking.Add ("SB",'C' );

		ranking.Add ("TD",'O' );
		ranking.Add ("TT",'K' );
		ranking.Add ("TS",'F' );
		ranking.Add ("TL",'F' );
		ranking.Add ("TB",'B' );

		ranking.Add ("DD",'N' );
		ranking.Add ("DT",'J' );
		ranking.Add ("DS",'E' );
		ranking.Add ("DL",'E' );
		ranking.Add ("DB",'A' );

	}

	
	//***************************************************************
	//------------------------- UPDATE ------------------------------
	//***************************************************************

	void Update () {
		
		//Zugmöglichkeiten--------------------------------------------------
		if (alreadyChecked == false) {
			alreadyChecked = true;

			//--- Chesscheck I ----------------------------------------------------------
			Chesscheck_I('B','W');
			Chesscheck_II('W','B');
			//---------------------------------------------------------------------------

			if(Checkmate == false){

				//reset best Move
				bestMove [0] = null;
				bestMove [1] = null;
				bestMove [2] = null;
				bestMove [3] = null;

				//Calculate it
				bestMove = minimaxRoot (depth, CopyGameboard, 'B');

				/*----------------------------------------------
				 * MOVING CHESS PIECES
				 * ---------------------------------------------*/

				ChessPiece = GameObject.Find (bestMove [0]);

				Script2 = ChessPiece.GetComponent<Move_Chesspiece> ();

				if (bestMove [1] [0] == '-') {
					ubergangsWert = 48 - ((int)bestMove [1] [1] - 48);
					bestMove [1] = "" + (char)ubergangsWert;
				}
				if (bestMove [2] [0] == '-') {
					ubergangsWert = 48 - ((int)bestMove [2] [1] - 48);
					bestMove [2] = "" + (char)ubergangsWert;
				}

				gegnerZugCounter++;

				Script.SetPieceOnTile (bestMove [0], (int)(ChessPiece.transform.position.z / -2) + (int)bestMove [1] [0] - 48, (int)((ChessPiece.transform.position.x - 1) / 2) + (int)bestMove [2] [0] - 48, (int)(ChessPiece.transform.position.z / -2), (int)((ChessPiece.transform.position.x - 1) / 2), 'B');
				Script2.moveTo ((int)((ChessPiece.transform.position.x - 1) / 2) + (int)bestMove [2] [0] - 47, (int)(ChessPiece.transform.position.z / -2) + (int)bestMove [1] [0] - 47);

				ResetArray ();

				//--- Chesscheck II ---------------------------------------------------------
				Chesscheck_II('B','W');
				Chesscheck_I('W','B');
				//---------------------------------------------------------------------------
			}
		}
		
	}

	//***************************************************************
	//------------------------- CHESSCHECK -----------------------
	//***************************************************************

	private void Chesscheck_I(char x, char y){
		int z = 0;
		if (x == 'W') {
			z++;
		}
		Script3.ShowPossibleMoves (x, y);

		for (int i = 0; PossibleMoves[i,0] != null; i++) {
			if(PossibleMoves[i,3] != null && PossibleMoves[i,3][2] == 'K'){

				checkNow[z] = true;
			}
		}

		if(checkNow[z] == true){
			check[z] = true;
			victoryText.text = "CHECKMATE !";
			checkLight[z].enabled = true;
			checkLight[z].color = Color.red;
			Checkmate = true;
		}else{
			check[z] = false;
			checkLight[z].enabled = false;
			checkLight[z].color = Color.yellow;
		}

		checkNow[z] = false;
	}

	private void Chesscheck_II(char x, char y){
		int z = 0;
		if (x == 'W') {
			z++;
		}
		Script3.ShowPossibleMoves (x, y);

		for (int i = 0; PossibleMoves[i,0] != null; i++) {
			if(PossibleMoves[i,3] != null && PossibleMoves[i,3][2] == 'K'){
				checkNow[z] = true;
			}
		}

		if(checkNow[z] == true){
			check[z] = true;
			checkLight[z].enabled = true;
			checkLight[z].color = Color.red;
		}else{
			check[z] = false;
			checkLight[z].enabled = false;
			checkLight[z].color = Color.yellow;
		}

		checkNow[z] = false;
	}

	public void kingSecurity(int x, int y){
		Script3.ShowPossibleMoves ('B','W');

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				if (CopyGameboard [i, j] !=null && CopyGameboard [i, j][0] == 'B' &&CopyGameboard [i, j][2] == 'B') {
					if ((i-1 == x && j-1 == y) || (i+1 == x && j-1 == y)) {
						protectTheKing = true;
						break;
					}
				}
			}
		}

		for (int k = 0; PossibleMoves[k,0] != null; k++) {
			for (int i = 0; i < 8; i++) {
				for (int j = 0; j < 8; j++) {
					if(CopyGameboard[i,j] == PossibleMoves[k,0]){
						if (PossibleMoves [k, 1] [0] == '-') {
							ubergangsWert = 48 - ((int)PossibleMoves [k, 1] [1] - 48);
							PossibleMoves [k, 1] = "" + (char)ubergangsWert;
						}
						if (PossibleMoves [k, 2] [0] == '-') {
							ubergangsWert = 48 - ((int)PossibleMoves [k, 2] [1] - 48);
							PossibleMoves [k, 2] = "" + (char)ubergangsWert;
						}
						if ((((int)PossibleMoves[k, 1][0]-48) + i == x) && (((int)PossibleMoves[k, 2][0])-48) + j == y) {
							protectTheKing = true;
							break;
						}
					}
				}
			}
		}
	}

	//***************************************************************
	//------------------------- SORTING (MAX) -----------------------
	//***************************************************************

	string[,] getSortedMaxArray(){

		int count = 0;
		string[] tempMove = new string[5];
		string[,] moves = new string[122, 5];
		string hashIndex;



		//get all moves with value != 0
		for (int y = 0; PossibleMoves[y,0] != null; y++) {

			if (PossibleMoves [y, 3] != null) {

				moves [count, 0] = PossibleMoves [y, 0];
				moves [count, 1] = PossibleMoves [y, 1];
				moves [count, 2] = PossibleMoves [y, 2];
				moves [count, 3] = PossibleMoves [y, 3];
				hashIndex = "" + moves [count , 0] [3] + moves [count, 3] [3];
				moves [count, 4] = (string) ranking[hashIndex];

				count++; 
				hashIndex = "";

			}

		}

		//sort them with bubble sort TODO: Optimizing -> another search algorithm?
		for(int n = count; n > 1; n = n-1){
			for (int i = 0; i < n - 1; i++) {
				
				if (moves [i + 1, 4] != null && (int)moves [i, 4][0] <(int)moves [i + 1, 4][0]) {
					
					tempMove [0] = moves [i, 0];
					tempMove [1] = moves [i, 1];
					tempMove [2] = moves [i, 2];
					tempMove [3] = moves [i, 3];
					tempMove [4] = moves [i, 4];

					moves [i, 0] = moves [i + 1, 0];
					moves [i, 1] = moves [i + 1, 1];
					moves [i, 2] = moves [i + 1, 2];
					moves [i, 3] = moves [i + 1, 3];
					moves [i, 4] = moves [i + 1, 4];

					moves [i + 1, 0] = tempMove [0];
					moves [i + 1, 0] = tempMove [1];
					moves [i + 1, 0] = tempMove [2];
					moves [i + 1, 0] = tempMove [3];
					moves [i + 1, 0] = tempMove [4];

				}

			}
		}

		//rest of the moves 
		for (int y = count; PossibleMoves[y,0] != null; y++) {

			if(moves[y,3] == null){
				moves [y, 0] = PossibleMoves [y, 0];
				moves [y, 1] = PossibleMoves [y, 1];
				moves [y, 2] = PossibleMoves [y, 2];
				moves [y, 3] = PossibleMoves [y, 3];
				moves [y, 4] = null;
			}

		}

		return moves;
	}


	//***************************************************************
	//------------------------- SORTING (MIN) -----------------------
	//***************************************************************

	string[,] getSortedMinArray(){

		int count = 0;
		string[] tempMove = new string[5];
		string[,] moves = new string[122, 5];
		string hashIndex;

		//get all moves with value != 0
		for (int y = 0; PossibleMoves[y,0] != null; y++) {

			if (PossibleMoves [y, 3] != null) {

				moves [count, 0] = PossibleMoves [y, 0];
				moves [count, 1] = PossibleMoves [y, 1];
				moves [count, 2] = PossibleMoves [y, 2];
				moves [count, 3] = PossibleMoves [y, 3];
				hashIndex = "" + moves [count , 0] [3] + moves [count, 3] [3];
				moves [count, 4] = (string) ranking[hashIndex];
				count++; 
				hashIndex = "";
			}

		}

		//sort them TODO: Optimizing -> another search algorithm?
		for(int n = count; n > 1; n = n-1){
			for (int i = 0; i < n - 1; i++) {
				
				if (moves [i + 1, 4] != null && (int)moves [i, 4][0] > (int)moves [i + 1, 4][0]) {
					
					tempMove [0] = moves [i, 0];
					tempMove [1] = moves [i, 1];
					tempMove [2] = moves [i, 2];
					tempMove [3] = moves [i, 3];
					tempMove [4] = moves [i, 4];

					moves [i, 0] = moves [i + 1, 0];
					moves [i, 1] = moves [i + 1, 1];
					moves [i, 2] = moves [i + 1, 2];
					moves [i, 3] = moves [i + 1, 3];
					moves [i, 4] = moves [i + 1, 4];

					moves [i + 1, 0] = tempMove [0];
					moves [i + 1, 0] = tempMove [1];
					moves [i + 1, 0] = tempMove [2];
					moves [i + 1, 0] = tempMove [3];
					moves [i + 1, 0] = tempMove [4];

				}

			}
		}

		//rest of the moves 
		for (int y = count; PossibleMoves[y,0] != null; y++) {

			if(moves[y,3] == null){
				moves [y, 0] = PossibleMoves [y, 0];
				moves [y, 1] = PossibleMoves [y, 1];
				moves [y, 2] = PossibleMoves [y, 2];
				moves [y, 3] = PossibleMoves [y, 3];
				moves [y, 4] = null;
			}

		}

		return moves;
	}


	//***************************************************************
	//------------------------- GET ALL MOVES -----------------------
	//***************************************************************

	string[,] getAllMoves(){
		
		string[,] moves = new string[122, 4];

		for (int y = 0; PossibleMoves != null; y++) {
			moves [y, 0] = PossibleMoves [y, 0];
			moves [y, 1] = PossibleMoves [y, 1];
			moves [y, 2] = PossibleMoves [y, 2];
			moves [y, 3] = PossibleMoves [y, 3];
		}

		return moves;
	}

	//***************************************************************
	//------------------------- MINMAX ROOT--------------------------
	//***************************************************************

	string[] minimaxRoot (int depth, string[,] copyboard, char player){
		//TODO: Find out, why it's so slow ... - maybe the engine itself?
		//TODO: Side Change

		string[] bestMoveFound = new string[4];
		string[,] moves = new string[122, 5];
		Script3.ShowPossibleMoves ('B','W');

		//Get all possible moves sorted
		moves = getSortedMaxArray ();

		//start to calculate minimax
		int bestSeenMove = -99999;
		for (int i = 0; moves [i, 0] != null; i++) {

			newBoard (moves [i, 0], moves [i, 1], moves [i, 2]);

			int value = min (CopyGameboard, depth-1, -1000000, 1000000);
			//Debug.Log ("This is the Value in minimaxRoot: " + value);
			//Debug.Log ("This is my bestSeenMove before if: " + bestSeenMove);

			undo (moves [i, 0], moves [i, 1], moves [i, 2],moves [i, 3]);

			if (value >= bestSeenMove) {
				bestSeenMove = value;
				//Debug.Log ("This is my bestSeenMove after if: " + bestSeenMove);
				bestMoveFound [0] = moves [i, 0];
				bestMoveFound [1] = moves [i, 1];
				bestMoveFound [2] = moves [i, 2];
				bestMoveFound [3] = moves [i, 3];
			}

		}

		return bestMoveFound;	
	}


	//***************************************************************
	//------------------------- MAX ---------------------------------
	//***************************************************************
	public int max(string[,]copyboard, int depth, int alpha, int beta){
		
		//Debug.Log ("Depth : " + depth);
		if (depth == 0 ) {
			return GetValue ();
		}

		Script3.ShowPossibleMoves ('B','W');
		string[,] moves = new string[122, 5];

		//get all possible move sorted
		moves = getSortedMaxArray ();

		// Maximizing
		int highestSeenValue = -99999;
		for (int i = 0; moves [i, 0] != null; i++) {

			newBoard (moves [i, 0], moves [i, 1], moves [i, 2]);

			int currentValue = min (copyboard, depth - 1, alpha, beta);
			if (currentValue >= highestSeenValue) {
				highestSeenValue = currentValue;
			}

			undo (moves [i, 0], moves [i, 1], moves [i, 2],moves [i, 3]);

			//Alpha-Beta Calc
			alpha = Mathf.Max (alpha, highestSeenValue);
			if (beta <= alpha) {
				return highestSeenValue;
			}
		}

		return highestSeenValue;
	}

	//***************************************************************
	//------------------------- MIN ---------------------------------
	//***************************************************************

	public int min(string[,]copyboard, int depth, int alpha, int beta){
		
		//Debug.Log ("Depth : " + depth);
		if (depth == 0 ) {
			return GetValue ();
		}

		Script3.ShowPossibleMoves ('W','B');
		string[,] moves = new string[122, 5];
	
		moves = getSortedMinArray ();
			
		//Minimizing Calc
		int lowestSeenValue = 99999;
		for (int i = 0; moves [i, 0] != null; i++) {
			
			newBoard (moves [i, 0], moves [i, 1], moves [i, 2]);

			int currentValue = max (copyboard, depth - 1, alpha,beta);
			if (currentValue <= lowestSeenValue) {
				lowestSeenValue = currentValue;
			}

			undo (moves [i, 0], moves [i, 1], moves [i, 2],moves [i, 3]);
	
			//Alpha-Beta Calc
			beta = Mathf.Min(beta, lowestSeenValue);
			if (beta <= alpha) {
				return lowestSeenValue;
			}
		}

		return lowestSeenValue;
	}
		

	//***************************************************************
	//------------------------- NEW BOARD ---------------------------
	//***************************************************************

	public void newBoard(string spielf, string x, string y){
		
		int xPlatz;
		int yPlatz;
		bool found = false;

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				
				if (CopyGameboard [j, i] == spielf) {
					CopyGameboard [j, i] = null;
					if (x [0] == '-') {
						xPlatz = (int)(48 - ((int)x [1] - 48));
					} else {
						xPlatz = (int)x [0];
					}
					if (y [0] == '-') {
						yPlatz = (int)(48 - ((int)y [1] - 48));
					}else {
						yPlatz = (int)y[0];
					}

					CopyGameboard [j + xPlatz-48,i + yPlatz-48 ] = spielf;
					found = true;
					break;
				}

			}

			if (found) {
				break;
			}
		}

	}


	//***************************************************************
	//------------------------- UNDO --------------------------------
	//***************************************************************

	public void undo (string spielf, string x, string y, string geschlagen){
		
		int xPlatz;
		int yPlatz;
		bool found = false;

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				
				if (CopyGameboard [j, i] == spielf) {
					if (x [0] == '-') {
						xPlatz = (int)(48 - ((int)x [1] - 48));
					} else {
						xPlatz = (int)x [0];
					}
					if (y [0] == '-') {
						yPlatz = (int)(48 - ((int)y [1] - 48));
					}else {
						yPlatz = (int)y[0];
					}
					CopyGameboard [j - (xPlatz-48),i - (yPlatz-48) ] = spielf;
					CopyGameboard [j, i] = geschlagen;
					found = true;
					break;
				}

			}
			if (found) {
				break;
			}
		}

	}


	//***************************************************************
	//------------------------- ARRAY RESET -------------------------
	//***************************************************************

	public void ResetArray(){
		
		for(int i = 0; PossibleMoves[i,0] != null; i++){
			for (int j = 0; j < 4; j++) {
				
				PossibleMoves [i, j] = null;

			}
		}

		MoveCounter = 0;

	}


	//***************************************************************
	//------------------------- SET ALREADY CHECKED -----------------
	//***************************************************************

	public void SetChecked(){
		
		this.alreadyChecked = false;

	}


	//***************************************************************
	//------------------------- SET COPYBOARD -----------------------
	//***************************************************************

	public void SetBoard(int x, int y, string value){
		
		this.CopyGameboard [x, y] = value;

	}

	
	//***************************************************************
	//------------------------- BEWERTUNGSFUNKTION ------------------
	//***************************************************************

	int GetValue(){
		//TODO: Optimzing the evaluation
		 int[,] knightBoard = {	
			{-50,-40,-30,-30,-30,-30,-40,-50},
			{-40,-20,  0,  0,  0,  0,-20,-40},
			{-30,  0, 10, 15, 15, 10,  0,-30},
			{-30,  5, 15, 20, 20, 15,  5,-30},
			{-30,  0, 15, 20, 20, 15,  0,-30},
			{-30,  5, 10, 15, 15, 10,  5,-30},
			{-40,-20,  0,  5,  5,  0,-20,-40},
			{-50,-40,-30,-30,-30,-30,-40,-50}};

		 int[,] pawnBoard = {	
			{0,  0,  0,  0,  0,  0,  0,  0},
			{50, 50, 50, 50, 50, 50, 50, 50},
			{10, 10, 20, 30, 30, 20, 10, 10},
			{5,  5, 10, 25, 25, 10,  5,  5},
			{0,  0,  0, 20, 20,  0,  0,  0},
			{5, -5,-10,  0,  0,-10, -5,  5},
			{5, 10, 10,-20,-20, 10, 10,  5},
			{0,  0,  0,  0,  0,  0,  0,  0}};

		 int[,] bishopBoard = {	
			{-20,-10,-10,-10,-10,-10,-10,-20},
			{-10,  0,  0,  0,  0,  0,  0,-10},
			{-10,  0,  5, 10, 10,  5,  0,-10},
			{-10,  5,  5, 10, 10,  5,  5,-10},
			{-10,  0, 10, 10, 10, 10,  0,-10},
			{-10, 10, 10, 10, 10, 10, 10,-10},
			{-10,  5,  0,  0,  0,  0,  5,-10},
			{-20,-10,-10,-10,-10,-10,-10,-20}};

		 int[,] castleBoard = {	
			{ 0,  0,  0,  0,  0,  0,  0,  0},
			{5, 10, 10, 10, 10, 10, 10,  5},
			{-5,  0,  0,  0,  0,  0,  0, -5},
			{-5,  0,  0,  0,  0,  0,  0, -5},
			{-5,  0,  0,  0,  0,  0,  0, -5},
			{-5,  0,  0,  0,  0,  0,  0, -5},
			{-5,  0,  0,  0,  0,  0,  0, -5},
			{0,  0,  0,  5,  5,  0,  0,  0}};

		 int[,] queenBoard = {	
			{-20,-10,-10, -5, -5,-10,-10,-20},
			{-10,  0,  0,  0,  0,  0,  0,-10},
			{-10,  0,  5,  5,  5,  5,  0,-10},
			{-5,  0,  5,  5,  5,  5,  0, -5},
			{0,  0,  5,  5,  5,  5,  0, -5},
			{-10,  5,  5,  5,  5,  5,  0,-10},
			{-10,  0,  5,  0,  0,  0,  0,-10},
			{-20,-10,-10, -5, -5,-10,-10,-20}};

		 int[,] kingBoard = {	
			{-30,-40,-40,-50,-50,-40,-40,-30},
			{-30,-40,-40,-50,-50,-40,-40,-30},
			{-30,-40,-40,-50,-50,-40,-40,-30},
			{-30,-40,-40,-50,-50,-40,-40,-30},
			{-20,-30,-30,-40,-40,-30,-30,-20},
			{-10,-20,-20,-20,-20,-20,-20,-10},
			{20, 20,  0,  0,  0,  0, 20, 20},
			{20, 30, 10,  0,  0, 10, 30, 20}};

		 int[,] kingBoardLate = {
			{-50,-40,-30,-20,-20,-30,-40,-50},
			{-30,-20,-10,  0,  0,-10,-20,-30},
			{-30,-10, 20, 30, 30, 20,-10,-30},
			{-30,-10, 30, 40, 40, 30,-10,-30},
			{-30,-10, 30, 40, 40, 30,-10,-30},
			{-30,-10, 20, 30, 30, 20,-10,-30},
			{-30,-30,  0,  0,  0,  0,-30,-30},
			{-50,-30,-30,-30,-30,-30,-30,-50}};
		

		int enemyValue = 0;
		int mateValue = 0;
		int Value = 0;
	
		
		for(int y = 0; y < 8; y++){
			for (int x = 0; x < 8; x++) {

				if (CopyGameboard [x, y] != null) {
					
					pieceCounter++;
					//REGEX-------------------------------------------------------------
					//White
					Match wPieces = Regex.Match (CopyGameboard [x, y], @"W_[BSKDTL][1-8]");
					Match wPawn = Regex.Match (CopyGameboard [x, y], @"W_B[1-8]");
					Match wQueen = Regex.Match (CopyGameboard [x, y], @"W_D[1-8]");
					Match wBishop = Regex.Match (CopyGameboard [x, y], @"W_L[1-8]");
					Match wKnight = Regex.Match (CopyGameboard [x, y], @"W_S[1-8]");
					Match wCastle = Regex.Match (CopyGameboard [x, y], @"W_T[1-8]");
					//Black
					Match bPieces = Regex.Match (CopyGameboard [x, y], @"B_[BSKDTL][1-8]");
					Match bPawn = Regex.Match (CopyGameboard [x, y], @"B_B[1-8]");
					Match bQueen = Regex.Match (CopyGameboard [x, y], @"B_D[1-8]");
					Match bBishop = Regex.Match (CopyGameboard [x, y], @"B_L[1-8]");
					Match bKnight = Regex.Match (CopyGameboard [x, y], @"B_S[1-8]");
					Match bCastle = Regex.Match (CopyGameboard [x, y], @"B_T[1-8]");
					//-------------------------------------------------------------------
			
					if (wPieces.Success) {
						
						if (wPawn.Success) {
							enemyValue += (100+pawnBoard[7-y, x]);
						} else if (CopyGameboard [x, y] == "W_K1") {
							if (lastPieceCounter <= 16) {
								enemyValue += (9001+kingBoardLate [7 - y, x]);
							} else {
								enemyValue += (9001+kingBoard [7 - y, x]);
							}
						} else if (wQueen.Success) {
							enemyValue +=(900+queenBoard[7-y, x]);
						} else if (wBishop.Success) {
							enemyValue += (300+ bishopBoard[7-y, x]);
						} else if (wKnight.Success) {
							enemyValue += (300+knightBoard[7-y, x]);
						} else if (wCastle.Success) {
							enemyValue += (500+castleBoard[7-y, x]);
						}

					} else if (bPieces.Success) {

						if (bPawn.Success) {
							mateValue += (100 + pawnBoard[y, x]);
						} else if (CopyGameboard [x, y] == "B_K1") {
							if (lastPieceCounter <= 16) {
								mateValue +=(9001+ kingBoardLate[y, x]);
							} else {
								mateValue +=(9001+ kingBoard[y, x]);
							}
						} else if (bQueen.Success) {
							mateValue += (900+queenBoard[y, x]);
						} else if (bBishop.Success) {
							mateValue += (300 + bishopBoard[y, x]);
						} else if (bKnight.Success) {
							mateValue += (300 + knightBoard[y, x]);
						} else if (bCastle.Success) {
							mateValue += (500+ castleBoard[y, x]);
						}

					} // END OF IF bPiece


				} //END OF IF != NULL

			}
		}
		lastPieceCounter = pieceCounter;
		pieceCounter = 0;
		Value = mateValue - enemyValue;
		//Debug.Log ("mateValue\t | enemyValue\t | Value \n" + mateValue + "\t | " + enemyValue + "\t\t | " + Value);

	return Value;

	}
	
}
