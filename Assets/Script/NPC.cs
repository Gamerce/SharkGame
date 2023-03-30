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

    public GameObject hitEffect;
    public GameObject collisionEffect;

    public GameObject hitBox;
    public NpcSpawner _npcspawner;
    float deadTimer = 0;
    public bool isBoss = false;

    public List<GameObject> ArmourParts = new List<GameObject>();
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

        GetComponent<Animator>().CrossFade("landing", 0.01f);


    }
    public float _hitDelay = 0;
    public int hitAnimation = 0;
    // Update is called once per frame
    void Update()
    {
        _hitDelay -= Time.deltaTime;
        //if(Input.GetKeyUp(KeyCode.T))
        //{
        //    Explode();
        //}

        Vector2 playerPos = new Vector2(GameHandler.instance.Shark.transform.position.x, GameHandler.instance.Shark.transform.position.z);
        Vector2 npcPos = new Vector2(transform.position.x, transform.position.z);
        float distToPlayer = Vector2.Distance(playerPos, npcPos);

        if (isRagdool || distToPlayer < 1.0f)
        {
            if (distToPlayer < 1.0f && isRagdool == false)
            {


                //Vector3 pos1 = SharkPlayer.instance._sharkarm.SharkHead.transform.position;
                //Vector3 pos2 = transform.position;
                //float dist = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));

                //dist = Mathf.Abs(pos1.x - pos2.x);
                //Rigidbody rb = SharkPlayer.instance._sharkarm.SharkHead.GetComponent<Rigidbody>();
                //if( Vector3.Distance(SharkPlayer.instance.avrDir[0] , SharkPlayer.instance.avrDir[4]) >1.5f)
                //{
                 
                //    if (dist < 1.0f && _hitDelay <=0)
                //    {
                //        if (isBoss && ArmourParts.Count>0)
                //        {
                //            if(hitAnimation == 0)
                //            {
                //                GetComponent<Animator>().Play("headhit", 0, 0);
                //                hitAnimation = 1;
                //            }
                //            else
                //            {
                //                GetComponent<Animator>().Play("headhit2", 0, 0);
                //                hitAnimation = 0;

                //            }

                //            if (hitCount >= 4)
                //            {
                //                ArmourParts[0].transform.parent = null;
                //                ExplodeObject(ArmourParts[0]);
                //                ArmourParts.RemoveAt(0);
                //                hitCount = 0;
                //            }
                //            _hitDelay = 0.4f;
                //            hitCount++;
                //        }
                //        else
                //        {
                //            Explode();
                //        }


                //    }
                //}


                if (!isBoss)
                UpdateStepBackLogic();



            }
            if (isRagdool)
            {
                deadTimer += Time.deltaTime;
                if (deadTimer > 0.2f)
                {
                    if(hasGivenScore == false)
                    {
                        hasGivenScore = true;
                        if( isBoss == false)
                            GameHandler.instance.GiveScore(0f);

                    }
                    _npcspawner.spawnedChars.Remove(this);
                    //Destroy(gameObject);
                }
    
            }

            return;
        }


        if (currentIndex > wpPath.Count-1)
            return;
        if (Vector3.Distance(wpPath[currentIndex].transform.position,transform.position) < 0.2f)
        {
            currentIndex++;
        }
        if (currentIndex > wpPath.Count-1)
            return;
        transform.position = Vector3.MoveTowards(transform.position, wpPath[currentIndex].transform.position, Time.deltaTime*6.0f);

        transform.forward = Vector3.Lerp(transform.forward, (wpPath[currentIndex].transform.position - transform.position).normalized, Time.deltaTime * 13);
        //transform.forward = (wpPath[currentIndex].transform.position- transform.position).normalized;



    }
    bool hasGivenScore = false;
    public int hitCount = 0;
    float _hitTimer = 0;
    public int currentState = 0;
    public void UpdateStepBackLogic()
    {
       

        _hitTimer += Time.deltaTime;
        if(_hitTimer >3)
        {

            if (currentState != 2)
            {
                GetComponent<Animator>().Play("crosspunsh");
            }
            currentState = 2;
            _hitTimer = 0;
        }
        else
        {
            hitBox.SetActive(true);
            if (currentState != 1)
            {
                GetComponent<Animator>().CrossFade("FightIdle", 0.07f);
            }
            currentState = 1;
        }

        Vector3 lookPos = SharkPlayer.instance.transform.position;
        lookPos.y = transform.position.y;
        transform.forward = Vector3.Lerp(transform.forward, (lookPos - transform.position).normalized, Time.deltaTime * 13);


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
        Vector3 adjustment = (SharkPlayer.instance.transform.forward+new Vector3(0,2f,0))* power*2;

        if(isBoss)
        {
             power = 250;
             dir = transform.position - avgPos;
             adjustment = (SharkPlayer.instance.transform.forward + new Vector3(0, 1.5f, 0)) * power * 2;
            GameHandler.instance._LevelManager.GameFinished();
            GameHandler.instance.GameHasEnded = true;
        }
      
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
    public void ExplodeObject(GameObject go)
    {
        go.AddComponent<Rigidbody>();
        go.AddComponent<MeshCollider>().convex = true;

        Vector3 velocity = SharkPlayer.instance._sharkarm.SharkHead.GetComponent<Rigidbody>().velocity;
        Vector3 pos1 = SharkPlayer.instance._sharkarm.SharkHead.transform.position;
        Vector3 pos2 = transform.position;
        pos = new Vector3(0, 0, 0);
        force = new Vector3(2500, 2500, 1000);

        Vector3 avgPos = new Vector3();
        for (int i = 0; i < SharkPlayer.instance.avrDir.Count; i++)
        {
            avgPos += SharkPlayer.instance.avrDir[i];
        }
        avgPos = avgPos / SharkPlayer.instance.avrDir.Count;

        float power = 150;
        Vector3 dir = transform.position - avgPos;
        Vector3 adjustment = (SharkPlayer.instance.transform.forward + new Vector3(0, 2, 0)) * power * 2;

        force = new Vector3(dir.x * power, dir.y * power, dir.z * power) + adjustment;

        go.GetComponent<Rigidbody>().isKinematic = false;
        go.GetComponent<Rigidbody>().AddForceAtPosition(force, transform.position);
    }
    bool hasTriggeredEffect = false;
    public void HitEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;


                GameObject go = GameObject.Instantiate(hitEffect);
                go.transform.position = contact.point;
                go.SetActive(true);


    }

    public void LandEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;
       // if (hasTriggeredEffect == false)
        {
            Debug.Log(collision.gameObject.name);
            if (collision.gameObject.name != "SharkTop" && collision.gameObject.name != "SharkBody" && collision.gameObject.name != "HitBox")
            {
                GameObject go = GameObject.Instantiate(collisionEffect);
                go.transform.position = contact.point;
                go.SetActive(true);
                go.transform.forward = -(transform.position - position).normalized;


              //  hasTriggeredEffect = true;
              //GameHandler.instance.GiveScore(0f);
            }
        }

    }



}
