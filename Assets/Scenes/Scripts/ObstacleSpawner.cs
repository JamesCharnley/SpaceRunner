using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleSpawner : MonoBehaviour
{

    [SerializeField] int[] spawnTypeScore;

    [SerializeField] GameObject[] x_small_Obstacles;
    [SerializeField] GameObject[] small_Obstacles;
    [SerializeField] GameObject[] small_medium_Obstacles;
    [SerializeField] GameObject[] medium_Obstacles;
    [SerializeField] GameObject[] large_Obstacles;
    [SerializeField] GameObject[] x_large_Obstacles;


    int currentHorizontalSpawnOffsetMultiplier = 0;
    [SerializeField] int maxHorizontalSpawnOffsetMultiplier = 6;
    [SerializeField] float horizontalSpawnOffsetSize = 0;
    [SerializeField] float spawnAxisY = 0;
    [SerializeField] float spawnAxisX = 0;
    [SerializeField] float horizontalSpawnDelay = 1;
    [SerializeField] float verticalSpawnDelay = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnDelayCoroutine(horizontalSpawnDelay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TrySpawnObstacle()
    {
        bool spawnSuccess = false;
        int lowestScore = 99999;
        int lowestScoreType = 0;
        for(int i = 0; i < spawnTypeScore.Length; i++)
        {
            if(spawnTypeScore[i] < lowestScore)
            {
                lowestScore = spawnTypeScore[i];
                lowestScoreType = i;
            }
        }

        GameObject possibleSpawn = GetRandomObstacleOfType(lowestScoreType);

        if(CanSpawnObstacle(possibleSpawn, new Vector3(spawnAxisX + horizontalSpawnOffsetSize * currentHorizontalSpawnOffsetMultiplier, spawnAxisY, 0)))
        {
            SpawnObstacle(possibleSpawn, lowestScoreType);
            spawnSuccess = true;
        }
        else
        {
            int maxRand = spawnTypeScore.Length;
            int randType = Random.Range(0, maxRand);
            possibleSpawn = GetRandomObstacleOfType(randType);
            if(CanSpawnObstacle(possibleSpawn, new Vector3(spawnAxisX + horizontalSpawnOffsetSize * currentHorizontalSpawnOffsetMultiplier, spawnAxisY, 0)))
            {
                SpawnObstacle(possibleSpawn, randType);
                spawnSuccess = true;
            }
        }

        currentHorizontalSpawnOffsetMultiplier += 1;
        if(currentHorizontalSpawnOffsetMultiplier > maxHorizontalSpawnOffsetMultiplier)
        {
            currentHorizontalSpawnOffsetMultiplier = 0;
            if(spawnSuccess)
            {
                StartCoroutine(SpawnDelayCoroutine(verticalSpawnDelay));
            }
            else
            {
                TrySpawnObstacle();
            }
        }
        else
        {
            if(spawnSuccess)
            {
                StartCoroutine(SpawnDelayCoroutine(horizontalSpawnDelay));
            }
            else
            {
                TrySpawnObstacle();
            }
        }
    }

    void SpawnObstacle(GameObject _obstacle, int _type)
    {
        Vector3 spawnPosition = new Vector3(spawnAxisX + horizontalSpawnOffsetSize * currentHorizontalSpawnOffsetMultiplier, spawnAxisY, 0);
        GameObject spawnedObstacle = Instantiate(_obstacle, spawnPosition, Quaternion.identity);
        spawnTypeScore[_type] += 1;
    }

    GameObject GetRandomObstacleOfType(int _typeIndex)
    {
        GameObject[] obs = null;
        switch (_typeIndex)
        {
            case 0: 
                obs = x_small_Obstacles;
                break;
            case 1:
                obs = small_Obstacles;
                break;
            case 2:
                obs = small_medium_Obstacles;
                break;
            case 3:
                obs = medium_Obstacles;
                break;
            case 4:
                obs = large_Obstacles;
                break;
            case 5:
                obs = x_large_Obstacles;
                break;
            default:
                break;
        }

        int maxRand = obs.Length;
        int rand = Random.Range(0, maxRand);

        return obs[rand];
    }

    bool CanSpawnObstacle(GameObject _obstacle, Vector3 _spawnPos)
    {
        Obstacle obs = _obstacle.GetComponent<Obstacle>();
        Vector2 tlc = new Vector2(_spawnPos.x, _spawnPos.y) + new Vector2(-(obs.data.width / 2), (obs.data.height / 2));
        Vector2 brc = new Vector2(_spawnPos.x, _spawnPos.y) + new Vector2((obs.data.width / 2), -(obs.data.height / 2));
        foreach(Obstacle otherObs in obstaclesInSpawnArea)
        {
            Vector2 tlcOther = new Vector2(otherObs.transform.position.x, otherObs.transform.position.y) + new Vector2(-(otherObs.data.width / 2), (otherObs.data.height / 2));
            Vector2 brcOther = new Vector2(otherObs.transform.position.x, otherObs.transform.position.y) + new Vector2((otherObs.data.width / 2), -(otherObs.data.height / 2));

            if(ObstaclesOverlap(tlc, brc, tlcOther, brcOther))
            {
                return false;
            }
        }
        return true;
    }

    bool ObstaclesOverlap(Vector2 l1, Vector2 r1, Vector2 l2, Vector2 r2)
    {
        if (l1.x > r2.x || l2.x > r1.x)
        {
            return false;
        }

        // If one rectangle is above other
        if (r1.y > l2.y || r2.y > l1.y)
        {
            return false;
        }

        return true;
    }
    IEnumerator SpawnDelayCoroutine(float _delayTime)
    {

        yield return new WaitForSeconds(_delayTime);

        TrySpawnObstacle();
    }

    [SerializeField] List<Obstacle> obstaclesInSpawnArea = new List<Obstacle>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacle obs = collision.GetComponent<Obstacle>();
        if(obs != null)
        {
            obstaclesInSpawnArea.Add(obs);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Obstacle obs = collision.GetComponent<Obstacle>();
        if (obs != null)
        {
            obstaclesInSpawnArea.Remove(obs);
        }
    }
}
