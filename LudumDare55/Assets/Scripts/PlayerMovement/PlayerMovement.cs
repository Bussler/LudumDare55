using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player movement script.
/// Gets input from MainControls system and relays it to BasicMovement in FixedUpdate.
/// </summary>
public class PlayerMovement : BasicMovement
{
    private Vector3 movementVector = Vector3.zero; // Movement input
    private MainControls input = null; // Input system

    private Dash dashScript = null;

    private void Awake()
    {
        input = new MainControls();
    }

    protected override void Start()
    {
        base.Start();
        dashScript = GetComponent<Dash>();
        if (dashScript != null)
        {
            dashScript.OnDashingChanged += setCanMove;
            dashScript.OnDashingChanged += setUseForceToApply;
        }
    }

    // Subscribe to events of input system
    private void OnEnable()
    {
        input.Enable();
        input.movement.dash.performed += OnBasicActionPerformed;
        input.movement.movement.performed += OnMovementPerformed;
        input.movement.movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.movement.dash.performed -= OnBasicActionPerformed;
        input.movement.movement.performed -= OnMovementPerformed;
        input.movement.movement.canceled -= OnMovementCancelled;
    }

    private void FixedUpdate()
    {
        Move(movementVector);
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        Vector3 inputMovementVector = value.ReadValue<Vector2>();
        movementVector = new Vector3(inputMovementVector.y, 0, -inputMovementVector.x);
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        movementVector = Vector3.zero;
    }

    private void OnBasicActionPerformed(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (dashScript != null)
            {
                StartCoroutine(dashScript.DoDash(movementVector));
            }
        }
    }
}
