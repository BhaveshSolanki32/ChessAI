using UnityEngine;

public class GridData : MonoBehaviour
{
    public Vector2Int GridSize;
    public float CellSize;

    public GameObject GetTile(Vector2Int _post)
    {
        int _index = (_post.x - 1) * GridSize.y + _post.y - 1;

        if (_index > transform.childCount - 1 || _index < 0 || _post.x<=0||_post.y<=0 || _post.x>GridSize.x || _post.y>GridSize.y)
            return null;
        else
        {
            GameObject _tileNode = transform.GetChild(_index).gameObject;
            if (_tileNode.GetComponent<GridNodeData>().GridPostion != _post) Debug.LogError("wrong tile sent asked for "+_post+"returned = "+ _tileNode.GetComponent<GridNodeData>().GridPostion,_tileNode);
            return _tileNode;
        }
            
    }

    public bool TileExists(Vector2Int _post)
    {
        int _index = (_post.x - 1) * GridSize.y + _post.y - 1;
        if (_index > transform.childCount - 1 || _index < 0 || _post.x <= 0 || _post.y <= 0 || _post.x > GridSize.x || _post.y > GridSize.y)
            return false;
        else
            return true;
    }
}
