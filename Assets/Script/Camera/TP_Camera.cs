
namespace Hua_yEnA.Camera
{
    using GGG.Tool;
    using UnityEngine;

    public class TP_Camera : MonoBehaviour
    {

        private Transform curCamera;
        [SerializeField] private GameObject lookTarget;

        //----------------------  旋转  ----------------------\\
        [SerializeField] private Vector2 clampCameraRange = new Vector2(-60f, 70f);
        [Range(0.1f, 1.0f), SerializeField, Header("鼠标灵敏度")] public float mouseInputSpeed = 0.1f;
        private Vector3 _camDirection;
        private float _cameraDistance;//主相机的本地位置
        private float yaw;
        private float pitch;
        [SerializeField] private float rotationLerpTime = 0.1f;//旋转参数，用于控制旋转过渡
        private Vector3 rotationSmoothVelocity;//无用变量，用于记录旋转速度
        private Vector3 currentRotation;//用于记录整体的旋转量



        [SerializeField, Header("相机碰撞")] private Vector2 _cameraDistanceMinMax = new Vector2(0.01f, 3f);//相机最近和最短的距离，用于相机碰撞检测
        [SerializeField] private float lookAtPlayerLerpTime = 10;
        public LayerMask collisionLayer;//相机可碰撞的层级
        private float cameraPositionOffset = 0.02f;//相机距离玩家位置，相机父物体固定的距离
        [SerializeField] private float colliderMotionLerpTime = 50f;//碰撞后的相机过渡时间
        [SerializeField] private float _detectionDistance = 3f;
        private Transform mainCamera;

        //目标锁定
        private float _maxLockDistance;
        private bool _isLock;
        [SerializeField]private Transform _lookTarget;

        public bool _isEnable;

        private void Awake()
        {
            mainCamera = Camera.main.transform;
        }
        private void OnEnable()
        {
            EventManager.MainInstance.AddEventListening<bool, Transform, float>("LockOrUnLockTarget", LockOrUnLockTarget);
        }
        private void OnDisable()
        {
            EventManager.MainInstance.RemoveEvent<bool, Transform, float>("LockOrUnLockTarget", LockOrUnLockTarget);
        }
        // Start is called before the first frame update
        void Start()
        {
            _camDirection = transform.localPosition.normalized;
            _cameraDistance = _cameraDistanceMinMax.y;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isEnable = true;
        }
        // Update is called once per frame
        void Update()
        {
            GetCameraInput();
            Debug.Log(_camDirection);
        }

        private void LateUpdate()
        {

            if (_isEnable)
            {
                UpdateCameraRotation();
                UpdateCameraPosition();
            }
            
            CheckCameraOcclusionAndCollision(mainCamera);
        }
        private void GetCameraInput()
        {
            if (CameraIsLock())
            {
                yaw = transform.eulerAngles.y;
                pitch = transform.eulerAngles.x;
                currentRotation = new Vector2(pitch, yaw);
            }
            else
            {
                yaw += GameInputManager.MainInstance.CameraLookVec.x * mouseInputSpeed;
                pitch -= GameInputManager.MainInstance.CameraLookVec.y * mouseInputSpeed;
                pitch = Mathf.Clamp(pitch, clampCameraRange.x, clampCameraRange.y);
            }
            
        }

        private bool CameraIsLock()
        {
            return _isLock && _lookTarget != null;
        }

        private void UpdateCameraRotation()
        {

            if (CameraIsLock())
            {
                if (DevelopmentToos.DistanceForTarget(transform, _lookTarget) > _maxLockDistance)
                {
                    EventManager.MainInstance.CallEvent<bool, Transform, float>("LockOrUnLockTarget", false, null, _maxLockDistance);
                    return;
                }
                //0.85f
                transform.LookAtTarget(_lookTarget.position + Vector3.up * 1f, 10f);
            }
            else
            {
                //更新相机旋转
                currentRotation = Vector3.SmoothDamp(currentRotation, new Vector2(pitch, yaw), ref rotationSmoothVelocity, rotationLerpTime);
                transform.eulerAngles = currentRotation;
            }

        }

        private void UpdateCameraPosition()
        {

            //更新相机位置
            Vector3 fanlePos = lookTarget.transform.position - transform.forward * cameraPositionOffset;

            transform.position = Vector3.Lerp(transform.position, fanlePos, lookAtPlayerLerpTime * Time.deltaTime);
        }

        /// <summary>
        /// 相机碰撞检测
        /// </summary>
        /// <param name="camera"></param>
        private void CheckCameraOcclusionAndCollision(Transform camera)
        {
            Debug.Log(transform.TransformPoint(_camDirection));
            Vector3 desiredCamPosition = transform.TransformPoint(_camDirection * _detectionDistance);

            if (Physics.Linecast(transform.position, desiredCamPosition, out var hit, collisionLayer))
            {
                _cameraDistance = Mathf.Clamp(hit.distance * .9f, _cameraDistanceMinMax.x, _cameraDistanceMinMax.y);
            }
            else
            {
                _cameraDistance = _cameraDistanceMinMax.y;

            }
            camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, _camDirection * (_cameraDistance - 0.1f), colliderMotionLerpTime * Time.deltaTime);

        }
        private void OnDrawGizmos()
        {
            Vector3 desiredCamPosition = transform.TransformPoint(_camDirection * _detectionDistance);
            Gizmos.DrawLine(transform.position, desiredCamPosition);
        }

        public void SetCameraDistance(float distance)
        {
            _cameraDistanceMinMax.y = distance;
        }


        private void LockOrUnLockTarget(bool isLock, Transform target = null,float maxLockDistance=10)
        {
            _isLock = isLock;
            _lookTarget = target;
            _maxLockDistance = maxLockDistance;
        }

    }
}

