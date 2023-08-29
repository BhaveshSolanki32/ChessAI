using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChessPieceData))]
public class KingMovePattern : BasePieceMovementPattern, IPiece
{
    public int MoveDinstance { get; set; } = 1;

    public List<Vector2Int> MovableTilePosts()
    {
        List<Vector2Int> _tilePosts = new();
        _tilePosts.AddRange(allDirectionalMovePattern(MoveDinstance, ThisPiece.Post));
        _tilePosts.AddRange(allDirectionalMovePattern(MoveDinstance, ThisPiece.Post, true));
        return _tilePosts;


    }
}
