using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IMiniMax //implemented by both minimax algo (jobs/simple)
{
    void GetBestMoves(Dictionary<Vector2Int, GameObject> _blackPiece, Dictionary<Vector2Int, GameObject> _whitePiece, int _depth, List<Tuple<Vector2Int, GameObject, int>> _possibleMoves);

}