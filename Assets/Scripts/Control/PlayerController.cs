using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using RPG.Movement;
using System;
using RPG.Combat;
using RPG.Core;
using PRG.Attributes;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {             
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;

        private Mover mover;
        private Fighter fighter;
        private Health health;

        void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (InteractWithUI()) return;

            if (health.isDead)
            {
                SetCursor(CursorType.None);
                return; 
            }

            if (InteractWithComponent()) return;
            if (MoveToCursor()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                Iraycatable[] iraycatables = hit.transform.GetComponents<Iraycatable>();
                foreach(Iraycatable raycastable in iraycatables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        //RaycastHit[] RaycastAllSorder()
        //{

        //}

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.None);
                return true;
            }

            return false;
        }
       private bool MoveToCursor()
        {
            //RaycastHit hit;
            //if (Physics.Raycast(GetMouseRay(), out hit))
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);               
            if (hasHit) 
            {
                if (Input.GetMouseButton(0))
                {
                    mover.MoveTo(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
             }
            return false;
            
        }

        private bool RaycastNavMesh(out Vector3 Target)
        {
            Target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit NavHit;
            bool hasCastInNavMesh = NavMesh.SamplePosition(
                hit.point, out NavHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastInNavMesh) return false;
            Target = NavHit.position;
            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping cursor in cursorMappings)
            {
                if (cursor.type != type) continue;

                return cursor;
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
