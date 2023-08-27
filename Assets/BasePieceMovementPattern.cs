using System.Collections.Generic;
using UnityEngine;

public class BasePieceMovementPattern : MonoBehaviour //contains function for common piece movement pattern
{
    public ChessPieceData ThisPiece { get; set; }
    protected GridData gridData;
    protected PiecesData pieceData;
    protected virtual void Awake()
    {
        ThisPiece = GetComponent<ChessPieceData>();
        gridData = ThisPiece.GridData;
        if (!transform.root.TryGetComponent<PiecesData>(out pieceData))
            Debug.LogError("PieceData not found");
    }
    protected virtual List<Vector2Int> fixedDirectionalMovePattern(int _moveDistance, Vector2Int _piecePost, Vector2Int _directionVector)
    {
        List<Vector2Int> _tilesPost = new();
        for (int i = 1; i <= _moveDistance; i++)
        {
            Vector2Int _nextPost = new(_piecePost.x + (_directionVector.x * i), _piecePost.y + (_directionVector.y * i));
            if (gridData.TileExists(_nextPost))
                if (!pieceData.DoesPieceExist(_nextPost,(transform.parent.name == "white peices")))//if white piece do not exist at the location
                {
                    _tilesPost.Add(_nextPost);
                    if (pieceData.DoesPieceExist(_nextPost, (transform.parent.name != "white peices")))
                    {
                        break;
                    }
                }
                else
                    break;
                    
        }

        return _tilesPost;
    }

    protected virtual List<Vector2Int> allDirectionalMovePattern(int _moveDistance, Vector2Int _piecePost, bool _isDiagonalMovement = false)
    {
        List<Vector2Int> _tiles = new();

        if (!_isDiagonalMovement)
        {
            _tiles.AddRange( fixedDirectionalMovePattern(_moveDistance, _piecePost, new(1, 0)));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(-1, 0)));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(0, 1)));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(0, -1)));
        }
        else
        {
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(1, 1)));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(-1, -1)));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(-1, 1)));
            _tiles.AddRange(fixedDirectionalMovePattern(_moveDistance, _piecePost, new(1, -1)));
        }



        return _tiles;
    }

}
