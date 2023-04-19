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
    Vector2 mouseLastFrame = new Vector2();
    public float MovemntAdjustment = 1;
        // Update is called once per frame
    void Update()
    {
        if (GameHandler.instance.GameOverScreen.activeSelf)
            return;

        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mouseDelta = mousePos - mouseLastFrame;
        mouseLastFrame = mousePos;

        if (Input.GetMouseButton(0))
        {

            float h = horizontalSpeed * Input.GetAxis("Mouse X");
            float v = verticalSpeed * Input.GetAxis("Mouse Y");


            
  if(MovemntAdjustment == 0)
            {
                h = horizontalSpeed * mouseDelta.x * Time.deltaTime * 0.25f ;
                v = verticalSpeed * mouseDelta.y * Time.deltaTime * 0.25f;
            }
  else
            {
                h = horizontalSpeed * mouseDelta.x * Time.deltaTime * 0.25f * Screen.dpi * MovemntAdjustment;
                v = verticalSpeed * mouseDelta.y * Time.deltaTime * 0.25f * Screen.dpi * MovemntAdjustment;
            }


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



            //transform.position = Vector3.Lerp(transform.position, originPoint.transform.position + originPoint.transform.forward * 0.5f, Time.deltaTime * 5);


            //transform.position = (originPoint.transform.position + originPoint.transform.forward*0.5f);




        

            //SharkMovemntBodyPart.transform.eulerAngles += new Vector3(v, h,0) * Time.deltaTime * sharFollowSpeed;

        }
        originPoint.transform.localEulerAngles = currentEulerAngles;
        transform.forward = transform.position - originPoint.transform.position;
        transform.position = Vector3.Lerp(transform.position, (originPoint.transform.position + originPoint.transform.forward * 0.5f), Time.deltaTime * 50);
    }
}
