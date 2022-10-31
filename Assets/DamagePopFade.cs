using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopFade : MonoBehaviour
{
    private float _damage;
    private TextMeshPro _text;
    private Vector3 _direction;
    private Vector3 _startPosition;


    public void Setup(float damageDone)
    {
        _damage = damageDone;
        _text.text = _damage.ToString();
    }

    void Awake()
    {
        _text = GetComponent<TextMeshPro>();
        _direction = this.transform.position;
        Vector3 test = (Camera.main.transform.position - _startPosition).normalized;
        _direction = new Vector3(test.x, 0, test.z);
        _direction = Quaternion.Euler(0, 90, 0) * _direction;
    }

    private void Update()
    {
        Debug.DrawRay(_startPosition, _direction, Color.cyan, 1, false);

        transform.position += _direction * Time.deltaTime;
        //_fadeSpeed *= 2 * Time.deltaTime;

        Color currentColor = _text.color;

        if (currentColor.a > 0)
        {
            _text.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a - 0.6f * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
