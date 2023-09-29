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
        Skill rende1 = new TriggerSkill()
        {
            name = nameof(rende1),
            trigger = (Triggerer.player, Timing.phaseUseBegin),
            silent = true,
            direct = true,
            content = (e) =>
            {
                Debug.Log("重置仁德数量");
                e.player.storage["rende"] = 0;
            }
        };
        Skill rende = new ActiveSkill()
        {
            name = nameof(rende),
            enable = Timing.phaseUse,
            filterCard = (card) => true,
            selectCard = (1, int.MaxValue),
            discard = false,
            filterTarget = (card, player, target) =>
            {
                return player != target;
            },
            check = (card) =>
            {
                throw new System.NotImplementedException();
            },
            content = (e) =>
            {
                e.player.Give(e.cards, e.target);
                int pre_rende = (int)e.player.storage["rende"];
                int rende = pre_rende + e.cards.Count;
                e.player.storage["rende"] = rende;
                if (pre_rende < 2 && rende >= 2)
                {
                    Debug.Log($"{e.player}发动仁德回血效果");
                }
            },
            CompanionSkills = new Skill[] { rende1 },
        };
        Skill jijiang = new ActiveSkill()
        {
            name = nameof(jijiang),
            type = SkillType.zhu,
            enable = Timing.phaseUse,
        };
        Skill wusheng = new ViewAsSkill()
        {
            name = nameof(wusheng),
        };

        characters.Add(new("liubei", 1, Sex.male, Country.shu, 4, rende, jijiang));
        characters.Add(new("guanyu", 2, Sex.male, Country.shu, 4, wusheng));
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
        characters.Add(new("caocao", 15, Sex.male, Country.wei, 4));
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