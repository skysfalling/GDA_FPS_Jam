using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FormObject : MonoBehaviour
{

    public BaseForm primaryForm;
    public BaseForm secondaryForm;
    public CinemachineVirtualCamera virtualCamera;

    public float _currentPrimaryCooldown = 0;
    public float _currentSecondaryCooldown = 0;

    public float _currentPrimaryEnergy = 0;
    public float _currentSecondaryEnergy = 0;

    public bool _regenPrimaryEnergy = false;
    public bool _regenSecondaryEnergy = false;

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

    void InitializePrimaryEnergy()
    {
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

                    if (primaryForm.energyRegenType == BaseForm.EnergyRegenerationType.EmptyFullFill)
                    {
                        if(_currentPrimaryEnergy <= 0)
                        {
                            _regenPrimaryEnergy = true;
                        }
                        else
                        {
                            _regenPrimaryEnergy = false;
                        }
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

                    if (secondaryForm.energyRegenType == BaseForm.EnergyRegenerationType.EmptyFullFill)
                    {
                        if (_currentSecondaryEnergy <= 0)
                        {
                            _regenSecondaryEnergy = true;
                        }
                        else
                        {
                            _regenSecondaryEnergy = false;
                        }
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

                    break;
                case BaseForm.EnergyRegenerationType.ConstantIncremental:
                    _currentPrimaryEnergy += primaryForm.energyRegenRate * Time.deltaTime;
                    break;
                case BaseForm.EnergyRegenerationType.EmptyFullFill:
                    StartCoroutine(FillEnergy(0));
                    _regenPrimaryEnergy = false;
                    break;
            
            }



            
        }

        if (_regenSecondaryEnergy)
        {
            switch (secondaryForm.energyRegenType)
            {
                case BaseForm.EnergyRegenerationType.ConstantFullFill:

                    break;
                case BaseForm.EnergyRegenerationType.ConstantIncremental:
                    _currentSecondaryEnergy += secondaryForm.energyRegenRate * Time.deltaTime;
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



        yield return new WaitForSeconds(1);
    }

    public void UsePrimaryAction(float context)
    {
        if(primaryForm == null)
        {
            return;
        }

        if (_currentPrimaryCooldown <= 0 && CheckValidEnergy(0))
        {
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
        }

        
    }

    public void UseSecondaryAction(float context)
    {
        if(secondaryForm == null || !CheckValidEnergy(1))
        {
            return;
        }

        if (_currentSecondaryCooldown <= 0 && CheckValidEnergy(1))
        {
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


    }






}
