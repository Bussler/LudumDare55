using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Splines;

public class RitualCircle : MonoBehaviour
{
    
    [SerializeField] private SplineIntersectionSensor splineIntersectionSensor;
    [SerializeField] private SplineTrailGenerator splineTrailGenerator;

    [SerializeField] private GameObject unitSphere;
    [SerializeField] private float minCircleSurface = 30.0f;

    IObservable<Tuple<Vector3, float>> RitualCircleCompleted => ritualCircleCompleted;

    private IObservable<Tuple<Vector3, float>> ritualCircleCompleted;

    [SerializeField] private LayerMask mask;
    [SerializeField] private ShootingComponent shootingComponent;

    [SerializeField] private GameObject ritualCirclePrefab;
   

    public void InitMeFirstPlease() {
        ritualCircleCompleted = splineIntersectionSensor.SplineIntersection
            .Select(intersection => {
                Spline spline = splineTrailGenerator.Spline.Spline;
                var circleKnots = spline.Knots.TakeLast(spline.Count - intersection).Select(x => x.Position).ToArray();
                var center = CalculateCenter(circleKnots);
                float r = 0.0f;
                foreach (var knot in circleKnots) {
                    Vector3 v = knot - center;
                    r += v.magnitude;
                }
                r /= circleKnots.Length;
                return new Tuple<float3[], Vector3, float>(circleKnots, center, r);
            })
            .Where(x => ValidateCircle(x.Item1, x.Item2))
            .Select(x => new Tuple<Vector3, float>(x.Item2, x.Item3));
        
        ritualCircleCompleted.Subscribe(OnRitualCircleCompleted).AddTo(this);
    }

    void Start() {
        InitMeFirstPlease();
    }

    private void OnRitualCircleCompleted(Tuple<Vector3, float> circle) {
        // unitSphere.transform.position = circle.Item1;
         //unitSphere.transform.position = circle.Item1;

        RaycastHit[] itemHit = Physics.SphereCastAll(circle.Item1, circle.Item2 / 2, Vector3.up, mask);

        List<ObjectEffect> effects = new List<ObjectEffect>();
      
        foreach (RaycastHit item in itemHit)
        {
            if (item.transform.gameObject.GetComponent<RitualComponent>() != null)
            {
                effects.Add(item.transform.gameObject.GetComponent<RitualComponent>().Effect);

                if (item.collider.gameObject.GetComponent<EnemyController>() != null)
                {
                    EnemySpawner.Instance.OnEnemyDied();
                }

                ObjectPoolManager.Instance.DespawnObject(item.transform.gameObject);
                PlayerStatManager.Instance.commitUsedBlood();
            }
        }
      
        if (effects.Count > 0)
        {
            shootingComponent.EquipGun(effects);
           
        }

        splineTrailGenerator.ResetSpline();

        var circleEffect = Instantiate(ritualCirclePrefab);
        circleEffect.transform.position = circle.Item1 + new Vector3(0, gameObject.transform.position.y + 0.5f, 0);
        circleEffect.transform.localScale = 0.25f * circle.Item2 * new Vector3(1, 1, 1);
    }


    private bool ValidateCircle(float3[] points, Vector3 m) {
        // Ensure at least three points are provided to form a polygon
        if (points.Length < 3)
        {
            throw new Exception("Should not happen!");
        }

        // Calculate the surface area using the Shoelace formula
        float area = 0f;
        int numPoints = points.Length;
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 currentPoint = points[i];
            Vector3 nextPoint = points[(i + 1) % numPoints];
            area += currentPoint.x * nextPoint.z - nextPoint.x * currentPoint.z;
        }
        Debug.Log("TEST" + Mathf.Abs(area / 2f));
        // The absolute value of the sum divided by 2 is the surface area
        return Mathf.Abs(area / 2f) > minCircleSurface;
    }

    private static float3 CalculateCenter(float3[] points) {
        float totalX = 0, totalY = 0, totalZ = 0;
        int count = points.Length;
        
        foreach (float3 point in points) {
            totalX += point.x;
            totalY += point.y;
            totalZ += point.z;
        }
        
        float centerX = totalX / count;
        float centerY = totalY / count;
        float centerZ = totalZ / count;
        
        return new float3(centerX, centerY, centerZ);
    }

}
