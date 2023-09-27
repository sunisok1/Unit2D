using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Lib.Init();

        TurnSystemUI.Init();
        CardUICreater.Init();

        Unit unit1 = UnitManager.SpawnUnit(0, 0, "01", false);
        unit1.SetCharactor(Standard.GetCharacter("caocao"));
        Unit unit2 = UnitManager.SpawnUnit(1, 1, "02", false);
        Unit unit3 = UnitManager.SpawnUnit(1, 4, "03", false);
        Unit unit4 = UnitManager.SpawnUnit(1, 5, "04", false);


        TurnSystem.StartGame();
    }

}
