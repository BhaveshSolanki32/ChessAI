using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputTaskHandler))]
public class InputReceiver : MonoBehaviour //recives input from user and selects the peice
{
    [SerializeField] Camera _cam;
    [SerializeField] SelectPiece _selectPiece;
    [SerializeField] bool _takeInput = true;
    bool _isPieceSelected = false;
    InputTaskHandler _inputTaskHandler;
    LayerMask _whitePieceLayer;
    LayerMask _blackPieceLayer;
    MovePiece _movePiece;
    bool _isWhiteTurn = true;


    private void Awake()
    {
        if (_selectPiece == null) Debug.LogError("SelectPiece not found", gameObject);
        

        if (!TryGetComponent<InputTaskHandler>(out _inputTaskHandler)) Debug.LogError("InputTaskHandler not found", gameObject);
        if (!_selectPiece.TryGetComponent<MovePiece>(out _movePiece)) Debug.LogError("MovePiece not found", _selectPiece.gameObject);


        _movePiece.OnPieceStartMoving += (GameObject temp, Vector2Int temp2) => _isPieceSelected = false;
        _selectPiece.OnPieceSelectedEvent += (List<Vector2Int> posts) => _isPieceSelected = true;
        _movePiece.OnPieceStartMoving += (GameObject temp, Vector2Int temp2) => _isWhiteTurn = !_isWhiteTurn;

        _movePiece.OnPieceStartMoving += (GameObject piece, Vector2Int temp) => _takeInput = !_takeInput;

        _movePiece.OnPieceEndMoving += (GameObject piece) => _takeInput = !_takeInput;
        
        _whitePieceLayer = LayerMask.GetMask("white");
        _blackPieceLayer = LayerMask.GetMask("black");

    }

    private void FixedUpdate()
    {
        if (_takeInput)
        {
            if (Input.GetMouseButton(0))
            {
                var newSelectedGameObject = RaycastTIleHit(Input.mousePosition, (_isWhiteTurn)?(_whitePieceLayer):(_blackPieceLayer) );

                if (newSelectedGameObject != null) //clicked on piece
                {
                    _inputTaskHandler.NewInputRecieved(newSelectedGameObject);
                }
                else if (_isPieceSelected) //clicked on random post
                {
                    var mouseWorldPost = _cam.ScreenToWorldPoint(Input.mousePosition);

                    _inputTaskHandler.NewInputRecieved(mouseWorldPost);
                }

            }
        }

    }

    private GameObject RaycastTIleHit(Vector2 mousePost, LayerMask layer)
    {
        var mouseWorldPost = _cam.ScreenToWorldPoint(mousePost);
        var hit = Physics2D.Raycast(mouseWorldPost, _cam.transform.forward, 100,layer);
        if (hit.collider != null)
            return hit.collider.gameObject;
        return null;
    }
}
