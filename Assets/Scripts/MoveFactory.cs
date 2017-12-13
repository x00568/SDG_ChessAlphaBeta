using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveFactory : MoveInterface
{
    private static MoveFactory instance;
    private MoveFactory() { }
    public static MoveFactory Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new MoveFactory();
            }
            return instance;
        }
    }

    Board _board;
    List<Move> moves = new List<Move>();
    Dictionary<Piece.pieceType, System.Action> pieceToFunction = new Dictionary<Piece.pieceType, System.Action>();

    private Piece _piece;
    private Piece.pieceType _type;
    private Piece.playerColor _player;
    private Vector2 _position;

    public MoveFactory(Board board)
    {

        _board = board;
        pieceToFunction.Add(Piece.pieceType.PAWN, _GetPawnMoves);
        pieceToFunction.Add(Piece.pieceType.ROOK, _GetRookMoves);
        pieceToFunction.Add(Piece.pieceType.KNIGHT, _GetKnightMoves);
        pieceToFunction.Add(Piece.pieceType.BISHOP, _GetBishopMoves);
        pieceToFunction.Add(Piece.pieceType.QUEEN, _GetQueenMoves);
        pieceToFunction.Add(Piece.pieceType.KING, _GetKingMoves);
    }

    public List<Move> GetMoves(Piece piece, Vector2 position)
    {
        _piece = piece;
        _type = piece.Type;
        _player = piece.Player;
        _position = position;

        foreach (KeyValuePair<Piece.pieceType, System.Action> p in pieceToFunction)
        {
            if (_type == p.Key)
            {
                p.Value.Invoke();
            }
        }

        return moves;
    }

   public void _GetPawnMoves()
    {
        if (_piece.Player == Piece.playerColor.BLACK)
        {
            int limit = _piece.HasMoved ? 2 : 3;
            _GenerateMove(limit, new Vector2(0, 1));

            Vector2 diagLeft = new Vector2(_position.x - 1, _position.y + 1);
            Vector2 diagRight = new Vector2(_position.x + 1, _position.y + 1);
            Tile dl = null;
            Tile dr = null;
            if (_IsOnBoard(diagLeft))
            {
                dl = _board.GetTileFromBoard(diagLeft);
            }
            if (_IsOnBoard(diagRight))
            {
                dr = _board.GetTileFromBoard(diagRight);
            }

            if (dl != null && _ContainsPiece(dl) && _IsEnemy(dl))
            {
                _CheckAndStoreMove(diagLeft);
            }
            if (dr != null && _ContainsPiece(dr) && _IsEnemy(dr))
            {
                _CheckAndStoreMove(diagRight);
            }
        }
        else
        {
            int limit = _piece.HasMoved ? 2 : 3;
            _GenerateMove(limit, new Vector2(0, -1));

            Vector2 diagLeft = new Vector2(_position.x - 1, _position.y - 1);
            Vector2 diagRight = new Vector2(_position.x + 1, _position.y - 1);
            Tile dl = null;
            Tile dr = null;
            if (_IsOnBoard(diagLeft))
            {
                dl = _board.GetTileFromBoard(diagLeft);
            }
            if (_IsOnBoard(diagRight))
            {
                dr = _board.GetTileFromBoard(diagRight);
            }

            if (dl != null && _ContainsPiece(dl) && _IsEnemy(dl))
            {
                _CheckAndStoreMove(diagLeft);
            }
            if (dr != null && _ContainsPiece(dr) && _IsEnemy(dr))
            {
                _CheckAndStoreMove(diagRight);
            }
        }
    }

   public void _GetRookMoves()
    {
        _GenerateMove(9, new Vector2(0, 1));
        _GenerateMove(9, new Vector2(0, -1));
        _GenerateMove(9, new Vector2(1, 0));
        _GenerateMove(9, new Vector2(-1, 0));
    }

   public void _GetKnightMoves()
    {
        Vector2 move;
        move = new Vector2(_position.x + 2, _position.y + 1);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x + 2, _position.y - 1);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x - 2, _position.y + 1);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x - 2, _position.y - 1);
        _CheckAndStoreMove(move);

        move = new Vector2(_position.x + 1, _position.y - 2);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x + 1, _position.y + 2);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x - 1, _position.y + 2);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x - 1, _position.y - 2);
        _CheckAndStoreMove(move);
    }

   public void _GetBishopMoves()
    {
        _GenerateMove(9, new Vector2(1, 1));
        _GenerateMove(9, new Vector2(-1, -1));
        _GenerateMove(9, new Vector2(1, -1));
        _GenerateMove(9, new Vector2(-1, 1));
    }

   public void _GetKingMoves()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                _CheckAndStoreMove(new Vector2(_position.x + x, _position.y + y));
            }
        }
    }

   public void _GetQueenMoves()
    {
        _GetBishopMoves();
        _GetRookMoves();
    }

    void _GenerateMove(int limit, Vector2 direction)
    {
        for (int i = 1; i < limit; i++)
        {
            Vector2 move = _position + direction * i;
            if (_IsOnBoard(move) && _ContainsPiece(_board.GetTileFromBoard(move)))
            {
                if (_IsEnemy(_board.GetTileFromBoard(move)) && _type != Piece.pieceType.PAWN)
                {
                    _CheckAndStoreMove(move);
                }
                break;
            }
            _CheckAndStoreMove(move);
        }
    }

    void _CheckAndStoreMove(Vector2 move)
    {
        if (_IsOnBoard(move) && (!_ContainsPiece(_board.GetTileFromBoard(move)) || _IsEnemy(_board.GetTileFromBoard(move))))
        {
            Move m = new Move();
            m.firstPosition = _board.GetTileFromBoard(_position);
            m.pieceMoved = _piece;
            m.secondPosition = _board.GetTileFromBoard(move);

            if (m.secondPosition != null)
                m.pieceKilled = m.secondPosition.CurrentPiece;
            moves.Add(m);
        }
    }

    bool _IsEnemy(Tile tile)
    {
        if (_player != tile.CurrentPiece.Player)
            return true;
        else
            return false;
    }

    bool _ContainsPiece(Tile tile)
    {
        if (!_IsOnBoard(tile.Position))
            return false;

        if (tile.CurrentPiece != null)
            return true;
        else
            return false;
    }

    bool _IsOnBoard(Vector2 point)
    {
        if (point.x >= 0 && point.y >= 0 && point.x < 8 && point.y < 8)
            return true;
        else
            return false;
    }

    Vector2 tempVector;
    public bool CheckKing()
    {
        kingAttack = false;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                tempVector = new Vector2(i, j);
                if (Board.Instance.GetTileFromBoard(tempVector).CurrentPiece != null)
                {
                    Piece p = Board.Instance.GetTileFromBoard(tempVector).CurrentPiece;
                    Piece.pieceType type = Board.Instance.GetTileFromBoard(tempVector).CurrentPiece.Type;
                    Piece.playerColor color = Board.Instance.GetTileFromBoard(tempVector).CurrentPiece.Player;
                    switch (type)
                    {
                        case Piece.pieceType.PAWN:
                            PawnMove(color, tempVector, p);
                            break;
                        case Piece.pieceType.ROOK:
                            RookMove(color, tempVector, p);
                            break;
                        case Piece.pieceType.BISHOP:
                            BishopMove(color, tempVector, p);
                            break;
                        case Piece.pieceType.KNIGHT:
                            KnightMove(color, tempVector, p);
                            break;
                        case Piece.pieceType.QUEEN:
                            QueenMove(color, tempVector, p);
                            break;
                        case Piece.pieceType.KING:
                            KingMove(color, tempVector, p);
                            break;
                    }
                }
            }
        }
        return kingAttack;
    }
     bool kingAttack = false;
     public bool checkColor = false;

    void PawnMove(Piece.playerColor c, Vector2 temp, Piece p)
    {
        if (p.Player == Piece.playerColor.BLACK)
        {
            int limit = p.HasMoved ? 2 : 3;
            _LineMove(limit, new Vector2(0, 1), temp, c);
            Vector2 diagLeft = new Vector2(temp.x - 1, temp.y + 1);
            Vector2 diagRight = new Vector2(temp.x + 1, temp.y + 1);
            if (_IsOnBoard(diagLeft))
            {
                CheckMove(diagLeft, c);
            }
            if (_IsOnBoard(diagRight))
            {
                CheckMove(diagRight, c);
            }
        }
    }

    void RookMove(Piece.playerColor c, Vector2 temp, Piece p)
    {
        _LineMove(9, new Vector2(0, 1), temp, c);
        _LineMove(9, new Vector2(0, -1), temp, c);
        _LineMove(9, new Vector2(1, 0), temp, c);
        _LineMove(9, new Vector2(-1, 0), temp, c);
    }
    void KnightMove(Piece.playerColor c, Vector2 temp, Piece p)
    {
        Vector2 move;
        move = new Vector2(temp.x + 2, temp.y + 1);
        CheckMove(move, c);
        move = new Vector2(temp.x + 2, temp.y - 1);
        CheckMove(move, c);
        move = new Vector2(temp.x - 2, temp.y + 1);
        CheckMove(move, c);
        move = new Vector2(temp.x - 2, temp.y - 1);
        CheckMove(move, c);

        move = new Vector2(temp.x + 1, temp.y - 2);
        CheckMove(move, c);
        move = new Vector2(temp.x + 1, temp.y + 2);
        CheckMove(move, c);
        move = new Vector2(temp.x - 1, temp.y + 2);
        CheckMove(move, c);
        move = new Vector2(temp.x - 1, temp.y - 2);
        CheckMove(move, c);
    }
    void BishopMove(Piece.playerColor c, Vector2 temp, Piece p)
    {
        _LineMove(9, new Vector2(1, 1), temp, c);
        _LineMove(9, new Vector2(-1, -1), temp, c);
        _LineMove(9, new Vector2(1, -1), temp, c);
        _LineMove(9, new Vector2(-1, 1), temp, c);
    }
    void KingMove(Piece.playerColor c, Vector2 temp, Piece p)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                CheckMove(new Vector2(_position.x + x, _position.y + y), c);
            }
        }
    }
    void QueenMove(Piece.playerColor c, Vector2 temp, Piece p)
    {
        BishopMove(c, temp, p);
        RookMove(c, temp, p);
    }

    //直走路径
    void _LineMove(int limit, Vector2 direction, Vector2 tempPosition, Piece.playerColor co)
    {
        for (int i = 1; i < limit; i++)
        {
            Vector2 move = tempPosition + direction * i;
            if (_IsOnBoard(move) && _ContainsPiece(Board.Instance.GetTileFromBoard(move)))
            {
                if (Board.Instance.GetTileFromBoard(tempPosition).CurrentPiece.Type != Piece.pieceType.PAWN)
                {
                    if (co != Board.Instance.GetTileFromBoard(move).CurrentPiece.Player)
                    {
                        CheckMove(move, co);
                    }
                    break;
                }
            }
        }
    }
    //通用检查
    void CheckMove(Vector2 v, Piece.playerColor c)
    {
        if (_IsOnBoard(v) && _ContainsPiece(Board.Instance.GetTileFromBoard(v)))
        {
            if (c != Board.Instance.GetTileFromBoard(v).CurrentPiece.Player)
            {
                if (Board.Instance.GetTileFromBoard(v).CurrentPiece.Type == Piece.pieceType.KING)
                {
                    kingAttack = true;
                    if (Board.Instance.GetTileFromBoard(v).CurrentPiece.Player == Piece.playerColor.BLACK)
                        checkColor = true;
                    else
                        checkColor = false;
                }
            }
        }
    }
}
