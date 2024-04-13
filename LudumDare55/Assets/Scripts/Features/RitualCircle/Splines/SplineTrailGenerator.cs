using System.Security.Cryptography.X509Certificates;
using UniRx;
using UnityEngine;
using UnityEngine.Splines;

public class SplineTrailGenerator : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private Transform origin;
    [SerializeField] private Vector3 originOffset = Vector3.zero;
    [SerializeField] private float splineKnotInterval = 1.0f;

    public SplineContainer Spline => spline;

    public void Awake() {
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
        spline.Spline.Add(new BezierKnot(origin.transform.position + originOffset));
    }
    
}
