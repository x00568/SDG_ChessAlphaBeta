using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    List<Move> _moveList = new List<Move>();
    AlphaBeta ab = new AlphaBeta();
    public bool _kingDead = false;
    public float timer = 0;
    Board _board;
    public bool _win;
    public bool _whoAttack;
    public bool _kingAttack;
    public bool playerTurn = true;
    void Start()
    {
        _board = Board.Instance;
        _board.SetupBoard();
    }

    void Update()
    {
       
        if (!playerTurn && timer < 3)
        {
            Debug.Log(111);
            timer += Time.deltaTime;
        }
        else if (!playerTurn && timer >= 3)
        {
            Move move = ab.GetMove();
            _DoAIMove(move);
            timer = 0;
        }
    }



    void _DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == Piece.pieceType.KING)
        {

            SwapPieces(move);
            if (secondPosition.CurrentPiece.Player == Piece.playerColor.BLACK)
                _win = true;
            else if (secondPosition.CurrentPiece.Player == Piece.playerColor.WHITE)
                _win = false;
            _kingDead = true;
        }
        else
        {
            SwapPieces(move);
        }
    }

    public void SwapPieces(Move move)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

        firstTile.CurrentPiece.MovePiece(new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y));

        if (secondTile.CurrentPiece != null)
        {
            if (secondTile.CurrentPiece.Type == Piece.pieceType.KING)
            {
                _kingDead = true;
                if (secondTile.CurrentPiece.Player == Piece.playerColor.BLACK)
                    _win = true;
                else if (secondTile.CurrentPiece.Player == Piece.playerColor.WHITE)
                    _win = false;
            }

            move.Killed = secondTile.CurrentPiece;
            // Destroy(secondTile.CurrentPiece.gameObject);
            secondTile.CurrentPiece.gameObject.SetActive(false);
        }

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.position = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

        _moveList.Add(move);
        _kingAttack = MoveFactory.Instance.CheckKing();
        if (_kingAttack)
        {
            if (MoveFactory.Instance.checkColor)
            {
                _whoAttack = true;
                Debug.Log("黑方Check");
            }
            else
            {
                _whoAttack = true;
                Debug.Log("白方Check");
            }
        }
        playerTurn = !playerTurn;
    }

    public void ReturnChess()
    {

        if (_moveList.Count <= 0)
            return;

        timer = 0;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }

        Move tempMove = _moveList[_moveList.Count - 1];
        _moveList.RemoveAt(_moveList.Count - 1);

        tempMove.secondPosition.CurrentPiece = null;
        if (tempMove.Killed != null)
        {
            // GameObject go = Instantiate(tempMove.Killed.gameObject);
            tempMove.Killed.gameObject.SetActive(true);
            tempMove.Killed.transform.position = new Vector3(-tempMove.secondPosition.Position.x, 0, tempMove.secondPosition.Position.y);
            tempMove.secondPosition.CurrentPiece = tempMove.Killed;

        }
        if (tempMove.pieceMoved != null)
        {
            tempMove.pieceMoved.MovePiece(new Vector3(-tempMove.firstPosition.Position.x, 0, tempMove.firstPosition.Position.y));
        }
        tempMove.firstPosition.CurrentPiece = tempMove.pieceMoved;
        tempMove.pieceMoved.position = tempMove.firstPosition.Position;
        if ((tempMove.firstPosition.CurrentPiece != null))
            if ((tempMove.firstPosition.CurrentPiece.position.y == 1 || tempMove.firstPosition.CurrentPiece.position.y == 6))
                tempMove.firstPosition.CurrentPiece.HasMoved = false;
        if (tempMove.secondPosition.CurrentPiece != null)
            if ((tempMove.secondPosition.CurrentPiece.position.y == 1 || tempMove.secondPosition.CurrentPiece.position.y == 6))
                tempMove.secondPosition.CurrentPiece.HasMoved = false;
        playerTurn = !playerTurn;
    }
}