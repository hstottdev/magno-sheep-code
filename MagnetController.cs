using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum charge
{
    neutral,
    positive,
    negative
}

public class MagnetController : MonoBehaviour
{
    Rigidbody rb;
    LayerMask defaultLayer;

    [Header("Charge")]
    [SerializeField] charge defaultCharge;
    public charge currentCharge;
    [SerializeField] GameObject negativeEffect;
    [SerializeField] GameObject positiveEffect;
    [SerializeField] GameObject negImpulseEffect;
    [SerializeField] GameObject posImpulseEffect;

    [Header("Magnetism")]
    [SerializeField] bool kinematic;
    [SerializeField] SphereCollider radiusCollider;
    [SerializeField] float magnetismRadius = 2;
    [SerializeField] float magnetismForce = 5;

    [Header("Impulse Blast")]
    [SerializeField] float impulseAttractionForce = 70;
    [SerializeField] float impulseAttractionRadius = 5;
    [SerializeField] float impulseRepulsionRadius = 3;

    [SerializeField] float impulseAttractionTime = 0.5f;
    float impulseAttractionTimeRemaining = 0;

    [SerializeField] float impulseCooldown;
    float impulseCooldownRemaining = 0;

    [SerializeField] float impulseRepulsionForce = 30;
    [SerializeField] float impulseRecoilForce = 10;


    [Header("Targets")]
    [SerializeField] GameObject positiveTarget;
    [SerializeField] GameObject negativeTarget;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody component found on the sphere! Please add one.");
        }

        currentCharge = defaultCharge;
        defaultLayer = gameObject.layer;

        SetMagnetismRadius();
    }

    void SetMagnetismRadius()
    {
        if (radiusCollider == null)
        {
            Debug.LogError("No radius trigger on the sphere! Please add one.");
        }
        else
        {
            radiusCollider.radius = magnetismRadius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentCharge)
        {
            case charge.neutral:
                negativeEffect.SetActive(false);
                positiveEffect.SetActive(false);
                gameObject.layer = defaultLayer;
                break;
            case charge.negative:
                negativeEffect.SetActive(true);
                positiveEffect.SetActive(false);
                gameObject.layer = LayerMask.NameToLayer("Negative");
                break;
            case charge.positive:
                positiveEffect.SetActive(true);
                negativeEffect.SetActive(false);
                gameObject.layer = LayerMask.NameToLayer("Positive");
                break;
        }
    }

    private void FixedUpdate()
    {
        float attractionForce = magnetismForce;
        if(impulseAttractionTimeRemaining > 0)
        {
            attractionForce = impulseAttractionForce;
            radiusCollider.radius = impulseAttractionRadius;
            impulseAttractionTimeRemaining -= Time.fixedDeltaTime;
            if (impulseAttractionTimeRemaining <= 0) radiusCollider.radius = magnetismRadius;
        }

        impulseCooldownRemaining -= Time.fixedDeltaTime;

        AttractionForce(attractionForce,ForceMode.Force);

        RepulsionForce(magnetismForce,ForceMode.Force, magnetismRadius);
    }

    public void SwitchPolarities()
    {
        bool positive = currentCharge == charge.positive;

        currentCharge = positive ? charge.negative : charge.positive;
        impulseAttractionTimeRemaining = 0;

        AudioManager.PlaySound("switch", Random.Range(2f, 2.5f), 0.2f);
    }

    void TargetSwappedCheck()
    {
        if(negativeTarget != null)
        {
            if (negativeTarget.layer == LayerMask.NameToLayer("Positive"))
            {
                positiveTarget = negativeTarget;
                negativeTarget = null;
            }
        }
        if (positiveTarget != null)
        {
            if (positiveTarget.layer == LayerMask.NameToLayer("Negative"))
            {
                negativeTarget = positiveTarget;
                positiveTarget = null;
            }
        }
    }

    void AttractionForce(float forceMagnitude, ForceMode forceMode, bool pull = false)
    {
        GameObject targetObject = null;

        TargetSwappedCheck();//checks if targets have swapped polarities at all
        if (currentCharge == charge.negative) targetObject = positiveTarget;
        if (currentCharge == charge.positive) targetObject = negativeTarget;
        if (targetObject == null) return;
        if (kinematic) return;
        if (Vector3.Distance(transform.position, targetObject.transform.position) < 1f) return;

        transform.LookAt(targetObject.transform);
        Vector3 force = transform.forward * forceMagnitude;

        rb.AddForce(force, forceMode);
  
        if (pull == false) return;
        //also pull the object to you (may want this in specific situations)

        MagnetController cMg = targetObject.GetComponent<MagnetController>();
        if (cMg == null) return;
        if (cMg.kinematic) return;
        cMg.rb.AddForce(-force, forceMode);
    }

    void RepulsionForce(float force, ForceMode forceMode, float radius, float upwardsModifier = 0, bool recoil = false)
    {
        string affectedLayer = "Null";

        if (currentCharge == charge.negative) affectedLayer = "Negative";
        if (currentCharge == charge.positive) affectedLayer = "Positive";
        if (affectedLayer == "Null") return;

        Collider[] repulsedObjects = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider c in repulsedObjects)
        {
            if (c.gameObject.layer != LayerMask.NameToLayer(affectedLayer)) continue;
            if (c.GetComponent<Rigidbody>() == null) continue;
            if (c.gameObject == gameObject) continue;

            c.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, radius, upwardsModifier, forceMode);

            if (recoil == false) return;

            MagnetController cMg = c.GetComponent<MagnetController>();
            if (cMg == null) return;

            Debug.Log("impulse recoil");
            rb.AddExplosionForce(cMg.impulseRecoilForce, c.gameObject.transform.position, cMg.magnetismRadius, upwardsModifier, forceMode);
        }
    }

    public void ImpulseBlast(charge chargeType)
    {
        if (impulseCooldownRemaining > 0) return;

        currentCharge = chargeType;

        //impulse repulsion
        RepulsionForce(impulseRepulsionForce, ForceMode.Impulse,impulseRepulsionRadius,1,true);

        //impulse attraction
        impulseAttractionTimeRemaining = impulseAttractionTime;
        impulseCooldownRemaining = impulseCooldown;

        //spawn effect
        GameObject impulseEffect = null;
        if (currentCharge == charge.negative) impulseEffect = negImpulseEffect;
        if (currentCharge == charge.positive) impulseEffect = posImpulseEffect;
        Instantiate(impulseEffect,transform.position,transform.rotation);

        AudioManager.PlaySound("impulse", Random.Range(1.5f, 2.5f),0.3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Positive"))
        {
            positiveTarget = other.gameObject;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Negative"))
        {
            negativeTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Positive"))
        {
            if (positiveTarget == other.gameObject)
            {
                positiveTarget = null;
            }          
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Negative"))
        {
            if (negativeTarget == other.gameObject)
            {
                negativeTarget = null;
            }
        }
    }
}
