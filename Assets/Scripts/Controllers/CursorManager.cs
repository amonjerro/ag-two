using Rooms;
using Unity.VisualScripting;
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
        private CameraMovement _camera;
        public bool isIgnoringCreatures;
        [SerializeField]
        private CursorStates currentState;

        [SerializeField]
        private SelectionBox selectionBox;

        RoomManager roomManagerRef;

        private void Awake()
        {
            Camera cam = Camera.main;
            _camera = cam.GetComponent<CameraMovement>();
        }

        private void Start()
        {
            roomManagerRef = ServiceLocator.Instance.GetService<RoomManager>();
        }


        public void SetCursorState(CursorStates state)
        {
            currentState = state;
        }

        public void Update()
        {
        }

        private void TestRoomHover()
        {
            Mouse mouse = Mouse.current;
            Vector3 mousePosition = new Vector3(mouse.position.x.value, mouse.position.y.value, 0);
            Vector3 realWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition) + new Vector3(0,0,10);
            int positionX = Mathf.FloorToInt(realWorldPosition.x);
            int positionY = Mathf.FloorToInt(realWorldPosition.y);

            if (roomManagerRef.IsRoomHovered(positionX, positionY)) {
                AbsRoomClickEvent roomClickEvent = roomManagerRef.HandleClick(positionX, positionY);
                switch (roomClickEvent.type)
                {
                    case RoomClickEventTypes.MenuOpen:
                        // Create logic for UI Manager opening up the specific panel
                        Debug.Log("This should be opening a menu and it doesn't");
                        break;
                    default:
                        MainSceneTransitionManager transitionManager = ServiceLocator.Instance.GetService<MainSceneTransitionManager>();
                        transitionManager.FadeToScene(roomClickEvent.GetValue());
                        break;
                }
            }
        }

        private void OnSelect(InputValue value)
        {
            switch (currentState) { 
                case CursorStates.Build:
                    
                    break;
                default:
                    TestRoomHover();
                    break;
            }
        }

        private void OnMove(InputValue value)
        {
            _camera.Move(value.Get<Vector2>());
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

