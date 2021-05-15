using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Rigidbody2D player;
    
    [Header(("Weapons"))]
    [SerializeField] private List<Gun> unlockedWeapons;
    [SerializeField] private Gun[] allWeapons;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private int maxAmountOfGuns;
    private int currentNumberOfGun;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            ChangeWeaponToNExtOneFromInventory();
    }

    private void FixedUpdate()
    {
        Move();
    }
    

    private void Move()
    {
        var moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var moveVelocity = moveInput.normalized * speed;
        player.MovePosition(player.position + moveVelocity * Time.fixedDeltaTime);
    }

    public void PickUpWeapon(Gun newWeapon)
    {
        var gun = unlockedWeapons.Find(weapon => weapon.Name == newWeapon.Name);
        if (gun != null)
            gun.AddCartiges();
        else if (unlockedWeapons.Count >= maxAmountOfGuns)
            ChangeCurrentWeaponOnNewWeapon(newWeapon.Name);
        else
        {
            AddWeaponInInventory(newWeapon.Name);
            ChangeWeaponToNExtOneFromInventory();
        }
        Destroy(newWeapon.gameObject);
    }
    
    private void AddWeaponInInventory(string nameOfNewWeapon)
    {
        var newGun = allWeapons.First(weapon => weapon.Name == nameOfNewWeapon);
        unlockedWeapons.Add(newGun);
    }
    
    private void ChangeCurrentWeaponOnNewWeapon(string nameOfNewWeapon)
    {
        var newGun = allWeapons.First(weapon => weapon.Name == nameOfNewWeapon);
        unlockedWeapons[currentNumberOfGun].gameObject.SetActive(false);
        unlockedWeapons[currentNumberOfGun] = newGun;
        newGun.gameObject.SetActive(true);
    }
    
    private void ChangeWeaponToNExtOneFromInventory()
    {
        if (unlockedWeapons.Count < 1)
            return;
        unlockedWeapons[currentNumberOfGun].gameObject.SetActive(false);
        currentNumberOfGun = (currentNumberOfGun + 1) % unlockedWeapons.Count;
        unlockedWeapons[currentNumberOfGun].gameObject.SetActive(true);
    }
}
