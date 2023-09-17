using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeField : MonoBehaviour
{
    [SerializeField] AnimationCurve distanceToCoefficientCurve;

    List<ITimeAdjustable> timeAdjustables;
    float scaleValue;

    private void Awake()
    {
        scaleValue = transform.lossyScale.x;
        timeAdjustables = new();
    }
    private void FixedUpdate()
    {
        foreach (var a in timeAdjustables)
        {
            var distance = Vector3.Distance(a.Position, transform.position);
            var radius = scaleValue / 2;
            a.TimeAdjustCoefficient = distanceToCoefficientCurve.Evaluate(distance / radius);
        }
    }

    public void SetScaleValue(float value)
    {
        scaleValue = value;
        transform.localScale = Vector3.one * scaleValue;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ITimeAdjustable>(out var a))
        {
            timeAdjustables.Add(a);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ITimeAdjustable>(out var a) && timeAdjustables.Contains(a))
        {
            timeAdjustables.Remove(a);
            a.TimeAdjustCoefficient = 1;
        }
    }
}
