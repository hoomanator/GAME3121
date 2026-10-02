using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Step1: install the InputSystem package from the Package Manager
using UnityEngine.InputSystem;


[RequireComponent(typeof(ParticleSystem))]

public class ToggleFireParticle2 : MonoBehaviour
{
    //Step2
    //Old InputManager
    //public KeyCode toggleKey = KeyCode.Space;
    //New InputSystem
    public Key toggleKey = Key.Space;

    private ParticleSystem fireParticle;
    public ParticleSystem igniteParticle;
    public ParticleSystem extinguishParticle;
    public GameObject pointLight;

    bool isPlaying = true;

    private void Start()
    {
        fireParticle = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        //Step3
        //if (Input.GetKeyDown(toggleKey))
        if(Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            if(isPlaying)
            {
                fireParticle.Stop();
                pointLight.SetActive(false);
                if (extinguishParticle != null)
                    extinguishParticle.Play();
                isPlaying = false;
            } 
            else
            {
                fireParticle.Play();
                pointLight.SetActive(true);
                if (igniteParticle != null)
                    igniteParticle.Play();
                isPlaying = true;
            }
        }
    }
}
