using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    public class WaypointContainer : MonoBehaviour
    {

        
        private void OnDrawGizmos()
        {
            Vector3 firstPosition = transform.GetChild(0).position;
            Vector3 previousPosition = firstPosition;
            foreach (Transform waypoint in transform)
            {
                Gizmos.color = new Color(255f, 255f, 0, .5f);
                Gizmos.DrawWireSphere(waypoint.position, .2f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
            Gizmos.DrawLine(previousPosition, firstPosition);




        }
    }
}
