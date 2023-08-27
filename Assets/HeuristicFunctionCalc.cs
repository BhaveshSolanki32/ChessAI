using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeuristicFunctionCalc : MonoBehaviour
{
    Dictionary<Type, int> pieceValDict = new();

    private void Awake()
    {
        pieceValDict.Add(typeof( PawnMovePattern), 1) ;
        pieceValDict.Add(typeof(KnightMovePattern), 3);
        pieceValDict.Add(typeof(BishopMovePattern), 3);
        pieceValDict.Add(typeof(RookMovePattern), 5);
        pieceValDict.Add(typeof(QueenMovePattern), 9);
        pieceValDict.Add(typeof(KingMovePattern), 999999999);

    }

    public int CalcHeuristics(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece)
    {
        int _blackScore=0;
        int _whiteScore=0;

        foreach(GameObject x in _blackPiece.Values)
        {
            _blackScore += pieceValDict[x.GetComponent<IPiece>().GetType()];
        }
        foreach (GameObject x in _whitePiece.Values)
        {
            _blackScore += pieceValDict[x.GetComponent<IPiece>().GetType()];
        }

        return _blackScore - _whiteScore;
    }
}
