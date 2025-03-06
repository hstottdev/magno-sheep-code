using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    Vector3 normalScale;
    Vector3 hoverScale;
    Vector3 targetScale;
    [SerializeField] float hoverScaleMultiplier;
    [SerializeField] float scaleSpeed = 5;
    [SerializeField] GameObject buttonText;
    [SerializeField] UnityEvent onClick;


    // Start is called before the first frame update
    void Start()
    {
        normalScale = transform.localScale;
        hoverScale = normalScale * hoverScaleMultiplier;
        targetScale = normalScale;

        if (buttonText != null)
        {
            buttonText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.localScale,targetScale) > 0.05f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
    }

    private void OnMouseOver()
    {
        targetScale = hoverScale;

        if(buttonText != null)
        {
            buttonText.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        Deselect();
    }

    public void Deselect()
    {
        targetScale = normalScale;

        if (buttonText != null)
        {
            buttonText.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        transform.localScale = normalScale;

        onClick.Invoke();
    }
}
