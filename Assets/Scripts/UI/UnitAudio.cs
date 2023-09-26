using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(AudioSource))]
public class UnitAudio : MonoBehaviour
{
    Unit unit;
    AudioSource audioSource;
    private const string AudioClipPath = "Audio/card/male/";

    Dictionary<string, AudioClip> EffectAudioClipDic = new();

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

    private void Unit_OnUseOrRespondCard(Card card)
    {
        AudioClip clip = Resources.Load<AudioClip>(AudioClipPath + card.Name);
        audioSource.PlayOneShot(clip);
    }
}