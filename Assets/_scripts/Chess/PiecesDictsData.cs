using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovePiece), typeof(SelectPiece))]
public class PiecesDictsData : MonoBehaviour // holds data for player and opponents piece postion and updates it
{
    [SerializeField] GameObject _playerPiecesParent;
    [SerializeField] GameObject _opponentPiecesParent;
    public Dictionary<Vector2Int, GameObject> WhitePieceDict = new();
    public Dictionary<Vector2Int, GameObject> BlackPieceDict= new();

    private void Awake()
    {
        foreach (var x in _playerPiecesParent.transform.GetComponentsInChildren<ChessPieceData>())
        {
            intitialAddPiecePost(x, true);
        }
        foreach (var x in _opponentPiecesParent.transform.GetComponentsInChildren<ChessPieceData>())
        {

            intitialAddPiecePost(x, false);
        }
        var movePiece =new MovePiece();
        if (TryGetComponent<MovePiece>(out movePiece))
            movePiece.OnPieceStartMoving += (GameObject piece, Vector2Int newPost) => UpdatePiecePost(piece.GetComponent<ChessPieceData>(), newPost);

    }


    public void UpdatePiecePost(ChessPieceData chessPieceData, Vector2Int newPost)
    {
        
        var isPlayerPost = WhitePieceDict.ContainsKey(chessPieceData.Post);
        
        if (DoesPieceExist(newPost, isPlayerPost)) return;


        if (isPlayerPost)
        {
            if (BlackPieceDict.ContainsKey(newPost)) pieceCaptured(ref BlackPieceDict, newPost);

            WhitePieceDict.Add(newPost, chessPieceData.gameObject);
            WhitePieceDict.Remove(chessPieceData.Post);
            chessPieceData.Post = newPost;
        }
        else
        {
            if (WhitePieceDict.ContainsKey(newPost)) pieceCaptured(ref WhitePieceDict, newPost);
   

            BlackPieceDict.Add(newPost, chessPieceData.gameObject);
            BlackPieceDict.Remove(chessPieceData.Post);
            chessPieceData.Post = newPost;
        }

    }

    public bool DoesPieceExist(Vector2Int post, bool isPlayerPost)
    {
        if (isPlayerPost)
            return WhitePieceDict.ContainsKey(post);
        else
            return BlackPieceDict.ContainsKey(post);
    }

    private void intitialAddPiecePost(ChessPieceData chessPieceData, bool isPlayerPost)
    {
        var newPost = chessPieceData.Post;
        if (isPlayerPost)
            WhitePieceDict.Add(newPost, chessPieceData.gameObject);
        else
            BlackPieceDict.Add(newPost, chessPieceData.gameObject);
    }


    void pieceCaptured(ref Dictionary<Vector2Int, GameObject> postData, Vector2Int post)
    {
        var piece = postData[post];
        postData.Remove(post);
        
        Destroy(piece);
    }

 

}
