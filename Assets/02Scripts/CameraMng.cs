using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMng : MonoBehaviour
{

    public Board board;
    private float x;
    private float y;

    [HideInInspector]
    public float adjvar;

    private void CameraSet()
    {

        x = board.stage.stats[board.stagenumber].width;
        y = board.stage.stats[board.stagenumber].height;

        if (board.stage.stats[0].width % 2 == 0)
        {
            y = y / 2;
            x = x / 2 - (float)0.5;

        }
        else
        {

            y = (int)(y / 2);
            x = (int)(x / 2);

        }
       
        var hmoveboard = (y * 0.0375) + 1.6; // board를 올리고 싶으면 +1을 낮추시오.

        viewprposion();

        var adjx = (x * 1.8) * adjvar;
        var adjy = y * adjvar + hmoveboard;

        Camera.main.transform.position = new Vector3((float)adjx, (float)adjy, -10);


    }

    public float viewprposion()
    {
        var height = 2 * Camera.main.orthographicSize;
        var width = height * Camera.main.aspect;
        
        return adjvar = (width / height);
    }

    // Start is called before the first frame update
    void Start()
    {
        CameraSet();
    }
}
