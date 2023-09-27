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
