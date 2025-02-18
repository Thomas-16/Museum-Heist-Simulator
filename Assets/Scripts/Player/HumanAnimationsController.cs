using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimationsController : MonoBehaviour
{
    private const string SPEED = "Speed";
    private const string DIRECTION = "Direction";
    private const string RUN = "Run";
    private const string ABS_SPEED = "AbsSpeed";
    private const string EMOTE = "Emote";
    private const string EMOTE_USED = "EmoteUsed";

    private Animator animator;
    
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void SetIsRunning(bool isRunning) {
        animator.SetBool(RUN, isRunning);
    }
    public void SetDirection(float direction) {
        animator.SetFloat(DIRECTION, direction);
    }
    public void SetSpeed(float speed) {
        animator.SetFloat(SPEED, speed);
    }
    public void SetAbsSpeed(float absSpeed) {
        animator.SetFloat(ABS_SPEED, absSpeed);
    }
    public void TriggerEmote(int emote) {
        animator.SetInteger(EMOTE_USED, emote);
        animator.SetTrigger(EMOTE);
    }
    public void StopEmoteAnimationInstantly() {
        animator.Play("idle", 0, 0f);
    }
    public Animator GetAnimator() { return animator; }
}
