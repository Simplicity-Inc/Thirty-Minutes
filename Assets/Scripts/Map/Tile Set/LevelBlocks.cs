using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class LevelBlocksData {
    public string Name;
    public GameObject Prefab;
}

[CreateAssetMenu]
public class LevelBlocks : ScriptableObject {
    public List<LevelBlocksData> Blocks = new List<LevelBlocksData>();
}
