using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPiece //should be implemented by every kind of piece
{
    int MoveDinstance { get; set; }
    ChessPieceData ThisPiece { get; set; }

    List<Vector2Int> MovableTilePosts(Vector2Int _post, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict);
}
