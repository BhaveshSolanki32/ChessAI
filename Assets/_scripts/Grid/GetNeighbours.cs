using System.Collections.Generic;
using UnityEngine;

public static class GetNeighbours
{
    static List<GameObject> NeighbourList = new() { };
    


    public static List<GameObject> s_FindNeighbour(GameObject _baseTile, GridData _gridData)
    {

        NeighbourList.Clear();

        GridNodeData _gridNode = _baseTile.GetComponent<GridNodeData>();
        Vector2Int _currentPost = _gridNode.GridPostion;




        for (int i = -1; i <= 1; i++)
        {
            if (i == 0) continue;

            s_updateNeighbourList(new((_currentPost.x + i), _currentPost.y), _gridData);


            s_updateNeighbourList(new(_currentPost.x, (_currentPost.y + i)),_gridData);
        }
        return NeighbourList;

    }

    static void s_updateNeighbourList(Vector2Int _neighPostion, GridData _gridData)
    {

        GameObject _tile = _gridData.GetTile(_neighPostion);
        if (_tile != null && _tile.GetComponent<GridNodeData>().GridPostion == _neighPostion)
            NeighbourList.Add(_tile);

    }
}
