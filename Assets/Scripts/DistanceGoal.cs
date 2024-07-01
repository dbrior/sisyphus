using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Distance Goal", menuName = "Distance Goal")]
public class DistanceGoal : ScriptableObject {
    public enum Type {EndGame, BossBattle}

    public string name;
    public Type type;
    public Sprite icon;
    public float distance;
    public float startDistance;
}