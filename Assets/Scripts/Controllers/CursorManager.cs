using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCursor
{
    public enum CursorStates
    {
        FreeHand,
        Build,
        QuestEvent,

        MenuOpen
    }

    public class CursorManager : MonoBehaviour
    {
        public bool isIgnoringCreatures;
        [SerializeField]
        private CursorStates currentState;

        [SerializeField]
        private SelectionBox selectionBox;

        

        private void Awake()
        {
            
            
        }

        private void Start()
        {
            
            
        }


        public void SetCursorState(CursorStates state)
        {
            currentState = state;
        }

        public void Update()
        {
        }

        private void TestNPCClick()
        {
            Mouse mouse = Mouse.current;
            Vector3 mousePosition = new Vector3(mouse.position.x.value, mouse.position.y.value, 0);
            Vector3 realWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition) + new Vector3(0,0,10);

            RaycastHit2D hit = Physics2D.Raycast(realWorldPosition, Vector2.zero);
            if (!hit)
            {
                return;
            }
            
            ANPC npc = hit.collider.gameObject.GetComponent<ANPC>();
            if (npc == null)
            {
                return;
            }

            ServiceLocator.Instance.GetService<AdventurerManager>().ShowAdventurerProfile(npc.GetInformation());
        }

        private void OnSelect(InputValue value)
        {
            //Debug.Log("On Select");
            switch (currentState) { 
                case CursorStates.Build:
                    
                    break;
                default:
                    //TestNPCClick();
                    break;
            }
        }

        private void OnCancel(InputValue value)
        {
            switch (currentState)
            {
                case CursorStates.Build:
                    
                    break;
                default:
                    break;
            }
        }

        // Selection Box Interaction
        public void ShowSelectionBox()
        {
            selectionBox.Show();
        }

        public void SetBoxOrigin(float x, float y)
        {
            selectionBox.SetOrigin(x, y);
        }

        public void HideSelectionBox()
        {
            selectionBox.Hide();
        }
    }
}

