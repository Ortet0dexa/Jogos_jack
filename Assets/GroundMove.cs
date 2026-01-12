using UnityEngine;

public class GroundMove : MonoBehaviour
{
    public float resetZ = -15f;
    public float startZ = 30f;

    void Update()
    {
        if (transform.position.z <= resetZ)
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                startZ
            );
        }
    }
}
