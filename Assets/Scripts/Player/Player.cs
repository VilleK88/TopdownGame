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

    [HideInInspector] public bool isPaused = false;
    public GameObject menuButtons;

    [Header("Attack and Combo Parameters")]
    bool attack1;
    bool attack2;
    bool attacking = false;
    float lastAttackMaxTime = 0.8f;
    float lastAttackTimer = 0;
    public bool strongAttack = true;
    float strongAttackMaxTime = 2;
    public float strongAttackTimer = 0;

    public VectorValue startingPosition;
    //public bool loadPlayerPosition;

    [Header("Audio Info")]
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioClip strongAttackSound;
    [SerializeField] AudioSource walkSound;
    [SerializeField] AudioSource runningSound;
    [SerializeField] AudioClip jumpSound;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        childSprite.GetComponent<Animator>();
        Physics.gravity *= gravityModifier;
        cam = Camera.main;
        motor = GetComponent<TestPlayerMotor>();
        //transform.position = new Vector3(GameManager.manager.x, GameManager.manager.y, GameManager.manager.z);
        //transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        //quest.initializeQuest();
        GameManager.manager.currentStamina = GameManager.manager.maxStamina;
        RechargeStamina();
        UpdateStaminaUI();
        if(GameManager.manager.loadPlayerPosition)
        {
            LoadPlayerTransformPosition();
            Debug.Log("Debuggaa t‰‰ll‰");
            GameManager.manager.loadPlayerPosition = false;
        }
        else
        {
            transform.position = startingPosition.initialValue;
        }
        //transform.position = startingPosition.initialValue;
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
            if(InventoryManager.instance.weaponOnHand)
            {
                Attack();
            }
            //Attack();
        }
        UpdateStaminaUI();

        //GameManager.manager.x = transform.position.x;
        //GameManager.manager.y = 1.2f;
        //GameManager.manager.z = transform.position.z;

        if (attack1)
        {
            if (lastAttackTimer < lastAttackMaxTime)
            {
                lastAttackTimer += Time.deltaTime;
            }
            else
            {
                attack1 = false;
                attack2 = false;
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

    public void SavePlayerTransformPosition()
    {
        GameManager.manager.x = transform.position.x;
        GameManager.manager.y = 1.2f;
        GameManager.manager.z = transform.position.z;
    }

    public void LoadPlayerTransformPosition()
    {
        transform.position = new Vector3(GameManager.manager.x, GameManager.manager.y, GameManager.manager.z);
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

    public void TogglePause()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                isPaused = false;
                menuButtons.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                isPaused = true;
                menuButtons.gameObject.SetActive(true);
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
                if(!runningSound.isPlaying)
                {
                    runningSound.Play();
                }
                moveSpeed = 5.5f; // 5
            }
            else
            {
                if (runningSound.isPlaying)
                {
                    runningSound.Stop();
                }

                if (!walkSound.isPlaying)
                {
                    walkSound.Play();
                }
                moveSpeed = 3.5f;
            }
        }
        else
        {
            if (runningSound.isPlaying)
            {
                runningSound.Stop();
            }
            running = false;
            childSprite.GetComponent<Animator>().SetBool("Running", false);
            moveSpeed = 3.5f;
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            if (!attacking && !running)
            {
                if(!walkSound.isPlaying)
                {
                    walkSound.Play();
                }
                childSprite.GetComponent<Animator>().SetBool("Walking", true);
                //AudioManager.instance.PlaySound(walkSound);
            }
            else
            {
                if(walkSound.isPlaying)
                {
                    walkSound.Stop();
                }
                childSprite.GetComponent<Animator>().SetBool("Walking", false);
            }
        }
        else
        {
            if (walkSound.isPlaying)
            {
                walkSound.Stop();
            }
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
                attack2 = true;
            }
            else if(attack1 && attack2)
            {
                AudioManager.instance.PlaySound(attackSound);
                childSprite.GetComponent<Animator>().SetTrigger("AxeAttack2");
                GameManager.manager.currentStamina -= attackCost;
                attack2 = false;
                //attack1 = false;
                //lastAttackTimer = 0;
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
            AudioManager.instance.PlaySound(jumpSound);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && jumps < 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumps++;
            AudioManager.instance.PlaySound(jumpSound);
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
