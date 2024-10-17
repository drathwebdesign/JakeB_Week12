using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed =5f;
    public Vector2 inputDirection,lookDirection;
    Animator anim;

    private Vector3 touchStart, touchEnd;
    public Image dPad;
    public float dPadRadius = 15;

    // Health variables
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthSlider;

    Touch theTouch;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //makes the character look down by default
        lookDirection = new Vector2(0, -1);

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
#if UNITY_EDITOR
        calculateDesktopInputs();
#elif UNITY_ANDROID || UNITY_WEBGL
        calculateMobileInput();
#elif UNITY_IOS
        calculateTouchInputs();
#endif

        //sets up the animator
        animationSetup();

        //moves the player
        transform.Translate(inputDirection * moveSpeed * Time.deltaTime);
    }

    void calculateDesktopInputs()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        inputDirection = new Vector2(x, y).normalized;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            attack();
        }
    }

    void calculateMobileInput() {
        Vector2 touchPosition = Input.mousePosition;

        float regionWidth = Screen.width * 0.25f;
        float regionHeight = Screen.height * 0.25f;

        if (touchPosition.x <= regionWidth && touchPosition.y <= regionHeight) {

            if (Input.GetMouseButton(0)) {
                dPad.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0)) {
                    touchStart = Input.mousePosition;
                }

                touchEnd = Input.mousePosition;

                float x = touchEnd.x - touchStart.x;
                float y = touchEnd.y - touchStart.y;

                inputDirection = new Vector2(x, y).normalized;

                if ((touchEnd - touchStart).magnitude > dPadRadius) {
                    dPad.transform.position = touchStart + (touchEnd - touchStart).normalized * dPadRadius;
                }
            } else {
                inputDirection = Vector2.zero;
                dPad.gameObject.SetActive(false);
            }
        }
    }

    void calculateTouchInputs() {
        if (Input.touchCount > 0) {
            Touch theTouch = Input.GetTouch(0);
            dPad.gameObject.SetActive(true);

            if (theTouch.phase == TouchPhase.Began) {
                touchStart = theTouch.position;
            } else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended) {
                touchEnd = theTouch.position;

                float x = touchEnd.x - touchStart.x;
                float y = touchEnd.y - touchStart.y;

                inputDirection = new Vector2(x, y).normalized;

                if ((touchEnd - touchStart).magnitude > dPadRadius) {
                    dPad.transform.position = touchStart + (touchEnd - touchStart).normalized * dPadRadius;
                } else {
                    dPad.transform.position = touchEnd;
                }
            } else {
                inputDirection = Vector2.zero;
                dPad.gameObject.SetActive(false);
            }
        } else {
            inputDirection = Vector2.zero;
            dPad.gameObject.SetActive(false);
        }
    }



    void animationSetup()
    {
        //checking if the player wants to move the character or not
        if (inputDirection.magnitude > 0.1f)
        {
            //changes look direction only when the player is moving, so that we remember the last direction the player was moving in
            lookDirection = inputDirection;

            //sets "isWalking" true. this triggers the walking blend tree
            anim.SetBool("isWalking", true);
        }
        else
        {
            // sets "isWalking" false. this triggers the idle blend tree
            anim.SetBool("isWalking", false);

        }

        //sets the values for input and lookdirection. this determines what animation to play in a blend tree
        anim.SetFloat("inputX", lookDirection.x);
        anim.SetFloat("inputY", lookDirection.y);
        anim.SetFloat("lookX", lookDirection.x);
        anim.SetFloat("lookY", lookDirection.y);
    }

    public void attack()
    {
        anim.SetTrigger("Attack");
    }

    public IEnumerator ScaleIncrease(float multiplier, float duration) {
        Vector3 originalScale = transform.localScale;

        transform.localScale *= multiplier;

        yield return new WaitForSeconds(duration);

        transform.localScale = originalScale;
    }

    public IEnumerator SpeedIncrease(float multiplier, float duration) {
        float originalSpeed = moveSpeed;

        moveSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            TakeDamage(20f);
        }
    }

    void TakeDamage(float damage) {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0) {
            Debug.Log("Player Died!");
        }
    }

    public void RestoreHealth(float amount) {
        currentHealth += amount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        healthSlider.value = currentHealth;
    }
}