using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ChessPieceData))]
public class RookMovePattern : BasePieceMovementPattern, IPiece
{
    public int MoveDinstance { get; set; } = 8;

    public List<Vector2Int> MovableTilePosts()
    {

        List<Vector2Int> _tilePost = allDirectionalMovePattern(MoveDinstance, ThisPiece.Post);

        return _tilePost;
    }

}
