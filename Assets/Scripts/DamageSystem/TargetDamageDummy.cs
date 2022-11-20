using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Example Damagable
 * 
 * This is a target dummy that when hit flashes white
 */

public class TargetDamageDummy : Damageable
{
    Material[] _materials;
    Color[] _colorOld;


    public override void Start()
    {
        base.Start();
        _materials = GetComponent<Renderer>().materials;

        // Save start colors
        _colorOld = new Color[_materials.Length];

        for (int i = 0; i < _materials.Length; i++)
        {
            _colorOld[i] = _materials[i].color;
        }
    }

    // Flash target on hit
    public override void OnHitEffect()
    {
        // Set all material colors to white
        for(int i = 0; i < _materials.Length; i++)
        {
            _materials[i].color = Color.white;
        }

        StartCoroutine(flashBack());
    }

    // Flash back
    private IEnumerator flashBack()
    {
        yield return new WaitForSeconds(0.125f);

        // Set each materials color back to its original
        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i].color = _colorOld[i];
        }
    }
}
