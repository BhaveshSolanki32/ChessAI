using System;
using System.Collections;
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
        
        if (pieceGameObject.TryGetComponent<MovePiece>(out movePiece)) movePiece.OnPieceMoved += initialize;
        else Debug.LogError("MovePiece not found", pieceGameObject);

        if (!pieceGameObject.TryGetComponent<PiecesData>(out piecesData))
         Debug.LogError("PieceData not found", pieceGameObject);

    }

    private void initialize(GameObject arg1, Vector2Int arg2)
    {
        Dictionary<Vector2Int, GameObject> _blackPiece = piecesData.BlackPieceDict;
        Dictionary<Vector2Int, GameObject> _whitePiece = piecesData.WhitePieceDict;

        List<Tuple<Vector2Int, GameObject, int>> _bestMoves =  GetComponent<MiniMaxHandler>().GetBestMoves(_blackPiece, _whitePiece, Depth);

        GameObject _selectedPiece = _bestMoves[0].Item2;
        Vector2Int _toMovePost = new();
        float _bestMove = Mathf.NegativeInfinity;
        foreach(Tuple<Vector2Int, GameObject, int> x in _bestMoves)
        {
            Debug.Log("post = " + x.Item1 + " GO = " + x.Item2.GetComponent<IPiece>().GetType() + " score = " + x.Item3);
            if(_bestMove<x.Item3)
            {
                _bestMove = x.Item3;
                _selectedPiece = x.Item2;
                _toMovePost = x.Item1;
            }
        }

        movePiece.MovePieceTo(_selectedPiece, _toMovePost,true);

    }
}
