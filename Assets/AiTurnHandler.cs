using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HeuristicFunctionCalc), typeof(MiniMaxHandler))]
public class AiTurnHandler : MonoBehaviour
{
    [SerializeField] GameObject pieceGameObject;
    PiecesData piecesData;
    MovePiece movePiece;
    public int Depth=5;
    private void Awake()
    {
        
        if (pieceGameObject.TryGetComponent<MovePiece>(out movePiece)) movePiece.OnPieceEndMoving += initialize;
        else UnityEngine.Debug.LogError("MovePiece not found", pieceGameObject);

        if (!pieceGameObject.TryGetComponent<PiecesData>(out piecesData))
            UnityEngine.Debug.LogError("PieceData not found", pieceGameObject);

    }

    private void initialize(GameObject _piece)
    {
        Stopwatch _stopWatch = new();
        _stopWatch.Start();
        Dictionary<Vector2Int, GameObject> _blackPiece = new( piecesData.BlackPieceDict);
        Dictionary<Vector2Int, GameObject> _whitePiece = new( piecesData.WhitePieceDict);

        if (_blackPiece.ContainsValue(_piece)) return;
        
        List<Tuple<Vector2Int, GameObject, int>> _possibleMoves =  GetComponent<MiniMaxHandler>().GetBestMoves(_blackPiece, _whitePiece, Depth);

        List<Tuple<Vector2Int, GameObject, int>> _bestPossibleMovesList = new();

        GameObject _selectedPiece = _possibleMoves[0].Item2;
        Vector2Int _toMovePost = new();
        float _bestMove = Mathf.NegativeInfinity;
        foreach(Tuple<Vector2Int, GameObject, int> x in _possibleMoves)
        {
          // UnityEngine.Debug.Log("new post = " + x.Item1 + " GO = " + x.Item2.GetComponent<IPiece>().GetType() + " score = " + x.Item3);
            if(_bestMove<=x.Item3)
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
        UnityEngine.Debug.Log(_toMovePost+"  "+_selectedPiece.GetComponent<IPiece>().GetType(), _selectedPiece);
        movePiece.MovePieceTo(_selectedPiece, _toMovePost,true);
        
    }
}
