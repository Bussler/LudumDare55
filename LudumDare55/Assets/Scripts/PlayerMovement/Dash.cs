using UnityEngine;

/// <summary>
/// Script that enables dashing with an IEnumerator that sets a rigidbody velocity for dashingTime.
/// </summary>
public class Dash : MonoBehaviour
{
    public bool canDash = true;
    private Rigidbody rb = null;

    // TODO: search for statmanager here
    [SerializeField]
    private int dashingPower = 15; // Speed at which the player dashes

    [SerializeField]
    private float dashingTime = 0.3f; // How long the player dashes

    [SerializeField]
    private float dashingCooldown = 1.0f; // Cooldown between dashes

    private bool _isDashing;
    public delegate void IsDashingChangedHandler(bool _isDashing);
    public event IsDashingChangedHandler OnDashingChanged;
    public bool isDashing
    {
        get { return _isDashing; }
        set
        {
            if (_isDashing != value)
            {
                _isDashing = value;
                OnDashingChanged?.Invoke(_isDashing);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.Log("No Rigidbody component found on " + this.gameObject.name);
        }
    }

    /// <summary>
    /// Set the player to untargetable while dashing.
    /// </summary>
    private void setUntargetable()
    {
        if (this.gameObject.tag == "Player") // TODO: change this to a better solution; should enemies also be untargetable?
        {
            // TODO set untatgetable, e.g. in player stat manager
        }
    }

    /// <summary>
    /// Perform a dash in the given direction.
    /// </summary>
    /// <param name="direction">Direction where to dash to.</param>
    /// <returns></returns>
    public IEnumerator DoDash(Vector3 direction)
    {
        if (!canDash || rb == null)
        {
            yield break;
        }

        canDash = false;
        isDashing = true;
        setUntargetable();

        rb.velocity = direction.normalized * dashingPower;
        yield return new WaitForSeconds(dashingTime);
        setUntargetable();
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
