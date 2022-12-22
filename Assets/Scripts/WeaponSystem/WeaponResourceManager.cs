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
    public string displayName;


    public WeaponInfo(string _id, string _weaponPrefabPath, string _meshPath, string _displayName)
    {
        this.id = _id;
        this.weaponPrefabPath = _weaponPrefabPath;
        this.meshPath = _meshPath;
        this.displayName = _displayName;

    }
}

public class WeaponResourceManager : UnitySingleton<WeaponResourceManager>
{

    public List<WeaponInfo> allFoundWeaponsInfo = new List<WeaponInfo>();
    [SerializeField] private GameObject interactableWeaponSwapPrefab;
    private Vector3 latestWeaponSwapPosition = Vector3.zero;
    [SerializeField] private Vector3 offset = Vector3.zero;

    public override void Awake()
    {
        base.Awake();
        FindAllResourceWeapons();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnInteractableResourceWeapons();
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
                allFoundWeaponsInfo.Add(JsonUtility.FromJson<WeaponInfo>(t.text));
            }
            
        }
            
    }

    void SpawnInteractableResourceWeapons()
    {
        foreach(WeaponInfo w in allFoundWeaponsInfo)
        {
            GameObject newSwapper = Instantiate(interactableWeaponSwapPrefab, transform);
            newSwapper.transform.localPosition = latestWeaponSwapPosition;
            latestWeaponSwapPosition = newSwapper.transform.localPosition + offset;
            newSwapper.GetComponent<PickupWeapon>().weaponPrefab = (Resources.Load(w.weaponPrefabPath, typeof(GameObject)) as GameObject);
            newSwapper.GetComponent<PickupWeapon>().SetMesh((Resources.Load(w.meshPath, typeof(GameObject)) as GameObject));
        }
    }

    public WeaponInfo FindWeaponInfoByID(string id)
    {
        foreach (WeaponInfo w in allFoundWeaponsInfo)
        {
            if(w.id == id)
            {
                return w;
            }
        }

        return null;
    }
}
