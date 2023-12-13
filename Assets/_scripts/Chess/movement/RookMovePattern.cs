using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ChessPieceData))]
public class RookMovePattern : BasePieceMovementPattern, IPiece
{
    public int MoveDinstance { get; set; } = 8;

    public List<Vector2Int> MovableTilePosts(Vector2Int post, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict)
    {

        var tilePost = allDirectionalMovePattern(MoveDinstance, post, whitePieceDict, blackPieceDict);

        return tilePost;
    }

}
