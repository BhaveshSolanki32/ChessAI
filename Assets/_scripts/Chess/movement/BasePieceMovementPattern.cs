using System.Collections.Generic;
using UnityEngine;

public class BasePieceMovementPattern : MonoBehaviour //contains function for common piece movement pattern
{
    public ChessPieceData ThisPiece { get; set; }
    protected GridData gridData;
    protected PiecesDictsData pieceData;
    protected virtual void Awake()
    {
        ThisPiece = GetComponent<ChessPieceData>();
        gridData = ThisPiece.GridData;
        if (!transform.root.TryGetComponent<PiecesDictsData>(out pieceData))
            Debug.LogError("PieceData not found");
    }
    protected virtual List<Vector2Int> fixedDirectionalMovePattern(int _moveDistance, Vector2Int _piecePost, Vector2Int _directionVector, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict )
    {
        List<Vector2Int> _tilesPost = new();
        Dictionary<Vector2Int, GameObject> _selectedPieceDict = new();
        Dictionary<Vector2Int, GameObject> _oppponentPieceDict = new();

        bool _isWhitePost = _whitePieceDict.ContainsKey(_piecePost);

        if (_isWhitePost)
        {
            _selectedPieceDict = _whitePieceDict;
            _oppponentPieceDict = _blackPieceDict;
        }
        else
        {
            _selectedPieceDict = _blackPieceDict;
            _oppponentPieceDict = _whitePieceDict;
        }
        for (int i = 1; i <= _moveDistance; i++)
        {
            Vector2Int _nextPost = new(_piecePost.x + (_directionVector.x * i), _piecePost.y + (_directionVector.y * i));
            if (gridData.TileExists(_nextPost))
                if (!_selectedPieceDict.ContainsKey(_nextPost))//if slected color piece do not exist at the location
                {
                    _tilesPost.Add(_nextPost);
                    if (_oppponentPieceDict.ContainsKey(_nextPost))
                    {
                        break;
                    }
                }
                else
                    break;

        }
        return _tilesPost;
    }

    protected virtual List<Vector2Int> allDirectionalMovePattern(int _moveDistance, Vector2Int _piecePost, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict, bool _isDiagonalMovement = false)
    {
        List<Vector2Int> _tiles = new();

        if (!_isDiagonalMovement)
        {
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(1, 0), _whitePieceDict, _blackPieceDict));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(-1, 0), _whitePieceDict, _blackPieceDict));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(0, 1), _whitePieceDict, _blackPieceDict));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(0, -1), _whitePieceDict, _blackPieceDict));
        }
        else
        {
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(1, 1), _whitePieceDict, _blackPieceDict));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(-1, -1), _whitePieceDict, _blackPieceDict));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(-1, 1), _whitePieceDict, _blackPieceDict));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(1, -1), _whitePieceDict, _blackPieceDict));
        }



        return _tiles;
    }


}
