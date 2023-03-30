using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float speed = 10;
    public GameObject pickupeffect;
    float collisionTimer = 0;
    public GameObject TargetTransform;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,1,0),Time.deltaTime*speed,Space.World);
        collisionTimer += Time.deltaTime;
        if(collisionTimer>1)
        {
            GetComponent<BoxCollider>().enabled = true;
        }

        transform.position = Vector3.Lerp(transform.position, TargetTransform.transform.position, Time.deltaTime * 3);

    }
    void OnCollisionEnter(Collision collision)
    {
 
        if(collision.gameObject.name.Contains("HitBox"))
        {
            GameObject go = GameObject.Instantiate(pickupeffect, transform.position, Quaternion.identity);
            go.transform.parent = transform.parent;
            Destroy(gameObject);
        }

    }
}
