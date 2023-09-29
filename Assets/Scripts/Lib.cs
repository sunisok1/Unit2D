using System;
using System.Collections.Generic;

public static class Lib
{
    private static readonly Dictionary<string, string> dict = new();
    static Lib()
    {
        LoadCard();
        LoadCharactor();
        LaodSkill();
    }

    private static void LaodSkill()
    {
        dict["rende"] = "仁德";
        dict["jijiang"] = "激将";
        dict["wusheng"] = "武圣";
    }

    private static void LoadCharactor()
    {
        dict["caocao"] = "曹操";
        dict["simayi"] = "司马懿";
        dict["xiahoudun"] = "夏侯惇";
        dict["zhangliao"] = "张辽";
        dict["xuzhu"] = "许褚";
        dict["guojia"] = "郭嘉";
        dict["zhenji"] = "甄宓";
        dict["liubei"] = "刘备";
        dict["guanyu"] = "关羽";
        dict["zhangfei"] = "张飞";
        dict["zhugeliang"] = "诸葛亮";
        dict["zhaoyun"] = "赵云";
        dict["machao"] = "马超";
        dict["huangyueying"] = "黄月英";
        dict["sunquan"] = "孙权";
        dict["ganning"] = "甘宁";
        dict["lvmeng"] = "吕蒙";
        dict["huanggai"] = "黄盖";
        dict["zhouyu"] = "周瑜";
        dict["daqiao"] = "大乔";
        dict["luxun"] = "陆逊";
        dict["sunshangxiang"] = "孙尚香";
        dict["huatuo"] = "华佗";
        dict["lvbu"] = "吕布";
        dict["diaochan"] = "貂蝉";
        dict["huaxiong"] = "华雄";
        dict["re_yuanshu"] = "袁术";
        dict["caozhang"] = "曹彰";

    }

    private static void LoadCard()
    {
        dict["sha"] = "杀";
        dict["huosha"] = "火杀";
        dict["leisha"] = "雷杀";
        dict["icesha"] = "冰杀";
        dict["kamisha"] = "神杀";
        dict["cisha"] = "刺杀";
        dict["shan"] = "闪";
        dict["tao"] = "桃";
        dict["bagua"] = "八卦阵";
        dict["bagua_bg"] = "卦";
        dict["bagua_skill"] = "八卦阵";
        dict["jueying"] = "绝影";
        dict["dilu"] = "的卢";
        dict["zhuahuang"] = "爪黄飞电";
        dict["jueying_bg"] = "+马";
        dict["dilu_bg"] = "+马";
        dict["zhuahuang_bg"] = "+马";
        dict["chitu"] = "赤兔";
        dict["chitu_bg"] = "-马";
        dict["dawan"] = "大宛";
        dict["dawan_bg"] = "-马";
        dict["zixin"] = "紫骍";
        dict["zixin_bg"] = "-马";
        dict["zhuge"] = "诸葛连弩";
        dict["cixiong"] = "雌雄双股剑";
        dict["zhuge_bg"] = "弩";
        dict["cixiong_bg"] = "双";
        dict["qinggang"] = "青釭剑";
        dict["qinglong"] = "青龙偃月刀";
        dict["zhangba"] = "丈八蛇矛";
        dict["qinglong_bg"] = "偃";
        dict["zhangba_bg"] = "蛇";
        dict["guanshi"] = "贯石斧";
        dict["fangtian"] = "方天画戟";
        dict["qilin"] = "麒麟弓";
        dict["qilin_bg"] = "弓";
        dict["zhuge_skill"] = "诸葛连弩";
        dict["cixiong_skill"] = "雌雄双股剑";
        dict["qinggang_skill"] = "青釭剑";
        dict["qinglong_skill"] = "青龙偃月刀";
        dict["qinglong_guozhan"] = "青龙偃月刀";
        dict["zhangba_skill"] = "丈八蛇矛";
        dict["guanshi_skill"] = "贯石斧";
        dict["fangtian_skill"] = "方天画戟";
        dict["qilin_skill"] = "麒麟弓";
        dict["wugu"] = "五谷丰登";
        dict["taoyuan"] = "桃园结义";
        dict["nanman"] = "南蛮入侵";
        dict["wanjian"] = "万箭齐发";
        dict["wuzhong"] = "无中生有";
        dict["juedou"] = "决斗";
        dict["wugu_bg"] = "谷";
        dict["taoyuan_bg"] = "园";
        dict["nanman_bg"] = "蛮";
        dict["wanjian_bg"] = "箭";
        dict["wuzhong_bg"] = "生";
        dict["juedou_bg"] = "斗";
        dict["shunshou"] = "顺手牵羊";
        dict["guohe"] = "过河拆桥";
        dict["guohe_bg"] = "拆";
        dict["jiedao"] = "借刀杀人";
        dict["wuxie"] = "无懈可击";
        dict["wuxie_bg"] = "懈";
        dict["lebu"] = "乐不思蜀";
        dict["shandian"] = "闪电";
        dict["shandian_bg"] = "电";
        dict["hanbing"] = "寒冰剑";
        dict["renwang"] = "仁王盾";
        dict["hanbing_bg"] = "冰";
        dict["renwang_bg"] = "盾";
        dict["hanbing_skill"] = "寒冰剑";
        dict["renwang_skill"] = "仁王盾";
    }

    public static string Translate(string text)
    {
        if (dict.ContainsKey(text))
        {
            return dict[text];
        }
        return text;
    }
}