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
            unit.OnDamage += Unit_OnBeDamaged;
            unit.OnRecover += Unit_OnRecover;
            unit.OnDead += Unit_OnDead;
        }
        private void PlayEffectAudioOneShot(string key)
        {
            if (!EffectAudioClipDic.TryGetValue(key, out AudioClip clip))
            {
                clip = EffectAudioClipDic[key] = Resources.Load<AudioClip>($"Audio/effect/{key}");
            }
            audioSource.PlayOneShot(clip);
        }

        private void Unit_OnRecover()
        {
            PlayEffectAudioOneShot("recover");
        }

        private void Unit_OnDead(object sender, System.EventArgs e)
        {
            Unit unit = sender as Unit;
            AudioClip clip = Resources.Load<AudioClip>($"Audio/die/{unit.name}");
            audioSource.PlayOneShot(clip);
        }

        private void Unit_OnBeDamaged(int amount)
        {
            switch (amount)
            {
                case 1:
                    PlayEffectAudioOneShot("damage");
                    break;
                case > 1:
                    PlayEffectAudioOneShot("damage2");
                    break;
                default:
                    break;
            }
        }

        private void Unit_OnUseOrRespondCard(object unit, Card card)
        {
            Dictionary<string, AudioClip> dic = (unit as Unit).sex switch
            {
                Sex.male => CardAudioClipDic_male,
                Sex.female => CardAudioClipDic_famale,
                _ => throw new System.NotImplementedException()
            };
            if (!dic.TryGetValue(card.name, out AudioClip clip))
            {
                clip = Resources.Load<AudioClip>($"Audio/card/{(unit as Unit).sex}/" + card.name);
            }
            audioSource.PlayOneShot(clip);
        }
    }
}