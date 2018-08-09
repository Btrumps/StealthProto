using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour {

    public float speed = 5;
    public float waitTime = 0.3f;
    public float turnSpeed = 90.0f;

    Color startingSpotlightColor;

    public Transform pathHolder;
    Transform player;

    public Light spotlight;
    public float viewDistance;
    float viewAngle;

    private void Start() {
        startingSpotlightColor = spotlight.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++) {
            waypoints[i] = pathHolder.GetChild(i).position;

            // We need to snap the waypoints to the guard's height, or it will be sunken into the ground
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    bool CanSeePlayer() {
        if (Vector3.Distance(transform.position, player.position) < viewDistance) {
            Vector3 dirToPlayer = player.transform.position - transform.position;

            // Gives an angle between the direction the guard is currently facing, and the direction of the player
            float angRange = Quaternion.Angle(Quaternion.LookRotation(dirToPlayer), transform.rotation);
            if (angRange < 40f) {
                return true;
            }
        }

        return false;
    }

    private void Update() {
        if (CanSeePlayer()) {
            spotlight.color = Color.red;
            GameManager.instance.GameOver();
        } else {
            spotlight.color = startingSpotlightColor;
        }
    }

    IEnumerator FollowPath(Vector3[] waypoints) {
        
        transform.position = waypoints[0]; // Position the guard at the first waypoint

        int targetWaypointIndex = 1; // Index of the guard's next waypoint
        Vector3 targetWaypoint = waypoints[targetWaypointIndex]; // Position of the next waypoint
        transform.LookAt(targetWaypoint); // Starts the guard looking at the first waypoint

        while (true) {

            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

            if (transform.position == targetWaypoint) {
                // Once the guard gets to it's target waypoint, increase the number by one.
                // The modulus is a handy trick to get the targetWaypointIndex to go back to zero
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }

            // Yield for 1 frame between each iteration of the while loop
            yield return null; // Without this, this would all happen in the same frame

        } // end of while loop
    } // end of follow path

    IEnumerator TurnToFace(Vector3 lookTarget) {
        /* This accomplishes the same effect, but without Quaternions, susceptible to gimble lock, and harsh on the eyes
        // Gives us a normalized direction to the target waypoint
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;

        // Z is considered the Y value, as we are using "Top Down" coordinates
        // 90 - is "phase shifting the value back", not sure why though
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        // While we are not within 0.05 degrees of the target, keep rotating towards the desired angle
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05) {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
        */

        Vector3 directionToLookTarget = lookTarget - transform.position;
        Quaternion angleToTarget = Quaternion.LookRotation(directionToLookTarget);

        // This gives us the angle between where the guard is currently looking, and the angle we want to look at
        float angBetween = Quaternion.Angle(transform.rotation, angleToTarget);

        while (angBetween > 3.0f) {
            // We could also say transform.rotation = angBetween to snap, but this has us slowly turning towards target
            transform.rotation = Quaternion.RotateTowards(transform.rotation, angleToTarget, turnSpeed * Time.deltaTime);

            angBetween = Quaternion.Angle(transform.rotation, angleToTarget);

            yield return null;
        }
    }

    private void OnDrawGizmos() {

        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder) {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }

        Gizmos.DrawLine(previousPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance); // draws a line to the end of his viewDistance
    }
}
