using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AiPiecePostUpdater), typeof(HeuristicFunctionCalc))]
public class MiniMaxHandler : MonoBehaviour, IMiniMax
{
    AiPiecePostUpdater _aiPiecePostUpdater;

    HeuristicFunctionCalc _heuristicFunctionCalc;
    Dictionary<GameObject, IPiece> _gameObjectIPieceDict= new();

    private void Awake()
    {
        _aiPiecePostUpdater = GetComponent<AiPiecePostUpdater>();
        _heuristicFunctionCalc = GetComponent<HeuristicFunctionCalc>();
    }

    //returns list of all posible moves with thier scores
    public void GetBestMoves(Dictionary<Vector2Int, GameObject> blackPieceDict, Dictionary<Vector2Int, GameObject> whitePieceDict, int depth, List<Tuple<Vector2Int, GameObject, float>> possibleMoves)
    {
        updateGameObjectIpieceDict(whitePieceDict,blackPieceDict);

        foreach (Vector2Int x in blackPieceDict.Keys)
        {
            var movableTiles = _gameObjectIPieceDict[blackPieceDict[x]].MovableTilePosts(x, whitePieceDict,blackPieceDict);
            foreach (Vector2Int y in movableTiles)
            {
                var newBlackPiece = new Dictionary<Vector2Int, GameObject>(blackPieceDict);
                var newWhitePiece = new Dictionary<Vector2Int, GameObject>(whitePieceDict);

                _aiPiecePostUpdater.UpdatePiecePost(blackPieceDict[x], y, newWhitePiece, newBlackPiece);


                var newScore = miniMaxFindBestMove(newBlackPiece, newWhitePiece, depth - 1, true, -999999999f, 999999999f);

                disposeVar(newWhitePiece);
                disposeVar(newBlackPiece);

                possibleMoves.Add(new(y, blackPieceDict[x], newScore));
                GC.Collect();
                GC.Collect();

            }
            disposeVar(movableTiles);

        }

        disposeVar(blackPieceDict);
    }

    private void updateGameObjectIpieceDict(Dictionary<Vector2Int, GameObject> whitePieceDict, Dictionary<Vector2Int, GameObject> blackPieceDict)
    {
        _gameObjectIPieceDict.Clear();
        foreach (GameObject x in whitePieceDict.Values) _gameObjectIPieceDict.Add(x, x.GetComponent<IPiece>());
        foreach (GameObject x in blackPieceDict.Values) _gameObjectIPieceDict.Add(x, x.GetComponent<IPiece>());

    }

    //recursively performs minimax to the desired depth
    float miniMaxFindBestMove(Dictionary<Vector2Int, GameObject> blackPieceDict, Dictionary<Vector2Int, GameObject> whitePieceDict, int depth, bool isWhiteTurn, float alpha, float beta)
    {

        var score = (isWhiteTurn) ? (Mathf.Infinity) : (Mathf.NegativeInfinity);

        if (depth < 1 || _aiPiecePostUpdater.IsKingDead(whitePieceDict, blackPieceDict))
        {

            score = _heuristicFunctionCalc.CalcHeuristics(blackPieceDict, whitePieceDict,_gameObjectIPieceDict);
            return score;
        }
        Dictionary<Vector2Int, GameObject> toCheckDict = (isWhiteTurn) ? (whitePieceDict) : (blackPieceDict);
        foreach (Vector2Int x in toCheckDict.Keys)
        {
            List<Vector2Int> movableTiles = _gameObjectIPieceDict[toCheckDict[x]].MovableTilePosts(x,whitePieceDict,blackPieceDict);
            foreach (Vector2Int y in movableTiles)
            {

                var newBlackPiece = new Dictionary<Vector2Int, GameObject>(blackPieceDict);
                var newWhitePiece = new Dictionary<Vector2Int, GameObject>(whitePieceDict);


                _aiPiecePostUpdater.UpdatePiecePost(toCheckDict[x], y, newWhitePiece, newBlackPiece);

                var newScore = miniMaxFindBestMove(newBlackPiece, newWhitePiece, depth - 1, !isWhiteTurn, alpha, beta);

                disposeVar(newWhitePiece);
                disposeVar(newBlackPiece);


                score = (isWhiteTurn) ? (Mathf.Min(score, newScore)) : (Mathf.Max(score, newScore));

                if (isWhiteTurn)//minimizing player
                {
                    score = Mathf.Min(score, newScore);
                    beta = Mathf.Min(newScore, beta);
                }
                else//maximising player
                {
                    score = Mathf.Max(score, newScore);
                    alpha = Mathf.Max(alpha, newScore);
                }

                if (beta <= alpha) break;


            }
            disposeVar(movableTiles);
        }
        disposeVar(whitePieceDict);
        disposeVar(blackPieceDict);
        disposeVar(toCheckDict);
        return (int)score;
    }


    void disposeVar(Dictionary<Vector2Int, GameObject> dict)
    {
        dict.Clear();
    }

    void disposeVar(List<Vector2Int> dict)
    {
        dict.Clear();
    }


}


