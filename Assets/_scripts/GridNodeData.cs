using System.Collections.Generic;
using UnityEngine;


public class GridNodeData : MonoBehaviour
{
    public Vector2Int GridPostion;
    public List<GameObject> NeighbourList { get; private set; } = new List<GameObject>() { };

    private void Awake()
    {
        NeighbourList.AddRange(GetNeighbours.s_FindNeighbour(gameObject, GetComponentInParent<GridData>()));
    }
}
