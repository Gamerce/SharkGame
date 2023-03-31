using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitTriggerEvent : MonoBehaviour
{
    public GameObject enableObj;
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
        if (enabled == false)
            return;

        //gameObject.SetActive(false);
        enableObj.SetActive(true);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshCollider>().enabled = false;


        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;

        for (int i = 0; i< enableObj.transform.childCount;i++)
        {
            Vector3 pos = enableObj.transform.GetChild(i).position;
            enableObj.transform.GetChild(i).GetComponent<Rigidbody>().AddForceAtPosition((pos - position)*50, position);
        }
    }
}
