

using UnityEngine;
using UnityEditor;


public enum GizomType
{
WP,
SpawnPoint,
Can,
EntryWP
};

public class GizmoDrawing : MonoBehaviour
{
    public GizomType myType;

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {



        if (myType == GizomType.WP)
        {
            Vector3 position = transform.position;

            Gizmos.DrawCube(position, new Vector3(1, 1, 1));

            LevelData ld = transform.parent.GetComponent<LevelData>();

            for (int i = 0; i < ld.wpPath.Count - 1; i++)
            {
                if (ld.wpPath[i].transform == transform)
                {
                    Gizmos.color = Color.red;

                    Gizmos.DrawLine(this.gameObject.transform.position, ld.wpPath[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f), ld.wpPath[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f) * 2, ld.wpPath[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f) * 3, ld.wpPath[i + 1].transform.position);
                }
            }
        }
        if (myType == GizomType.EntryWP)
        {
            Vector3 position = transform.position;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(position, 0.3f);

            NpcSpawner ld = transform.parent.parent.GetComponent<NpcSpawner>();
            if(ld == null)
                ld = transform.parent.GetComponent<NpcSpawner>();


            for (int i = 0; i < ld.wpPath.Count - 1; i++)
            {
                if (ld.wpPath[i].transform == transform)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(this.gameObject.transform.position, ld.wpPath[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f), ld.wpPath[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f) * 2, ld.wpPath[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f) * 3, ld.wpPath[i + 1].transform.position);
                }
            }
            for (int i = 0; i < ld.wpPath2.Count - 1; i++)
            {
                if (ld.wpPath2[i].transform == transform)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(this.gameObject.transform.position, ld.wpPath2[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f), ld.wpPath2[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f) * 2, ld.wpPath2[i + 1].transform.position);
                    Gizmos.DrawLine(this.gameObject.transform.position + new Vector3(0.01f, 0.01f, 0.01f) * 3, ld.wpPath2[i + 1].transform.position);
                }
            }
        }
        else if(myType == GizomType.SpawnPoint)
        {
            Vector3 position = transform.position;
            Gizmos.color = Color.green;

            Gizmos.DrawSphere(position, 1);
        }
        else if (myType == GizomType.Can)
        {
            Vector3 position = transform.position;
            Gizmos.color = Color.red;

            Gizmos.DrawSphere(position, 0.5f);
        }


    }
}

