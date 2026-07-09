using UnityEngine;

public class InputObject : MonoBehaviour
{
    public LevelManager.inputNames inputType;
    [SerializeField] private Animator animate;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool usesTrigger = true;
    //[SerializeField] private AudioClip[] clip;

    private void Awake()
    {
        if (usesTrigger)
        {
            return;
        }
        animate.enabled = false;
    }
    public void Animate()
    {
        if (animate != null)
        {
            if (usesTrigger)
            {
                animate.SetTrigger("Animate");
            }
            else
            {
                animate.enabled = true;
            }
        }
    }
    public void DisableAnimator()
    {
        animate.enabled = false;
    }

    public void PlaySound()
    {
        if (audioSource != null) audioSource.Play();
    }
}
