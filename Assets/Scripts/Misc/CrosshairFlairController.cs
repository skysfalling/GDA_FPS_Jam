using UnityEngine;
using UnityEngine.UI;

public class CrosshairFlairController : MonoBehaviour
{
    Transform _crosshair_transform;
    float _shrink_amount = 1f;
    float max_size = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _crosshair_transform = GetComponent<Image>().transform;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If crosshair flair out, strink back overtime!
        if(_crosshair_transform.localScale.x > 0)
        {
            _crosshair_transform.localScale -= Vector3.one * (_shrink_amount * Time.fixedDeltaTime);
        }
        else
        {
            // Otherwise, set to scale of zero
            _crosshair_transform.localScale = Vector3.zero;
        }
    }

    // Call to flair the crosshair flair out
    public void Flair(float growSize)
    {
        if(_crosshair_transform.localScale == Vector3.zero)
        {
            _crosshair_transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }

        Vector3 targetScale = _crosshair_transform.localScale + (Vector3.one * growSize) * Time.fixedDeltaTime;

        // Check if Flair scale excedes max
        if (targetScale.x > max_size)
        {
            // Exponentially decrease scale up
            _crosshair_transform.localScale += ((Vector3.one * (growSize * (targetScale.x - max_size) * 0.1f)));
        }
        else
        {
            // Scale up normally
            _crosshair_transform.localScale = targetScale;
        }
    }
}
