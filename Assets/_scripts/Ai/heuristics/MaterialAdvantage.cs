using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAdvantage : MonoBehaviour
{

    Dictionary<Type, int> _pieceValDict = new();
    int _maxScore = 0;
    float _aiAdvantageImportanceToEnemy=1.6f;

    private void Awake()
    {
        _pieceValDict.Add(typeof(PawnMovePattern), 1);
        _pieceValDict.Add(typeof(KnightMovePattern), 3);
        _pieceValDict.Add(typeof(BishopMovePattern), 3);
        _pieceValDict.Add(typeof(RookMovePattern), 5);
        _pieceValDict.Add(typeof(QueenMovePattern), 9);
        _pieceValDict.Add(typeof(KingMovePattern), 99999);


        foreach (int x in _pieceValDict.Values) _maxScore += x;


    }

    public float Calculate(Dictionary<Vector2Int, GameObject> whitePiece, Dictionary<Vector2Int, GameObject> blackPiece, Dictionary<GameObject, IPiece> gameObjectIpieceDict)
    {

        var blackScore = 0;
        var whiteScore = 0;

        foreach (GameObject x in blackPiece.Values)
        {
            var pieceType = gameObjectIpieceDict[x].GetType();
            blackScore += (pieceType == typeof(KingMovePattern) ? (_pieceValDict[pieceType] * 3) : _pieceValDict[pieceType]);
        }
        foreach (GameObject x in whitePiece.Values)
        {
            whiteScore += _pieceValDict[gameObjectIpieceDict[x].GetType()];
        }
        return (blackScore*_aiAdvantageImportanceToEnemy - whiteScore);

    }

}
