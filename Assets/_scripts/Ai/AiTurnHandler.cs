using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(HeuristicFunctionCalc), typeof(IMiniMax))]
public class AiTurnHandler : MonoBehaviour
{
    [SerializeField] GameObject _pieceGameObject;
    PiecesDictsData _piecesData;
    MovePiece _movePiece;
    int _depth = 5;

    private void Awake()
    {

        if (_pieceGameObject.TryGetComponent<MovePiece>(out _movePiece)) _movePiece.OnPieceEndMoving += initialize;
        else UnityEngine.Debug.LogError("MovePiece not found", _pieceGameObject);

        if (!_pieceGameObject.TryGetComponent<PiecesDictsData>(out _piecesData))
            UnityEngine.Debug.LogError("PieceData not found", _pieceGameObject);

    }

    private void initialize(GameObject piece)
    {
        GC.Collect();
        GC.Collect();
        GC.Collect();


        var stopWatch = new Stopwatch();
        stopWatch.Start();
        var blackPiece = new Dictionary<Vector2Int, GameObject>(_piecesData.BlackPieceDict);
        var whitePiece = new Dictionary<Vector2Int, GameObject>(_piecesData.WhitePieceDict);

        if (blackPiece.ContainsValue(piece)) return; //it was black's turns previously

        var possibleMoves = new List<Tuple<Vector2Int, GameObject, float>>();
        GetComponent<IMiniMax>().GetBestMoves(blackPiece, whitePiece, _depth, possibleMoves);

        var bestPossibleMovesList = new List<Tuple<Vector2Int, GameObject, float>>();

        var selectedPiece = possibleMoves[0].Item2;
        var toMovePost = new Vector2Int();
        var bestMove = Mathf.NegativeInfinity;
        foreach (var x in possibleMoves)
        {

            if (bestMove <= x.Item3)
            {
                if (bestMove < x.Item3) bestPossibleMovesList = new();
                bestMove = x.Item3;
                bestPossibleMovesList.Add(x);
            }
        }
        var randomIndex = UnityEngine.Random.Range(0, bestPossibleMovesList.Count);
        selectedPiece = bestPossibleMovesList[randomIndex].Item2;
        toMovePost = bestPossibleMovesList[randomIndex].Item1;

        stopWatch.Stop();

        UnityEngine.Debug.Log($"time taken by ai = ,{ stopWatch.ElapsedMilliseconds}");

        UnityEngine.Debug.Log($"{toMovePost}  {selectedPiece.GetComponent<IPiece>().GetType()} score = { bestMove}", selectedPiece);
        _movePiece.MovePieceTo(selectedPiece, toMovePost, true);


    }

    private void OnDestroy()
    {
        _movePiece.OnPieceEndMoving -= initialize;
    }

}
