using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField]
    private float _speed = 3.5f;
    private float _gravity = 9.81f;
    [SerializeField]
    private GameObject _muzzleFlash;
    [SerializeField]
    private GameObject _hitMarkerPrefab;
    [SerializeField]
    private AudioSource _weaponAudio;
    [SerializeField]
    private int currentAmmo;
    private int maxAmmo = 50;
    

    private UI_Manager _uiManager;

    private bool _isRealoading = false;

    public bool hasCoin = false;

    void Start()
    {
        //Customized CharacterController
        _controller = GetComponent<CharacterController>();

        //Lock Mouse and make it invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentAmmo = maxAmmo;
        
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    void Update()
    {
        //Raycasting
        if (Input.GetMouseButton(0) && currentAmmo > 0) //left click
        {
            //Enable sound for shooting and animation for muzzleFlash
            Shoot();
        }
        else
        {
            _muzzleFlash.SetActive(false);
            _weaponAudio.Stop();
        }

        //Reaload
        if (Input.GetKeyDown(KeyCode.R) && _isRealoading == false)
        {
            _isRealoading = true;
            StartCoroutine(Reload());
        }

        //unlock and show cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        //Customized Character controller   
        CalculateMovement();
    }

    void Shoot()
    {
        //Create animation on prefab when shooting
        _muzzleFlash.SetActive(true);

        //Update ammo
        currentAmmo--;
        _uiManager.UpdateAmmo(currentAmmo);

        //Create Sound
        if (_weaponAudio.isPlaying == false)
        {
            _weaponAudio.Play();
        }

        //RayCasting
        Ray rayOrignin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // == Ray rayOrignin = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrignin, out hitInfo))
        {
            Debug.Log("Raycast Hit: " + hitInfo.transform.name);
            GameObject hitMarker = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(hitMarker, 1f);
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * _speed;
        velocity.y -= _gravity;

        //local space => world space 
        velocity = transform.transform.TransformDirection(velocity);

        //function for moving using Customized CharacterController
        _controller.Move(velocity * Time.deltaTime);
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1.5f);
        currentAmmo = maxAmmo;
        _uiManager.UpdateAmmo(currentAmmo);
        _isRealoading = false;

    }
}
