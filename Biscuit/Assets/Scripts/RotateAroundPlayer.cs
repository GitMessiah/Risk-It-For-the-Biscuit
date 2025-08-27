using UnityEngine;

public class RotateAroundPlayer : MonoBehaviour
{

    Vector2 mousePos;
    Camera cam;
    Transform player;

    public float distanceFromPlayer = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = FindFirstObjectByType<Camera>();
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - new Vector2(player.position.x, player.position.y);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        lookDir.Normalize();
        Vector3 lookDir3 = new Vector3(lookDir.x, lookDir.y, 0);
        transform.position = player.position + lookDir3 * distanceFromPlayer;
    }
}
