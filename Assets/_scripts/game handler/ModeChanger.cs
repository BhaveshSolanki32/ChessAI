using System;
using UnityEngine;

public class ModeChanger : MonoBehaviour
{
    public Action<bool> OnModeChange;
    [SerializeField] PiecesDictsData piecesData;
    [SerializeField] GameObject aiGameObject;

    public void ChangeMode(bool _isVsAi)
    {
        OnModeChange?.Invoke(_isVsAi);
        if (!_isVsAi)
        {
            foreach (GameObject x in piecesData.BlackPieceDict.Values)
            {
                x.AddComponent<BoxCollider2D>();
            }
            Destroy(aiGameObject);
        }
            
    }

}
