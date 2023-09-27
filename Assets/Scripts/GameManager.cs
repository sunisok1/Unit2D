using UnityEngine;
using UI;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Lib.Init();

        TurnSystemUI.Init();
        CardUICreater.Init();

        Unit unit1 = UnitManager.SpawnUnit(0, 0, "01", false);
        Character character = Standard.GetCharacter("caocao");
        unit1.SetCharactor(character);
        Unit unit2 = UnitManager.SpawnUnit(1, 1, "02", false);
        Unit unit3 = UnitManager.SpawnUnit(1, 4, "03", false);
        Unit unit4 = UnitManager.SpawnUnit(1, 5, "04", false);

        StartCoroutine(Crawler.DownloadSkin(Standard.characters));

        TurnSystem.StartGame();
    }

}
