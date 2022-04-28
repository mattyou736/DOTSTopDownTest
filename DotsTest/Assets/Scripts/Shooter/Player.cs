using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

public class Player : MonoBehaviour
{

    [SerializeField] 
    Camera sceneCamera;
    [SerializeField]
    float speed = 3f;
    [Range(0.05f, 0.3f)][SerializeField] 
    float turnSpeed = 0.1f;

    private Rigidbody playerRigidbody;

    // where the weapon's bullet appears
    [SerializeField]
    Transform bulletSpawn;
    [SerializeField]
    GameObject bulletPrefab;

    // time between shots
    [SerializeField]
    float rateOfFire = 0.15f;
    // timer until weapon and shoot again
    float shotTimer;
    
    EntityManager entityManager;
    // prefab converted into an entityPrefab
    Entity bulletEntityPrefab;

    // layer mask to detect mouse position
    public LayerMask groundLayerMask;


    // return input vector in camera space
    public Vector3 GetCameraSpaceInputDirection(Camera cam)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // xz-vector from input values
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

        if (cam == null)
        {
            return inputDirection;
        }

        // multiply input by cam axes to convert to cam space
        Vector3 cameraRight = cam.transform.right;
        Vector3 cameraForward = cam.transform.forward;

        return cameraRight * inputDirection.x + cameraForward * inputDirection.z;
    }

    public void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // get reference to current EntityManager
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // create entity prefab from the game object prefab, using default conversion settings
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        bulletEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
    }

    protected virtual void Update()
    {
        // count up to the next time we can shoot
        shotTimer += Time.deltaTime;
        if (shotTimer >= rateOfFire && Input.GetButton("Fire1"))
        {
            // fire and reset the timer
            FireBullet();
            shotTimer = 0f;
        }
    }

    void FixedUpdate()
    {

        // get the keyboard input converted to camera space
        Vector3 input = GetCameraSpaceInputDirection(sceneCamera);

        // use input to move the player
        MovePlayer(input);

        // aim the turret to the mouse position
        AimAtMousePosition(sceneCamera);
    }
    
    #region Movement/rotation
    public void MovePlayer(Vector3 direction)
    {
        Vector3 moveDir = new Vector3(direction.x, 0f, direction.z);
        moveDir = moveDir.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + moveDir);
    }

    private Quaternion GetRotationToTarget(Transform xform, Vector3 targetPosition)
    {
        // get a normalized vector to the target on the xz-plane
        Vector3 direction = targetPosition - xform.position;
        direction.y = 0f;
        direction.Normalize();

        // convert Vector3 to Quaternion and return
        return Quaternion.LookRotation(direction);
    }

    // returns correction rotation to face the mouse pointer (using y rotation)
    private Quaternion GetRotationToMouse(Transform xform, Camera cam)
    {
        if (cam == null)
        {
            Debug.Log("PLAYERMOVER GetRotationToMouse: no camera");
            return xform.rotation;
        }

        // use Raycast and GroundLayer mask to calculate mouse position in world space
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, groundLayerMask))
        {
            return GetRotationToTarget(xform, hit.point);
        }

        return xform.rotation;
    }

    // turn the player/turret to face the mouse position
    public void AimAtMousePosition(Camera cam)
    {
        if (cam != null)
        {
            transform.rotation = GetRotationToMouse(transform, cam);
        }
    }
    #endregion


    public virtual void FireBullet()
    {
        // create an entity based on the entity prefab
        Entity bullet = entityManager.Instantiate(bulletEntityPrefab);

        // set it to the muzzle angle and position
        entityManager.SetComponentData(bullet, new Translation { Value = bulletSpawn.position });
        entityManager.SetComponentData(bullet, new Rotation { Value = bulletSpawn.rotation });
    }

   
}
