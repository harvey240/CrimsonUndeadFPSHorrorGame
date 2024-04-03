using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float health = 50f;
    public PlayerTest playerTest;
    public bool isPassive = false;
    private bool isAttacking = false;
    public Animator animator = null;
    public EnemyController enemyController;
    [SerializeField] private AudioClip[] damageSoundClips;
    [HideInInspector]
    public bool dead = false;
    private Transform startingTransform;
    private UnityEngine.Object explosionRef;

    void Awake()
    {
        playerTest = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTest>();
    }

    void Start()
    {
        startingTransform = transform;
        animator = GetComponentInChildren<Animator>();
        explosionRef = Resources.Load("EnemyExplode");
    }

    public void TakeDamage(float amount)
    {
        if (!dead)
        {
            if (!isPassive)
            {
                SoundFXManager.instance.PlayRandomSoundFXClip(damageSoundClips, transform, 1f);
                enemyController.isPursuing = true;
            }
            
            health -= amount;
            if (health <= 0f)
            {
                Die();
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player") && !isAttacking && !isPassive && !dead && !playerTest.isDead)
        {
            // playerTest.TakeDamage(10);
            StartCoroutine(attack());
        }
    }

    IEnumerator attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        // playerTest.TakeDamage(20);
        yield return new WaitForSeconds(2.4f);
        isAttacking = false;
    }

    void Die()
    {
        if (!isPassive) StudyMetricManager.instance.killCounter++;

        dead = true;
        StopCoroutine(attack());

        if (!isPassive)
        {
            EnemyManager.instance.enemyKilled();

            GameObject explosion = (GameObject) Instantiate(explosionRef);
            explosion.transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z);
            animator.SetTrigger("Dead");
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(WaitToDestroy());
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void Respawn()
    {
        Debug.Log("in respawn method");
        // StopCoroutine(WaitToDestroy());
        // gameObject.SetActive(true);
        health = 50f;
        dead = false;

        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        // gameObject.GetComponent<NavMeshAgent>().speed = 2f;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;


        animator.SetTrigger("Alive");

        // CODE FOUND ONLINE AND NEED TO TEST
        // animator.Rebind();
        // animator.Update(0f);    

        // gameObject.GetComponent<NavMeshAgent>().enabled = true;
        // gameObject.GetComponent<EnemyController>().enabled = true;

    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(10f);
        transform.position = startingTransform.position;
        transform.rotation = startingTransform.rotation;
        // Destroy(gameObject);
        Debug.Log("about to deactivate");
        gameObject.SetActive(false);
    }
}
