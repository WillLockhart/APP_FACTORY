using UnityEngine;

public class CirclePulse : MonoBehaviour
{
    [SerializeField] private Animator CircleAnim;

    public void Pulse()
    {
        CircleAnim.SetTrigger("BeatPulse");
    }
}


