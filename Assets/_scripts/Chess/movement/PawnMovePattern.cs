using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChessPieceData))]
public class PawnMovePattern : BasePieceMovementPattern, IPiece
{
    MovePiece _movePiece;
    public int MoveDinstance { get; set; } = 2;


    public void OnDestroy()
    {
        _movePiece.OnPieceStartMoving -= firstMoveDone;
    }

    protected override void Awake()
    {
        base.Awake();

        if (transform.parent.parent.TryGetComponent<MovePiece>(out _movePiece))
            _movePiece.OnPieceStartMoving += firstMoveDone;
        else
            Debug.LogError("MovePiece not fund", gameObject);

    }


    public List<Vector2Int> MovableTilePosts(Vector2Int post, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict)
    {
        var movementDir = 1; // for the black team we have to get forward posts
        var isWhiteTurn = whitePieceDict.ContainsKey(post);

        if (isWhiteTurn) //is white piece
            movementDir = 1;
        else
            movementDir = -1;

       var tiles = fixedDirectionalMovePattern(MoveDinstance, post, new(0, 1 * movementDir), whitePieceDict, blackPieceDict);
        checkPawnAttackLocations(ref tiles, isWhiteTurn, post, whitePieceDict, blackPieceDict);
        return tiles;
    }

    protected override List<Vector2Int> fixedDirectionalMovePattern(int moveDistance, Vector2Int piecePost, Vector2Int directionVector, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict)
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
                if (!selectedPieceDict.ContainsKey(nextPost)) //if selected color piece do not exist at the location
                {

                    if (oppponentPieceDict.ContainsKey(nextPost))
                    {
                        break;
                    }
                    tilesPost.Add(nextPost);
                }
                else
                    break;

        }
        return tilesPost;
    }

    void firstMoveDone(GameObject piece, Vector2Int vector2Int)
    {
        if (piece == gameObject)
        {
            _movePiece.OnPieceStartMoving -= firstMoveDone;
            MoveDinstance = 1;
        }

    }

    void checkPawnAttackLocations(ref List<Vector2Int> tiles, bool isWhitePost, Vector2Int post, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict, int moveDir = 1)
    {
        var oppponentPieceDict = new Dictionary<Vector2Int, GameObject>();

        if (isWhitePost)
        {
            moveDir = 1;
            oppponentPieceDict = blackPieceDict;
        }
        else
        {
            moveDir = -1;
            oppponentPieceDict = whitePieceDict;
        }

        var capturePosts = base.fixedDirectionalMovePattern(1, post, new(1, 1 * moveDir), whitePieceDict, blackPieceDict); //checking for location pawn can capture

        for (int i = 0; i <= 1; i++)
        {
            if (capturePosts.Count > 0)
                if (oppponentPieceDict.ContainsKey(capturePosts[0]))
                    tiles.Add(capturePosts[0]);

            capturePosts = base.fixedDirectionalMovePattern(1, post, new(-1, 1 * moveDir), whitePieceDict, blackPieceDict); //checking for location pawn can capture
        }


    }

}
