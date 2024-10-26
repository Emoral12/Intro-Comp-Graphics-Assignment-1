using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float xforce;
    float zforce;
    Vector3 playerRot;
    Vector3 cameraRot;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float lookSpeed = 2;
    [SerializeField] GameObject cam;
    Rigidbody rb;
    [SerializeField] Vector3 boxSize;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float jumpForce = 3;
    [SerializeField] GameObject bullethole;
    RaycastHit hit;
    [SerializeField] float fireRate = 0.1f;
    bool canFire = true;
    AudioSource aud;
    [SerializeField] ParticleSystem gunfire;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerMovement();

        LookAround();

        if (GroundCheck() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(transform.up * jumpForce);
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 300f);
            aud.Play();
            gunfire.Play();
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            else if (hit.collider != null)
            {
                GameObject bullet = Instantiate(bullethole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            }
            canFire = false;
            Invoke("FireRateReset", fireRate);

        }

    }

    void FireRateReset()
    {
        gunfire.Stop();
        canFire = true;
    }

    void LookAround()
    {
        cameraRot = cam.transform.rotation.eulerAngles;
        cameraRot.x += -Input.GetAxis("Mouse Y") * lookSpeed;
        cameraRot.x = Mathf.Clamp((cameraRot.x <= 180) ? cameraRot.x : -(360 - cameraRot.x), -80f, 80f);
        cam.transform.rotation = Quaternion.Euler(cameraRot);
        playerRot.y = Input.GetAxis("Mouse X") * lookSpeed;
        transform.Rotate(playerRot);

    }

    void PlayerMovement()
    {
        xforce = Input.GetAxis("Horizontal") * moveSpeed;
        zforce = Input.GetAxis("Vertical") * moveSpeed;
        rb.velocity = transform.forward * zforce + transform.right * xforce + transform.up * rb.velocity.y;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }
    bool GroundCheck()
    {
        if (Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
