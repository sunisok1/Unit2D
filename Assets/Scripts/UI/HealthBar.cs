using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    int hp;
    int maxHp;
    [SerializeField] GameObject hpPrefab;

    List<GameObject> hpList = new();

    [SerializeField] Sprite hpSprite1;
    [SerializeField] Sprite hpSprite2;
    [SerializeField] Sprite hpSprite3;
    [SerializeField] Sprite hpSprite4;

    private void Awake()
    {
        Unit unit = GetComponentInParent<Unit>();
        unit.OnHpChange += SetHp;
        unit.OnMaxHpChange += SetMaxHp;
    }
    public void SetHp(int hp)
    {
        Sprite sprite;
        sprite = ((float)hp / maxHp) switch
        {
            >= 0.75f => hpSprite1,
            >= 0.50f => hpSprite2,
            _ => hpSprite3,
        };
        switch ((float)hp / maxHp)
        {
            case >= 0.75f:
                sprite = hpSprite1;
                break;
            case >= 0.50f:
                sprite = hpSprite2;
                break;
            case >= 0.25f:
                sprite = hpSprite3;
                break;
            default:
                break;
        }
        int i = 0;
        while (i < hp)
        {
            hpList[i].GetComponent<Image>().sprite = sprite;
            i++;
        }
        while (i < hpList.Count)
        {
            hpList[i].GetComponent<Image>().sprite = hpSprite4;
            i++;
        }
    }
    public void SetMaxHp(int maxHp)
    {
        this.maxHp = maxHp;
        for (int i = hpList.Count; i < maxHp; i++)
        {
            GameObject hp = Instantiate(hpPrefab, transform);
            hpList.Add(hp);
        }
    }
}
