using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalsFX2_ActivatonDelay : MonoBehaviour {

    // Use this for initialization
        public GameObject objectToActivate;
    public float Delay;

        private void Start()
        {
            StartCoroutine(ActivationRoutine());
        }

        private IEnumerator ActivationRoutine()
        {
            //Wait for 14 secs.
            yield return new WaitForSeconds(Delay);

            //Turn My game object that is set to false(off) to True(on).
            objectToActivate.SetActive(true);

            //Turn the Game Oject back off after 1 sec.
            //yield return new WaitForSeconds(1);

            //Game object will turn off
            //objectToActivate.SetActive(false);
        }
    }
