using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelBlocksData {
    public string Name = "";
    public GameObject Prefab;

    public LevelBlocksData(GameObject prefab) {
        Name = prefab.name;
        Prefab = prefab;
    }
}