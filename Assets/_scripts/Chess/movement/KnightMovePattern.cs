using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChessPieceData))]
public class KnightMovePattern : BasePieceMovementPattern, IPiece
{
    public int MoveDinstance { get; set; } = 3;

    public List<Vector2Int> MovableTilePosts(Vector2Int post, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict)
    {
        var tilePost = allDirectionalMovePattern(MoveDinstance - 1, post,whitePieceDict, blackPieceDict);

        return tilePost;
    }

    protected override List<Vector2Int> allDirectionalMovePattern(int moveDistance, Vector2Int piecePost, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict , bool isDiagonalMovement = false)
    {
        var tilesPost = new List<Vector2Int>();
        var isWhitePost = whitePieceDict.ContainsKey(piecePost);

        for (int i = -1; i <= 1; i++)
        {
            if (i == 0) continue;

            addTilePost(tilesPost, new(piecePost.x + (i * moveDistance), piecePost.y + i), whitePieceDict, blackPieceDict, isWhitePost);
            addTilePost(tilesPost, new(piecePost.x + (i * moveDistance), piecePost.y - i), whitePieceDict, blackPieceDict, isWhitePost);
            addTilePost(tilesPost, new(piecePost.x - i, piecePost.y + (i * moveDistance)), whitePieceDict, blackPieceDict, isWhitePost);
            addTilePost(tilesPost, new(piecePost.x + i, piecePost.y + (i * moveDistance)), whitePieceDict, blackPieceDict, isWhitePost);
        }

        return tilesPost;

    }

    void addTilePost(List<Vector2Int> tilePost, Vector2Int tileToAdd, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict,  bool isWhitePost)
    {

        var selectedPieceDict = new Dictionary<Vector2Int, GameObject>();


        if (isWhitePost)
        {
            selectedPieceDict = whitePieceDict;
        }
        else
        {
            selectedPieceDict = blackPieceDict;
        }

        if (gridData.TileExists(tileToAdd))
            if (!selectedPieceDict.ContainsKey(tileToAdd))
                tilePost.Add(tileToAdd);

    }

}
