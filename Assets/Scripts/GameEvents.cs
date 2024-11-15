using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<GameObject> OnCharacterSelected;
    public static Action<Vector3> OnMoveCommand;
    public static void ClearAll()
    {

    }
}
