using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SFXScript : MonoBehaviour
{
    [SerializeField] private AudioSource sfxAudio;
    [SerializeField] private List<AudioClip> menuForward;
    [SerializeField] private List<AudioClip> menuBackward;
    [SerializeField] private List<AudioClip> correct;
    [SerializeField] private List<AudioClip> incorrect;
    [SerializeField] private List<AudioClip> shortIncorrect;
    [SerializeField] private List<AudioClip> ruReady;

    public void MenuForward()
    {
        playFromList(menuForward);
    }

    public void MenuBackwards()
    {
        playFromList(menuBackward);
    }
    public void Correct()
    {
        playFromList(correct);
    }
    public void Incorrect()
    {
        playFromList(incorrect);
    }
    public void ShortIncorrect()
    {
        playFromList(shortIncorrect);
    }
    public void RuReady()
    {
        playFromList(ruReady);
    }

    private void playFromList (List<AudioClip> list)
    {
        if (list.Count > 0)
        {
            sfxAudio.clip = list[Random.Range(0, list.Count)];
            sfxAudio.Play();
        }
    }
}
