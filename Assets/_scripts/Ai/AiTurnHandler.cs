using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(HeuristicFunctionCalc), typeof(IMiniMax))]
public class AiTurnHandler : MonoBehaviour
{
    [SerializeField] GameObject pieceGameObject;
    PiecesData piecesData;
    MovePiece movePiece;
    public int Depth = 5;
    private void Awake()
    {

        if (pieceGameObject.TryGetComponent<MovePiece>(out movePiece)) movePiece.OnPieceEndMoving += initialize;
        else UnityEngine.Debug.LogError("MovePiece not found", pieceGameObject);

        if (!pieceGameObject.TryGetComponent<PiecesData>(out piecesData))
            UnityEngine.Debug.LogError("PieceData not found", pieceGameObject);

    }

    private void initialize(GameObject _piece)
    {
        GC.Collect();
        GC.Collect();
        GC.Collect();


        Stopwatch _stopWatch = new();
        _stopWatch.Start();
        Dictionary<Vector2Int, GameObject> _blackPiece = new(piecesData.BlackPieceDict);
        Dictionary<Vector2Int, GameObject> _whitePiece = new(piecesData.WhitePieceDict);

        if (_blackPiece.ContainsValue(_piece)) return; //it was black's turns previously

        List<Tuple<Vector2Int, GameObject, float>> _possibleMoves = new();
        GetComponent<IMiniMax>().GetBestMoves(_blackPiece, _whitePiece, Depth, _possibleMoves);

        List<Tuple<Vector2Int, GameObject, float>> _bestPossibleMovesList = new();

        GameObject _selectedPiece = _possibleMoves[0].Item2;
        Vector2Int _toMovePost = new();
        float _bestMove = Mathf.NegativeInfinity;
        foreach (Tuple<Vector2Int, GameObject, float> x in _possibleMoves)
        {

            if (_bestMove <= x.Item3)
            {
                if (_bestMove < x.Item3) _bestPossibleMovesList = new();
                _bestMove = x.Item3;
                _bestPossibleMovesList.Add(x);
            }
        }
        int _randomIndex = UnityEngine.Random.Range(0, _bestPossibleMovesList.Count);
        _selectedPiece = _bestPossibleMovesList[_randomIndex].Item2;
        _toMovePost = _bestPossibleMovesList[_randomIndex].Item1;

        _stopWatch.Stop();

        UnityEngine.Debug.Log("time taken by ai = " + _stopWatch.ElapsedMilliseconds);
        UnityEngine.Debug.Log(_toMovePost + "  " + _selectedPiece.GetComponent<IPiece>().GetType() + "score = " + _bestMove, _selectedPiece);
        movePiece.MovePieceTo(_selectedPiece, _toMovePost, true);


    }

    private void OnDestroy()
    {
        movePiece.OnPieceEndMoving -= initialize;
    }

}
