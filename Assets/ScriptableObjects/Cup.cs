using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cup", menuName = "Collectables/Cup")]
public class Cup : ScriptableObject
{
    public string cupName;

    public Material mat;

    public int price;
}
