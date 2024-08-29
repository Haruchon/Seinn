using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    Transform target;

    float topLeftX, topLeftY, botRightX, botRightY;

    // Start is called before the first frame update
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        //Screen.SetResolution(800,600,false);
    }

    // Update is called once per frame
    void Update()
    {
        //if(Camera.main.aspect != 1)
        //{
        //    Screen.SetResolution(800, 600, false);
        //}

        //if (Input.GetKey("escape")) Application.Quit();


        transform.position = new Vector3(
            //target.position.x,
            //target.position.y,  
            Mathf.Clamp(target.position.x, topLeftX, botRightX),
            Mathf.Clamp(target.position.y, botRightY, topLeftY),
            transform.position.z);

        //Debug.og("Posicion x: " + target.position.x + " Posicion y: " + target.position.y);
    }

    public void SetBound(GameObject map)
    {
        Tiled2Unity.TiledMap config = map.GetComponent<Tiled2Unity.TiledMap>();

        float cameraSize = Camera.main.orthographicSize;

        topLeftX = map.transform.position.x + cameraSize;
        topLeftY = map.transform.position.y - cameraSize;
        botRightX = map.transform.position.x + config.NumTilesWide - cameraSize;
        botRightY = map.transform.position.y - config.NumTilesHigh + cameraSize;

    }




}
