using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

[RequireComponent(typeof(AiPiecePostUpdater), typeof(HeuristicFunctionCalc))]
public class MiniMaxHandler : MonoBehaviour
{
    AiPiecePostUpdater aiPiecePostUpdater;

    HeuristicFunctionCalc heuristicFunctionCalc;


    private void Awake()
    {
        aiPiecePostUpdater = GetComponent<AiPiecePostUpdater>();
        heuristicFunctionCalc = GetComponent<HeuristicFunctionCalc>();
    }

    //returns list of all posible moves with thier scores
    public List<Tuple<Vector2Int, GameObject, int>> GetBestMoves(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth)
    {

        List<Tuple<Vector2Int, GameObject, int>> _possibleMoves = new();

        NativeList<JobHandle> _miniMaxJobHandleList = new NativeList<JobHandle>(Allocator.Temp);
        List<MiniMaxJobStruct> _minimaxJobList = new List<MiniMaxJobStruct>();
        NativeArray<int> _scoreJobArray = new NativeArray<int>(1,Allocator.TempJob);
        Dictionary<GameObject, IPiece> _pieceIPieceDict = new();

        foreach (GameObject x in _blackPiece.Values) _pieceIPieceDict.Add(x, x.GetComponent<IPiece>());
        foreach (GameObject x in _whitePiece.Values) _pieceIPieceDict.Add(x, x.GetComponent<IPiece>());



        foreach (GameObject x in _blackPiece.Values)
        {
            List<Vector2Int> _movableTiles = x.GetComponent<IPiece>().MovableTilePosts();
            foreach (Vector2Int y in _movableTiles)
            {
                Tuple<Dictionary<Vector2Int, GameObject>, Dictionary<Vector2Int, GameObject>> _newPieceDict = aiPiecePostUpdater.UpdatePiecePost(x, y, new(_whitePiece),new( _blackPiece));

                Dictionary<Vector2Int, GameObject> _newBlackPiece = _newPieceDict.Item2;
                Dictionary<Vector2Int, GameObject> _newWhitePiece = _newPieceDict.Item1;



                //int _score = miniMaxFindBestMove(_newBlackPiece, _newWhitePiece, _depth - 1, true, -999999999, 999999999);
                //_possibleMoves.Add(new(y, x, _score));


                MiniMaxJobStruct _miniMaxJob = new MiniMaxJobStruct(_newBlackPiece, _newWhitePiece, _depth - 1, true, -999999999, 999999999, aiPiecePostUpdater, heuristicFunctionCalc, _scoreJobArray, y, x, _pieceIPieceDict);
                _minimaxJobList.Add(_miniMaxJob);
                JobHandle _miniMaxJobHandle = _miniMaxJob.Schedule();
                _miniMaxJobHandleList.Add(_miniMaxJobHandle);
            }
        }
        JobHandle.CompleteAll(_miniMaxJobHandleList);
        foreach (MiniMaxJobStruct x in _minimaxJobList)
        {
            _possibleMoves.Add(new(x.Post, x.PieceGameObject, x.Score[0]));
        }
        _miniMaxJobHandleList.Dispose();
        _scoreJobArray.Dispose();
        return _possibleMoves;
    }

    //recursively performs minimax to the desired depth
    int miniMaxFindBestMove(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth, bool _isWhiteTurn, int _alpha, int _beta)
    {

        int _score=(_isWhiteTurn)?(999999) :(-999999);

        if (_depth < 1 || aiPiecePostUpdater.IsKingDead(  _whitePiece, _blackPiece))
        {
            _score =  heuristicFunctionCalc.CalcHeuristics(_blackPiece, _whitePiece);

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

    public struct MiniMaxJobStruct : IJob
    {
        Dictionary<Vector2Int, GameObject> blackPiece;
        Dictionary<Vector2Int, GameObject> whitePiece;
        Dictionary<GameObject, IPiece> pieceIPieceDict;
        int depth;
        bool isWhiteTurn;
        int alpha, beta;
        public NativeArray<int> Score;
         AiPiecePostUpdater aiPiecePostUpdater;
         HeuristicFunctionCalc heuristicFunctionCalc;
        public Vector2Int Post;
        public GameObject PieceGameObject;

        public MiniMaxJobStruct( Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth, bool _isWhiteTurn, int _alpha, int _beta,
         AiPiecePostUpdater _aiPiecePostUpdater, HeuristicFunctionCalc _heuristicFunctionCalc, NativeArray<int> _score, Vector2Int _post, GameObject _piece, Dictionary<GameObject, IPiece> _pieceIPieceDict)
        {
            blackPiece = _blackPiece;
            whitePiece = _whitePiece;
            depth = _depth;
            isWhiteTurn = _isWhiteTurn;
            alpha = _alpha;
            beta = _beta;
            aiPiecePostUpdater = _aiPiecePostUpdater;
            heuristicFunctionCalc = _heuristicFunctionCalc;
            Score =_score;
            Post = _post;
            PieceGameObject = _piece;
            pieceIPieceDict = _pieceIPieceDict;
        }

        public void Execute()
        {

            Score[0] = startMiniMax(blackPiece, whitePiece,depth,  isWhiteTurn,  alpha,  beta);
        }

        int startMiniMax(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth, bool _isWhiteTurn, int _alpha, int _beta)
        {

            int _score = (_isWhiteTurn) ? (999999) : (-999999);

            if (_depth < 1 || aiPiecePostUpdater.IsKingDead(_whitePiece, _blackPiece))
            {
                _score = heuristicFunctionCalc.CalcHeuristics(_blackPiece, _whitePiece);

                return _score;
            }
            Dictionary<Vector2Int, GameObject> _toCheckDict = (_isWhiteTurn) ? (_whitePiece) : (_blackPiece);
            foreach (GameObject x in _toCheckDict.Values)
            {
                List<Vector2Int> _movableTiles = pieceIPieceDict[x].MovableTilePosts();
                foreach (Vector2Int y in _movableTiles)
                {
                    Tuple<Dictionary<Vector2Int, GameObject>, Dictionary<Vector2Int, GameObject>> _newPieceDict = aiPiecePostUpdater.UpdatePiecePost(x, y, new(_whitePiece), new(_blackPiece));

                    Dictionary<Vector2Int, GameObject> _newBlackPiece = _newPieceDict.Item2;
                    Dictionary<Vector2Int, GameObject> _newWhitePiece = _newPieceDict.Item1;


                    int _newScore = startMiniMax(_newBlackPiece, _newWhitePiece, _depth - 1, !_isWhiteTurn, _alpha, _beta);

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



}


