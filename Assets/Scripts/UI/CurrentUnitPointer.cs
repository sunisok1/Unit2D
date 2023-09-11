using UnityEngine;

public class CurrentUnitPointer : MonoBehaviour
{
    [SerializeField] private float offset;
    private void Awake()
    {
        TurnSystem.OnCurrentUnitEnter += TurnSystem_OnCurrentUnitEnter;
    }

    private void TurnSystem_OnCurrentUnitEnter(object obj)
    {
        Unit unit = obj as Unit;
        transform.position = unit.transform.position + offset * Vector3.up;
    }
}
