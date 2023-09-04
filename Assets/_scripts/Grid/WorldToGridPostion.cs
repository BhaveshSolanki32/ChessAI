using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldToGridPostion
{
    public static Vector2Int Convert(Vector3 _post,GridData _gridData)
    {
        Vector2 _convertedPost = _post - _gridData.gameObject.transform.position;
        return new(Mathf.RoundToInt(_convertedPost.x), Mathf.RoundToInt(_convertedPost.y));
    }
}
