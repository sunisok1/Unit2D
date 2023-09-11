
using System;
using System.Collections.Generic;

public static class Lib
{
    public static Dictionary<string, string> Translate = new();

    internal static void Init()
    {
        Translate["sha"] = "и╠";
        Translate["shan"] = "иа";
    }
}