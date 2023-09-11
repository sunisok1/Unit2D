using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Lib.Init();

        TurnSystemUI.Init();
        CardUICreater.Init();

        UnitManager.SpawnUnit(0, 0, "01");
        UnitManager.SpawnUnit(1, 1, "02");
        UnitManager.SpawnUnit(1, 4, "03");

        TurnSystem.StartGame();
    }

}
