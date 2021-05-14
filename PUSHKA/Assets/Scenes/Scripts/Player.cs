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
            ChangeWeapon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Gun pickUpWeapon;
        if (collision.TryGetComponent<Gun>(out pickUpWeapon))
        {
            var gun = unlockedWeapons.Find(weapon => weapon.Name == pickUpWeapon.Name);
            if (gun != null)
                gun.AddCartiges();
            else if (unlockedWeapons.Count >= maxAmountOfGuns)
                ChangeWeaponInInventory(pickUpWeapon.Name);
            else
            {
                AddWeaponInInventory(pickUpWeapon.Name);
                ChangeWeapon();
            }
            Destroy(collision.gameObject);
        }
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

    private void AddWeaponInInventory(string nameOfNewWeapon)
    {
        var newGun = allWeapons.First(weapon => weapon.Name == nameOfNewWeapon);
        unlockedWeapons.Add(newGun);
    }
    
    private void ChangeWeaponInInventory(string nameOfNewWeapon)
    {
        var newGun = allWeapons.First(weapon => weapon.Name == nameOfNewWeapon);
        unlockedWeapons[currentNumberOfGun].gameObject.SetActive(false);
        unlockedWeapons[currentNumberOfGun] = newGun;
        newGun.gameObject.SetActive(true);
    }
    
    private void ChangeWeapon()
    {
        if (unlockedWeapons.Count < 1)
            return;
        unlockedWeapons[currentNumberOfGun].gameObject.SetActive(false);
        currentNumberOfGun = (currentNumberOfGun + 1) % unlockedWeapons.Count;
        unlockedWeapons[currentNumberOfGun].gameObject.SetActive(true);
    }
}
