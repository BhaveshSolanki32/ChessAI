using System.Collections.Generic;
using UnityEngine;

public class BasePieceMovementPattern : MonoBehaviour //contains function for common piece movement pattern
{

    protected GridData gridData;
    protected PiecesDictsData _pieceData;
    public ChessPieceData ThisPiece { get; set; }
    
    protected virtual void Awake()
    {
        ThisPiece = GetComponent<ChessPieceData>();
        gridData = ThisPiece.GridData;
        if (!transform.root.TryGetComponent<PiecesDictsData>(out _pieceData))
            Debug.LogError("PieceData not found");
    }

    protected virtual List<Vector2Int> fixedDirectionalMovePattern(int moveDistance, Vector2Int piecePost, Vector2Int directionVector, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict )
    {
        var tilesPost = new List<Vector2Int>();
        var selectedPieceDict = new Dictionary<Vector2Int, GameObject>();
        var oppponentPieceDict = new Dictionary<Vector2Int, GameObject>();

        var isWhitePost = whitePieceDict.ContainsKey(piecePost);

        if (isWhitePost)
        {
            selectedPieceDict = whitePieceDict;
            oppponentPieceDict = blackPieceDict;
        }
        else
        {
            selectedPieceDict = blackPieceDict;
            oppponentPieceDict = whitePieceDict;
        }
        for (int i = 1; i <= moveDistance; i++)
        {
            var nextPost = new Vector2Int(piecePost.x + (directionVector.x * i), piecePost.y + (directionVector.y * i));
            if (gridData.TileExists(nextPost))
                if (!selectedPieceDict.ContainsKey(nextPost))//if slected color piece do not exist at the location
                {
                    tilesPost.Add(nextPost);
                    if (oppponentPieceDict.ContainsKey(nextPost))
                    {
                        break;
                    }
                }
                else
                    break;

        }
        return tilesPost;
    }

    protected virtual List<Vector2Int> allDirectionalMovePattern(int moveDistance, Vector2Int piecePost, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict, bool isDiagonalMovement = false)
    {
        var tiles = new List<Vector2Int>();

        if (!isDiagonalMovement)
        {
            tiles.AddRange(fixedDirectionalMovePattern(moveDistance, piecePost, new(1, 0), whitePieceDict, blackPieceDict));
            tiles.AddRange(fixedDirectionalMovePattern(moveDistance, piecePost, new(-1, 0), whitePieceDict, blackPieceDict));
            tiles.AddRange(fixedDirectionalMovePattern(moveDistance, piecePost, new(0, 1), whitePieceDict, blackPieceDict));
            tiles.AddRange(fixedDirectionalMovePattern(moveDistance, piecePost, new(0, -1), whitePieceDict, blackPieceDict));
        }
        else
        {
            tiles.AddRange(fixedDirectionalMovePattern(moveDistance, piecePost, new(1, 1), whitePieceDict, blackPieceDict));
            tiles.AddRange(fixedDirectionalMovePattern(moveDistance, piecePost, new(-1, -1), whitePieceDict, blackPieceDict));
            tiles.AddRange(fixedDirectionalMovePattern(moveDistance, piecePost, new(-1, 1), whitePieceDict, blackPieceDict));
            tiles.AddRange(fixedDirectionalMovePattern(moveDistance, piecePost, new(1, -1), whitePieceDict, blackPieceDict));
        }



        return tiles;
    }


}
