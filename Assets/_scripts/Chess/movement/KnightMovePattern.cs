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

            addTilePost(_tilesPost, new(_piecePost.x + (i * _moveDistance), _piecePost.y + i), _whitePieceDict, _blackPieceDict);
            addTilePost(_tilesPost, new(_piecePost.x + (i * _moveDistance), _piecePost.y - i), _whitePieceDict, _blackPieceDict);
            addTilePost(_tilesPost, new(_piecePost.x - i, _piecePost.y + (i * _moveDistance)), _whitePieceDict, _blackPieceDict);
            addTilePost(_tilesPost, new(_piecePost.x + i, _piecePost.y + (i * _moveDistance)), _whitePieceDict, _blackPieceDict);
        }

        return _tilesPost;

    }

    void addTilePost(List<Vector2Int> _tilePost, Vector2Int _tileToAdd, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {

        Dictionary<Vector2Int, GameObject> _selectedPieceDict = new();

        bool _isWhitePost = _whitePieceDict.ContainsKey(_tileToAdd);

        if (_isWhitePost)
        {
            _selectedPieceDict = _whitePieceDict;
        }
        else
        {
            _selectedPieceDict = _blackPieceDict;
        }

        if (gridData.TileExists(_tileToAdd))
            if (!_selectedPieceDict.ContainsKey(_tileToAdd))
                _tilePost.Add(_tileToAdd);

    }

}
