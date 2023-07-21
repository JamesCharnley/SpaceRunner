using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSetup : MonoBehaviour
{
    [SerializeField] EdgeCollider2D leftScreenEdge;
    [SerializeField] EdgeCollider2D rightScreenEdge;
    [SerializeField] EdgeCollider2D bottomScreenEdge;
    [SerializeField] EdgeCollider2D topScreenEdge;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        // get components
        cam = Camera.main;

        SetupScreenEdges();

        Physics2D.IgnoreLayerCollision(0, 7);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Debug.Log(Input.touches[0].position);
        }
    }

    void SetupScreenEdges()
    {
        // calculate bottom left screen coord and translate to local position
        Vector3 blScreenCorner = new Vector3(0, 0, 0);
        Vector3 blWorldCorner = cam.ScreenToWorldPoint(blScreenCorner);
        Vector3 blLocalCorner = leftScreenEdge.transform.InverseTransformPoint(blWorldCorner);

        // calculate top left screen coord and translate to local position
        Vector3 tlScreenCorner = new Vector3(0, Screen.height, 0);
        Vector3 tlWorldCorner = cam.ScreenToWorldPoint(tlScreenCorner);
        Vector3 tlLocalCorner = leftScreenEdge.transform.InverseTransformPoint(tlWorldCorner);

        // calculate bottom right screen coord and translate to local position
        Vector3 brScreenCorner = new Vector3(Screen.width, 0, 0);
        Vector3 brWorldCorner = cam.ScreenToWorldPoint(brScreenCorner);
        Vector3 brLocalCorner = leftScreenEdge.transform.InverseTransformPoint(brWorldCorner);

        // calculate top right screen coord and translate to local position
        Vector3 trScreenCorner = new Vector3(Screen.width, Screen.height, 0);
        Vector3 trWorldCorner = cam.ScreenToWorldPoint(trScreenCorner);
        Vector3 trLocalCorner = leftScreenEdge.transform.InverseTransformPoint(trWorldCorner);

        List<Vector2> leftPoints = new List<Vector2>();
        leftPoints.Add(new Vector2(blLocalCorner.x, blLocalCorner.y));
        leftPoints.Add(new Vector2(tlLocalCorner.x, tlLocalCorner.y));
        leftScreenEdge.SetPoints(leftPoints);

        List<Vector2> rightPoints = new List<Vector2>();
        rightPoints.Add(new Vector2(brLocalCorner.x, brLocalCorner.y));
        rightPoints.Add(new Vector2(trLocalCorner.x, trLocalCorner.y));
        rightScreenEdge.SetPoints(rightPoints);

        List<Vector2> bottomPoints = new List<Vector2>();
        bottomPoints.Add(new Vector2(blLocalCorner.x, blLocalCorner.y));
        bottomPoints.Add(new Vector2(brLocalCorner.x, brLocalCorner.y));
        bottomScreenEdge.SetPoints(bottomPoints);

        List<Vector2> topPoints = new List<Vector2>();
        topPoints.Add(new Vector2(tlLocalCorner.x, tlLocalCorner.y));
        topPoints.Add(new Vector2(trLocalCorner.x, trLocalCorner.y));
        topScreenEdge.SetPoints(topPoints);
    }
}
