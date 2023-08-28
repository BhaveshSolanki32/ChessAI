using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChessPieceData))]
public class PawnMovePattern : BasePieceMovementPattern, IPiece
{
    public int MoveDinstance { get; set; } = 2;
    MovePiece movePiece;
    
    protected override void Awake()
    {
        base.Awake();

        if (transform.parent.parent.TryGetComponent<MovePiece>(out movePiece))
            movePiece.OnPieceStartMoving += firstMoveDone;
        else
            Debug.LogError("MovePiece not fund", gameObject);

    }

     void firstMoveDone(GameObject _piece, Vector2Int vector2Int)
    {
        if(_piece == gameObject)
        {
            movePiece.OnPieceStartMoving -= firstMoveDone;
            MoveDinstance = 1;
        }
       
    }

    public void OnDestroy()
    {
        movePiece.OnPieceStartMoving -= firstMoveDone;
    }



    public List<Vector2Int> MovableTilePosts()
    {
        int _movementDir = 1; // for the black team we have to get forward posts
        bool _isWhiteTurn = pieceData.DoesPieceExist(GetComponent<ChessPieceData>().Post, true);
        if (_isWhiteTurn) //is white piece
         _movementDir = 1; 
        else
            _movementDir = -1;

        List<Vector2Int> _tiles = fixedDirectionalMovePattern(MoveDinstance, ThisPiece.Post, new(0, 1 * _movementDir));
        checkPawnAttackLocations(ref _tiles, _isWhiteTurn);
        return _tiles;
    }

    protected override List<Vector2Int> fixedDirectionalMovePattern(int _moveDistance, Vector2Int _piecePost, Vector2Int _directionVector)
    {
        List<Vector2Int> _tilesPost = new();
        for (int i = 1; i <= _moveDistance; i++)
        {
            Vector2Int _nextPost = new(_piecePost.x + (_directionVector.x * i), _piecePost.y + (_directionVector.y * i));
            if (gridData.TileExists(_nextPost))
                if (!pieceData.DoesPieceExist(_nextPost, (transform.parent.name == "white peices"))) //if white piece do not exist at the location
                {
                    
                    if (pieceData.DoesPieceExist(_nextPost, (transform.parent.name != "white peices")))
                    {
                        break;
                    }
                    _tilesPost.Add(_nextPost);
                }
                else
                    break;

        }

        return _tilesPost;
    }

    void checkPawnAttackLocations(ref List<Vector2Int> _tiles, bool _isWhiteTurn, int _moveDir=1)
    {
        if (_isWhiteTurn) //is white piece
            _moveDir = 1;
        else
            _moveDir = -1;

        List<Vector2Int> _capturePosts = base.fixedDirectionalMovePattern(1, ThisPiece.Post, new(1, 1 * _moveDir)); //checking for location pawn can capture

        if(_capturePosts.Count>0)
            if (pieceData.DoesPieceExist(_capturePosts[0], !_isWhiteTurn))
                _tiles.Add(_capturePosts[0]);

        _capturePosts = base.fixedDirectionalMovePattern(1, ThisPiece.Post, new(-1, 1 * _moveDir)); //checking for location pawn can capture

        if (_capturePosts.Count > 0)
            if (pieceData.DoesPieceExist(_capturePosts[0], !_isWhiteTurn))
                _tiles.Add(_capturePosts[0]);

    }

}
