using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerRageAttack : MonoBehaviour
{
    //Sword
    public GameObject SwordProjectile;
    public Transform spawnPoint;
    public float force= 20f;
    public Animator anim;
    public int SwordDamage = 30;

    //Railgun
    public float range = 100f;
    public int RailDamage = 50;
    public LineRenderer laserLine;
    public LayerMask hitLayers;
    public float laserDuration = 0.05f;

    private PlayerControls controls;
    private bool canDamage = false;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Attack.performed += ctx => SwordAttack();
        controls.Player.Shoot.performed += ctx => RailGun();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    void Start()
    {
        laserLine.positionCount = 2; // Ensure it has two points

        laserLine.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.05f),
            new Keyframe(1, 0.2f)
        ); 
    }
    void SwordAttack()
    {
        anim.SetTrigger("Sword");
        canDamage = true;

        // Get player's current rotation angles
        Vector3 playerEuler = transform.rotation.eulerAngles;

        // Apply only X and Y rotation to the projectile
        Quaternion projRotation = Quaternion.Euler(playerEuler.x, playerEuler.y, 0f);

        Quaternion offset = Quaternion.Euler(55f, -90f, 0f); // Example fix for sideways mesh
        Quaternion finalRotation = projRotation * offset;

        // Spawn projectile with the custom rotation
        GameObject proj = Instantiate(SwordProjectile, spawnPoint.position, finalRotation);

        // Apply forward force from spawnPoint
        Rigidbody projRb = proj.GetComponent<Rigidbody>();
        if (projRb != null)
        {
            projRb.velocity = proj.transform.right * force;
        }

        Destroy(proj, 5f);
    }





    void RailGun()
    {
        RaycastHit hit;
        Vector3 fireDirection = spawnPoint.forward;

        // Perform the raycast
        if (Physics.Raycast(spawnPoint.position, fireDirection, out hit, range, hitLayers))
        {
            // Try to damage an enemy
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(RailDamage);
            }

            // Always stop laser at hit point, even if it's not an enemy
            StartCoroutine(ShowLaser(spawnPoint.position, hit.point));
        }
        else
        {
            // No hit: laser goes full range
            StartCoroutine(ShowLaser(spawnPoint.position, spawnPoint.position + fireDirection * range));
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(SwordDamage);
            }
        }
    }
    System.Collections.IEnumerator ShowLaser(Vector3 start, Vector3 end)
    {
        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);
        laserLine.enabled = true;

        yield return new WaitForSeconds(laserDuration);

        laserLine.enabled = false;
    }

}
