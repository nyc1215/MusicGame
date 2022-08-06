using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.Assertions;

public class SimpleNoteEvent : MonoBehaviour
{
    public string normalNoteEventID;
    public GameObject normalNotePrefab;
    private KoreographyEvent normalCurrentEvent;

    public string lifeNoteEventID;
    public GameObject lifeNotePrefab;

    public string shieldNoteEventID;
    public GameObject shieldNotePrefab;

    public int noteSpeed = 25;

    public GameObject[] walls;

    void Start()
    {
        Koreographer.Instance.RegisterForEventsWithTime(normalNoteEventID, NormalNoteFly);
        Koreographer.Instance.RegisterForEventsWithTime(lifeNoteEventID, LifeNoteFly);
        Koreographer.Instance.RegisterForEventsWithTime(shieldNoteEventID, ShieldNoteFly);
    }

    private void NormalNoteFly(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreoEvent.HasColorPayload())
        {
            if (normalCurrentEvent == null || normalCurrentEvent != koreoEvent)
            {
                normalCurrentEvent = koreoEvent;

                Color PosAndScale = koreoEvent.GetColorValue();
                GameObject note = Instantiate(normalNotePrefab,
                    new Vector3(-3f + 6 * PosAndScale.r, -1f + 4 * PosAndScale.g, 50),
                    Quaternion.Euler(0, 0, 0), transform
                    );
                note.GetComponent<NormalNote>().noteSpeed = noteSpeed;
                note.GetComponent<MeshRenderer>().material.color = new Color(
                    PosAndScale.r, PosAndScale.g, PosAndScale.b, 1.0f
                    );
                Debug.Log("note: " + note.transform.position);
                note.transform.parent = transform;
                //r,g 对应位置x,y  b，a对应缩放x,y  rgba范围为0-1
                if (koreoEvent.EndSample != koreoEvent.StartSample)
                {
                    note.transform.localScale = new Vector3(
                         PosAndScale.b * note.transform.localScale.x,
                         PosAndScale.a * note.transform.localScale.y,
                         (koreoEvent.EndSample - koreoEvent.StartSample) / 1000 * note.transform.localScale.z
                         );
                }
                else
                {
                    note.transform.localScale = new Vector3(
                         PosAndScale.b * note.transform.localScale.x,
                         PosAndScale.a * note.transform.localScale.y,
                         note.transform.localScale.z
                         );
                }
                note.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -noteSpeed);

                foreach (GameObject awall in walls)
                {
                    MeshRenderer wall_material = awall.GetComponent<MeshRenderer>();
                    if (wall_material != null)
                    {
                        wall_material.material.color = new Color(
                        PosAndScale.r, PosAndScale.g, PosAndScale.b, 1.0f
                        );
                    }
                }
            }
        }
    }

    private void LifeNoteFly(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreoEvent.HasColorPayload())
        {
            Color PosAndScale = koreoEvent.GetColorValue();
            GameObject note = Instantiate(lifeNotePrefab,
                new Vector3(0, 0, 50),
                Quaternion.Euler(0, 0, 0), transform
                );
            note.GetComponent<LifeNote>().noteSpeed = noteSpeed;
            Debug.Log("note: " + note.transform.position);
            note.transform.parent = transform;
            //r,g 对应位置x,y  b，a对应缩放x,y  rgba范围为0-1
            note.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -noteSpeed);
        }
        else
        {
            GameObject note = Instantiate(lifeNotePrefab,
               new Vector3(0, 0, 50),
               Quaternion.Euler(0, 0, 0), transform
           );
            note.GetComponent<LifeNote>().noteSpeed = noteSpeed;
            note.transform.parent = transform;
            note.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -noteSpeed);
        }
    }

    private void ShieldNoteFly(KoreographyEvent koreoEvent, int sampleTime, int sampleDelta, DeltaSlice deltaSlice)
    {
        if (koreoEvent.HasColorPayload())
        {
            Color PosAndScale = koreoEvent.GetColorValue();
            GameObject note = Instantiate(shieldNotePrefab,
                new Vector3(0, 0, 50),
                Quaternion.Euler(0, 0, 0), transform
                );
            note.GetComponent<ShieldNote>().noteSpeed = noteSpeed;
            Debug.Log("note: " + note.transform.position);
            note.transform.parent = transform;
            //r,g 对应位置x,y  b，a对应缩放x,y  rgba范围为0-1
            note.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -noteSpeed);
        }
        else
        {
            GameObject note = Instantiate(shieldNotePrefab,
               new Vector3(0, 0, 50),
               Quaternion.Euler(0, 0, 0), transform
           );
            note.GetComponent<ShieldNote>().noteSpeed = noteSpeed;
            note.transform.parent = transform;
            note.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -noteSpeed);
        }
    }

    public void ClearAllNote()
    {
        BroadcastMessage("DestoryMyself", SendMessageOptions.DontRequireReceiver);
    }
}
