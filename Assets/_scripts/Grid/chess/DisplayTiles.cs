using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTiles : MonoBehaviour // mark the tiles
{
    List<GameObject> previouslySelectedTiles = new();
    [SerializeField] GridData gridData;
    private PiecesDictsData pieceData;
    bool isWhiteTurn = true;

    private void Awake()
    {

        if (!transform.parent.TryGetComponent<PiecesDictsData>(out pieceData)) Debug.LogError("PieceData not found", gameObject);
        pieceData.GetComponent<SelectPiece>().OnPieceSelectedEvent += MarkTiles;
        pieceData.GetComponent<MovePiece>().OnPieceStartMoving += (GameObject temp, Vector2Int temp1) => isWhiteTurn = !isWhiteTurn;
    }

    public void MarkTiles(List<Vector2Int> _tiles)
    {
        DeselectTile();
        List<GameObject> _tilesGameObject = new();
        
        foreach(Vector2Int y in _tiles)
        {
            GameObject _tile = gridData.GetTile(y);
            if (_tile != null)
            {
                _tilesGameObject.Add(_tile);

                SpriteRenderer _spriteRenderer = _tile.transform.GetChild(0).GetComponent<SpriteRenderer>();

                if (pieceData.DoesPieceExist(y, !isWhiteTurn))
                    _spriteRenderer.color = new Color(1, 0, 0.04200459f, 0.5686275f); //red
                else
                    _spriteRenderer.color = new Color(0, 1, 0.6819811f, 0.5686275f); //green

                _spriteRenderer.enabled = true;
            }
        }

        previouslySelectedTiles = _tilesGameObject;
    }

    public void DeselectTile()
    {
        foreach (GameObject x in previouslySelectedTiles)
        {
            SpriteRenderer _spriteRenderer = x.transform.GetChild(0).GetComponent<SpriteRenderer>();

            _spriteRenderer.enabled = false;

        }
    }
}
