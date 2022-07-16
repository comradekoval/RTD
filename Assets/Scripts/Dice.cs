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
    private Renderer _diceRenderer;

    private DiceConveyorBelt _conveyorBelt = null;
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
        _diceRenderer = gameObject.GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (_didFireTimeout <= Time.time && _didFireTimeout != 0)
        {
            _didFireAfterRoll = false;
            _didFireTimeout = 0;
        }
        
        _diceRenderer.material.color = _lastPos == transform.position ? Color.green : Color.red;
        
        if (_lastPos == transform.position && !_didFireAfterRoll)
        {
            _didFireAfterRoll = true;
            Instantiate(damagePrefab, transform.position, quaternion.identity);
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
}
