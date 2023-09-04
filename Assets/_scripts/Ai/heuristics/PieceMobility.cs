using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMobility : MonoBehaviour
{
    public float AiMobilityImportanceComparedToenemy = 0.4f;
    int maxScore = 30;


    public float Calculate(Dictionary<Vector2Int, GameObject> _whitePieceDict, Dictionary<Vector2Int, GameObject> _blackPieceDict, Dictionary<GameObject, IPiece> _gameObjectIpieceDict)
    {
        int _blackScore = 0;
        int _whiteScore = 0;

        foreach (Vector2Int x in _whitePieceDict.Keys) _whiteScore += _gameObjectIpieceDict[_whitePieceDict[x]].MovableTilePosts(x, _whitePieceDict, _blackPieceDict).Count;

        foreach (Vector2Int x in _blackPieceDict.Keys) _blackScore += _gameObjectIpieceDict[_blackPieceDict[x]].MovableTilePosts(x, _whitePieceDict, _blackPieceDict).Count;


        return (_blackScore - (_whiteScore*AiMobilityImportanceComparedToenemy))/maxScore;//nomalized score

    }
}
