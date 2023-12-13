using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WorldToGridPostion
{
    public static Vector2Int Convert(Vector3 post,GridData gridData)
    {
        var convertedPost = post - gridData.gameObject.transform.position;
        return new(Mathf.RoundToInt(convertedPost.x), Mathf.RoundToInt(convertedPost.y));
    }
}
