using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMobility), typeof(MaterialAdvantage))]
public class HeuristicFunctionCalc : MonoBehaviour
{
    MaterialAdvantage _materialAdvantage;
    PieceMobility _pieceMobility;
    float _materialAdvantageBias = 10;
    float _pieceMobilityBias = 3.5f;
    int _heuristicCalled = 0;

    private void Awake()
    {
        _materialAdvantage = GetComponent<MaterialAdvantage>();
        _pieceMobility = GetComponent<PieceMobility>();
    }



    public float CalcHeuristics(Dictionary<Vector2Int, GameObject> blackPiece, Dictionary<Vector2Int, GameObject> whitePiece, Dictionary<GameObject, IPiece> gameObjectIpieceDict)
    {
        _heuristicCalled++;
        var materialAdvantage = this._materialAdvantage.Calculate(whitePiece, blackPiece, gameObjectIpieceDict);
        //   float pieceMobility = pieceMobility.Calculate(whitePiece, blackPiece, gameObjectIpieceDict);

        return materialAdvantage * _materialAdvantageBias;// + pieceMobility * PieceMobilityBias;
    }

    

}
