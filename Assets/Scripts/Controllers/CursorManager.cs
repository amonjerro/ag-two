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
                        (int, int) snappedPositions = roomManagerRef.WorldGridSnap(positionX, positionY);
                        Debug.Log($"{snappedPositions.Item1},{snappedPositions.Item2}");
                        _camera.FocusOn(new Vector3(snappedPositions.Item1, snappedPositions.Item2,-10));
                        UIManager manager = ServiceLocator.Instance.GetService<UIManager>();
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
    }
}

