using System.Collections.Generic;
using Unity.VisualScripting;

public class Character
{
    public string name;
    public Sex sex;
    public Country country;
    public int maxHp;
    public List<Skill> skills = new();

    public Character(string name, Sex sex, Country country, int maxHp, params Skill[] skills)
    {
        this.name = name;
        this.sex = sex;
        this.country = country;
        this.maxHp = maxHp;
        this.skills.AddRange(skills);
    }
}
