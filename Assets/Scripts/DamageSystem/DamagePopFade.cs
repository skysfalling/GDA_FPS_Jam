using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopFade : MonoBehaviour
{
    private TextMeshPro _text;

    private Vector3 _direction;
    private bool _startFade;

    private float _damage;
    private float _speed;

    public void Setup(float damageDone)
    {
        _damage = damageDone;
        _text.text = _damage.ToString();
        _speed = Random.Range(24, 34);
    }

    void Awake()
    {
        // Pair text component
        _text = GetComponent<TextMeshPro>();

        // Get rotation relative to camera Y
        float cameraYROT = Camera.main.transform.localEulerAngles.y;

        // Find direction above target head
        _direction = transform.localRotation.eulerAngles.normalized;

        // Rotate direction relative to camera, -45 to 45 degrees
        _direction = Quaternion.Euler(0, cameraYROT, Random.Range(-45, 45)) * _direction;

        // Start fading popup
        StartCoroutine(startTheFade());
    }

    // Start fade in half a second
    private IEnumerator startTheFade()
    {
        yield return new WaitForSeconds(0.5f);
        _startFade = true;
    } 

    private void FixedUpdate()
    {

        // Move our popup
        transform.position += _direction * _speed * Time.fixedDeltaTime;
        
        // Decrease speed
        _speed *= 0.625f;

        // Fade alpha then destroy half a second after creation
        Color currentColor = _text.color;

        if (_startFade)
        {
            if (currentColor.a > 0)
            {
                _text.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a - 5f * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}
