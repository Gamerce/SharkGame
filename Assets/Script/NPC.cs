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


    public List<GameObject> RandomBodies = new List<GameObject>();
    public List<GameObject> RandomFaces = new List<GameObject>();
    public List<GameObject> RandomItems = new List<GameObject>();

    public GameObject FireEffect;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        allRG = transform.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < allRG.Length; i++)
        {
            allRG[i].isKinematic = true;
        }

        for (int i = 0; i < allColiders.Length; i++)
        {
            allColiders[i].isTrigger = true;
        }

        GetComponent<Animator>().Play("landing");
        if(isBoss)
            MusicManager.instance.PlayAudioClip(4, 0.0f, 0.5f);


        if (RandomBodies != null && RandomBodies.Count>0)
        {
            for (int i = 0; i < RandomBodies.Count; i++)
            {
                RandomBodies[i].SetActive(false);
            }
            RandomBodies[Random.Range(0, RandomBodies.Count - 1)].SetActive(true);
        }

        if (RandomBodies != null && RandomBodies.Count > 0)
        {
            for (int i = 0; i < RandomFaces.Count; i++)
            {
                RandomFaces[i].SetActive(false);
            }
            RandomFaces[Random.Range(0, RandomFaces.Count - 1)].SetActive(true);

        }
        if (RandomBodies != null && RandomBodies.Count > 0)
        {
            for (int i = 0; i < RandomItems.Count; i++)
            {
                RandomItems[i].SetActive(false);
            }
            RandomItems[Random.Range(0, RandomItems.Count - 1)].SetActive(true);

        }

      


    }
    public float _hitDelay = 0;
    public int hitAnimation = 0;
    // Update is called once per frame
    void Update()
    {

        if(obToFollow != null)
        {
            transform.position = obToFollow.transform.position + offsetToFollow;
            return;
        }




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

            if( _npcspawner.isRunning)
            {
                
                    if (allRG[0].transform.position.y > 6 && hasTriggeredEat == false && isBoss == false)
                    {
                        if (Random.Range(0, 100) < 25)
                        {
                            GameHandler.instance.SharkHeadEat.transform.position = new Vector3(allRG[0].transform.position.x, GameHandler.instance.SharkHeadEat.transform.position.y, allRG[0].transform.position.z);


                            GameHandler.instance.SharkHeadEat.SetActive(false);
                            GameHandler.instance.SharkHeadEat.SetActive(true);
                            GameHandler.instance.SharkHeadEat.transform.GetChild(0).GetComponent<Animator>().Play("SharkEatBodyAnimation");
                            GameHandler.instance.SharkHeadEat.GetComponent<EatPeopleHeadFollow>().SetFollow(allRG[0].transform, this);
                            //GameHandler.instance.SharkHeadEat.transform.position += Camera.main.transform.forward * 1.5f;
                        }
                        hasTriggeredEat = true;

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
    bool hasTriggeredEat = false;
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


            MusicManager.instance.PlayAudioClip(4, 0.1f, 0.5f);
        }
        else
        {

                if (Random.Range(0, 100) < 50)
                {
                    MusicManager.instance.PlayAudioClip(1, 0.3f, 0.2f);
                }
                else
                    MusicManager.instance.PlayAudioClip(2, 0.3f, 0.2f);




            if (animator.GetCurrentAnimatorStateInfo(0).IsName("FastRunPlace"))
            {
                GameHandler.instance.PunshText();
            }

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

        MusicManager.instance.PlayAudioClip(3, 0, 0.5f);
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
    public GameObject obToFollow=null;
    public Vector3 offsetToFollow;
    public void FollowObject(GameObject follow)
    {
        obToFollow = follow;
        offsetToFollow = transform.position - obToFollow.transform.position;

        GetComponent<Animator>().enabled = false;
        for (int i = 0; i < allRG.Length; i++)
        {
            allRG[i].isKinematic = true;
            
        }

    }


}
