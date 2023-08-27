using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePiece), typeof(SelectPiece))]
public class PiecesData : MonoBehaviour // holds data for player and opponents piece postion
{
    public Dictionary<Vector2Int, GameObject> WhitePieceDict = new();
    public Dictionary<Vector2Int, GameObject> BlackPieceDict = new();
    [SerializeField] GameObject playerPiecesParent;
    [SerializeField] GameObject OpponentPiecesParent;

    private void Awake()
    {
        foreach (ChessPieceData x in playerPiecesParent.transform.GetComponentsInChildren<ChessPieceData>())
        {
            intitialAddPiecePost(x, true);
        }
        foreach (ChessPieceData x in OpponentPiecesParent.transform.GetComponentsInChildren<ChessPieceData>())
        {
            intitialAddPiecePost(x, false);
        }
        MovePiece _movePiece;
        if (TryGetComponent<MovePiece>(out _movePiece))
            _movePiece.OnPieceMoved += (GameObject _piece, Vector2Int _newPost) => UpdatePiecePost(_piece.GetComponent<ChessPieceData>(), _newPost);
    }

    private void intitialAddPiecePost(ChessPieceData _chessPieceData, bool _isPlayerPost)
    {
        Vector2Int _newPost = _chessPieceData.Post;
        if (_isPlayerPost)
            WhitePieceDict.Add(_newPost, _chessPieceData.gameObject);
        else
            BlackPieceDict.Add(_newPost, _chessPieceData.gameObject);
    }

    public void UpdatePiecePost(ChessPieceData _chessPieceData, Vector2Int _newPost)
    {


        bool _isPlayerPost = WhitePieceDict.ContainsKey(_chessPieceData.Post);

        if (DoesPieceExist(_newPost, _isPlayerPost)) return;


        if (_isPlayerPost)
        {
            if (BlackPieceDict.ContainsKey(_newPost)) pieceCaptured(ref BlackPieceDict, _newPost);

            WhitePieceDict.Add(_newPost, _chessPieceData.gameObject);
            WhitePieceDict.Remove(_chessPieceData.Post);
            _chessPieceData.Post = _newPost;
        }
        else
        {
            if (WhitePieceDict.ContainsKey(_newPost)) pieceCaptured(ref WhitePieceDict, _newPost);

            BlackPieceDict.Add(_newPost, _chessPieceData.gameObject);
            BlackPieceDict.Remove(_chessPieceData.Post);
            _chessPieceData.Post = _newPost;
        }

    }

    void pieceCaptured(ref Dictionary<Vector2Int, GameObject> _postData, Vector2Int _post)
    {
        GameObject _piece = _postData[_post];
        _postData.Remove(_post);
        Destroy(_piece);
    }

    public bool DoesPieceExist(Vector2Int _post, bool _isPlayerPost)
    {
        if (_isPlayerPost)
            return WhitePieceDict.ContainsKey(_post);
        else
            return BlackPieceDict.ContainsKey(_post);
    }

}
