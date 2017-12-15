using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPiece : MonoBehaviour {
    Piece piece;

    private List<Move> moves = new List<Move>();
    void Start()
    {
        piece = this.GetComponent<Piece>();
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && piece.Player == Piece.playerColor.WHITE && GameManager.Instance.playerTurn)
        {
            moves.Clear();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }
            moves = GameControl.Instance.CanMoves(piece,piece.position);
            foreach (Move move in moves)
            {
                if (move.pieceKilled == null)
                {
                    GameObject instance = Instantiate(Resources.Load("MoveCube")) as GameObject;
                    instance.transform.position = new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y);
                    instance.GetComponent<Container>()._move = move;
                }
                else if (move.pieceKilled != null)
                {
                    GameObject instance = Instantiate(Resources.Load("KillCube")) as GameObject;
                    instance.transform.position = new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y);
                    instance.GetComponent<Container>()._move = move;
                }
            }

            GameObject i = Instantiate(Resources.Load("CurrentPiece")) as GameObject;
            i.transform.position = this.transform.position;
        }
    }
}
