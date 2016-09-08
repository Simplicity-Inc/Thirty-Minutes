using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseTargets : MonoBehaviour {

    public List<GameObject> potentialTargetsList = new List<GameObject>();
    public List<GameObject> TargetsList = new List<GameObject>();

    public int desiredNumOfTargets;
    private int currNumOfTargets;
    // Use this for initialization
    void Start () {
        potentialTargetsList.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
        FindTargets();
    }
	
	// Update is called once per frame
	void Update () {
 
    }

    void FindTargets()
    {
        for (int i = 0; i < potentialTargetsList.Count; i++)
        {
            int result = Random.Range(1, 100);
            Debug.Log(result);
            if (result <= 50)
            {
                if (currNumOfTargets < desiredNumOfTargets)
                {
                    if (potentialTargetsList[i].GetComponent<AiBase>().isTarget == false)
                    {
                        potentialTargetsList[i].gameObject.GetComponent<AiBase>().isTarget = true;
                        TargetsList.Add(potentialTargetsList[i]);
                        potentialTargetsList[i].GetComponent<Renderer>().material.color = Color.red;
                        currNumOfTargets++;
                    }
                }
            }
        }
        if (currNumOfTargets < desiredNumOfTargets)
        {
            FindTargets();
        }
    }
}
