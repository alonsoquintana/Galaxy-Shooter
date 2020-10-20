using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool canTripleShot = false;
    public bool isSpeedBoostActive = false;
    public bool shieldsActive = false;

    public int lives = 3;

    [SerializeField]
    private GameObject _explosionPrefab;
    [SerializeField]
    private GameObject _laserPrefabs;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldGameObject;

    [SerializeField]
    private float _fireRate = 0.25f;
    
    private float _canFire = 0.0f;

    [SerializeField]
    public float _speed = 5.0f;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    private void Start()
    {
        //current pos = new position
        transform.position = new Vector3(0, 0, 0);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager != null)
        {
            _uiManager.UpdateLives(lives);
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines();
        }

        _audioSource = GetComponent<AudioSource>();
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
        _audioSource.Play();
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

        if (isSpeedBoostActive == true)
        {
            transform.Translate(Vector3.right * _speed * 1.5f * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * 1.5f * verticalInput * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
            transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);
        }

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

    public void Damage()
    {
        if (shieldsActive == true)
        {
            shieldsActive = false;
            _shieldGameObject.SetActive(false);
            return;
        }

        lives--;
        _uiManager.UpdateLives(lives);

        if (lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotPowerupOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostPowerupOn()
    {
        isSpeedBoostActive = true;
        StartCoroutine(SpeedBostPowerDownRoutine());
    }

    public void EnabledShields()
    {
        shieldsActive = true;
        _shieldGameObject.SetActive(true);
    }

    public IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

    public IEnumerator SpeedBostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isSpeedBoostActive = false;        
    }
}
