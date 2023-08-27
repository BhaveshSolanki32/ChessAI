using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PiecesData))]
public class SelectPiece : MonoBehaviour //selects the peice 
{
    public event Action<List<Vector2Int>> OnPieceSelectedEvent;


    public void Select(GameObject _pieceGameObject)
    {
        IPiece _peice;

        if (!_pieceGameObject.TryGetComponent<IPiece>(out _peice))
            Debug.LogError("no Ipiece interface found", gameObject);

        List<Vector2Int> _movablesTilePosts = _peice.MovableTilePosts();

        OnPieceSelectedEvent?.Invoke(_movablesTilePosts);

    }
}
