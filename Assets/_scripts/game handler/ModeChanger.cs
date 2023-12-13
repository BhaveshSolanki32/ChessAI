using System;
using UnityEngine;

public class ModeChanger : MonoBehaviour
{
    [SerializeField] PiecesDictsData _piecesData;
    [SerializeField] GameObject _aiGameObject;
    public Action<bool> OnModeChange;


    public void ChangeMode(bool isVsAi)
    {
        OnModeChange?.Invoke(isVsAi);
        if (!isVsAi)
        {
            foreach (var x in _piecesData.BlackPieceDict.Values)
            {
                x.AddComponent<BoxCollider2D>();
            }
            Destroy(_aiGameObject);
        }
            
    }

}
