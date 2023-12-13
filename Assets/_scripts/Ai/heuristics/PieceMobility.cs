using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMobility : MonoBehaviour
{
    float _aiMobilityImportanceComparedToenemy = 0.4f;
    int _maxScore = 30;


    public float Calculate(Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict, Dictionary<GameObject, IPiece> gameObjectIpieceDict)
    {
        var blackScore = 0;
        var whiteScore = 0;

        foreach (Vector2Int x in whitePieceDict.Keys) whiteScore += gameObjectIpieceDict[whitePieceDict[x]].MovableTilePosts(x, whitePieceDict, blackPieceDict).Count;

        foreach (Vector2Int x in blackPieceDict.Keys) blackScore += gameObjectIpieceDict[blackPieceDict[x]].MovableTilePosts(x, whitePieceDict, blackPieceDict).Count;


        return (blackScore - (whiteScore*_aiMobilityImportanceComparedToenemy));

    }
}
