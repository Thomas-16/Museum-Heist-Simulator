using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimationsController : MonoBehaviour
{
    private const string SPEED = "Speed";
    private const string DIRECTION = "Direction";
    private const string RUN = "Run";

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
        animator.SetFloat("AbsSpeed", absSpeed);
    }
}
