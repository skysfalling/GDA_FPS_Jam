using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FormObject : MonoBehaviour
{

    public BaseForm primaryForm;
    public BaseForm secondaryForm;
    public CinemachineVirtualCamera virtualCamera;
    public Transform barrelSpawn;
    public string weaponID;

    public float _currentPrimaryCooldown = 0;
    public float _currentSecondaryCooldown = 0;

    public float _currentPrimaryEnergy = 0;
    public float _currentSecondaryEnergy = 0;

    public float _currentPrimaryEnergyRegenTimer = 0;
    public float _currentSecondaryEnergyRegenTimer = 0;

    public bool _regenPrimaryEnergy = false;
    public bool _regenSecondaryEnergy = false;
    private bool _bothFormsShareEnergy = false;

    [SerializeField] private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        if(primaryForm != null)
        {
            InitializePrimaryEnergy();
        }

        if(secondaryForm != null)
        {
            InitializeSecondaryEnergy();
        }
    }

    private void Update()
    {
        DecrementTimers();
        RegenerateEnergy();
    }

    public void UpdateUI()
    {

    }

    void InitializePrimaryEnergy()
    {
        if (_bothFormsShareEnergy)
        {
            return;
        }

        if (primaryForm.shareEnergyWithOtherForm)
        {
            _bothFormsShareEnergy = true;
        }

        _currentPrimaryEnergy = primaryForm.energyMax;
        if(primaryForm.energyRegenType == BaseForm.EnergyRegenerationType.EmptyFullFill)
        {
            _regenPrimaryEnergy = false;
        }
        else
        {
            _regenPrimaryEnergy = true;
        }
    }

    void InitializeSecondaryEnergy()
    {
        if (_bothFormsShareEnergy)
        {

            return;
        }

        if (secondaryForm.shareEnergyWithOtherForm)
        {
            _bothFormsShareEnergy = true;
        }

        _currentSecondaryEnergy = secondaryForm.energyMax;
        if (secondaryForm.energyRegenType == BaseForm.EnergyRegenerationType.EmptyFullFill)
        {
            _regenSecondaryEnergy = false;
        }
        else
        {
            _regenSecondaryEnergy = true;
        }
    }

    bool CheckValidEnergy(int formIndex)
    {
        if(formIndex == 1 && _bothFormsShareEnergy)
        {
            formIndex = 2;
        }

        switch (formIndex)
        {
            case 0:

                if(primaryForm.energyType == BaseForm.EnergyUsage.Unlimited)
                {
                    return true;
                }

                if (_currentPrimaryEnergy - primaryForm.energyCost < 0)
                {
                    return false;
                }
                else
                {
                    _currentPrimaryEnergy -= primaryForm.energyCost;
                    WeaponPanelUIController.Instance.UpdateCurrentWeaponAmmoUI(this);

                    if (primaryForm.energyRegenType == BaseForm.EnergyRegenerationType.EmptyFullFill)
                    {
                        _regenPrimaryEnergy = (_currentPrimaryEnergy <= 0);
                    }

                    if (primaryForm.energyRegenType == BaseForm.EnergyRegenerationType.ConstantIncremental)
                    {
                        _currentPrimaryEnergyRegenTimer = primaryForm.energyRegenCooldown;
                    }

                    return true;
                }
            case 1:

                if (secondaryForm.energyType == BaseForm.EnergyUsage.Unlimited)
                {
                    return true;
                }

                if (_currentSecondaryEnergy - secondaryForm.energyCost < 0)
                {
                    return false;
                }
                else
                {
                    _currentSecondaryEnergy -= secondaryForm.energyCost;
                    WeaponPanelUIController.Instance.UpdateCurrentWeaponAmmoUI(this);

                    if (secondaryForm.energyRegenType == BaseForm.EnergyRegenerationType.EmptyFullFill)
                    {
                        _regenSecondaryEnergy = (_currentSecondaryEnergy <= 0);
                    }

                    if (secondaryForm.energyRegenType == BaseForm.EnergyRegenerationType.ConstantIncremental)
                    {
                        _currentSecondaryEnergyRegenTimer = secondaryForm.energyRegenCooldown;
                    }

                    return true;
                }
            case 2:

                if (primaryForm.energyType == BaseForm.EnergyUsage.Unlimited)
                {
                    return true;
                }

                if (_currentPrimaryEnergy - secondaryForm.energyCost < 0)
                {
                    return false;
                }
                else
                {
                    _currentPrimaryEnergy -= secondaryForm.energyCost;

                    if (primaryForm.energyRegenType == BaseForm.EnergyRegenerationType.EmptyFullFill)
                    {
                        _regenPrimaryEnergy = (_currentPrimaryEnergy <= 0);

                    }

                    if(primaryForm.energyRegenType == BaseForm.EnergyRegenerationType.ConstantIncremental)
                    {
                        _currentPrimaryEnergyRegenTimer = primaryForm.energyRegenCooldown;
                    }

                    return true;
                }
            default:
                return false;
        }

        
    }

    void RegenerateEnergy()
    {
        if (_regenPrimaryEnergy)
        {
            switch (primaryForm.energyRegenType) {
                case BaseForm.EnergyRegenerationType.ConstantFullFill:
                    WeaponPanelUIController.Instance.UpdateCurrentWeaponAmmoUI(this);
                    break;
                case BaseForm.EnergyRegenerationType.ConstantIncremental:
                    _currentPrimaryEnergy += primaryForm.energyRegenRate * Time.deltaTime;
                    _currentPrimaryEnergy = Mathf.Clamp(_currentPrimaryEnergy, 0, primaryForm.energyMax);
                    WeaponPanelUIController.Instance.UpdateCurrentWeaponAmmoUI(this);
                    break;
                case BaseForm.EnergyRegenerationType.EmptyFullFill:
                    StartCoroutine(FillEnergy(0));
                    _regenPrimaryEnergy = false;
                    break;
            
            }



            
        }

        if (primaryForm.shareEnergyWithOtherForm || secondaryForm.shareEnergyWithOtherForm)
        {
            return;
        }

        if (_regenSecondaryEnergy)
        {
            switch (secondaryForm.energyRegenType)
            {
                case BaseForm.EnergyRegenerationType.ConstantFullFill:
                    WeaponPanelUIController.Instance.UpdateCurrentWeaponAmmoUI(this);
                    break;
                case BaseForm.EnergyRegenerationType.ConstantIncremental:
                    _currentSecondaryEnergy += secondaryForm.energyRegenRate * Time.deltaTime;
                    _currentSecondaryEnergy = Mathf.Clamp(_currentSecondaryEnergy, 0, secondaryForm.energyMax);
                    WeaponPanelUIController.Instance.UpdateCurrentWeaponAmmoUI(this);
                    break;
                case BaseForm.EnergyRegenerationType.EmptyFullFill:
                    StartCoroutine(FillEnergy(1));
                    _regenSecondaryEnergy = false;
                    break;

            }
        }
    }

    IEnumerator FillEnergy(int formIndex)
    {
        switch (formIndex) {
            case 0:
                yield return new WaitForSeconds(primaryForm.energyRegenCooldown);
                _currentPrimaryEnergy = primaryForm.energyMax;
                
                break;
            case 1:
                yield return new WaitForSeconds(secondaryForm.energyRegenCooldown);
                _currentSecondaryEnergy = secondaryForm.energyMax;
                
                break;
        }

        WeaponPanelUIController.Instance.UpdateCurrentWeaponAmmoUI(this);

        yield return new WaitForSeconds(1);
    }

    public void UsePrimaryAction(float context)
    {
        if(primaryForm == null)
        {
            return;
        }

        if (_currentPrimaryCooldown > 0 || !CheckValidEnergy(0))
        {
            return;
        }

        if (primaryForm.firingType == BaseForm.FireType.Auto)
        {
            _currentPrimaryCooldown = primaryForm.actionCooldown;
            primaryForm.FormAction(context);

        }
        else if (primaryForm.firingType == BaseForm.FireType.Semi)
        {
            _currentPrimaryCooldown = primaryForm.actionCooldown;
            primaryForm.FormAction(context);
        }
        else if (primaryForm.firingType == BaseForm.FireType.Hold)
        {
            _currentPrimaryCooldown = primaryForm.actionCooldown;
            primaryForm.FormAction(context);

        }

        GeneratePrimaryImpulse();


    }

    

    public void UseSecondaryAction(float context)
    {
        if(secondaryForm == null)
        {
            return;
        }

        if (_currentSecondaryCooldown > 0 || !CheckValidEnergy(1))
        {
            return;
        }

        if (secondaryForm.firingType == BaseForm.FireType.Auto)
        {
            _currentSecondaryCooldown = secondaryForm.actionCooldown;
            secondaryForm.FormAction(context);
        }
        else if (secondaryForm.firingType == BaseForm.FireType.Semi)
        {
            _currentSecondaryCooldown = secondaryForm.actionCooldown;
            secondaryForm.FormAction(context);
        }
        else if (secondaryForm.firingType == BaseForm.FireType.Hold)
        {
            _currentSecondaryCooldown = secondaryForm.actionCooldown;
            secondaryForm.FormAction(context);
        }

        GenerateSecondaryImpulse();
    }

    void GeneratePrimaryImpulse()
    {
        if (primaryForm.screenShakeImpulseDirection == Vector3.zero)
        {
            impulseSource.GenerateImpulse(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * primaryForm.screenShakeImpulseMagnitude);
        }
        else
        {
            impulseSource.GenerateImpulse(primaryForm.screenShakeImpulseDirection * primaryForm.screenShakeImpulseMagnitude);
        }
    }

    void GenerateSecondaryImpulse()
    {
        if (secondaryForm.screenShakeImpulseDirection == Vector3.zero)
        {
            impulseSource.GenerateImpulse(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * secondaryForm.screenShakeImpulseMagnitude);
        }
        else
        {
            impulseSource.GenerateImpulse(secondaryForm.screenShakeImpulseDirection * secondaryForm.screenShakeImpulseMagnitude);
        }
    }
    void DecrementTimers()
    {
        if (_currentPrimaryCooldown > 0)
        {
            _currentPrimaryCooldown -= Time.deltaTime;
        }

        if (_currentSecondaryCooldown > 0)
        {
            _currentSecondaryCooldown -= Time.deltaTime;
        }

        if (_currentPrimaryEnergyRegenTimer > 0)
        {
            _currentPrimaryEnergyRegenTimer -= Time.deltaTime;
            _regenPrimaryEnergy = (_currentPrimaryEnergyRegenTimer < 0);
        }

        if (_currentSecondaryEnergyRegenTimer > 0)
        {
            _currentSecondaryEnergyRegenTimer -= Time.deltaTime;
            _regenSecondaryEnergy = (_currentSecondaryEnergyRegenTimer < 0);
        }


    }






}
