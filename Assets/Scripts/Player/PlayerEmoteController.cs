using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: SOME EMOTES SHOULD PLAY MUSIC TOO, HANDLE THAT HERE
public class PlayerEmoteController : MonoBehaviour
{

    [SerializeField] HumanAnimationsController playerAnimationController;

    private float timeLastEmoted;

    private void Start() {
        InputManager.Instance.OnEmotePressed += OnEmotePressedHandler;
    }

    private void OnEmotePressedHandler() {
        playerAnimationController.TriggerEmote(UnityEngine.Random.Range(1, 7));
        timeLastEmoted = Time.time;
    }

    public void StopEmoting() {
        playerAnimationController.StopEmoteAnimationInstantly();
    }
    public float GetTimeSinceEmoteStarted() {
        if (!IsEmoting()) return 0;
        return Time.time - timeLastEmoted;
    }
    public bool IsEmoting() {
        return playerAnimationController.GetAnimator().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Emote 1") ||
            playerAnimationController.GetAnimator().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Emote 2") ||
            playerAnimationController.GetAnimator().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Emote 3") ||
            playerAnimationController.GetAnimator().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Emote 4") ||
            playerAnimationController.GetAnimator().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Emote 5") ||
            playerAnimationController.GetAnimator().GetCurrentAnimatorStateInfo(0).shortNameHash == Animator.StringToHash("Emote 6");
    }
}
