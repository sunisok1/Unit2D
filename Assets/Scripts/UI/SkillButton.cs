using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;
    public void SetSkill(Skill skill, Unit player)
    {
        nameText.text = Lib.Translate(skill.name);
        button.onClick.AddListener(() => player.UseSkill(skill));
    }
}
