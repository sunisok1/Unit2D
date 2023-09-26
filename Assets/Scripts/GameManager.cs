using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Lib.Init();

        TurnSystemUI.Init();
        CardUICreater.Init();

        UnitManager.SpawnUnit(0, 0, "01", false);
        var u = UnitManager.SpawnUnit(1, 1, "02", false);
        UnitManager.SpawnUnit(1, 4, "03", false);
        UnitManager.SpawnUnit(1, 5, "04", false);

        u.Draw(5);

        TurnSystem.StartGame();
    }

}
