using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
interface IMiniMax //implemented by both minimax algo (jobs/simple)
{
    void GetBestMoves(Dictionary<Vector2Int, GameObject> _blackPieceDict, Dictionary<Vector2Int, GameObject> _whitePieceDict, int _depth, List<Tuple<Vector2Int, GameObject, float>> _possibleMoves);

}