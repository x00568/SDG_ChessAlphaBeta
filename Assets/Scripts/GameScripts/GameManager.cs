using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DifficultyEnum
{
    simple = 2,
    medium = 3,
    difficult = 4
}
public class GameManager : MonoBehaviour
{

    List<Move> moveList = new List<Move>();
    AlphaBeta ab = new AlphaBeta();
    public bool _kingDead = false;
    public float timer = 0;
    Board _board;
    public bool win;
    public bool whoWin;
    public bool kingAttack;
    public bool playerTurn = true;
    void Start()
    {
        _board = Board.Instance;
        SetupBoard();
    }
    //难度设置
    public int SetDifficult(DifficultyEnum diffi)
    {
        return (int)diffi;
    }
    void Update()
    {

        if (!playerTurn && timer < 3)
        {
            timer += Time.deltaTime;
        }
        else if (!playerTurn && timer >= 3)
        {
            AIMove();
            timer = 0;
        }
    }

    //初始化棋盘
    public void SetupBoard()
    {
        _board.SetupBoard();
    }
    //调用AI
    public void AIMove()
    {
        Move move = ab.GetMove();
        _DoAIMove(move);
    }
    void _DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == Piece.pieceType.KING)
        {

            SwapPieces(move);
            if (secondPosition.CurrentPiece.Player == Piece.playerColor.BLACK)
                win = true;
            else if (secondPosition.CurrentPiece.Player == Piece.playerColor.WHITE)
                win = false;
            _kingDead = true;
        }
        else
        {
            SwapPieces(move);
        }
    }
    //选择棋子走动位置,并将棋子的移动记录在列表中以供悔棋使用
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
                    win = true;
                else if (secondTile.CurrentPiece.Player == Piece.playerColor.WHITE)
                    win = false;
            }

            move.Killed = secondTile.CurrentPiece;
            secondTile.CurrentPiece.gameObject.SetActive(false);
        }

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.position = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

        moveList.Add(move);
        kingAttack = MoveFactory.Instance.CheckKing();
        if (kingAttack)
        {
            if (MoveFactory.Instance.checkColor)
            {
                whoWin = true;
                Debug.Log("黑方Check");
            }
            else
            {
                whoWin = true;
                Debug.Log("白方Check");
            }
        }
        playerTurn = !playerTurn;
    }
   //悔棋
    public void ReturnChess()
    {
        if (moveList.Count <= 0)
            return;

        timer = 0;
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }

        Move tempMove = moveList[moveList.Count - 1];
        moveList.RemoveAt(moveList.Count - 1);

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

    //判断谁走棋
    public bool WhoPlay()
    {
        return playerTurn;
    }
    //白棋赢
    public bool WhiteWin()
    {
        if(_kingDead)
        {
            if (win == false)
            return win;
        }
        return false;
    }
    //黑色赢
    public bool BlackWin()
    {
        if (_kingDead)
        {
               return true;
        }
        return false;
    }
    //白色King受威胁
    public bool WhiteAttack()
    {
        if (kingAttack)
        {
            if (MoveFactory.Instance.checkColor)
                return true;
        }
        return false;
    }
    //黑色King受威胁
    public bool BlackAttack()
    {
        if (kingAttack)
        {
            if (!MoveFactory.Instance.checkColor)
                return true;
        }
        return false;
    }
   
}
