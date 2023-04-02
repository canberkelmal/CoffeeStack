using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Obstacle", menuName = "Obstacles/Obstacle")]
public class Obstacle : ScriptableObject
{
    public string obstacleName;

    public Material mat;

    public bool hitBack;
}
