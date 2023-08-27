using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputTaskHandler))]
public class InputReceiver : MonoBehaviour //recives input from user and selects the peice
{
    [SerializeField] Camera cam;
    [SerializeField] SelectPiece selectPiece;
    bool isPieceSelected = false;
    [SerializeField]bool takeInput = true;
    InputTaskHandler inputTaskHandler;
    LayerMask whitePieceLayer;
    LayerMask blackPieceLayer;
    MovePiece movePiece;
    bool isWhiteTurn = true;


    private void Awake()
    {
        if (selectPiece == null) Debug.LogError("SelectPiece not found", gameObject);
        

        if (!TryGetComponent<InputTaskHandler>(out inputTaskHandler)) Debug.LogError("InputTaskHandler not found", gameObject);
        if (!selectPiece.TryGetComponent<MovePiece>(out movePiece)) Debug.LogError("MovePiece not found", selectPiece.gameObject);


        movePiece.OnPieceMoved += (GameObject temp, Vector2Int _temp2) => isPieceSelected = false;
        selectPiece.OnPieceSelectedEvent += (List<Vector2Int> _posts) => isPieceSelected = true;
        movePiece.OnPieceMoved += (GameObject temp, Vector2Int _temp2) => isWhiteTurn = !isWhiteTurn;


        whitePieceLayer = LayerMask.GetMask("white");
        blackPieceLayer = LayerMask.GetMask("black");

    }
    public void TakeInput(bool _takeInput) => takeInput = _takeInput;

    private void FixedUpdate()
    {
        if (takeInput)
        {
            if (Input.GetMouseButton(0))
            {
                GameObject _newSelectedGameObject = RaycastTIleHit(Input.mousePosition, (isWhiteTurn)?(whitePieceLayer):(blackPieceLayer) );

                if (_newSelectedGameObject != null) //clicked on piece
                {
                    inputTaskHandler.NewInputRecieved(_newSelectedGameObject);
                }
                else if (isPieceSelected) //clicked on random post
                {
                    Vector3 _mouseWorldPost = cam.ScreenToWorldPoint(Input.mousePosition);

                    inputTaskHandler.NewInputRecieved(_mouseWorldPost);
                }

            }
        }

    }

    private GameObject RaycastTIleHit(Vector2 _mousePost, LayerMask _layer)
    {
        Vector3 _mouseWorldPost = cam.ScreenToWorldPoint(_mousePost);
        RaycastHit2D _hit = Physics2D.Raycast(_mouseWorldPost, cam.transform.forward, 100,_layer);
        if (_hit.collider != null)
            return _hit.collider.gameObject;
        return null;
    }
}
