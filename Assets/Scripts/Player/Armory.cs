using Lindon.TowerUpper.Data;
using Lindon.TowerUpper.GameController.Events;
using Lindon.TowerUpper.Profile;
using System.Collections.Generic;
using UnityEngine;

public class Armory : MonoBehaviour
{
    [SerializeField] private List<WeaponModel> m_WeaponModels;
    private int m_lastId = -1;

    private void OnEnable()
    {
        GameStarter.OnStartGame += StartGame;
    }

    private void OnDisable()
    {
        GameStarter.OnStartGame -= StartGame;
    }

    private void StartGame()
    {
        if (m_lastId == -1) return;
        ActiveWeapon(m_lastId);
    }

    public bool AddWeapon(WeaponModel newModel)
    {
        foreach(var model in m_WeaponModels)
        {
            if (model.Equals(newModel.Id))
            {
                return false;
            }
        }
        m_WeaponModels.Add(newModel);
        return true;
    }

    public Weapon ActiveWeapon()
    {
        var weaponId = m_WeaponModels.RemoveRandom().Id;
        return ActiveWeapon(weaponId);
    }

    public Weapon ActiveWeapon(int weaponId)
    {
        m_lastId = weaponId;

        Weapon weapon = null;
        foreach (var weaponModel in m_WeaponModels)
        {
            weaponModel.gameObject.SetActive(false);

            if (weaponModel.Equals(weaponId) && weapon == null)
            {
                weaponModel.gameObject.SetActive(true);
                weapon = weaponModel.GetComponent<Weapon>();
            }
        }
        return weapon;
    }

    public void SetActive(bool value)
    {
        if (m_lastId == -1)
        {
            m_lastId = ProfileController.Instance.Profile.GetActiveItem(ItemCategory.Weapon);
        }

        foreach (var weaponModel in m_WeaponModels)
        {
            weaponModel.gameObject.SetActive(false);
            if (value && weaponModel.Equals(m_lastId))
            {
                weaponModel.gameObject.SetActive(true);
            }
        }
    }
}
