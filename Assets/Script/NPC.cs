using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject spine;
    public Rigidbody[] allRG;

    public Collider[] allColiders;
    public int currentIndex = 0;
    public List<Transform> wpPath;
    public bool isRagdool = false;

    public GameObject collisionEffect;

    public GameObject hitBox;

    float deadTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        allRG = transform.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < allRG.Length; i++)
        {
            allRG[i].isKinematic = true;
        }

        for (int i = 0; i < allColiders.Length; i++)
        {
            allColiders[i].isTrigger = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyUp(KeyCode.T))
        //{
        //    Explode();
        //}
   



        if (isRagdool || Vector3.Distance(GameHandler.instance.Shark.transform.position, transform.position) < 1.7f)
        {
            if (Vector3.Distance(GameHandler.instance.Shark.transform.position, transform.position) < 1.7f && isRagdool == false)
            {


                Vector3 pos1 = SharkPlayer.instance._sharkarm.SharkHead.transform.position;
                Vector3 pos2 = transform.position;
                float dist = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));
                //Debug.Log("Dist: " + dist);
                Rigidbody rb = SharkPlayer.instance._sharkarm.SharkHead.GetComponent<Rigidbody>();
                //Debug.Log("Veloc:" + rb.velocity.magnitude);
                if (rb.velocity.magnitude>1.9f)
                if(dist < 0.9f)
                {
                    Explode();
                }

                UpdateStepBackLogic();



            }
            if (isRagdool)
            {
                deadTimer += Time.deltaTime;
                if (deadTimer > 2)
                {
                    if(hasTriggeredEffect == false)
                    {
                        hasTriggeredEffect = true;
                        GameHandler.instance.GiveScore(0f);

                    }
                    Destroy(gameObject);
                }
    
            }

            return;
        }
        for (int i = 0; i < GameHandler.instance.AllNPC.Count; i++)
        {
            if (GameHandler.instance.AllNPC[i] != null)
                if (Vector3.Distance(GameHandler.instance.AllNPC[i].transform.position, SharkPlayer.instance.transform.position) < 1.7)
                {
                    return;
                }
        }


        if (Vector3.Distance(wpPath[currentIndex].transform.position,transform.position) < 0.1f)
        {
            currentIndex++;
        }
        transform.position = Vector3.MoveTowards(transform.position, wpPath[currentIndex].transform.position, Time.deltaTime*1.5f);

        transform.forward = (wpPath[currentIndex].transform.position- transform.position).normalized;



    }
    float _hitTimer = 0;
    public void UpdateStepBackLogic()
    {
       

        _hitTimer += Time.deltaTime;
        if(_hitTimer >3)
        {
            GetComponent<Animator>().Play("crosspunsh");
            _hitTimer = 0;
        }
        else
        {
            GetComponent<Animator>().Play("FightIdle");
        }


    }
    public Vector3 force;
    public Vector3 pos;

    public void Explode()
    {
        for (int i = 0; i < allColiders.Length; i++)
        {
            allColiders[i].isTrigger = false;
        }

        Vector3 velocity = SharkPlayer.instance._sharkarm.SharkHead.GetComponent<Rigidbody>().velocity;

        Debug.Log("Hit Vel: "+velocity);
        //if (velocity.z > 0)
        //{
        //    pos = new Vector3(0, 0, 0);
        //    force = new Vector3(2500, 2500, 1000);

        //}
        //if (velocity.x < 0)
        //{
        //    pos = new Vector3(0, 0, 0);
        //    force = new Vector3(2500, 2500, -1000);
        //}

        Vector3 pos1 = SharkPlayer.instance._sharkarm.SharkHead.transform.position;
        Vector3 pos2 = transform.position;


        pos = new Vector3(0, 0, 0);
        force = new Vector3(2500, 2500, 1000);


        Vector3 avgPos = new Vector3();
        for(int i = 0; i < SharkPlayer.instance.avrDir.Count;i++)
        {
            avgPos += SharkPlayer.instance.avrDir[i];
        }
        avgPos = avgPos / SharkPlayer.instance.avrDir.Count;

        float power = 1000;
        Vector3 dir = transform.position - avgPos;
        Vector3 adjustment = (SharkPlayer.instance.transform.forward+new Vector3(0,2,0))* power*2;

        force = new Vector3(dir.x* power, dir.y* power, dir.z* power)+ adjustment;
        


        GetComponent<Animator>().enabled = false;
        for (int i = 0; i < allRG.Length; i++)
        {
            allRG[i].isKinematic = false;
            allRG[i].AddForceAtPosition(force, transform.position);
        }
        isRagdool = true;
        hitBox.SetActive(false);



    }
    bool hasTriggeredEffect = false;
    public void LandEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
        if (hasTriggeredEffect == false)
        {
            if(collision.gameObject.name != "SharkTop" && collision.gameObject.name != "SharkBody")
            {
                collisionEffect.transform.position = contact.point;
                collisionEffect.SetActive(true);

                hasTriggeredEffect = true;
                GameHandler.instance.GiveScore(0f);
            }
        }

    }




}
