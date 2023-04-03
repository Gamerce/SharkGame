using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPeopleHeadFollow : MonoBehaviour
{
    public Transform followObj;
    NPC theNPC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followObj != null)
        {
            transform.position = followObj.transform.position;
        }
    }
    public void SetFollow(Transform tf, NPC aNPC)
    {
        theNPC = aNPC;
        followObj = tf;
        StartCoroutine(EnAfterTime(0.5f));

    }
    IEnumerator EnAfterTime(float time)
    {
        yield return new WaitForSeconds(0.15f);

      
        Vector3 force = new Vector3(0, -800, 0) * 2.5f;
        force += Camera.main.transform.forward*400;
        force += -Camera.main.transform.right * 800;
        for (int i = 0; i < theNPC.allRG.Length; i++)
        {
            theNPC.allRG[i].velocity = new Vector3(0, 0, 0);
            theNPC.allRG[i].AddForceAtPosition(force, followObj.transform.position);
        }
        followObj = null;



    }
}
