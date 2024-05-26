using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "TeamSpinnySword/Level", order = 0)]
public class Level : ScriptableObject
{
    public bool levelComplete;
    public  List<Steps> steps;
}
