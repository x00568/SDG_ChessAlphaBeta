using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DifficultyEnum
{
    simple = 2,
    medium = 3,
    difficult = 4
}
public class GameControl : MonoBehaviour
{
    private static GameControl _instance = null;
    public static GameControl Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameControl();
            }
            return _instance;
        }
    }
   
    AlphaBeta _alphaBeta = new AlphaBeta();


    //初始化棋盘
    public void SetupBoard()
    {
        Board.Instance.SetupBoard();
    }
    //难度选择
    public void SetDifficult(DifficultyEnum diffi)
    {
        _alphaBeta._maxDepth = (int)diffi;
    }
    //悔棋
    public void Undo()
    {
        GameManager.Instance.ReturnChess();
    }
    //判断白色棋子赢
    public bool WhiteWin()
    {
        if (GameManager.Instance._kingDead)
        {
            if (GameManager.Instance._win == false)
                return true;
        }
        return false;
    }
    //判断黑色棋子赢
    public bool BlackWin()
    {
        if (GameManager.Instance._kingDead)
        {
            if (GameManager.Instance._win)
                return true;
        }
        return false;
    }
    //白色棋子移动
    public bool WhichPlay()
    {
        if (GameManager.Instance.playerTurn)
            return true;
        else
            return false;
    }
    //白色棋子移动

    //白色King棋子是否受到威胁
    public bool WhiteKingCheck()
    {
        if (GameManager.Instance._kingAttack)
        {
            if (GameManager.Instance._whoAttack)
                return true;
            else
                return false;
        }
        else
            return false;
    }
    //黑色King棋子是否受到威胁
    public bool BlackKingCheck()
    {
        if (GameManager.Instance._kingAttack)
        {
            if (GameManager.Instance._whoAttack == false)
                return true;
            else
                return false;
        }
        else
            return false;
    }
    List<Move> moves = new List<Move>();
    //计算当前棋子的可走区域,将可走区域和吃子添加到Move的List列表中，记录第一块棋子和棋子将要落得位置，同时将Move参数绑定道可移动位置，为后面交换棋子做准备
    public List<Move> CanMoves(Piece piece, Vector2 selfPosition)
    {
        moves.Clear();
        MoveFactory factory = new MoveFactory(Board.Instance);
        moves = factory.GetMoves(piece, selfPosition);
        return moves;
    }
    //选择棋子走动位置,并将棋子的移动记录在列表中以供悔棋使用
    public void SwapChess(Move move)
    {
        GameManager.Instance.SwapPieces(move);
    }

}
//记录当前选择棋子，以及可走区域的信息
public class Move
{
    public Tile firstPosition = null;
    public Tile secondPosition = null;
    public Piece pieceMoved = null;
    public Piece pieceKilled = null;
    public Piece Killed = null;
    public int score = -100000000;
}
//棋盘格，记录每个棋盘格的信息
public class Tile
{
    private Vector2 _position = Vector2.zero;
    public Vector2 Position
    {
        get { return _position; }
    }

    private Piece _currentPiece = null;
    public Piece CurrentPiece
    {
        get { return _currentPiece; }
        set { _currentPiece = value; }
    }

    public Tile(int x, int y)
    {
        _position.x = x;
        _position.y = y;

        if (y == 0 || y == 1 || y == 6 || y == 7)
        {
            _currentPiece = GameObject.Find(x.ToString() + " " + y.ToString()).GetComponent<Piece>();
        }
    }

    public void SwapFakePieces(Piece newPiece)
    {
        _currentPiece = newPiece;
    }
}
//划分棋盘格
public class Board
{
    private static Board _instance = null;
    public static Board Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Board();
            }
            return _instance;
        }
    }

    private Tile[,] _board = new Tile[8, 8];

    public void SetupBoard()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                _board[x, y] = new Tile(x, y);
            }
        }
    }

    public Tile GetTileFromBoard(Vector2 tile)
    {
        return _board[(int)tile.x, (int)tile.y];
    }
}



