using System;

[Flags]
public enum Timing
{
    phaseUseBegin,
    phaseUse,
    damageEnd
}
public enum Triggerer
{
    global, player, source
}

public enum SkillType
{
    common, locking, zhu
}