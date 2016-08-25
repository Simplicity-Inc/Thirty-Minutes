using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {
    public bool pickUp;
    public int maxInvSize;
    public struct Items
    {
        public int ID;
        public GameObject Model;
        public string Name;
        public bool Lethal;
    }

    public List<Items> ItemsList = new List<Items>();
    public List<GameObject> CollectablesList = new List<GameObject>();
    public List<GameObject> InteractableList = new List<GameObject>();

    Items Poison = new Items { ID = 1, Model = null, Name = "Poison", Lethal = true };
    Items Key = new Items { ID = 2, Model = null, Name = "Key", Lethal = false };

    // Use this for initialization
    public void Start()
    {
        //maybe move this so it can be added in collectable script
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
            Debug.Log("Collectables looped through");
            if (CheckXDis(transform, CollectablesList[i].transform) < 2)
            {
                Debug.Log("Distance checked");
                if (ItemsList.Capacity <= maxInvSize)
                {
                    if (CollectablesList[i].gameObject.name == ("Poison"))
                    {
                        ItemsList.Add(Poison);
                        CollectablesList[i].SetActive(false);
                        Debug.Log("Poison added");
                        break;
                    }
                    if (CollectablesList[i].gameObject.name == ("Key"))
                    {
                        ItemsList.Add(Key);
                        CollectablesList[i].SetActive(false);
                        Debug.Log("Key added");
                        break;
                    }
                }
                else { Debug.Log("Inventory full!"); }
            }
            else { InteractItem(); }
        }
    }
    void InteractItem()
    {
        Debug.Log("Trap Item Ran");
        for (int i = 0; i < InteractableList.Count; i++)
        {
            if (CheckXDis(transform, InteractableList[i].transform) < 2)
            {
                if (InteractableList[i].name == "Coffee")
                {
                    if (ItemsList.Contains(Poison))
                    {
                        ItemsList.Remove(Poison);
                        Debug.Log("Poison Removed");
                        InteractableList[i].GetComponent<Traps>().lethal = true;
                        InteractableList[i].GetComponent<Renderer>().material.color = Color.red;
                        Debug.Log("Shits lethal bruh");
                    }
                }
                if (InteractableList[i].name == "Door")
                {
                    if (ItemsList.Contains(Key))
                    {
                        ItemsList.Remove(Key);                      
                        InteractableList[i].GetComponent<Door>().locked = false;
                        Debug.Log("Door unlocked");
                    }
                }
            }
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