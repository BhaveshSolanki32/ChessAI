using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AiPiecePostUpdater))]
public class MiniMaxHandler : MonoBehaviour
{
    AiPiecePostUpdater aiPiecePostUpdater;


    private void Awake()
    {
        aiPiecePostUpdater = GetComponent<AiPiecePostUpdater>();
    }

    //returns list of all posible moves with thier scores
    public List<Tuple<Vector2Int, GameObject, int>> GetBestMoves(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth)
    {

        List<Tuple<Vector2Int, GameObject, int>> _possibleMoves = new(); 

        foreach (GameObject x in _blackPiece.Values)
        {
            List<Vector2Int> _movableTiles = x.GetComponent<IPiece>().MovableTilePosts();
            foreach (Vector2Int y in _movableTiles)
            {
                Tuple<Dictionary<Vector2Int, GameObject>, Dictionary<Vector2Int, GameObject>> _newPieceDict = aiPiecePostUpdater.UpdatePiecePost(x, y, new(_whitePiece),new( _blackPiece));

                Dictionary<Vector2Int, GameObject> _newBlackPiece = _newPieceDict.Item2;
                Dictionary<Vector2Int, GameObject> _newWhitePiece = _newPieceDict.Item1;


                int _score = (int) miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth-1, true, -999999999, 999999999);

                _possibleMoves.Add(new(y, x, _score));
            }
        }

        return _possibleMoves;
    }

    //recursively performs minimax to the desired depth
    int miniMaxFindBestMove(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth, bool _isWhiteTurn, int _alpha, int _beta)
    {

        int _score=(_isWhiteTurn)?(999999) :(-999999);

        if (_depth < 1 || aiPiecePostUpdater.IsKingDead(  _whitePiece, _blackPiece))
        {
           _score =  GetComponent<HeuristicFunctionCalc>().CalcHeuristics(_blackPiece, _whitePiece);

            return _score;
        }
        Dictionary<Vector2Int, GameObject> _toCheckDict = (_isWhiteTurn)?(_whitePiece):(_blackPiece);
        foreach (GameObject x in _toCheckDict.Values)
        {
            List<Vector2Int> _movableTiles = x.GetComponent<IPiece>().MovableTilePosts();
            foreach(Vector2Int y in _movableTiles)
            {
                Tuple<Dictionary<Vector2Int, GameObject>, Dictionary<Vector2Int, GameObject>> _newPieceDict = aiPiecePostUpdater.UpdatePiecePost(x, y, new(_whitePiece),new( _blackPiece));

                Dictionary<Vector2Int, GameObject> _newBlackPiece = _newPieceDict.Item2;
                Dictionary<Vector2Int, GameObject> _newWhitePiece = _newPieceDict.Item1;


                int _newScore = miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth-1, !_isWhiteTurn, _alpha, _beta);

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
        }

        return _score;
    }




}


