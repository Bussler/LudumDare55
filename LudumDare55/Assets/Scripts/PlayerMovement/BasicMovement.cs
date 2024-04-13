using UnityEngine;

/// <summary>
/// Class to manage the player's input and movement.
/// </summary>
public class BasicMovement : MonoBehaviour
{
    public Vector3 ForceToApply { get => forceToApply; set => forceToApply = value; }

    protected Rigidbody rb = null; // Rigidbody component through which we apply force
    private bool canMove = true; // Flag to check if the player can move

    // Knockback variables
    private Vector3 forceToApply = Vector3.zero; // used for knockback if a projectile hits the player
    [SerializeField]
    private float forceDamping = 1.2f; // damping factor for knockback
    private bool useForceToApply = true; // flag to check if we should get knocked back

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.Log("No Rigidbody component found on " + this.gameObject.name);
        }
    }

    /// <summary>
    /// Method to move the player. Is called in FixedUpdate by PlayerMovement.
    /// </summary>
    /// <param name="movementVector"></param>
    protected void Move(Vector3 movementVector)
    {
        if (!canMove || rb == null || PlayerStatManager.Instance == null)
        {
            return;
        }

        Vector3 moveForce = movementVector.normalized * PlayerStatManager.Instance.MovementSpeed;

        if (useForceToApply)
            moveForce += forceToApply;

        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.z) <= 0.01f)
        {
            forceToApply = Vector3.zero;
        }

        rb.velocity = moveForce;
    }

    public void setCanMove(bool value)
    {
        this.canMove = !value;
    }

    public void setUseForceToApply(bool value)
    {
        this.useForceToApply = !value;
    }
}
