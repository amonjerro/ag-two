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
        QuestEvent
    }

    public class CursorManager : MonoBehaviour
    {
        public bool isIgnoringCreatures;
        [SerializeField]
        private CursorStates currentState;

        [SerializeField]
        private SelectionBox selectionBox;

        BuildStateMachine buildStateMachine;
        BuildController buildController;

        private void Awake()
        {
            buildController = new BuildController();
            
        }

        private void Start()
        {
            TileManager tileManager = ServiceLocator.Instance.GetService<TileManager>();
            buildController.SetTileManagerReference(tileManager);
            buildStateMachine = new BuildStateMachine(buildController);
        }

        private void HandleClickOnAdventurer(Adventurer adventurer)
        {
            return;
        }

        public void SetCursorState(CursorStates state)
        {
            currentState = state;
        }

        public void Update()
        {
            buildStateMachine.Update();
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
                    Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    buildStateMachine.HandleClick(mouseLocation);
                    break;
                default:
                    TestNPCClick();
                    break;
            }
        }

        private void OnMove(InputValue value)
        {
            switch (currentState) {
                case CursorStates.Build:
                    // Update the UI if necessary
                    Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    break;
                default:
                    return;
            }
        }
    }
}

