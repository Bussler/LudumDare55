using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class RitualCircle : MonoBehaviour
{
    
    [SerializeField] private SplineIntersectionSensor splineIntersectionSensor;
    [SerializeField] private SplineTrailGenerator splineTrailGenerator;

    [SerializeField] private GameObject unitSphere;

    [SerializeField] private LayerMask ItemMask;

    [SerializeField] private ShootingComponent shootingComponent;

    IObservable<Tuple<Vector3, float>> RitualCircleCompleted => ritualCircleCompleted;

    private IObservable<Tuple<Vector3, float>> ritualCircleCompleted;

    public void InitMeFirstPlease() {
        ritualCircleCompleted = splineIntersectionSensor.SplineIntersection
            .Select(intersection => {
                Spline spline = splineTrailGenerator.Spline.Spline;
                var circleKnots = spline.Knots.TakeLast(spline.Count - intersection).Select(x => x.Position).ToArray();
                var center = CalculateCenter(circleKnots);
                Vector3 v = (spline.Knots.First().Position - center);
                return new Tuple<Vector3, float>(center, v.magnitude);
            });
        
        ritualCircleCompleted.Subscribe(OnRitualCircleCompleted).AddTo(this);
    }

    void Start() {
        InitMeFirstPlease();
    }

    private void OnRitualCircleCompleted(Tuple<Vector3, float> circle) {
        unitSphere.transform.position = circle.Item1;

        RaycastHit[] itemHit = Physics.SphereCastAll(circle.Item1, circle.Item2 / 2, Vector3.up, ItemMask);

        List<Gun.GunEffect> effects = new List<Gun.GunEffect>();

        foreach (RaycastHit item in itemHit)
        {
            if (item.transform.gameObject.GetComponent<RitualItem>() != null)
            {
                effects.Add(item.transform.gameObject.GetComponent<RitualItem>().GunEffect);
                Destroy(item.transform.gameObject);
            }
        }
      
        shootingComponent.EquipGun(effects);


        splineTrailGenerator.ResetSpline();
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
