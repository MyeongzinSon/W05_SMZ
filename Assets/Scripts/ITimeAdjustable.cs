using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ITimeAdjustable
{
    public bool InTimeField { get; set; }
    public float TimeAdjustCoefficient { get; set; }
    public Vector3 Position { get; }
    List<ITimeAdjustable> targetList { get; set; }

    public void AddThisToList(List<ITimeAdjustable> list)
    {
        targetList = list;
        targetList.Add(this);
    }
    public void OnDestroy()
    {
        targetList.Remove(this);
    }
}
