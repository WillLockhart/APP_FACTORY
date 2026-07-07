using UnityEngine;

public class InputObject : MonoBehaviour
{
    public LevelManager.inputNames inputType;
    [SerializeField] private Animator animate;
    [SerializeField] private AudioSource audioSource;
    //[SerializeField] private AudioClip[] clip;

    public void Animate()
    {
        if (animate != null) animate.SetTrigger("Animate");
    }

    public void PlaySound()
    {
        if (audioSource != null) audioSource.Play();
    }
}
