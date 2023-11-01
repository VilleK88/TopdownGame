using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(TestPlayerMotor))]
public class TestPlayer : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] GameObject childSprite;
    Camera cam;
    TestPlayerMotor motor;
    //NavMeshAgent agent;
    public Interactable focus;


    [Header("Player Movement and Mouse Aiming Parameters")]
    float moveSpeed = 3.5f;
    Vector3 movement;
    Ray ray;
    RaycastHit hit;
    Vector3 cursorPosition;
    bool running = false;


    [Header("Jump Parameters")]
    float jumpForce = 5;
    float gravityModifier = 1;
    int jumps = 0;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    public bool blocking; // if player is blocking or not

    [Header("Stamina Parameters")]
    float maxStamina = 100;
    public float currentStamina;
    public Image frontStaminaBar;
    public Image backStaminaBar;
    float chipSpeed = 2;
    float lerpTimer;
    float attackCost = 6;
    float runCost = 15;
    float chargeRate = 33;
    Coroutine recharge;

    private void Start()
    {
        currentStamina = maxStamina;
        rb = GetComponent<Rigidbody>();
        childSprite.GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        //Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
        motor = GetComponent<TestPlayerMotor>();
        //agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        MouseMovement();
        Move();
        MouseAiming();
        Block();
        if(!blocking)
        {
            Jump();
            Attack();
        }
        UpdateStaminaUI();
    }

    private void FixedUpdate()
    {
        FixedMove();
    }

    void MouseMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, ground))
            {
                //motor.MoveToPoint(hit.point);
                RemoveFocus();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if(interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    void SetFocus(Interactable newFocus)
    {
        if(newFocus != focus)
        {
            if(focus !=  null)
            {
                focus.OnDefocused();
            }

            focus = newFocus;
            //motor.FollowTarget(newFocus);
        }

        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if(focus != null)
        {
            focus.OnDefocused();
        }

        focus = null;
        //motor.StopFollowingTarget();
    }

    void Move()
    {
        if(Input.GetKey("left shift") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            running = true;
            //currentStamina -= runCost * Time.deltaTime;
            if(currentStamina > 0)
            {
                moveSpeed = 6;
            }
            else
            {
                moveSpeed = 3.5f;
            }
        }
        else
        {
            running = false;
            moveSpeed = 3.5f;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        if(running)
        {
            currentStamina -= runCost * Time.deltaTime;
            if(currentStamina < 0)
            {
                currentStamina = 0;
            }
            frontStaminaBar.fillAmount = currentStamina / maxStamina;
            if (recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }
    }

    void FixedMove()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void MouseAiming()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            cursorPosition = hit.point - transform.position;

            transform.forward = new Vector3(cursorPosition.x, 0, cursorPosition.z);
        }
    }

    public void Attack()
    {
        if(Input.GetMouseButtonDown(0) && currentStamina > 5)
        {
            childSprite.GetComponent<Animator>().SetTrigger("AxeAttack1");
            currentStamina -= attackCost;
            /*if (currentStamina < 0)
            {
                currentStamina = 0;
            }*/
            frontStaminaBar.fillAmount = currentStamina / maxStamina;

            if(recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }
    }

    void Block()
    {
        if(Input.GetKey(KeyCode.Q) && currentStamina > 5)
        {
            childSprite.GetComponent<Animator>().SetBool("AxeBlock", true);
            blocking = true;

            frontStaminaBar.fillAmount = currentStamina / maxStamina;

            /*if (recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());*/
        }
        else
        {
            childSprite.GetComponent<Animator>().SetBool("AxeBlock", false);
            blocking = false;
        }

        if(blocking)
        {
            if (currentStamina < 0)
            {
                currentStamina = 0;
            }
            frontStaminaBar.fillAmount = currentStamina / maxStamina;
            if (recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumps = 0;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumps < 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumps++;
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 1f, ground);
    }

    public void UpdateStaminaUI()
    {
        float fillF = frontStaminaBar.fillAmount;
        float fillB = backStaminaBar.fillAmount;
        float hFraction = currentStamina / maxStamina;
        if (fillB > hFraction)
        {
            frontStaminaBar.fillAmount = hFraction;
            backStaminaBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backStaminaBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backStaminaBar.fillAmount = hFraction;
            backStaminaBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontStaminaBar.fillAmount = Mathf.Lerp(fillF, backStaminaBar.fillAmount, percentComplete);
        }

        /*if(currentStamina < 99 && !running)
        {
            currentStamina += Time.deltaTime * 10;
        }*/
    }

    IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1);

        while(currentStamina < maxStamina)
        {
            currentStamina += chargeRate / 10;
            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            frontStaminaBar.fillAmount = currentStamina / maxStamina;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void IncreaseStamina(int level)
    {
        maxStamina += (currentStamina * 0.01f) * ((50 - level) * 0.1f);
        currentStamina = maxStamina;
    }
}
