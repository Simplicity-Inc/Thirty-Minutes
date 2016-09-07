using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    public int maxInvSize;
    public struct Items
    {
        public int ID;
        public GameObject Model;
        public string Name;
    }
    //players inventory
    public List<Items> ItemsList = new List<Items>();
    //all the objects that can be picked up
    public List<GameObject> CollectablesList = new List<GameObject>();
    //all objects that can be interacted with but not picked up
    public List<GameObject> InteractableList = new List<GameObject>();

    Items Poison = new Items { ID = 1, Model = null, Name = "Poison" };
    Items Key = new Items { ID = 2, Model = null, Name = "Key" };

    // Use this for initialization
    public void Start()
    {
        //fill out the two lists
        CollectablesList.AddRange(GameObject.FindGameObjectsWithTag("Collectable"));
        InteractableList.AddRange(GameObject.FindGameObjectsWithTag("Interactable"));


    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("key pressed");
            //Pickup will determine if you pick it up or interact w/ it
            PickUpItem();
        }
    }
    void PickUpItem()
    {
        for (int i = 0; i < CollectablesList.Count; i++)
        {
            if (CheckXDis(transform, CollectablesList[i].transform) < 2)
            {
                if (ItemsList.Capacity <= maxInvSize)
                {
                    if (CollectablesList[i].gameObject.name == ("Poison"))
                    {
                        ItemsList.Add(Poison);
                        CollectablesList[i].SetActive(false);
                        break;
                    }
                    if (CollectablesList[i].gameObject.name == ("Key"))
                    {
                        ItemsList.Add(Key);
                        CollectablesList[i].SetActive(false);
                        break;
                    }
                }
                else { Debug.Log("Inventory full!"); }
            }
            //if check fails check if its an interactable item
            else { InteractItem(); }
        }
    }
    void InteractItem()
    {
        for (int i = 0; i < InteractableList.Count; i++)
        {            
            if (CheckXDis(transform, InteractableList[i].transform) < 2)
            {               
                if (InteractableList[i].name == "Coffee")
                {
                    UseItem(Poison);              
                    InteractableList[i].GetComponent<Traps>().lethal = true;
                    InteractableList[i].GetComponent<Renderer>().material.color = Color.red;
                    
                }
                if (InteractableList[i].name == "Door")
                {
                    UseItem(Key);                   
                    InteractableList[i].GetComponent<Door>().locked = false;
                }
            }
        }
    }
    //checks if item is in the inventory and removes it if so
    void UseItem(Items item)
    {
        if(ItemsList.Contains(item))
        {
            ItemsList.Remove(item);
        }
    }
    float CheckXDis(Transform a, Transform b)
    {
        return Vector3.Distance(new Vector3(a.position.x, 0, 0), new Vector3(b.position.x, 0, 0));
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2); 
    }
}