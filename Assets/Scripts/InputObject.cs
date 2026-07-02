using UnityEngine;

public class InputObject : MonoBehaviour
{
    public LevelManager.inputNames inputType;
    [SerializeField] private Animator circleAnim;
    [SerializeField] private AudioSource audioSource;

    public void Animate()
    {
        if (circleAnim != null) circleAnim.SetTrigger("BeatPulse");
    }

    public void PlaySound()
    {
        if (audioSource != null) audioSource.Play();
    }
}
