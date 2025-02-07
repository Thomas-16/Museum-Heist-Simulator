using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {  get; private set; }

    // Events
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;
    public event Action OnEmotePressed;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if(Input.GetKeyDown("Jump")) {
            OnJumpPressed?.Invoke();
        }
        if(Input.GetButtonUp("Jump")) {
            OnJumpReleased?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.T)) {
            OnEmotePressed?.Invoke();
        }
    }
    public bool IsControllingPlayer() {
        return IsCrouchPressed() || GetMouseInputVector().sqrMagnitude > 0.02f || GetMovementInputVector().sqrMagnitude > 0.02f || IsJumpPressed();
    }

    public bool IsCrouchPressed() {
        return Input.GetKey(KeyCode.LeftShift);
    }
    public Vector2 GetMouseInputVector() {
        return new Vector2 {
            x = Input.GetAxisRaw("Mouse X"),
            y = Input.GetAxisRaw("Mouse Y")
        };
    }
    public Vector2 GetMovementInputVector() {
        return new Vector2() {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
    }
    public bool IsJumpPressed() {
        return Input.GetKey("Jump");
    }
}
