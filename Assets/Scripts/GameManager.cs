using UnityEngine;
using UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        TurnSystemUI.Init();
        CardUICreater.Init();

        Unit unit1 = UnitManager.SpawnUnit(0, 0, "01", false);
        Unit unit2 = UnitManager.SpawnUnit(1, 1, "02", false);
        Unit unit3 = UnitManager.SpawnUnit(1, 4, "03", false);
        Unit unit4 = UnitManager.SpawnUnit(1, 5, "04", false);
        unit1.SetCharactor(Standard.GetCharacter("liubei"));
        unit2.SetCharactor(Standard.GetCharacter("guanyu"));
        unit3.SetCharactor(Standard.GetCharacter("zhangfei"));
        unit4.SetCharactor(Standard.GetCharacter("huangyueying"));

        TurnSystem.StartGame();
    }

}
