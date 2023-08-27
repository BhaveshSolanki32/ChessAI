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
                Tuple<Dictionary<Vector2Int, GameObject>, Dictionary<Vector2Int, GameObject>> _newPieceDict = aiPiecePostUpdater.UpdatePiecePost(x, y, _whitePiece, _blackPiece);

                Dictionary<Vector2Int, GameObject> _newBlackPiece = _newPieceDict.Item1;
                Dictionary<Vector2Int, GameObject> _newWhitePiece = _newPieceDict.Item2;


                int _score = (int) miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth-1, false);

                _possibleMoves.Add(new(y, x, _score));
            }
        }

        return _possibleMoves;
    }

    //recursively performs minimax to the desired depth
    float miniMaxFindBestMove(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth, bool _isWhiteTurn)
    {

        float _score=(_isWhiteTurn)?(Mathf.Infinity):(Mathf.NegativeInfinity);

        if (_depth <= 1 || aiPiecePostUpdater.IsKingDead( ref _whitePiece,ref _blackPiece))
        {
           _score =  GetComponent<HeuristicFunctionCalc>().CalcHeuristics(_blackPiece, _whitePiece);
            Debug.Log("calc heurao " + _score);
            return _score;
        }
        Dictionary<Vector2Int, GameObject> _toCheckDict = (_isWhiteTurn)?(_whitePiece):(_blackPiece);
        foreach (GameObject x in _toCheckDict.Values)
        {
            List<Vector2Int> _movableTiles = x.GetComponent<IPiece>().MovableTilePosts();
            foreach(Vector2Int y in _movableTiles)
            {
                Tuple<Dictionary<Vector2Int, GameObject>, Dictionary<Vector2Int, GameObject>> _newPieceDict = aiPiecePostUpdater.UpdatePiecePost(x, y, _whitePiece, _blackPiece);

                Dictionary<Vector2Int, GameObject> _newBlackPiece = _newPieceDict.Item1;
                Dictionary<Vector2Int, GameObject> _newWhitePiece = _newPieceDict.Item2;


                float _newScore = miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth-1, !_isWhiteTurn);

                _score = (_isWhiteTurn) ? (Mathf.Min(_score, _newScore)) : (Mathf.Max(_score, _newScore));
                
            }
        }

        return _score;
    }




}


