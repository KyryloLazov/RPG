using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Controller {
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), 0.5f);

                int j = GetNextWaypoint(i);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextWaypoint(int i)
        {
            int j = i + 1;
            if(j < transform.childCount)
            {
                return j;
            }
            else
            {
                return 0;
            }
        }
    }
}
