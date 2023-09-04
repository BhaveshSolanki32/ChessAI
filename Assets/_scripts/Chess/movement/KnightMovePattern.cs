using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChessPieceData))]
public class KnightMovePattern : BasePieceMovementPattern, IPiece
{
    public int MoveDinstance { get; set; } = 3;

    public List<Vector2Int> MovableTilePosts(Vector2Int _post, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        List<Vector2Int> _tilePost = allDirectionalMovePattern(MoveDinstance - 1, _post,_whitePieceDict, _blackPieceDict);

        return _tilePost;
    }

    protected override List<Vector2Int> allDirectionalMovePattern(int _moveDistance, Vector2Int _piecePost, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict , bool _isDiagonalMovement = false)
    {
        List<Vector2Int> _tilesPost = new();

        for (int i = -1; i <= 1; i++)
        {
            if (i == 0) continue;

            addTilePost(ref _tilesPost, new(_piecePost.x + (i * _moveDistance), _piecePost.y + i));
            addTilePost(ref _tilesPost, new(_piecePost.x + (i * _moveDistance), _piecePost.y - i));
            addTilePost(ref _tilesPost, new(_piecePost.x - i, _piecePost.y + (i * _moveDistance)));
            addTilePost(ref _tilesPost, new(_piecePost.x + i, _piecePost.y + (i * _moveDistance)));
        }

        return _tilesPost;

    }

    void addTilePost(ref List<Vector2Int> _tilePost, Vector2Int _tileToAdd)
    {
        if (gridData.TileExists(_tileToAdd))
            if (!pieceData.DoesPieceExist(_tileToAdd, (transform.parent.name == "white peices")))
                _tilePost.Add(_tileToAdd);
    }

}
