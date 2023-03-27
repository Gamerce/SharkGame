using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkArm : MonoBehaviour
{
    public GameObject SharkHead;
    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;

    public float Idletimer = 0;

    public Animator anim;

    public List<BoxCollider> bx;


    public float minRotation = -45;
    public float maxRotation = 45;
    public Vector3 currentEulerAngles;
    public float rotationSpeed = 1;

    public GameObject SharkMovemntBodyPart;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public GameObject originPoint;
    public float sharFollowSpeed = 1;
        // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");




            //    transform.Rotate(-h, 0, -v);

          //  Debug.Log("v: "+v + "h: "+h);
            v = Mathf.Clamp(v, -4, 4);
            h = Mathf.Clamp(h, -4, 4);


            //modifying the Vector3, based on input multiplied by speed and time
            currentEulerAngles += new Vector3(-v, h,0) * Time.deltaTime * rotationSpeed;

            if (currentEulerAngles.x > -10)
                currentEulerAngles.x = -10;
            if (currentEulerAngles.x < -50)
                currentEulerAngles.x = -50;

            if (currentEulerAngles.y > 50)
                currentEulerAngles.y = 50;
            if (currentEulerAngles.y < -50)
                currentEulerAngles.y = -50;

            originPoint.transform.localEulerAngles = currentEulerAngles;

            //transform.position = Vector3.Lerp(transform.position, originPoint.transform.position + originPoint.transform.forward * 0.5f, Time.deltaTime * 5);
            transform.position = (originPoint.transform.position + originPoint.transform.forward*0.5f);
            transform.forward = transform.position- originPoint.transform.position;

            SharkMovemntBodyPart.transform.eulerAngles += new Vector3(v, h,0) * Time.deltaTime * sharFollowSpeed;

        }
    }
}
