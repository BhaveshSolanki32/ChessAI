using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChessPieceData))]
public class KingMovePattern : BasePieceMovementPattern, IPiece
{
    public int MoveDinstance { get; set; } = 1;

    public List<Vector2Int> MovableTilePosts()
    {
        return allDirectionalMovePattern(MoveDinstance, ThisPiece.Post);
    }
}
