using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiPiecePostUpdater : MonoBehaviour
{
    [SerializeField] GameObject _whiteKing;
    [SerializeField] GameObject _blackKing;

    // updates dictionary using it's ref
    public void UpdatePiecePost(GameObject pieceGameObject, Vector2Int newPost, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict)
    {
        var oldPost= new Vector2Int();
        var isWhitePost=false;
        if (whitePieceDict.ContainsValue(pieceGameObject))
        {
            oldPost = whitePieceDict.FirstOrDefault(x => x.Value == pieceGameObject).Key;
            isWhitePost = true;
        }
        else
        {
            isWhitePost = false;
            oldPost = blackPieceDict.FirstOrDefault(x => x.Value == pieceGameObject).Key;
        }



        if (doesPieceExist(newPost, isWhitePost, whitePieceDict, blackPieceDict)) return;


        if (isWhitePost)
        {
            if (blackPieceDict.ContainsKey(newPost)) blackPieceDict.Remove(newPost);

            whitePieceDict.Add(newPost, pieceGameObject);
            whitePieceDict.Remove(oldPost);
        }
        else
        {
            if (whitePieceDict.ContainsKey(newPost)) whitePieceDict.Remove(newPost);

            blackPieceDict.Add(newPost, pieceGameObject);
            blackPieceDict.Remove(oldPost);
        }


    }

    public bool IsKingDead(Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict)
    {
        return !(whitePieceDict.ContainsValue(_whiteKing) || blackPieceDict.ContainsValue(_blackKing));
    }

    bool doesPieceExist(Vector2Int post, bool isPlayerPost, Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict)
    {
        if (isPlayerPost)
            return whitePieceDict.ContainsKey(post);
        else
            return blackPieceDict.ContainsKey(post);
    }
}
