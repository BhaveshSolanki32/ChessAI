using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour // generates a grid based on given parameters
{
    [SerializeField] int height, width;
    public GameObject PlaceHolder;
    [SerializeField] float cellSize;
    public void CreateGrid()
    {
        GameObject _tiles = new GameObject();
        _tiles.name = "tiles";
        GridData _gridData = _tiles.AddComponent<GridData>();
        for (int i = 1; i <= width; i++)
        {
            for (int j = 1; j <= height; j++)
            {
                GameObject _tile = Instantiate(PlaceHolder, new Vector3(i, j, 0) * cellSize, PlaceHolder.transform.rotation);
                _tile.transform.SetParent(_tiles.transform);
                _tile.GetComponent<GridNodeData>().GridPostion = new(i,j);
            }
        }
        _gridData.GridSize = new(width, height);
        _gridData.CellSize = cellSize;
        _tiles.transform.position = new Vector3(width + 1, height + 1, 0) * -1 * cellSize / 2;
    }
}

