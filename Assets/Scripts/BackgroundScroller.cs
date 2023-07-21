using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] GameObject[] tiles;
    int curIndex = 0;

    [SerializeField] float scrollSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i].transform.position -= new Vector3(0, scrollSpeed * Time.deltaTime, 0);
        }

        if(tiles[curIndex].transform.position.y <= 0)
        {
            int otherIndex = 0;
            if(curIndex == 0)
            {
                otherIndex = 1;
                curIndex = 1;
            }
            else
            {
                curIndex = 0;
            }

            tiles[otherIndex].transform.position = new Vector3(0, 21, 0);
        }
    }
}
