using System;
using System.Linq;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineIntersectionSensor : MonoBehaviour
{
    public IObservable<int> SplineIn<tersection => splineIntersection;
    public IObservable<Unit> SensorInterval { set; get; }

    private IObservable<int> splineIntersection;

    // The spline
    [SerializeField] private SplineContainer spline;

    public SplineContainer Spline => spline;

    public void Awake() {
        SensorInterval = Observable.EveryUpdate().Select(_ => Unit.Default);
        splineIntersection = SensorInterval
            .Select(_ => RunIntersectionTest())
            .DistinctUntilChanged()
            .Where(x => !(x < 0));

        // SplineIntersection.Subscribe(x => Debug.Log("Found spline intersection: " + x)).AddTo(this);
    }
    
    private int RunIntersectionTest() {

        if (spline.Spline.Count < 3) {
            return -1;
        }

        var knots = spline.Spline.Knots.ToArray();
        var knotsN = knots.Length;

        // We only check for the last segment because whenever a circle is completed, a new spline begins.
        var p1 = knots[knotsN - 1];
        var p2 = knots[knotsN - 2];

        for (int i = 1; i < knotsN - 2; i++) {
            var p3 = knots[i];
            var p4 = knots[i - 1];
            
            if (RunIntersectionTest2(new float2(p1.Position.x, p1.Position.z),
                new float2(p2.Position.x, p2.Position.z),
                new float2(p3.Position.x, p3.Position.z),
                new float2(p4.Position.x, p4.Position.z))) {
                    return i;
                }
        }

        return -1;
    }

    private bool RunIntersectionTest2(float2 p1, float2 p2, float2 p3, float2 p4) {
        // Calculate slopes
        double slope1 = (p2.y - p1.y) / (p2.x - p1.x);
        double slope2 = (p4.y - p3.y) / (p4.x - p3.x);

        // Check if lines are parallel
        if (Math.Abs(slope1 - slope2) < 0.0001) // using tolerance for floating point comparisons
            return false;

        // Calculate y-intercepts
        double intercept1 = p1.y - slope1 * p1.x;
        double intercept2 = p3.y - slope2 * p3.x;

        // Calculate intersection point
        double x_intersect, y_intersect;
        if (double.IsInfinity(slope1)) // Line 1 is vertical
        {
            x_intersect = p1.x;
            y_intersect = slope2 * x_intersect + intercept2;
        }
        else if (double.IsInfinity(slope2)) // Line 2 is vertical
        {
            x_intersect = p3.x;
            y_intersect = slope1 * x_intersect + intercept1;
        }
        else
        {
            x_intersect = (intercept2 - intercept1) / (slope1 - slope2);
            y_intersect = slope1 * x_intersect + intercept1;
        }

        // Check if intersection point lies within line segments
        return Math.Min(p1.x, p2.x) <= x_intersect && x_intersect <= Math.Max(p1.x, p2.x) &&
                Math.Min(p1.y, p2.y) <= y_intersect && y_intersect <= Math.Max(p1.y, p2.y) &&
                Math.Min(p3.x, p4.x) <= x_intersect && x_intersect <= Math.Max(p3.x, p4.x) &&
                Math.Min(p3.y, p4.y) <= y_intersect && y_intersect <= Math.Max(p3.y, p4.y);
    }

}
