using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(AudioSource))]
public class UnitAudio : MonoBehaviour
{
    Unit unit;
    AudioSource audioSource;
    private const string AudioClipPath = "Audio/card/male/";
    private void Start()
    {
        unit = GetComponent<Unit>();
        audioSource = GetComponent<AudioSource>();
        unit.OnUseOrRespondCard += Unit_OnUseOrRespondCard;
    }

    private void Unit_OnUseOrRespondCard(Card card)
    {
        AudioClip clip = Resources.Load<AudioClip>(AudioClipPath + card.Name);
        audioSource.PlayOneShot(clip);
    }
}