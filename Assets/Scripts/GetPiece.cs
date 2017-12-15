using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPiece : MonoBehaviour {
    Piece _piece;
    GameManager manager;
    MoveFactory factory = new MoveFactory(Board.Instance);
    private List<Move> moves = new List<Move>();
	// Use this for initialization
	void Start () {
        _piece = this.GetComponent<Piece>();
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && _piece.Player == Piece. playerColor.WHITE && manager.playerTurn)
        {
            moves.Clear();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }

            moves = factory.GetMoves(_piece, _piece.position);
            foreach (Move move in moves)
            {
                if (move.pieceKilled == null)
                {
                    GameObject instance = Instantiate(Resources.Load("MoveCube")) as GameObject;
                    instance.transform.position = new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y);
                    instance.GetComponent<Container>().move = move;
                }
                else if (move.pieceKilled != null)
                {
                    GameObject instance = Instantiate(Resources.Load("KillCube")) as GameObject;
                    instance.transform.position = new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y);
                    instance.GetComponent<Container>().move = move;
                }
            }
            GameObject i = Instantiate(Resources.Load("CurrentPiece")) as GameObject;
            i.transform.position = _piece .transform.position;
        }
    }
}
