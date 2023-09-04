using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PieceMobility), typeof(MaterialAdvantage))]
public class HeuristicFunctionCalc : MonoBehaviour
{
    public float MaterialAdvantageBias = 10;
    public float PieceMobilityBias = 3.5f;
    MaterialAdvantage materialAdvantage;
    PieceMobility pieceMobility;
    public int HeuristicCalled = 0;

    private void Awake()
    {
        materialAdvantage = GetComponent<MaterialAdvantage>();
        pieceMobility = GetComponent<PieceMobility>();
    }



    public float CalcHeuristics(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece)
    {
        HeuristicCalled++;
        float _materialAdvantage = materialAdvantage.Calculate(_whitePiece, _blackPiece);
          float _pieceMobility = pieceMobility.Calculate(_whitePiece, _blackPiece);

        return _materialAdvantage * MaterialAdvantageBias+ _pieceMobility * PieceMobilityBias;
    }

    

}
