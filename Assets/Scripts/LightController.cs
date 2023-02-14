using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] float updateTime;
    [SerializeField] float speed;
    [SerializeField] float waitTime;
    [SerializeField] float movePerTurn;
    [SerializeField] List<Vector3> spots;
    [SerializeField] List<Color> colors;
    [SerializeField] List<Color> skyColors;
    [SerializeField] Light myLight;
    [SerializeField] Camera myCamera;

    int currentSpot;
    int prevSpot;
    float time;
    float totalMove;

    private void Start()
    {
        prevSpot = spots.Count - 1;
    }

    private void Update()
    {
        if (movePerTurn * (TurnController.currentTurn) >= totalMove)
        {
            time += Time.deltaTime;
            if (time >= updateTime)
            {
                time = 0f;
                transform.LookAt(Vector3.zero);
                if (Vector3.Distance(transform.position, spots[currentSpot]) < 1f)
                {
                    time -= waitTime;
                    prevSpot = currentSpot;
                    currentSpot++;
                    if (currentSpot == spots.Count)
                    {
                        currentSpot = 0;
                    }
                }
                totalMove += speed;
                transform.position = Vector3.MoveTowards(transform.position, spots[currentSpot], speed);
                float progress = Vector3.Distance(transform.position, spots[currentSpot]) / Vector3.Distance(spots[prevSpot], spots[currentSpot]);
                myLight.color = Color.Lerp(colors[prevSpot], colors[currentSpot], 1 - progress);
                myCamera.backgroundColor = Color.Lerp(skyColors[prevSpot], skyColors[currentSpot], 1 - progress);
            }
        }
    }
}
