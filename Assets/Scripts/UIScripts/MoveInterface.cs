using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MoveInterface {
    void _GetPawnMoves();
    void _GetRookMoves();
    void _GetKnightMoves();
    void _GetBishopMoves();
    void _GetKingMoves();
    void _GetQueenMoves();
}
