using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PiecesDictsData))]
public class SelectPiece : MonoBehaviour //selects the peice 
{
    public event Action<List<Vector2Int>> OnPieceSelectedEvent;


    public void Select(GameObject pieceGameObject)
    {
        IPiece peice;

        if (!pieceGameObject.TryGetComponent<IPiece>(out peice))
            Debug.LogError("no Ipiece interface found", gameObject);

        var _movablesTilePosts = peice.MovableTilePosts(pieceGameObject.GetComponent<ChessPieceData>().Post, GetComponent<PiecesDictsData>().WhitePieceDict, GetComponent<PiecesDictsData>().BlackPieceDict);

        OnPieceSelectedEvent?.Invoke(_movablesTilePosts);

    }
}
