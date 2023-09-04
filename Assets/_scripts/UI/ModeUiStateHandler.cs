using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeUiStateHandler : MonoBehaviour, IUserInterface
{
    [SerializeField] ModeChanger modeChanger;
    private void Awake()
    {
        modeChanger.OnModeChange += (bool _temp)=> DeactivateScreen();
    }
    public void ActivateScreen()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateScreen()
    {
        gameObject.SetActive(false);
    }
}
