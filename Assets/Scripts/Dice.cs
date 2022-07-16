using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Renderer))]
public class Dice : MonoBehaviour
{
    public GameObject damagePrefab;
    
    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    private Camera _mainCamera;

    private DiceConveyorBelt _conveyorBelt;
    private bool _shouldFollowCursor;
    private bool _forceSelectorActive;

    private Vector3 _lastPos;
    private bool _didFireAfterRoll = true;
    private float _didFireTimeout;


    public void AddToConveyorBelt(DiceConveyorBelt conveyorBelt)
    {
        _conveyorBelt = conveyorBelt;
    }
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _mainCamera = Camera.main;

        _lastPos = transform.position;
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
            var value = GetCurrentValue();
            var dmgZone = Instantiate(damagePrefab, transform.position, quaternion.identity);

            dmgZone.GetComponent<DamageZone>().dmg = value;
            Destroy(gameObject);
        }
        
        _lastPos = transform.position;
    }

    private void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f);
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
            var pos = transform.position;
            _lineRenderer.enabled = true;
            _lineRenderer.SetPositions(new []{pos, pos + direction});
            transform.position = position;
        }
        
        if (_forceSelectorActive && Input.GetButtonUp("Fire1"))
        {
            _forceSelectorActive = false;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(Vector3.up * 160f);
            _rigidbody.AddForce(-direction * 100f);
            _rigidbody.AddTorque(Random.insideUnitSphere.normalized * 4);
            _lineRenderer.enabled = false;
            _didFireTimeout = Time.time + 0.3f;
            return;
        }
        
        if (_shouldFollowCursor && Input.GetButtonDown("Fire1"))
        {
            _forceSelectorActive = true;
            _shouldFollowCursor = false;
            return;
        }

        if (!Input.GetButtonUp("Fire1")) return;
        
        if (!Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Dice"))) return;
        if (hit.transform != transform) return;
        _shouldFollowCursor = true;
        _conveyorBelt.RemoveDiceFromBelt(transform);
    }

    private int GetCurrentValue()
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
            testHitVector = HitVector(side);
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

        return value;
    }

    private Vector3 HitVector(int side)
    {
        switch (side)
        {
            case 1: return new Vector3(0F, 0F, -1F);
            case 2: return new Vector3(-1F, 0F, 0F);
            case 3: return new Vector3(0F, 1F, 0F);
            case 4: return new Vector3(1F, 0F, 0F);
            case 5: return new Vector3(0F, -1F, 0F);
            case 6: return new Vector3(0F, 0F, 1F);
        }
        return Vector3.zero;
    }
}
