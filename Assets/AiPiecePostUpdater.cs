using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiPiecePostUpdater : MonoBehaviour
{
    [SerializeField] GameObject whiteKing;
    [SerializeField] GameObject blackKing;

    // updates dictionary using it's ref
    public void UpdatePiecePost(GameObject _pieceGameObject, Vector2Int _newPost,Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        Vector2Int _oldPost;
        bool _isWhitePost;
        if (_whitePieceDict.ContainsValue(_pieceGameObject))
        {
            _oldPost = _whitePieceDict.FirstOrDefault(x => x.Value == _pieceGameObject).Key;
            _isWhitePost = true;
        }
        else
        {
            _isWhitePost = false;
            _oldPost = _blackPieceDict.FirstOrDefault(x => x.Value == _pieceGameObject).Key;
        }



        if (doesPieceExist(_newPost, _isWhitePost, _whitePieceDict, _blackPieceDict)) return;


        if (_isWhitePost)
        {
            if (_blackPieceDict.ContainsKey(_newPost)) _blackPieceDict.Remove(_newPost);

            _whitePieceDict.Add(_newPost, _pieceGameObject);
            _whitePieceDict.Remove(_oldPost);
        }
        else
        {
            if (_whitePieceDict.ContainsKey(_newPost)) _whitePieceDict.Remove(_newPost);

            _blackPieceDict.Add(_newPost, _pieceGameObject);
            _blackPieceDict.Remove(_oldPost);
        }

    }

    bool doesPieceExist(Vector2Int _post, bool _isPlayerPost,  Dictionary<Vector2Int, GameObject> _whitePieceDict,  Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        if (_isPlayerPost)
            return _whitePieceDict.ContainsKey(_post);
        else
            return _blackPieceDict.ContainsKey(_post);
    }

    public bool IsKingDead(Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        return !(_whitePieceDict.ContainsValue(whiteKing) || _blackPieceDict.ContainsValue(blackKing));
    }
}
