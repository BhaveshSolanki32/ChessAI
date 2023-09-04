using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChessPieceData))]
public class KingMovePattern : BasePieceMovementPattern, IPiece
{
    public int MoveDinstance { get; set; } = 1;

    public List<Vector2Int> MovableTilePosts(Vector2Int _post, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        List<Vector2Int> _tilePosts = new();
        _tilePosts.AddRange(allDirectionalMovePattern(MoveDinstance, _post,_whitePieceDict,_blackPieceDict));
        _tilePosts.AddRange(allDirectionalMovePattern(MoveDinstance, _post,_whitePieceDict,_blackPieceDict,  true));
        return _tilePosts;


    }
}
