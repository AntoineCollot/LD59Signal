using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    Plane floorPlane;
    public float rawAimDist {  get; private set; }
    public float AimDist01 => Mathf.InverseLerp(minDist, maxDist, rawAimDist);
    public Vector3 mousePoint {  get; private set; }

    [SerializeField] float minDist;
    [SerializeField] float maxDist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        floorPlane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!floorPlane.Raycast(camRay, out float dist))
        {
            Debug.Log("No intersection between cam ray and floor plane");
            return;
        }

        mousePoint = camRay.origin + camRay.direction * dist;
        Debug.DrawLine(transform.position, mousePoint, Color.red);

        rawAimDist = Vector3.Distance(transform.position, mousePoint);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDist);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxDist);
    }
#endif
}
