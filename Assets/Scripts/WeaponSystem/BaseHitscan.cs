using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHitscan : MonoBehaviour
{
    public float damage = 1.0f;
    public float lifespan = 0.5f;

    [SerializeField] private float hipSpread;
    [SerializeField] private float ADSspread;

    public RaycastHit hitInfo;
    public Vector3 targetPosition;
    public bool hasRaycastHit;
    public LineRenderer lineRenderer;
    [SerializeField] private LayerMask raycastCheckLayers;

    public virtual void Start()
    {
        Destroy(gameObject, lifespan);

        if (!GameController.Instance.ownedFormController.isADS)
        {
            transform.forward += new Vector3(Random.Range(-hipSpread, hipSpread), Random.Range(-hipSpread, hipSpread), Random.Range(-hipSpread, hipSpread));
        }
        else
        {
            transform.forward += new Vector3(Random.Range(-ADSspread, ADSspread), Random.Range(-ADSspread, ADSspread), Random.Range(-ADSspread, ADSspread));
        }

        if (Physics.Raycast(Camera.main.transform.position, transform.forward, out hitInfo, 200.0f, raycastCheckLayers))
        {
            targetPosition = hitInfo.point;
            hasRaycastHit = true;
        }
        else
        {
            SetTargetPosition(new Ray(Camera.main.transform.position, transform.forward).GetPoint(200));
        }
        LeanTween.alpha(gameObject, 0, lifespan);

        lineRenderer.SetPosition(0, GameController.Instance.ownedFormController.currentForm.barrelSpawn.position);
        lineRenderer.SetPosition(1, targetPosition);
        
        if(!hasRaycastHit)
        {
            return;
        }

        Damageable damageable = hitInfo.transform.gameObject.GetComponent<Damageable>();

        // Check if target is a Damageable
        if (damageable != null)
        {
            // If it does, damage the target
            damageable.ProcessDamage(damage);
        }

        HitInteractable hitable = hitInfo.transform.gameObject.GetComponent<HitInteractable>();

        // Check if target is a hitable
        if (hitable != null)
        {
            // If it does, damage the target
            hitable.ProcessHit();
        }

    }

    public void SetHitInfo(RaycastHit info)
    {
        hitInfo = info;
        targetPosition = hitInfo.point;
        hasRaycastHit = true;
    }

    public void SetTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
    }

    public void SetTargetDirection(Vector3 dir)
    {
        transform.forward = dir;
    }

    public virtual void OnImpact(Collision collision) {}




}
