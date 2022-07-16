using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class Dice : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    private Camera _mainCamera;
    
    
    private bool _shouldFollowCursor = false;
    private bool _forceSelectorActive = false;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f);
        Vector3 direction = new Vector3();
        new Plane(Vector3.up, transform.position).Raycast(ray, out float enterDistance);
        if (enterDistance != 0)
        {
            direction = (ray.origin + ray.direction.normalized * enterDistance) - transform.position;
        }
        
        if (_forceSelectorActive)
        {
            _lineRenderer.SetPositions(new []{transform.position,transform.position + direction});
            Debug.DrawRay(transform.position, direction * 10);
        }
        
        if (_forceSelectorActive && Input.GetButtonUp("Fire1"))
        {
            _forceSelectorActive = false;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(Vector3.up * 100f);
            _rigidbody.AddForce(-direction * 100f);
            _rigidbody.AddTorque(Random.insideUnitSphere.normalized);
            _lineRenderer.enabled = false;
            return;
        }
        
        if (_shouldFollowCursor && Input.GetButtonDown("Fire1"))
        {
            _lineRenderer.enabled = true;
            _forceSelectorActive = true;
            _shouldFollowCursor = false;
            return;
        }

        if (!Input.GetButtonUp("Fire1")) return;
        
        if (!Physics.Raycast(ray, out RaycastHit hit, 100, LayerMask.GetMask("Dice"))) return;
        Debug.Log(hit.transform.name);
        if (!hit.transform.name.Equals("Dice")) return;
        _shouldFollowCursor = true;
    }
}
