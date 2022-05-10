using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] Waypoints;
    public int currentWaypointGoal;
    public Animator m_Animator;
    public float timeForOneMove;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
        Waypoints = GameObject.FindGameObjectsWithTag("Chicken Waypoints");
        Debug.Log("Waypoints = " + Waypoints.Length.ToString());
    }
    
    // Update is called once per frame
    void Update()
    {
        if(agent.hasPath == false)
        {
            timeForOneMove = 0;
            currentWaypointGoal = Random.Range(0, Waypoints.Length);
            agent.SetDestination(Waypoints[currentWaypointGoal].transform.position);
            //Debug.Log("Moving to Waypoint = " + currentWaypointGoal.ToString());
            
        }
        else
        {
            m_Animator.SetBool("Run", true);
            timeForOneMove += Time.deltaTime;
            if(timeForOneMove > 5)
            {
                currentWaypointGoal = Random.Range(0, Waypoints.Length);
                agent.SetDestination(Waypoints[currentWaypointGoal].transform.position);
                //Debug.Log("Stuck, Moving to new Waypoint = " + currentWaypointGoal.ToString());
            }
        }

    }
}
