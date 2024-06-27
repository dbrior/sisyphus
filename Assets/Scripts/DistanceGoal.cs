using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Distance Goal", menuName = "Distance Goal")]
public class DistanceGoal : ScriptableObject {
    public string name;
    public Sprite icon;
    public float distance;
}