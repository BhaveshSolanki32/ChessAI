using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserInterface  //should be applied to all UICanvas
{
    void DeactivateScreen();
    void ActivateScreen();
}
