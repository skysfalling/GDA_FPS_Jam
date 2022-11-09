using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopFade : MonoBehaviour
{
    private TextMeshPro _text;

    private Vector3 _direction;
    private Vector3 _startPosition;

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
        _text = GetComponent<TextMeshPro>();
        _startPosition = this.transform.position;

        // Find direction from origin to camera
        _direction = (Camera.main.transform.position - _startPosition).normalized;

        // Flatten direction y
        _direction = new Vector3(_direction.x, 0, _direction.z);

        // Find perpendicular to direction, going either left or right relative from camera
        int[] range = new int[] { -1, 1 };
        _direction = Quaternion.Euler(-45, range[Random.Range(0, 2)] * 90, 0) * _direction;

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

        // Move left or right relative from camera
        transform.position += _direction * _speed * Time.fixedDeltaTime;

        _speed *= 0.625f;

        Color currentColor = _text.color;

        // Fade alpha then destroy half a second after creation
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
