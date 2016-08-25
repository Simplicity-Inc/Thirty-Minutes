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
    // Use this for initialization
    public void Start()
    {
        //maybe move this so it can be added in collectable script
        CollectablesList.AddRange(GameObject.FindGameObjectsWithTag("Collectable"));

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("key pressed");
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
                            Items Poison = new Items { ID = 1, Model = null, Name = "Poison", Lethal = true };
                            ItemsList.Add(Poison);
                            CollectablesList[i].SetActive(false);
                            Debug.Log("Poison added");
                            break;
                        }
                        if (CollectablesList[i].gameObject.name == ("Key"))
                        {
                            Items Key = new Items { ID = 2, Model = null, Name = "Key", Lethal = false };
                            ItemsList.Add(Key);
                            CollectablesList[i].SetActive(false);
                            Debug.Log("Key added");
                            break;
                        } 
                    }
                    else
                    {
                        Debug.Log("Inventory full!");
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