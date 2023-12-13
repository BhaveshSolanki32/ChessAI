using System.Collections.Generic;
using UnityEngine;


public class GridNodeData : MonoBehaviour
{
    Vector2Int _gridPostion;
    List<GameObject> _neighbourList = new List<GameObject>();
    public Vector2Int GridPostion { get { return _gridPostion; } set { _gridPostion = value; } }
    public List<GameObject> NeighbourList { get { return _neighbourList; } set { _neighbourList = value; } }

    private void Awake()
    {
        NeighbourList = new List<GameObject>();
        _neighbourList.AddRange(GetNeighbours.s_FindNeighbour(gameObject, GetComponentInParent<GridData>()));
    }
}
