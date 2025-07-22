using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject Sword;
    public Transform spwanPoint;
    public int damage = 25;
    public bool comboActive;

    private bool canDamage=false;

    public float arrowForce = 5f;
    private float temp;

    private PlayerControls controls;

    public float comboRestTime = 1.5f;
    public int leftClickCount;
    private float lastClickTime;
    

    public Animator anim;

    public int combo;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Attack.performed += ctx => SwordAttack();
        controls.Player.Shoot.performed += ctx => ShootAttack();
    }

    private void Start()
    {
        temp = arrowForce;
       
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    

    void SwordAttack()
    {
        anim.SetTrigger("Sword");
        canDamage = true;
        // Reset clicks if too much time passed
        if (Time.time - lastClickTime > comboRestTime)
        {
            leftClickCount = 0;
            comboActive = false;
        }

        leftClickCount++;
        lastClickTime = Time.time;

        Debug.Log("Sword Attack Count: " + leftClickCount);

        // Combo only activates on exactly 3 clicks
        if (leftClickCount == 3)
        {
            comboActive = true;
            Debug.Log("Combo Ready!");
        }
        else if (leftClickCount > 3)
        {
            comboActive = false;
            Debug.Log("Too many clicks — combo cancelled");
        }
    }

    void ShootAttack()
    {
        float currentArrowForce = temp;

        if (comboActive)
        {
            damage = 50;
            currentArrowForce = 50f;
            comboActive = false;
            Debug.Log("Combo Shot!");
        }

        // After shooting, reset clicks and combo state
        leftClickCount = 0;
        comboActive = false;

        Quaternion arrowRotation = Quaternion.Euler(0, 0, 90);
        GameObject arrow = Instantiate(Arrow, spwanPoint.position, arrowRotation);

        PlayerProjectile PLP = arrow.GetComponent<PlayerProjectile>();
        PLP.damage = damage;
       
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = spwanPoint.forward * currentArrowForce;
        Destroy(arrow, 2f);

        Debug.Log("Arrow Force: " + currentArrowForce);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

}
