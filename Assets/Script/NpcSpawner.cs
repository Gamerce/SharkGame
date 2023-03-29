using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public List<Transform> wpPath;
    public List<Transform> wpPath2;

    public bool hasTriggered = false;
    public bool isRunning = false;

    public int spawnEnemies = 0;

    public int _spawnedCount = 0;

    public List<NPC> spawnedChars = new List<NPC>();

    public bool isBoss = false;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            if(spawnedChars.Count ==0)
            {
                if(_spawnedCount>= spawnEnemies)
                {
                    isRunning = false;
                }
                else
                {
                    SpawnEnemy();
                }
            }
        }
    }
    public void Trigger()
    {
        if (hasTriggered)
            return;

        SpawnEnemy();
        hasTriggered = true;
        isRunning = true;
    }
    int currentpath = 0;
    public void SpawnEnemy()
    {
        GameObject prefab = GameHandler.instance.NpcPrefab;
        if (isBoss)
            prefab = GameHandler.instance.BossPrefab;

        GameObject np = GameObject.Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
        np.gameObject.SetActive(true);
        GameHandler.instance.AllNPC.Add(np.GetComponent<NPC>());

        np.transform.forward = -SharkPlayer.instance.transform.forward;

        if (isBoss)
        {
            np.transform.position = transform.position - (np.transform.forward/3);
        }
        else
        {
            if (currentpath == 0)
            {
                np.GetComponent<NPC>().wpPath = wpPath;
                np.transform.position = wpPath[0].transform.position;
                currentpath = 1;
            }

            else
            {
                np.GetComponent<NPC>().wpPath = wpPath2;
                np.transform.position = wpPath2[0].transform.position;

                currentpath = 0;
            }
        }

    


        _spawnedCount++;
        spawnedChars.Add(np.GetComponent<NPC>());
        np.GetComponent<NPC>()._npcspawner = this;
        np.GetComponent<NPC>().isBoss = isBoss;
    }
}
