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
        characters.Add(new("liubei", 1, Sex.male, Country.shu, 4));
        characters.Add(new("guanyu", 2, Sex.male, Country.shu, 4));
        characters.Add(new("zhangfei", 3, Sex.male, Country.shu, 4));
        characters.Add(new("zhugeliang", 4, Sex.male, Country.shu, 3));
        characters.Add(new("zhaoyun", 5, Sex.male, Country.shu, 4));
        characters.Add(new("machao", 6, Sex.male, Country.shu, 4));
        characters.Add(new("huangyueying", 7, Sex.female, Country.shu, 3));
        characters.Add(new("sunquan", 8, Sex.male, Country.wu, 4));
        characters.Add(new("ganning", 9, Sex.male, Country.wu, 4));
        characters.Add(new("lvmeng", 10, Sex.male, Country.wu, 4));
        characters.Add(new("huanggai", 11, Sex.male, Country.wu, 4));
        characters.Add(new("zhouyu", 12, Sex.male, Country.wu, 3));
        characters.Add(new("daqiao", 13, Sex.female, Country.wu, 3));
        characters.Add(new("luxun", 14, Sex.male, Country.wu, 3));
        characters.Add(new("caocao", 15, Sex.male, Country.wei, 4, jianxiong));
        characters.Add(new("simayi", 16, Sex.male, Country.wei, 3));
        characters.Add(new("xiahoudun", 17, Sex.male, Country.wei, 4));
        characters.Add(new("zhangliao", 18, Sex.male, Country.wei, 4));
        characters.Add(new("xuchu", 19, Sex.male, Country.wei, 4));
        characters.Add(new("guojia", 20, Sex.male, Country.wei, 3));
        characters.Add(new("zhenji", 21, Sex.female, Country.wei, 3));
        characters.Add(new("huatuo", 22, Sex.male, Country.qun, 3));
        characters.Add(new("lvbu", 23, Sex.male, Country.qun, 4));
        characters.Add(new("diaochan", 24, Sex.female, Country.qun, 3));
    }
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