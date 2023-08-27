using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiPiecePostUpdater : MonoBehaviour
{
    [SerializeField] GameObject whiteKing;
    [SerializeField] GameObject blackKing;

    //returns updated dictionary
    public Tuple<Dictionary<Vector2Int, GameObject>, Dictionary<Vector2Int, GameObject>> UpdatePiecePost(GameObject _pieceGameObject, Vector2Int _newPost, Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        Vector2Int _oldPost;
        bool _isPlayerPost;
        if (_whitePieceDict.ContainsValue(_pieceGameObject))
        {
            _oldPost = _whitePieceDict.FirstOrDefault(x => x.Value == _pieceGameObject).Key;
            _isPlayerPost = true;
        }
        else
        {
            _isPlayerPost = true;
            _oldPost = _blackPieceDict.FirstOrDefault(x => x.Value == _pieceGameObject).Key;
        }



        if (DoesPieceExist(_newPost, _isPlayerPost, ref _whitePieceDict, ref _blackPieceDict)) return new(_whitePieceDict, _blackPieceDict);


        if (_isPlayerPost)
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

        return new(_whitePieceDict, (_blackPieceDict));

    }

    public bool DoesPieceExist(Vector2Int _post, bool _isPlayerPost, ref Dictionary<Vector2Int, GameObject> _whitePieceDict, ref Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        if (_isPlayerPost)
            return _whitePieceDict.ContainsKey(_post);
        else
            return _blackPieceDict.ContainsKey(_post);
    }

    public bool IsKingDead(ref Dictionary<Vector2Int, GameObject> _whitePieceDict, ref Dictionary<Vector2Int, GameObject> _blackPieceDict)
    {
        return (_whitePieceDict.ContainsValue(whiteKing) || _blackPieceDict.ContainsValue(blackKing));
    }
}
