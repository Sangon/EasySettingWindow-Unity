using UnityEngine;
public struct CameraEventArgs {
    public Vector3 currentRotation;
    public Vector3 currentPosition;

    public float currentZoomLevel;

    public bool beingDragged;

}
public class CameraController : MonoBehaviour {

    public delegate void CameraEvent(CameraEventArgs e);

    public CameraEvent CameraRotating;
    public CameraEvent CameraMoving;
    public CameraEvent CameraZoomLevelChanged;
    public CameraEvent CameraMoveStopped;
    public CameraEvent CameraMoveStarted;

    private Transform m_cameraPosition;
    private Transform m_pivotPosition;

    private Vector3 m_cameraOffset;
    private Vector3 m_localRotation;

    private float m_cameraDistance = -7.241397f;

    [SerializeField]
    [SettingWindowItem("Minimum distance")]
    private float m_cameraMinDistance = 1.5f;

    [SerializeField]
    [SettingWindowItem("Maximum distance")]
    private float m_cameraMaxDistance = 100f;


    [SerializeField]
    private float m_cameraMovementSpeedModifier = 1.8f;

    [SerializeField]
    public float rotationSensitivity = 4f;

    [SerializeField]
    [SettingWindowItem("Zoom sensitivity")]
    public float scrollSensitvity = 2f;

    [SerializeField]
    [SettingWindowItem("Orbit dampening")]
    public float orbitDampening = 10f;

    [SerializeField]
    [SettingWindowItem("Zoom dampening")]
    public float scrollDampening = 6f;

    public bool wasDragged = false;

    [SettingWindowItem("Rotation lock")]
    private bool m_rotationLocked;
    [SettingWindowItem("Movement lock")]
    private bool m_movementLocked;
    [SettingWindowItem("Zooming lock")]
    private bool m_zoomLocked;

    public float ZoomLevel {
        get {
            return m_cameraDistance;
        }

        set {
            m_cameraDistance = Mathf.Clamp(value, m_cameraMinDistance, m_cameraMaxDistance);
        }
    }

    public float ZoomLevelSmooth {
        get {
            return -m_cameraPosition.localPosition.z;
        }
    }

    public Vector3 CameraTarget {
        get; private set;
    }

    public Transform FocusedObject {
        get; private set;
    }

    public Vector3 CameraOffset {
        get;set;
    }

    private void Start() {

        m_cameraPosition = transform.Find("CameraOffset").Find("OrbitCamera");
        m_pivotPosition = transform;

        //Initialize the local values so we can set the initial values in the editor.
        m_localRotation.x = m_pivotPosition.rotation.eulerAngles.y;
        m_localRotation.y = m_pivotPosition.rotation.eulerAngles.x;

        ZoomLevel = m_cameraPosition.localPosition.z * -1f;
        CameraOffset = Vector3.zero;
        CameraTarget = m_pivotPosition.position;

        //Add the object to the setting window handler so it knows to add our settings.
        SettingWindowHandler.Instance.AddBlock(this, "Camera settings");
    }




    private void FixedUpdate() {

        if (FocusedObject) {
            CameraTarget = FocusedObject.position;
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(m_localRotation.y, m_localRotation.x, 0);
        m_pivotPosition.rotation = Quaternion.Lerp(m_pivotPosition.rotation, QT, Time.deltaTime * orbitDampening);

        //Handle zooming.
        if (m_cameraPosition.localPosition.z != ZoomLevel * -1f) {
            m_cameraPosition.localPosition = new Vector3(0f, 0f, Mathf.Lerp(m_cameraPosition.localPosition.z, ZoomLevel * -1f, .1f));//Time.deltaTime * scrollDampening));
        }

        //Handle camera offset.
        if (!Mathfx.Approx(m_cameraPosition.parent.localPosition, CameraOffset, 0.05f)) {
            m_cameraPosition.parent.localPosition = Mathfx.Hermite(m_cameraPosition.parent.localPosition, CameraOffset, .2f);
        } else {
            m_cameraPosition.parent.localPosition = CameraOffset;
        }

        //Handle camera movement.
        if (!Mathfx.Approx(m_pivotPosition.position, CameraTarget, 0.05f)) {
            m_pivotPosition.position = Mathfx.Hermite(m_pivotPosition.position, CameraTarget, .2f);
        } else {
            m_pivotPosition.position = CameraTarget;
        }

    }

    private void LateUpdate() {

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {

            if (wasDragged) {
                wasDragged = false;
            }

        }

        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {

            //Zooming Input from our Mouse Scroll Wheel
            if (Input.GetAxis("Mouse ScrollWheel") != 0f && !m_zoomLocked) {
                ZoomLevel -= (ZoomLevel * 0.3f) * Input.GetAxis("Mouse ScrollWheel") * scrollSensitvity;
            }

            float mouseXDelta = Input.GetAxis("Mouse X");
            float mouseYDelta = Input.GetAxis("Mouse Y");

            if (Input.GetMouseButton(1) && !m_rotationLocked) {
                //Rotation of the Camera based on Mouse Coordinates
                if (mouseXDelta != 0 || mouseYDelta != 0) {
                    wasDragged = true;
                    m_localRotation.x += mouseXDelta * rotationSensitivity;
                    m_localRotation.y -= mouseYDelta * rotationSensitivity;

                    if (CameraRotating != null) {
                        var p = GetCameraState();
                        CameraRotating(p);
                    }

                    //Clamp y rotation
                    if (m_localRotation.y < 5f) {
                        m_localRotation.y = 5f;
                    } else if (m_localRotation.y > 85f) {
                        m_localRotation.y = 85f;
                    }
                }

            } else if (Input.GetMouseButton(0) && !m_movementLocked) {

                if (mouseXDelta != 0 || mouseYDelta != 0) {

                    if (!wasDragged) {
                        if (CameraMoveStarted != null) {
                            var p = GetCameraState();
                            CameraMoveStarted(p);
                        }
                    }

                    wasDragged = true;
                    TranslateCameraTarget(mouseXDelta, mouseYDelta);
                }

            }

        }
    }


    public void Lock(bool movement, bool rotation, bool zoom) {

        m_movementLocked = movement;
        m_rotationLocked = rotation;
        m_zoomLocked = zoom;
        print("CAMERA LOCKED - Rotation:(" + rotation + ") Movement: (" + movement + ") Zoom: (" + zoom + ") ");
    }

    public void RotateCameraY(float y) {
        m_localRotation.x += y;
    }

    public void Focus(Transform target) {
        FocusedObject = target;
        CameraTarget = FocusedObject.position;
    }

    public void DeFocus(){
        FocusedObject = null;
    }

    public void TranslateCameraTarget(float x, float y) {

        float c_cos = Mathf.Cos(Mathf.Deg2Rad * m_pivotPosition.rotation.eulerAngles.y);
        float c_sin = Mathf.Sin(Mathf.Deg2Rad * m_pivotPosition.rotation.eulerAngles.y);

        float c_speed = (ZoomLevel / m_cameraMaxDistance) * m_cameraMovementSpeedModifier;
        CameraTarget += new Vector3(-y * c_sin, 0, -y * c_cos) * c_speed;
        CameraTarget += new Vector3(-x * c_cos, 0, x * c_sin) * c_speed;

    }

    public void TranslateCameraTarget(Vector3 target) {
        TranslateCameraTarget(target.x, target.z);
    }

    private CameraEventArgs GetCameraState() {
        var p = new CameraEventArgs();

        p.beingDragged = wasDragged;
        p.currentPosition = m_cameraPosition.position;
        p.currentRotation = m_localRotation;

        return p;
    }

}
