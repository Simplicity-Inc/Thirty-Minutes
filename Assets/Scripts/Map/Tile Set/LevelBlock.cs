using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CreateAssetMenu]
public class LevelBlock : ScriptableObject {
    public List<LevelBlocksData> Blocks = new List<LevelBlocksData>();
}
