using System.Security.Cryptography.X509Certificates;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using UnityEngine.Windows;

public class SplineTrailGenerator : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private Transform origin;
    [SerializeField] private Vector3 originOffset = Vector3.zero;
    [SerializeField] private float splineKnotInterval = 1.0f;
    [SerializeField] private int maxKnots = 32;
    [SerializeField] private PlayerStatManager statManager;
    private MainControls input = null;
    private bool isPressing;

    public SplineContainer Spline => spline;

    public void Awake() {
        input = new MainControls();
        statManager = GetComponent<PlayerStatManager>();
        Observable.EveryUpdate()
            .Select(x => origin.transform.position)
            .Pairwise()
            .Select(x => (x.Current - x.Previous).magnitude)
            .Scan((accumulator, item) => accumulator + item)
            .Select(x => (int) x / splineKnotInterval)
            .Distinct()
            .Subscribe(_ => OnNewKnot())
            .AddTo(this);
    }

    public void ResetSpline() {
        spline.Spline.Clear();
    }

    private void OnNewKnot() {
        if (isPressing&&statManager.CurrentBlood>0)
        {
            spline.Spline.Add(new BezierKnot(origin.transform.position + originOffset));
            if (spline.Spline.Count > maxKnots)
            {
                spline.Spline.RemoveAt(0);
            }
            statManager.useBlood(statManager.BloodDrain);
        }
        else
        {
            spline.Spline.Clear();
        }
    }

    private void OnEnable()
    {
        input.Enable();
        input.draw.draw.performed += OnBasicActionPerformed;
        input.draw.draw.canceled += OnBasicActionCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.draw.draw.performed -= OnBasicActionPerformed;
        input.draw.draw.canceled -= OnBasicActionCancelled;
    }

    public void OnBasicActionPerformed(InputAction.CallbackContext value)
    {
        isPressing = true;
    }
    public void OnBasicActionCancelled(InputAction.CallbackContext value)
    {
        isPressing = false;
    }

}
