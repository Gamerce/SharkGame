using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkPlayer : MonoBehaviour
{
    public int currentIndex = 0;
    public List<Transform> wpPath;
    public SharkArm _sharkarm;

    public static SharkPlayer instance;

    public List<Vector3> avrDir= new List<Vector3>();

    // Start is called before the first frame update
    public void Init()
    {
        instance = this;
    }

    float _addTimer = 0;
    // Update is called once per frame
    void Update()
    {
        _addTimer += Time.deltaTime;
        if (_addTimer>0.05f)
        {
            avrDir.Add(_sharkarm.SharkHead.transform.position);

            if (avrDir.Count > 5)
                avrDir.RemoveAt(0);

            _addTimer = 0;
        }




        for (int i = 0; i < GameHandler.instance.AllNpcSpawners.Count; i++)
        {
            if (GameHandler.instance.AllNpcSpawners[i].hasTriggered == false)
                if (Vector3.Distance(GameHandler.instance.AllNpcSpawners[i].transform.position, transform.position) < 1.5f)
                {
                    GameHandler.instance.AllNpcSpawners[i].Trigger();
                    return;
                }
            if(GameHandler.instance.AllNpcSpawners[i].isRunning)
                return;
        }


        if (currentIndex < wpPath.Count && GameHandler.instance.GameHasEnded == false)
        {
            if (Vector2.Distance(new Vector2(wpPath[currentIndex].transform.position.x, wpPath[currentIndex].transform.position.z), new Vector2(transform.position.x, transform.position.z)) < 0.1f)
            {
                currentIndex++;
            }
            if (currentIndex < wpPath.Count)
            {
                Vector3 targetPos = wpPath[currentIndex].transform.position;
                targetPos.y = transform.position.y;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 4f);
                transform.forward = Vector3.Lerp(transform.forward, (targetPos - transform.position).normalized, Time.deltaTime * 3);
            }
        }

    }
}
