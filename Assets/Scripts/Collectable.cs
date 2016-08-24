using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

    public bool isCollectable;
    public enum itemType { Poison, Key};
    public itemType type;

    Inventory inv;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
