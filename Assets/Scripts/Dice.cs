using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Renderer))]
public abstract class Dice : MonoBehaviour
{
    public GameObject damagePrefab;
    public AudioController audioController;

    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    private Camera _mainCamera;
    
    private DiceConveyorBelt _conveyorBelt;
    private bool _shouldFollowCursor;
    private bool _forceSelectorActive;

    private Vector3 _lastPos;
    private bool _didFireAfterRoll = true;
    private float _didFireTimeout;

    public void AddDiceToBelt(DiceConveyorBelt conveyorBelt)
    {
        _conveyorBelt = conveyorBelt;
    }

    public void CollectDice(Shop shop)
    {
        shop.AddMoney(GetCurrentValue());
        _conveyorBelt.RemoveDiceFromBelt(transform);
        Destroy(gameObject);
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _mainCamera = Camera.main;

        _lastPos = transform.position;
        _rigidbody.isKinematic = false;
    }

    private void FixedUpdate()
    {
        if (_didFireTimeout <= Time.time && _didFireTimeout != 0)
        {
            _didFireAfterRoll = false;
            _didFireTimeout = 0;
        }

        if (_lastPos == transform.position && !_didFireAfterRoll)
        {
            _didFireAfterRoll = true;
            OnExplode();
        }
        
        _lastPos = transform.position;
    }

    private void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 direction = new Vector3();
        Vector3 position = new Vector3();
        new Plane(Vector3.up, transform.position).Raycast(ray, out float enterDistance);
        if (enterDistance != 0)
        {
            position = ray.origin + ray.direction.normalized * enterDistance;
            direction = position - transform.position;
        }
        
        if (_shouldFollowCursor)
        {
            _rigidbody.isKinematic = true;
            transform.position = position;
        }

        if (_forceSelectorActive)
        {
            var pos = transform.position;
            _lineRenderer.SetPositions(new []{pos, pos + direction});
        }
        
        if (_forceSelectorActive && Input.GetButtonUp("Fire1"))
        {
            AllowExplosion();
            _rigidbody.AddForce(Vector3.up * direction.magnitude / 10, ForceMode.VelocityChange );
            _rigidbody.AddForce(-direction, ForceMode.VelocityChange);
            _rigidbody.AddTorque(Random.insideUnitSphere.normalized * (direction.magnitude * 8));
            return;
        }
        
        if (_shouldFollowCursor && Input.GetButtonDown("Fire1"))
        {
            _forceSelectorActive = true;
            _shouldFollowCursor = false;
            _lineRenderer.enabled = true;
            return;
        }

        if (!Input.GetButtonUp("Fire1")) return;
        
        if (!Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Dice"))) return;
        if (hit.transform != transform) return;
        _shouldFollowCursor = true;
        _conveyorBelt.RemoveDiceFromBelt(transform);
    }

    public void AllowExplosion()
    {
        _shouldFollowCursor = false;
        _forceSelectorActive = false;
        _rigidbody.isKinematic = false;
        _lineRenderer.enabled = false;
        _didFireTimeout = Time.time + 0.3f;
    }

    public virtual void OnExplode()
    {
        var value = GetCurrentValue();
        var dmgZone = Instantiate(damagePrefab, transform.position, quaternion.identity);

        dmgZone.GetComponent<DamageZone>().dmg = value;
        audioController.PlayExplosionSound();
        Destroy(gameObject);
    }

    public int GetCurrentValue()
    {
        Vector3 localHitNormalized;
        var t = transform;
        Ray ray = new Ray( t.position + (new Vector3(0, 2, 0) * t.localScale.magnitude), Vector3.up * -1);

        if (GetComponent<Collider>().Raycast(ray, out var hit, 3 * transform.localScale.magnitude))
        {
            localHitNormalized = transform.InverseTransformPoint(hit.point.x, hit.point.y, hit.point.z).normalized;
        }
        else
        {
            return 0;
        }
        
        var value = 0;
        float delta = 1;
        int side = 1;
        Vector3 testHitVector;
        do
        {
            testHitVector = GetVectorForSide(side);
            if (testHitVector != Vector3.zero)
            {
                float nDelta = Mathf.Abs(localHitNormalized.x - testHitVector.x) + Mathf.Abs(localHitNormalized.y - testHitVector.y) + Mathf.Abs(localHitNormalized.z - testHitVector.z);
                if (nDelta < delta)
                {
                    value = side;
                    delta = nDelta;
                }
            }
            side++;
            // if we got a Vector.zero as the testHitVector we have checked all sides of this die
        } while (testHitVector != Vector3.zero);

        // Debug.Log(value);
        
        return value;
    }

    public abstract Vector3 GetVectorForSide(int side);
    public abstract int GetMaxValue();
}
