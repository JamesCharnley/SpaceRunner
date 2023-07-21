using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EObstacleSize
{
    x_small, //0.5
    small, // 1
    small_medium, // 1.5
    medium, // 2
    large, // 2.5
    x_large // 3

}
[System.Serializable]
public struct FObstacleData
{
    public EObstacleSize type;
    public float height;
    public float width;
}
public class Obstacle : MonoBehaviour
{
    public FObstacleData data;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -1) * 9;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
