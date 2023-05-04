using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "rate", menuName = "Upgrade", order = 1)]
public class UpgradeRate : ScriptableObject
{
    public List<float> rate;
    public float rollerRate = 1;
}
