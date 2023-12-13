using System.Collections.Generic;
using UnityEngine;

public static class GetNeighbours
{
    static List<GameObject> NeighbourList = new() { };
    

    public static List<GameObject> s_FindNeighbour(GameObject baseTile, GridData gridData)
    {

        NeighbourList.Clear();

        var gridNode = baseTile.GetComponent<GridNodeData>();
        var currentPost = gridNode.GridPostion;




        for (int i = -1; i <= 1; i++)
        {
            if (i == 0) continue;

            s_updateNeighbourList(new((currentPost.x + i), currentPost.y), gridData);


            s_updateNeighbourList(new(currentPost.x, (currentPost.y + i)),gridData);
        }
        return NeighbourList;

    }

    static void s_updateNeighbourList(Vector2Int neighPostion, GridData gridData)
    {

        var tile = gridData.GetTile(neighPostion);
        if (tile != null && tile.GetComponent<GridNodeData>().GridPostion == neighPostion)
            NeighbourList.Add(tile);

    }
}
