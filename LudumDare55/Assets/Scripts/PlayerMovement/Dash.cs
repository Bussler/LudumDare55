using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Script that enables dashing with an IEnumerator that sets a rigidbody velocity for dashingTime.
/// </summary>
public class Dash : MonoBehaviour
{
    public bool canDash = true;
    private Rigidbody rb = null;

    private PlayerStatManager statManager = null;

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
        statManager = PlayerStatManager.instance;
    }

    /// <summary>
    /// Set the player to untargetable while dashing.
    /// </summary>
    private void setUntargetable()
    {
        if (this.gameObject.tag == "Player")
        {
            statManager.IsTargetable = !statManager.IsTargetable;
        }
    }

    /// <summary>
    /// Perform a dash in the given direction.
    /// </summary>
    /// <param name="direction">Direction where to dash to.</param>
    /// <returns></returns>
    public IEnumerator DoDash(Vector3 direction)
    {
        if (!canDash || rb == null || statManager == null)
        {
            yield break;
        }

        canDash = false;
        isDashing = true;
        setUntargetable();

        rb.velocity = direction.normalized * statManager.DashingPower;
        yield return new WaitForSeconds(statManager.DashingTime);
        setUntargetable();
        isDashing = false;
        yield return new WaitForSeconds(statManager.DashingCooldown);
        canDash = true;
    }
}
