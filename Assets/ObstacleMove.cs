using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    public float speed = 8f;
    public float resetZ = -15f;
    public float startZ = 30f;
    public float laneDistance = 3f;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < resetZ)
            ResetObstacle();
    }

    void ResetObstacle()
    {
        int lane = Random.Range(0, 3);

        transform.position = new Vector3(
            (lane - 1) * laneDistance,
            transform.position.y,
            startZ
        );
    }
}
