using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAdvantage : MonoBehaviour
{

    Dictionary<Type, int> pieceValDict = new();
    int maxScore = 0;

    private void Awake()
    {
        pieceValDict.Add(typeof(PawnMovePattern), 1);
        pieceValDict.Add(typeof(KnightMovePattern), 3);
        pieceValDict.Add(typeof(BishopMovePattern), 3);
        pieceValDict.Add(typeof(RookMovePattern), 5);
        pieceValDict.Add(typeof(QueenMovePattern), 9);
        pieceValDict.Add(typeof(KingMovePattern), 99999);


        foreach (int x in pieceValDict.Values) maxScore += x;


    }

    public float Calculate(Dictionary<Vector2Int, GameObject> _whitePiece, Dictionary<Vector2Int, GameObject> _blackPiece)
    {

        int _blackScore = 0;
        int _whiteScore = 0;

        foreach (GameObject x in _blackPiece.Values)
        {
            Type _pieceType = x.GetComponent<IPiece>().GetType();
            _blackScore += (_pieceType == typeof(KingMovePattern) ? (pieceValDict[_pieceType] * 3) : pieceValDict[_pieceType]);
        }
        foreach (GameObject x in _whitePiece.Values)
        {
            _whiteScore += pieceValDict[x.GetComponent<IPiece>().GetType()];
        }
        return (_blackScore - _whiteScore)/maxScore;

    }

}
