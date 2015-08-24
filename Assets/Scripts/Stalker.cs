using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;

public class Stalker : MonoBehaviour
{

    [SerializeField] private Transform _target;
    private Camera _cam;

    void Awake()
    {
        _cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        _cam.orthographicSize = (Screen.height/125f)/6f;
        var goal = _target.position;
        goal.x += 0.1f;
        goal.z = -10;
        transform.position = Vector3.Lerp(transform.position, goal, 0.09f);
    }

    void ChangeTarget(Transform newTarget)
    {
        _target = newTarget;
    }
}
