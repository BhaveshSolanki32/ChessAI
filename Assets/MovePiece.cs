using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PiecesData))]
public class MovePiece : MonoBehaviour //moves the piece to the desired postion and updates data with an event
{
    [SerializeField] float speed=0.3f;
    [SerializeField] GridData gridData;
    [SerializeField] InputReceiver inputReceiver;
    List<Vector2Int> movableTiles=new();
    public Action<GameObject,Vector2Int> OnPieceStartMoving;
    public Action<GameObject> OnPieceEndMoving;
    private void Awake()
    {
        SelectPiece _selectPiece;
        if (TryGetComponent<SelectPiece>(out _selectPiece))
            _selectPiece.OnPieceSelectedEvent += (List<Vector2Int> _movablePosts) => movableTiles = _movablePosts;
        else
            Debug.LogError("SelectPiece not found", gameObject);

        if (inputReceiver == null) Debug.LogError("InputReceicer not assigned", gameObject);
    }

    public void MovePieceTo(GameObject _piece, Vector2Int _toWhereGridPost, bool _skipMovableTilesListContainCheckForAI = false)
    {
        _piece.transform.parent.GetComponent<DisplayTiles>().MarkTiles(new());


        if (!movableTiles.Contains(_toWhereGridPost) && !_skipMovableTilesListContainCheckForAI)
        {
            return;
        }
        
        Vector3 _newPost = gridData.GetTile(_toWhereGridPost).transform.position;
        Transform _pieceTransform = _piece.transform;
        
        StartCoroutine(lerpPostion(_pieceTransform, _newPost));
        OnPieceStartMoving?.Invoke(_pieceTransform.gameObject, WorldToGridPostion.Convert(_newPost, gridData));
       
    }

    IEnumerator lerpPostion(Transform _pieceTransform, Vector3 _newPost)
    {
        while (_pieceTransform.position != _newPost)
        {
            _pieceTransform.position = Vector3.MoveTowards(_pieceTransform.position, _newPost, speed);
            yield return new WaitForSeconds(0.08f);
        }
        
        OnPieceEndMoving?.Invoke(_pieceTransform.gameObject);
    }

}
