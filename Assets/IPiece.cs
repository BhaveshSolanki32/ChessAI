using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPiece //should be implemented by every kind of piece
{
    int MoveDinstance { get; set; }
    ChessPieceData ThisPiece { get; set; }

    List<Vector2Int> MovableTilePosts();
}
