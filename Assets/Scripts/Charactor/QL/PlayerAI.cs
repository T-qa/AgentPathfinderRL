using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 3f;

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.deltaTime;

        // Apply the new position to the player's transform
        transform.position = newPosition;
    }
}
