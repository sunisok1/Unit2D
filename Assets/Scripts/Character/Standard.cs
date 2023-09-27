using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 标准包
/// </summary>
public static class Standard
{
    public static List<Character> characters = new();

    static Standard()
    {
        Skill jianxiong = new()
        {
            trigger = Trigger.damageEnd,
            content = () =>
            {
                Debug.Log("发动奸雄");
            },
        };
        Add("caocao", Sex.male, Country.wei, 4, jianxiong);


    }
    private static void Add(string name, Sex sex, Country country, int maxHp, params Skill[] skills) => characters.Add(new(name, sex, country, maxHp, skills));
    public static Character GetCharacter(string name)
    {
        foreach (Character character in characters)
        {
            if (character.name == name)
            {
                return character;
            }
        }
        return null;
    }
}