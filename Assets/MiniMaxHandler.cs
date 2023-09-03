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
    public void GetBestMoves(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth, List<Tuple<Vector2Int, GameObject, int>> _possibleMoves)
    {

        foreach (GameObject x in _blackPiece.Values)
        {
            List<Vector2Int> _movableTiles = x.GetComponent<IPiece>().MovableTilePosts();
            foreach (Vector2Int y in _movableTiles)
            {
                Dictionary<Vector2Int, GameObject> _newBlackPiece = new(_blackPiece);
                Dictionary<Vector2Int, GameObject> _newWhitePiece = new(_whitePiece);

                aiPiecePostUpdater.UpdatePiecePost(x, y, _newWhitePiece, _newBlackPiece);


                int _newScore = miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth - 1, true, -999999999, 999999999);

                disposeVar(_newWhitePiece);
                disposeVar(_newBlackPiece);

                _possibleMoves.Add(new(y, x, _newScore));

            }
            disposeVar(_movableTiles);

        }
        disposeVar(_blackPiece);
    }

    //recursively performs minimax to the desired depth
    int miniMaxFindBestMove(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth, bool _isWhiteTurn, int _alpha, int _beta)
    {

        float _score = (_isWhiteTurn) ? (Mathf.Infinity) : (Mathf.NegativeInfinity);

        if (_depth < 1 || aiPiecePostUpdater.IsKingDead(_whitePiece, _blackPiece))
        {
            _score = heuristicFunctionCalc.CalcHeuristics(_blackPiece, _whitePiece);
            return (int)_score;
        }
        Dictionary<Vector2Int, GameObject> _toCheckDict = (_isWhiteTurn) ? (_whitePiece) : (_blackPiece);
        foreach (GameObject x in _toCheckDict.Values)
        {
            List<Vector2Int> _movableTiles = x.GetComponent<IPiece>().MovableTilePosts();
            foreach (Vector2Int y in _movableTiles)
            {

                Dictionary<Vector2Int, GameObject> _newBlackPiece = new(_blackPiece);
                Dictionary<Vector2Int, GameObject> _newWhitePiece = new(_whitePiece);


                aiPiecePostUpdater.UpdatePiecePost(x, y, _newWhitePiece, _newBlackPiece);

                int _newScore = miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth - 1, !_isWhiteTurn, _alpha, _beta);

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
        disposeVar(_whitePiece);
        disposeVar(_blackPiece);
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


