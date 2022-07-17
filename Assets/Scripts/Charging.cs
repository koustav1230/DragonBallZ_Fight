using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charging : MonoBehaviour
{
    public AudioSource charge;
    public ParticleSystem ChargingAuraEffect;
    void Start()
    {
        charge = gameObject.GetComponent<AudioSource>();
    }

    
    void Update()
    {
        ChargeController();
    }

    void ChargeController()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (!charge.isPlaying)
            {
                ChargingAuraEffect.Play();
                charge.Play();
            }

        }
        else
        {
            ChargingAuraEffect.Stop();
            charge.Stop();
        }
    }
}
