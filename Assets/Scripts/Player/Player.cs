using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
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
    public Image frontStaminaBar;
    public Image backStaminaBar;
    float chipSpeed = 2;
    float lerpTimer;
    float attackCost = 6;
    float strongAttackCost = 12;
    float runCost = 15;
    float chargeRate = 33;
    Coroutine recharge;

    bool isPaused = false;
    public GameObject menuButtons;

    [Header("Attack and Combo Parameters")]
    bool attack1;
    bool attacking = false;
    float lastAttackMaxTime = 0.8f;
    float lastAttackTimer = 0;
    public bool strongAttack = true;
    float strongAttackMaxTime = 2;
    public float strongAttackTimer = 0;

    //public QuestBase quest;

    [Header("Audio Info")]
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip strongAttackSound;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        childSprite.GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        cam = Camera.main;
        motor = GetComponent<TestPlayerMotor>();
        //transform.position = new Vector3(GameManager.manager.x, GameManager.manager.y, GameManager.manager.z);
        transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        //quest.initializeQuest();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        TogglePause();

        if (CheckFreeze()) return;

        MouseMovement();
        Move();
        MouseAiming();
        Block();
        if (!blocking)
        {
            Jump();
            Attack();
        }
        UpdateStaminaUI();

        GameManager.manager.x = transform.position.x;
        GameManager.manager.y = transform.position.y;
        GameManager.manager.z = transform.position.z;

        if (attack1)
        {
            if (lastAttackTimer < lastAttackMaxTime)
            {
                lastAttackTimer += Time.deltaTime;
            }
            else
            {
                attack1 = false;
                lastAttackTimer = 0;
            }
        }

        if(!strongAttack)
        {
            if(strongAttackMaxTime > strongAttackTimer)
            {
                strongAttackTimer += Time.deltaTime;
            }
            else
            {
                strongAttack = true;
                strongAttackTimer = 0;
            }
        }

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Q) || Input.GetMouseButtonDown(1))
        {
            attacking = true;
        }
        else if(attacking)
        {
            StartCoroutine(CanMove());
        }
    }

    private void FixedUpdate()
    {
        if(!attacking)
        {
            FixedMove();
        }
    }

    IEnumerator CanMove()
    {
        yield return new WaitForSeconds(0.2f);
        attacking = false;
    }

    public bool CheckFreeze()
    {
        bool isFrozen;

        if(DialogueManager.instance.inDialogue)
        {
            isFrozen = true;
        }
        else if(DialogueManager.instance.isDialogueOption)
        {
            isFrozen = true;
        }
        /*else if(InventoryManager.instance.isInventoryActive)
        {
            isFrozen = true;
        }*/
        else if(RewardManager.instance.isRewardUIActive)
        {
            isFrozen = true;
        }
        else if(QuestManager.questManager.inQuestUI)
        {
            isFrozen = true;
        }
        else if(isPaused)
        {
            isFrozen = true;
        }
        else
        {
            isFrozen = false;
        }

        return isFrozen;
    }

    void TogglePause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                isPaused = false;
                //menuButtons.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                isPaused = true;
                //menuButtons.gameObject.SetActive(true);
            }
        }
    }

    void MouseMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, ground))
            {
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
                /*CharacterDialogue characterDialogue = hit.collider.GetComponent<CharacterDialogue>();
                if(characterDialogue != null)
                {
                    SetFocus(characterDialogue);
                }*/
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
    }

    void Move()
    {
        if(Input.GetKey("left shift") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            running = true;
            childSprite.GetComponent<Animator>().SetBool("Running", true);
            if (GameManager.manager.currentStamina > 0)
            {
                moveSpeed = 7; // 5
            }
            else
            {
                moveSpeed = 3.5f;
            }
        }
        else
        {
            running = false;
            childSprite.GetComponent<Animator>().SetBool("Running", false);
            moveSpeed = 3.5f;
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            if(!attacking && !running)
            {
                childSprite.GetComponent<Animator>().SetBool("Walking", true);
            }
            else
            {
                childSprite.GetComponent<Animator>().SetBool("Walking", false);
            }
        }
        else
        {
            childSprite.GetComponent<Animator>().SetBool("Walking", false);
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        if(running)
        {
            GameManager.manager.currentStamina -= runCost * Time.deltaTime;
            if(GameManager.manager.currentStamina < 0)
            {
                GameManager.manager.currentStamina = 0;
            }
            frontStaminaBar.fillAmount = GameManager.manager.currentStamina / GameManager.manager.maxStamina;
            if (recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }
    }

    void FixedMove()
    {
        if(!CheckFreeze())
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void MouseAiming()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!CheckFreeze())
        {
            if (Physics.Raycast(ray, out hit))
            {
                cursorPosition = hit.point - transform.position;

                transform.forward = new Vector3(cursorPosition.x, 0, cursorPosition.z);
            }
        }
    }

    public void Attack()
    {
        if(Input.GetMouseButtonDown(0) && GameManager.manager.currentStamina > attackCost &&
            !Input.GetMouseButtonDown(1))
        {
            if(!attack1)
            {
                AudioManager.instance.PlaySound(attackSound);
                childSprite.GetComponent<Animator>().SetTrigger("AxeAttack1");
                GameManager.manager.currentStamina -= attackCost;
                attack1 = true;
            }
            else if(attack1)
            {
                AudioManager.instance.PlaySound(attackSound);
                childSprite.GetComponent<Animator>().SetTrigger("AxeAttack2");
                GameManager.manager.currentStamina -= attackCost;
                attack1 = false;
                lastAttackTimer = 0;
            }

            frontStaminaBar.fillAmount = GameManager.manager.currentStamina / GameManager.manager.maxStamina;

            if(recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }
        else if(Input.GetMouseButtonDown(1) && GameManager.manager.currentStamina > strongAttackCost &&
            !Input.GetMouseButtonDown(0) && strongAttack)
        {
            AudioManager.instance.PlaySound(strongAttackSound);
            childSprite.GetComponent<Animator>().SetTrigger("StrongAxeAttack");
            GameManager.manager.currentStamina -= strongAttackCost;
            strongAttack = false;

            frontStaminaBar.fillAmount = GameManager.manager.currentStamina / GameManager.manager.maxStamina;

            if (recharge != null)
            {
                StopCoroutine(recharge);
            }
            recharge = StartCoroutine(RechargeStamina());
        }
    }

    void Block()
    {
        if(Input.GetKey(KeyCode.Q) && GameManager.manager.currentStamina > 5)
        {
            childSprite.GetComponent<Animator>().SetBool("AxeBlock", true);
            blocking = true;

            frontStaminaBar.fillAmount = GameManager.manager.currentStamina / GameManager.manager.maxStamina;
        }
        else
        {
            childSprite.GetComponent<Animator>().SetBool("AxeBlock", false);
            blocking = false;
        }

        if(blocking)
        {
            if (GameManager.manager.currentStamina < 0)
            {
                GameManager.manager.currentStamina = 0;
            }
            frontStaminaBar.fillAmount = GameManager.manager.currentStamina / GameManager.manager.maxStamina;
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
        float hFraction = GameManager.manager.currentStamina / GameManager.manager.maxStamina;
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

        while(GameManager.manager.currentStamina < GameManager.manager.maxStamina)
        {
            GameManager.manager.currentStamina += chargeRate / 10;
            if(GameManager.manager.currentStamina > GameManager.manager.maxStamina)
            {
                GameManager.manager.currentStamina = GameManager.manager.maxStamina;
            }
            frontStaminaBar.fillAmount = GameManager.manager.currentStamina / GameManager.manager.maxStamina;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void IncreaseStamina(int level)
    {
        GameManager.manager.maxStamina += (GameManager.manager.currentStamina * 0.01f) * ((50 - level) * 0.1f);
        GameManager.manager.currentStamina = GameManager.manager.maxStamina;
    }
}
