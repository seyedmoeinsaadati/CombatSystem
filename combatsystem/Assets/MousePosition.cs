using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private float yPos = 1;
    private Vector3 worldPosition;
    private Plane plane;

    private void Start()
    {
        plane = new Plane(Vector2.up, yPos);
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            worldPosition = ray.GetPoint(distance);
            transform.position = worldPosition;
        }
    }
}
