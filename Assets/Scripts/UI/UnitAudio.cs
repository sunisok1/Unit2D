using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Unit))]
    [RequireComponent(typeof(AudioSource))]
    public class UnitAudio : MonoBehaviour
    {
        Unit unit;
        AudioSource audioSource;

        readonly Dictionary<string, AudioClip> CardAudioClipDic_male = new();
        readonly Dictionary<string, AudioClip> CardAudioClipDic_famale = new();
        readonly Dictionary<string, AudioClip> EffectAudioClipDic = new();

        private void Start()
        {
            unit = GetComponent<Unit>();
            audioSource = GetComponent<AudioSource>();
            unit.OnUseOrRespondCard += Unit_OnUseOrRespondCard;
            unit.OnBeDamaged += Unit_OnBeDamaged;
        }

        private void Unit_OnBeDamaged(int amount)
        {
            AudioClip clip = null;
            switch (amount)
            {
                case 1:
                    if (!EffectAudioClipDic.TryGetValue("damage", out clip))
                    {
                        clip = EffectAudioClipDic["damage"] = Resources.Load<AudioClip>($"Audio/effect/damage");
                    }
                    break;
                case > 1:
                    if (!EffectAudioClipDic.TryGetValue("damage2", out clip))
                    {
                        clip = EffectAudioClipDic["damage2"] = Resources.Load<AudioClip>($"Audio/effect/damage2");
                    }
                    break;
                default:
                    break;
            }
            audioSource.PlayOneShot(clip);
        }

        private void Unit_OnUseOrRespondCard(object unit, Card card)
        {
            Dictionary<string, AudioClip> dic = (unit as Unit).sex switch
            {
                Sex.male => CardAudioClipDic_male,
                Sex.female => CardAudioClipDic_famale,
                _ => throw new System.NotImplementedException()
            };
            if (!dic.TryGetValue(card.Name, out AudioClip clip))
            {
                clip = Resources.Load<AudioClip>($"Audio/card/{(unit as Unit).sex}/" + card.Name);
            }
            audioSource.PlayOneShot(clip);
        }
    }
}