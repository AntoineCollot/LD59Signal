using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float moveSpeedSmooth;
    float refMoveSpeed, currentMoveSpeed;
    Vector3 movementDir;
    Vector3 movement;
    Rigidbody body;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();
        movementDir = Vector3.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameIsPlaying)
            return;

        Vector2 inputs = InputManager.MovementInput;
        bool isMoving = inputs.magnitude > 0.2f;

        float targetMoveSpeed = 0;
        if (isMoving)
        {
            targetMoveSpeed = moveSpeed;
            targetMoveSpeed *= StatsManager.Instance.MoveSpeedMult;
            if (PowerUpManager.Instance.HasPowerUp(PowerUp.HeavyArtillery)) // slow of heavy artillery
                targetMoveSpeed *= PowerUpManager.HEAVY_ARTILLERY_MOVE_MULT;

            movementDir = new Vector3(inputs.x, 0, inputs.y);
            movementDir.Normalize();
        }

        //Move Speed smoothing
        currentMoveSpeed = Mathf.SmoothDamp(currentMoveSpeed, targetMoveSpeed, ref refMoveSpeed, moveSpeedSmooth);

        //Apply movement
       // movement = movementDir * currentMoveSpeed * Time.deltaTime;
        movement = movementDir * currentMoveSpeed;
        //transform.Translate(movement, Space.World);
    }

    private void FixedUpdate()
    {
        body.linearVelocity = Vector3.zero;
        body.MovePosition(transform.position + movement * Time.fixedDeltaTime);
    }
}
