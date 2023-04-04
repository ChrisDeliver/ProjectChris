using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float shiftedSpeed = 2;
    [SerializeField] private float walkAnimationSpeed = 1f;
    [SerializeField] private float shiftedAnimationSpeed = 1.5f;

    private Animator animator;
    private bool isMoving = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool isShifted = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        
        if (horizontal != 0 || vertical != 0)
        {
            float speed = isShifted ? shiftedSpeed : walkSpeed;
            Vector3 movement = new Vector3(horizontal, vertical, 0f);

            transform.position += movement.normalized * speed * Time.deltaTime;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        SetAnimation(horizontal, vertical, isShifted);
    }

    private void SetAnimation(float horizontal, float vertical, bool isShifted)
    {
        if (isMoving)
        {
            if (horizontal > 0f)
            {
                animator.Play("Right");
            }
            else if (horizontal < 0f)
            {
                animator.Play("Left");
            }
            else if (vertical > 0f)
            {
                animator.Play("Up");
            }
            else if (vertical < 0f)
            {
                animator.Play("Down");
            }
        }
        else
        {
            animator.Play("Idle");
        }

        float animationSpeed = isShifted ? shiftedAnimationSpeed : walkAnimationSpeed;
        animator.speed = animationSpeed;
    }
}