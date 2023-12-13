using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PiecesDictsData))]
public class MovePiece : MonoBehaviour //moves the piece to the desired postion and updates data with an event
{
    [SerializeField] float _speed=0.3f;
    [SerializeField] GridData _gridData;
    [SerializeField] InputReceiver _inputReceiver;
    List<Vector2Int> _movableTiles=new();
    public Action<GameObject,Vector2Int> OnPieceStartMoving;
    public Action<GameObject> OnPieceEndMoving;
    private void Awake()
    {
        var selectPiece = new SelectPiece();
        if (TryGetComponent<SelectPiece>(out selectPiece))
            selectPiece.OnPieceSelectedEvent += (List<Vector2Int> movablePosts) => _movableTiles = movablePosts;
        else
            Debug.LogError("SelectPiece not found", gameObject);

        if (_inputReceiver == null) Debug.LogError("InputReceicer not assigned", gameObject);
    }

    public void MovePieceTo(GameObject piece, Vector2Int toWhereGridPost, bool skipMovableTilesListContainCheckForAI = false)
    {
        piece.transform.parent.GetComponent<DisplayTiles>().MarkTiles(new());


        if (!_movableTiles.Contains(toWhereGridPost) && !skipMovableTilesListContainCheckForAI)
        {
            return;
        }
        
        var newPost = _gridData.GetTile(toWhereGridPost).transform.position;
        var pieceTransform = piece.transform;
        
        StartCoroutine(lerpPostion(pieceTransform, newPost));
        OnPieceStartMoving?.Invoke(pieceTransform.gameObject, WorldToGridPostion.Convert(newPost, _gridData));
       
    }

    IEnumerator lerpPostion(Transform pieceTransform, Vector3 newPost)
    {
        while (pieceTransform.position != newPost)
        {
            pieceTransform.position = Vector3.MoveTowards(pieceTransform.position, newPost, _speed);
            yield return new WaitForSeconds(0.08f);
        }
        
        OnPieceEndMoving?.Invoke(pieceTransform.gameObject);
    }

}
