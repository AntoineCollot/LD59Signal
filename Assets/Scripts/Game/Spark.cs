using System.Collections;
using UnityEngine;

public class Spark : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] float pickUpDistance;

    bool isBeingPickedUp;
    PlayerState player;

    const float PICK_UP_MAX_SPEED = 5f;
    const float PICK_UP_VALIDATE_DIST = 0.4f;
    const float PICK_UP_SMOOTH = 0.15f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = PlayerState.Instance;
    }

    private void OnEnable()
    {
        isBeingPickedUp = false;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        if (!isBeingPickedUp && Vector3.Distance(player.transform.position, transform.position)< pickUpDistance * PlayerPowerUps.Instance.PickUpDistanceMult)
        {
            StartCoroutine(PickUp());
        }
    }

    IEnumerator PickUp()
    {
        isBeingPickedUp = true;
        Vector3 refPos = Vector3.zero;

        while (Vector3.Distance(transform.position, player.transform.position) > PICK_UP_VALIDATE_DIST)
        {
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref refPos, PICK_UP_SMOOTH, PICK_UP_MAX_SPEED);

            yield return null;
        }

        isBeingPickedUp = false;
        gameObject.SetActive(false);
        XPManager.Instance.SparkPickedUp(this);
    }

    //IEnumerator PickUp()
    //{
    //    isBeingPickedUp = true;
    //    Vector3 startPos = transform.position;

    //    while (Vector3.Distance(transform.position, player.transform.position) > 0.5f)
    //    {

    //        transform.position = Curves.QuadEaseIn(startPos, player.transform.position + Vector3.up * 0.3f, Mathf.Clamp01(t));

    //        yield return null;
    //    }

    //    isBeingPickedUp = false;
    //    gameObject.SetActive(false);
    //}

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickUpDistance);
    }
#endif
}
