using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private InputAction moveAction;
    private CharacterController characterController;
    private float speedFactor = 5f;
    private bool isMoving = false;
    private Animator animator;
    private float burstPeriod = 10f;
    private float burstLeft;

    public float burstLevel => burstLeft / burstPeriod;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        GameState.AddListener(nameof(GameState.isBurst), OnBurstChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = !isMoving;
        }
        if (isMoving)
        {
            bool isShiftPressed =
                Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (isShiftPressed)
            {
                speedFactor = 15f;
            }
            else
            {
                speedFactor = 5f;
            }
            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            Vector3 move = Camera.main.transform.forward; // напрям погляду камери
            move.y = 0.0f; // проєктуємо на горизонтальну площину
            if (move == Vector3.zero) // вектор був вертикальним (погляд вниз)
            {
                move = Camera.main.transform.up; // тоді вперед "дивиться" вісь Y
            }
            move.Normalize(); // видовжуємо вектор після проєктування
            // у даному місці move - напрям постійного руху (польоту)

            Vector3 moveForward = move; // зберігаємо для повороту персонажа

            // додаємо до нього управління, яке теж орієнтовано по камері
            move += moveValue.x * Camera.main.transform.right;
            move.y = moveValue.y;
            move.y -= 55f * Time.deltaTime; // падіння
            characterController.Move(speedFactor * Time.deltaTime * move);
            this.transform.forward = moveForward; // повертаємо персонажа у напряму руху
            // Debug.Log(this.transform.position.y - Terrain.activeTerrain.SampleHeight(this.transform.position));
            // перемикаємось між анімаціями руху/польоту в залежності від
            // висоти над землею
            if (
                this.transform.position.y
                    - Terrain.activeTerrain.SampleHeight(this.transform.position)
                > 1.5f
            )
            {
                animator.SetInteger("MoveState", 2);
                if (!isShiftPressed)
                {
                    speedFactor = 10f;
                }
            }
            else
            {
                animator.SetInteger("MoveState", 1);
            }
        }
        else
        {
            animator.SetInteger("MoveState", 0);
        }
    }

    private void LateUpdate()
    {
        if (burstLeft > 0f)
        {
            burstLeft -= Time.deltaTime;
            if (burstLeft <= 0f)
            {
                burstLeft = 0f;
                GameState.isBurst = false;
            }
        }
    }

    private void OnBurstChanged(string ignored)
    {
        if (GameState.isBurst)
        {
            Debug.Log("Burst ++");
            burstLeft = burstPeriod;
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(nameof(GameState.isBurst), OnBurstChanged);
    }
}
/* Д.З. Обмежити поворот камери по вертикалі: не менше 0, не більше 90
 * Реалізувати горизонтальний рух таким чином, щоб зберігалась висота
 * над поверхнею землі (відповідно до рельєфу), але не вище за 20 одиниць
 * (в абсолютних координатах)
 *
 */
