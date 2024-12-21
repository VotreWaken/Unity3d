using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    private GameObject character;
    private Vector3 s;
    private InputAction lookAction;
    private float angleH,
        angleH0; // накопичений та стартовий кут повороту камери по горизонталі
    private float angleV,
        angleV0; // --..-- по вертикалі

    void Start()
    {
        character = GameObject.Find("Character");
        lookAction = InputSystem.actions.FindAction("Look");
        s = this.transform.position - character.transform.position;
        angleH0 = angleH = transform.eulerAngles.y;
        angleV0 = angleV = transform.eulerAngles.x;
    }

    void Update()
    {
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        float sensitivityFactor = 10f * Time.deltaTime;
        angleH += lookValue.x * sensitivityFactor;
        if (
            0 < angleV - lookValue.y * sensitivityFactor
            && angleV - lookValue.y * sensitivityFactor < 90
        )
        {
            angleV -= lookValue.y * sensitivityFactor;
        }
        this.transform.eulerAngles = new Vector3(angleV, angleH, 0f);
        this.transform.position =
            character.transform.position
            + Quaternion.Euler(angleV - angleV0, angleH - angleH0, 0f) * s;
    }
}
