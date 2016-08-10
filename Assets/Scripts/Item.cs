using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Item : MonoBehaviour {

    struct Items
    {
        public int ID;
        public GameObject Model;
        public string Name;
        public bool Lethal;
        public bool Collectable;
    }

	// Use this for initialization
	void Start () {
        List<Items> items = new List<Items>();
        Items Poison = new Items { ID = 1, Model = null, Name = "Poison", Lethal = true, Collectable = true };
        Items Coffee = new Items { ID = 2, Model = null, Name = "Coffee", Lethal = false, Collectable = false };

        items.Add(Poison);
        items.Add(Coffee);
        
        }
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
