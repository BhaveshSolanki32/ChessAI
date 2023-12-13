using System.Collections.Generic;
using UnityEngine;

public class DisplayTiles : MonoBehaviour // mark the tiles
{
    [SerializeField] GridData _gridData;
    List<GameObject> _previouslySelectedTiles = new();
    private PiecesDictsData _pieceData;
    bool _isWhiteTurn = true;

    private void Awake()
    {
        if (!transform.parent.TryGetComponent<PiecesDictsData>(out _pieceData)) Debug.LogError("PieceData not found", gameObject);
        _pieceData.GetComponent<SelectPiece>().OnPieceSelectedEvent += MarkTiles;
        _pieceData.GetComponent<MovePiece>().OnPieceStartMoving += (GameObject temp, Vector2Int temp1) => _isWhiteTurn = !_isWhiteTurn;
    }

    public void MarkTiles(List<Vector2Int> tiles)
    {
        DeselectTile();
        var tilesGameObject = new List<GameObject>();

        foreach (Vector2Int y in tiles)
        {
            var tile = _gridData.GetTile(y);
            if (tile != null)
            {
                tilesGameObject.Add(tile);

                var spriteRenderer = tile.transform.GetChild(0).GetComponent<SpriteRenderer>();

                if (_pieceData.DoesPieceExist(y, !_isWhiteTurn))
                    spriteRenderer.color = new Color(1, 0, 0.04200459f, 0.5686275f); //red
                else
                    spriteRenderer.color = new Color(0, 1, 0.6819811f, 0.5686275f); //green

                spriteRenderer.enabled = true;
            }
        }

        _previouslySelectedTiles = tilesGameObject;
    }

    public void DeselectTile()
    {
        foreach (GameObject x in _previouslySelectedTiles)
        {
            var spriteRenderer = x.transform.GetChild(0).GetComponent<SpriteRenderer>();

            spriteRenderer.enabled = false;

        }
    }
}
