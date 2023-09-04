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



    public List<Vector2Int> MovableTilePosts(Vector2Int _post, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        int _movementDir = 1; // for the black team we have to get forward posts
        bool _isWhiteTurn = _whitePieceDict.ContainsKey(_post);
        if (_isWhiteTurn) //is white piece
         _movementDir = 1; 
        else
            _movementDir = -1;

        List<Vector2Int> _tiles = fixedDirectionalMovePattern(MoveDinstance, _post, new(0, 1 * _movementDir),_whitePieceDict, _blackPieceDict);
        checkPawnAttackLocations(ref _tiles, _isWhiteTurn, _post,_whitePieceDict,_blackPieceDict);
        return _tiles;
    }

    protected override List<Vector2Int> fixedDirectionalMovePattern(int _moveDistance, Vector2Int _piecePost, Vector2Int _directionVector, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict)
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
                if (!_selectedPieceDict.ContainsKey(_nextPost)) //if selected color piece do not exist at the location
                {
                    
                    if (_oppponentPieceDict.ContainsKey(_nextPost))
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

    void checkPawnAttackLocations(ref List<Vector2Int> _tiles, bool _isWhitePost, Vector2Int _post, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict,  int _moveDir=1)
    {
        Dictionary<Vector2Int, GameObject> _oppponentPieceDict = new();

        if (_isWhitePost)
        {
            _moveDir = 1;
            _oppponentPieceDict = _blackPieceDict;
        }
        else
        {
            _moveDir = -1;
            _oppponentPieceDict = _whitePieceDict;
        }

        List<Vector2Int> _capturePosts = base.fixedDirectionalMovePattern(1, _post, new(1, 1 * _moveDir),_whitePieceDict,_blackPieceDict); //checking for location pawn can capture

        for(int i = 0; i <= 1; i++)
        {
            if (_capturePosts.Count > 0)
                if (_oppponentPieceDict.ContainsKey(_capturePosts[0]))
                    _tiles.Add(_capturePosts[0]);

            _capturePosts = base.fixedDirectionalMovePattern(1, _post, new(-1, 1 * _moveDir), _whitePieceDict, _blackPieceDict); //checking for location pawn can capture
        }


    }

}
