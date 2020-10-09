using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool canTripleShot = false;

    [SerializeField]
    private GameObject _laserPrefabs;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.25f;
    
    private float _canFire = 0.0f;

    [SerializeField]
    public float _speed = 5.0f;
    
   
    private void Start()
    {
        //current pos = new position
        transform.position = new Vector3(0, 0, 0);
    }

    private void Update()
    {
        movemet();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            shoot();
        }
    }

    private void shoot()
    {
        if (Time.time > _canFire)
        {
            if (canTripleShot == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefabs, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
            }            
            _canFire = Time.time + _fireRate;
        }
    }

    private void movemet()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
        transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);


        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        }

        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if(transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }
    }
    
    public void TripleShotPowerupOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }
}
