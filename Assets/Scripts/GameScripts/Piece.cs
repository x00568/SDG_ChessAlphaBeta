using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
    public enum pieceType { KING, QUEEN, BISHOP, ROOK, KNIGHT, PAWN, UNKNOWN = -1 };
    public enum playerColor { BLACK, WHITE, UNKNOWN = -1 };

    [SerializeField]
    private pieceType _type = pieceType.UNKNOWN;
    [SerializeField]
    private playerColor _player = playerColor.UNKNOWN;
    public pieceType Type
    {
        get { return _type; }
    }
    public playerColor Player
    {
        get { return _player; }
    }

    public Sprite pieceImage = null;
    public Vector2 position;
    private Vector3 moveTo;


    private bool _hasMoved = false;
    public bool HasMoved
    {
        get { return _hasMoved; }
        set { _hasMoved = value; }
    }

    public void MovePiece(Vector3 position)
    {
        moveTo = position;
    }

    void Start()
    {
        moveTo = this.transform.position;
        
    }

    void Update()
    {
        transform.position = Vector3.Lerp(this.transform.position, moveTo, 3 * Time.deltaTime);
    }
}
