using UnityEngine;

public class CharacterMovementController : Controller<CharacterMovement>
{
    [SerializeField] private int xInvert = 0;
    [SerializeField] private int yInvert = 0;
    [SerializeField] private float angleOffset = 0;
    Vector2 moveInput;
    

    public override void Control()
    {
        moveInput.x = Input.GetAxis("Vertical") * xInvert;
        moveInput.y = Input.GetAxis("Horizontal") * yInvert;
        target.Sprint(Input.GetKey(KeyCode.LeftShift));

        moveInput = Rotate(moveInput, angleOffset);
        target.Move(moveInput);

    }

    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

}
