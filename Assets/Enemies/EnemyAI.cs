using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    Transform player;
    public Transform firePos;
    public GameObject projectile;
   
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;


    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    float searchNewCooldown; //for if the enemy has walked 
    //attack
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    

    //states 
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        if (agent == null ) 
        {
            Debug.LogError("NO NAVMESH FOUND");
        }

    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if(!playerInSightRange && !playerInAttackRange) 
        {
            RandomlyPatroling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }
    private void RandomlyPatroling() 
    {
        //creates new point if one does not exsist
        if (!walkPointSet) 
        {
            SearchWalkPoint();
        }
        if (walkPointSet) 
        {
            agent.SetDestination(walkPoint);
            //makes shure a new point is given after a little time 
            if (searchNewCooldown < Time.time) 
            { 
                walkPointSet = false;
            }
        }
        //checks distance from enemy to walkpoint. 
        Vector3 distanceToWalkPoint = transform.position-walkPoint;
        //if its close enouth it has reached the walk point and shuld make a new one. 
        if (distanceToWalkPoint.magnitude < 1.4f) 
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    { 
        //makes random walk point for patrolling
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //saves the new random position to variable
        walkPoint.z = transform.position.z + randomZ;
        walkPoint.y = transform.position.y;
        walkPoint.x = transform.position.x + randomX;

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) 
        {
            walkPointSet = true;
            //resets timer 
            searchNewCooldown = Time.time + 5f;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        

        if (!alreadyAttacked) 
        {
            //attack code 
            
            GameObject projectileRef = Instantiate(projectile, transform.position, Quaternion.identity);
            projectileRef.GetComponent<Rigidbody>().AddForce(transform.forward * 32f, ForceMode.Impulse);
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void ResetAttack() 
    {
        alreadyAttacked = false;   
    }
}
