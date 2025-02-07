using ECM2.Examples.FirstPerson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerInput : MonoBehaviour
{
    [Space(15.0f)]
    public bool invertLook = false;
    [Tooltip("Mouse look sensitivity")]
    public Vector2 mouseSensitivity = new Vector2(1.0f, 1.0f);

    [Space(15.0f)]
    [Tooltip("How far in degrees can you move the camera down.")]
    public float minPitch = -80.0f;
    [Tooltip("How far in degrees can you move the camera up.")]
    public float maxPitch = 80.0f;

    private Player _character;

    private void Awake() {
        _character = GetComponent<Player>();
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        InputManager.Instance.OnJumpPressed += OnJumpPressedHandler;
        InputManager.Instance.OnJumpReleased += OnJumpReleasedHandler;
    }
    
    private void OnJumpPressedHandler() {
        _character.Jump();
    }
    private void OnJumpReleasedHandler() {
        _character.StopJumping();
    }
    private void Update() {
        Vector2 lookInput = InputManager.Instance.GetMouseInputVector();

        lookInput *= mouseSensitivity;

        _character.AddControlYawInput(lookInput.x);
        _character.AddControlPitchInput(invertLook ? lookInput.y : -lookInput.y);


        // Movement input, relative to character's view direction
        Vector2 inputMove = InputManager.Instance.GetMovementInputVector();

        Vector3 movementDirection = Vector3.zero;

        movementDirection += _character.GetRightVector() * inputMove.x;
        movementDirection += _character.GetForwardVector() * inputMove.y;

        _character.SetMovementDirection(movementDirection);

    }

}
