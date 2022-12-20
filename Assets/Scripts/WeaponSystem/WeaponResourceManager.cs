using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class WeaponInfo
{
    public string id;
    public string weaponPrefabPath;
    public string meshPath;


    public WeaponInfo(string _id, string _weaponPrefabPath, string _meshPath)
    {
        this.id = _id;
        this.weaponPrefabPath = _weaponPrefabPath;
        this.meshPath = _meshPath;

    }
}

public class WeaponResourceManager : MonoBehaviour
{

    public List<WeaponInfo> allFoundWeaponsInfo = new List<WeaponInfo>();

    private void Awake()
    {
        FindAllResourceWeapons();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindAllResourceWeapons()
    {
        var allWeaponInfo = Resources.LoadAll("", typeof(TextAsset)).Cast<TextAsset>().ToArray();
        foreach (var t in allWeaponInfo)
        {
            if(t.name == "weapon_info")
            {
                allFoundWeapons.Add(JsonUtility.FromJson<WeaponInfo>(t.text));
            }
            
        }
            
    }
}
