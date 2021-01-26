using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public bool horizontalMovement = true;
    public float speed = 10f;
    public float changeDirectionTime = 0.5f;
    public bool movingRightOrUp = true;

    void Start()
    {
        InvokeRepeating("ChangeDirection", changeDirectionTime, changeDirectionTime);
    }

    void Update()
    {
        if (horizontalMovement && movingRightOrUp)
            transform.Translate(Vector2.right * Time.deltaTime * speed, Space.World);
        else if (!horizontalMovement && movingRightOrUp)
            transform.Translate(Vector2.up * Time.deltaTime * speed, Space.World);
        else if (horizontalMovement && !movingRightOrUp)
            transform.Translate(Vector2.left * Time.deltaTime * speed, Space.World);
        else if (!horizontalMovement && !movingRightOrUp)
            transform.Translate(Vector2.down * Time.deltaTime * speed, Space.World);
    }

    void ChangeDirection()
    {
        if (movingRightOrUp) movingRightOrUp = false;
        else movingRightOrUp = true;
    }
}
