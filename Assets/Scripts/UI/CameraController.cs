using Cinemachine;
using UnityEngine;

namespace CAMERA
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private CinemachineVirtualCamera cinemachine;

        private float OrthographicSize = 5;

        private Vector2 mousePos;

        private void Start()
        {
            TurnSystem.OnCurrentUnitEnter += TurnSystem_OnCurrentUnitEnter;
        }

        private void Update()
        {
            Move();
            Zoom();
        }

        private void Move()
        {
            Vector3 moveVector = Vector3.zero;
            if (Input.GetMouseButtonDown(2))
            {
                mousePos = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                Vector2 curMousePos = Input.mousePosition;
                Vector2 move = Camera.main.ScreenToWorldPoint(mousePos) - Camera.main.ScreenToWorldPoint(curMousePos);
                moveVector.x = move.x;
                moveVector.y = move.y;
                mousePos = curMousePos;
            }
            else
            {
                moveVector.x = Input.GetAxis("Horizontal");
                moveVector.y = Input.GetAxis("Vertical");
                moveVector *= speed * Time.deltaTime;
            }
            transform.position += moveVector;
        }

        private void Zoom()
        {
            OrthographicSize -= Input.GetAxis("Mouse ScrollWheel");
            cinemachine.m_Lens.OrthographicSize = Mathf.Lerp(cinemachine.m_Lens.OrthographicSize, OrthographicSize, 0.2f);
        }

        private void TurnSystem_OnCurrentUnitEnter(object unit)
        {
            transform.position = (unit as MonoBehaviour).transform.position;
        }
    }
}
