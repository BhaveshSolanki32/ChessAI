using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AiPiecePostUpdater), typeof(HeuristicFunctionCalc))]
public class MiniMaxHandler : MonoBehaviour, IMiniMax
{
    AiPiecePostUpdater aiPiecePostUpdater;

    HeuristicFunctionCalc heuristicFunctionCalc;


    private void Awake()
    {
        aiPiecePostUpdater = GetComponent<AiPiecePostUpdater>();
        heuristicFunctionCalc = GetComponent<HeuristicFunctionCalc>();
    }

    //returns list of all posible moves with thier scores
    public void GetBestMoves(Dictionary<Vector2Int, GameObject> _blackPieceDict, Dictionary<Vector2Int, GameObject> _whitePieceDict, int _depth, List<Tuple<Vector2Int, GameObject, float>> _possibleMoves)
    {


        foreach (Vector2Int x in _blackPieceDict.Keys)
        {
            List<Vector2Int> _movableTiles = _blackPieceDict[x].GetComponent<IPiece>().MovableTilePosts(x, _whitePieceDict,_blackPieceDict);
            foreach (Vector2Int y in _movableTiles)
            {
                Dictionary<Vector2Int, GameObject> _newBlackPiece = new(_blackPieceDict);
                Dictionary<Vector2Int, GameObject> _newWhitePiece = new(_whitePieceDict);

                aiPiecePostUpdater.UpdatePiecePost(_blackPieceDict[x], y, _newWhitePiece, _newBlackPiece);


                float _newScore = miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth - 1, true, -999999999f, 999999999f);

                disposeVar(_newWhitePiece);
                disposeVar(_newBlackPiece);

                _possibleMoves.Add(new(y, _blackPieceDict[x], _newScore));
                GC.Collect();
                GC.Collect();
                GC.Collect();

            }
            disposeVar(_movableTiles);

        }

        disposeVar(_blackPieceDict);
    }

    //recursively performs minimax to the desired depth
    float miniMaxFindBestMove(Dictionary<Vector2Int, GameObject> _blackPieceDict, Dictionary<Vector2Int, GameObject> _whitePieceDict, int _depth, bool _isWhiteTurn, float _alpha, float _beta)
    {

        float _score = (_isWhiteTurn) ? (Mathf.Infinity) : (Mathf.NegativeInfinity);

        if (_depth < 1 || aiPiecePostUpdater.IsKingDead(_whitePieceDict, _blackPieceDict))
        {

            _score = heuristicFunctionCalc.CalcHeuristics(_blackPieceDict, _whitePieceDict);
            return _score;
        }
        Dictionary<Vector2Int, GameObject> _toCheckDict = (_isWhiteTurn) ? (_whitePieceDict) : (_blackPieceDict);
        foreach (Vector2Int x in _toCheckDict.Keys)
        {
            List<Vector2Int> _movableTiles = _toCheckDict[x].GetComponent<IPiece>().MovableTilePosts(x,_whitePieceDict,_blackPieceDict);
            foreach (Vector2Int y in _movableTiles)
            {

                Dictionary<Vector2Int, GameObject> _newBlackPiece = new(_blackPieceDict);
                Dictionary<Vector2Int, GameObject> _newWhitePiece = new(_whitePieceDict);


                aiPiecePostUpdater.UpdatePiecePost(_toCheckDict[x], y, _newWhitePiece, _newBlackPiece);

                float _newScore = miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth - 1, !_isWhiteTurn, _alpha, _beta);

                disposeVar(_newWhitePiece);
                disposeVar(_newBlackPiece);


                _score = (_isWhiteTurn) ? (Mathf.Min(_score, _newScore)) : (Mathf.Max(_score, _newScore));

                if (_isWhiteTurn)//minimizing player
                {
                    _score = Mathf.Min(_score, _newScore);
                    _beta = Mathf.Min(_newScore, _beta);
                }
                else//maximising player
                {
                    _score = Mathf.Max(_score, _newScore);
                    _alpha = Mathf.Max(_alpha, _newScore);
                }

                if (_beta <= _alpha) break;


            }
            disposeVar(_movableTiles);
        }
        disposeVar(_whitePieceDict);
        disposeVar(_blackPieceDict);
        disposeVar(_toCheckDict);
        return (int)_score;
    }


    void disposeVar(Dictionary<Vector2Int, GameObject> _dict)
    {
        _dict.Clear();
    }

    void disposeVar(List<Vector2Int> _dict)
    {
        _dict.Clear();
    }


}


