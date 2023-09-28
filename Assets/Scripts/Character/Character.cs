using System.Collections.Generic;
using Unity.VisualScripting;

public class Character
{
    public string name;
    public readonly int id;
    public Sex sex;
    public Country country;
    public int maxHp;
    public List<Skill> skills = new();
    /// <summary>
    /// 添加武将
    /// </summary>
    /// <param name="name">姓名</param>
    /// <param name="id">角色id</param>
    /// <param name="sex">性别</param>
    /// <param name="country">势力</param>
    /// <param name="maxHp">血上限</param>
    /// <param name="skills">技能</param>
    public Character(string name, int id, Sex sex, Country country, int maxHp, params Skill[] skills)
    {
        this.name = name;
        this.id = id;
        this.sex = sex;
        this.country = country;
        this.maxHp = maxHp;
        this.skills.AddRange(skills);
    }
}
