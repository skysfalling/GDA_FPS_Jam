using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPanelUIController : UnitySingleton<WeaponPanelUIController>
{
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponPrimaryAmmoText;
    public TextMeshProUGUI weaponPrimaryAmmoMaxText;
    public TextMeshProUGUI weaponSecondaryAmmoText;
    public TextMeshProUGUI weaponSecondaryAmmoMaxText;
    public GameObject secondaryAmmoUI;
    public GameObject primaryInfiniteUI;
    public GameObject secondaryInfiniteUI;
    public GameObject primaryAmmoDivider;
    public GameObject secondaryAmmoDivider;

    public void SetWeaponName(string weaponName)
    {
        weaponNameText.text = weaponName;
    }

    public void ToggleSecondaryAmmoUI(bool state)
    {
        secondaryAmmoUI.SetActive(state);
        if (!state)
        {
            secondaryInfiniteUI.SetActive(false);
        }
    }

    public void SetPrimaryAmmoUIToInfinite(bool state)
    {
        weaponPrimaryAmmoText.enabled = !state;
        weaponPrimaryAmmoMaxText.enabled = !state;
        primaryInfiniteUI.SetActive(state);
        primaryAmmoDivider.SetActive(!state);
    }

    public void SetSecondaryAmmoUIToInfinite(bool state)
    {
        weaponSecondaryAmmoText.enabled = !state;
        weaponSecondaryAmmoMaxText.enabled = !state;
        secondaryInfiniteUI.SetActive(state);
        secondaryAmmoDivider.SetActive(!state);
    }

    public void UpdateCurrentWeaponAmmoUI(FormObject obj)
    {
        if(obj.primaryForm != null && obj.primaryForm.energyType != BaseForm.EnergyUsage.Unlimited)
        {
            weaponPrimaryAmmoText.text = Mathf.FloorToInt(obj._currentPrimaryEnergy) + "";
        }

        if (obj.secondaryForm != null && obj.secondaryForm.energyType != BaseForm.EnergyUsage.Unlimited)
        {
            weaponSecondaryAmmoText.text = Mathf.FloorToInt(obj._currentSecondaryEnergy) + "";
        }

    }

    public void InitializeNewWeapon(FormObject obj)
    {
        WeaponInfo newWeaponInfo = WeaponResourceManager.Instance.FindWeaponInfoByID(obj.weaponID);

        if (newWeaponInfo != null)
        {
            weaponNameText.text = newWeaponInfo.displayName;
        }

        if(obj.primaryForm != null)
        {
            if(obj.primaryForm.energyType == BaseForm.EnergyUsage.Unlimited)
            {
                SetPrimaryAmmoUIToInfinite(true);
            }
            else
            {
                SetPrimaryAmmoUIToInfinite(false);
                weaponPrimaryAmmoText.text = Mathf.FloorToInt(obj._currentPrimaryEnergy) + "";
                weaponPrimaryAmmoMaxText.text = Mathf.FloorToInt(obj.primaryForm.energyMax) + "";
            }

            
        }

        if (obj.secondaryForm == null || obj.primaryForm.shareEnergyWithOtherForm || obj.secondaryForm.shareEnergyWithOtherForm)
        {
            ToggleSecondaryAmmoUI(false);
        }
        else
        {
            ToggleSecondaryAmmoUI(true);
            if (obj.secondaryForm.energyType == BaseForm.EnergyUsage.Unlimited)
            {
                SetSecondaryAmmoUIToInfinite(true);
            }
            else
            {
                SetSecondaryAmmoUIToInfinite(false);
                weaponSecondaryAmmoText.text = obj._currentSecondaryEnergy + "";
                weaponSecondaryAmmoMaxText.text = obj.secondaryForm.energyMax + "";
            }


        }
    }
}
