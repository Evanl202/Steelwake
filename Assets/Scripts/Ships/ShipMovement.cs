using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [Header ("Movement")]
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 4f;

    [Header("Ship Dimensions")]
    public float shipLength = 10f;

    [Header ("Turning")]
    public float turnSpeed = 60f;
    
    private float currentSpeed = 0f;

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameRunning)
        {
            return;
        }
        
        HandleMovement();
        HandleTurning();
    }

    private void HandleMovement()
    {
        float input = Input.GetAxis("Vertical");

        if (input > 0)
        {
            currentSpeed += acceleration * Time.deltaTime; 
        }
        else if (input < 0)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else
        {
            //Decel
            currentSpeed = Mathf.MoveTowards(
                currentSpeed,
                0f,
                deceleration * Time.deltaTime
            );
        }
        currentSpeed = Mathf.Clamp(
            currentSpeed,
            -maxSpeed * 0.5f,
            maxSpeed
        );

        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    private void HandleTurning()
    {
        float turnInput = Input.GetAxis("Horizontal");

        if (Math.Abs(currentSpeed) < 0.01f)
            return;

        //Longer ships turn more slowly
        float lengthFactor - 10f / shipLength

        //Ships turn slower at high speed
        flot speedFactor = Mathf.Clamp01(
            Mathf.Abs(currentSpeed) / maxSpeed
        );

        float turnSpeed = baseTurnSpeed * lengthFactor * Mathf.Lerp(1f, 0.4f, speedFactor);

        transform.Rotate(
            Vector3.up,
            turnInput * turnSpeed * Time.deltaTime
        );
    }

}