using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionObject : MonoBehaviour
{
    public NPC _npc;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
  
    }
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;

//        Debug.Log("Collision:" + collision.gameObject.name);

        //float impactforce = collision.relativeVelocity.x + collision.relativeVelocity.y + collision.relativeVelocity.z;
        //Debug.Log(impactforce);
        //if (impactforce > 1.5 || impactforce < -1.5)
        //{
            if (_npc.isRagdool == false)
            {
                //GetComponent<BoxCollider>().enabled = false;
                //_npc.Explode(collision);
            }
            else
            {
                _npc.LandEffect(collision);
            }
        //}

    }
}
