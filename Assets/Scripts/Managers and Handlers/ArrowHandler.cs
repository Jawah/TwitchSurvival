using UnityEngine;

public class ArrowHandler : MonoBehaviour {

    public ParticleSystem foodArrowGreen;
    public ParticleSystem foodArrowRed;
    public ParticleSystem firewoodArrowGreen;
    public ParticleSystem firewoodArrowRed;
    public ParticleSystem medPackArrowGreen;
    public ParticleSystem medPackArrowRed;

    public void ArrowDisplayRessources(int newVal, string valueName)
    {
        switch (valueName)
        {
            case "Food":
                if (newVal > GameManager.Instance.oldFoodValue)
                {
                    if (foodArrowGreen != null)
                        foodArrowGreen.Play();
                }

                else
                {
                    if (foodArrowRed != null)
                        foodArrowRed.Play();
                }

                GameManager.Instance.oldFoodValue = newVal;
                break;

            case "MedPack":
                if (newVal > GameManager.Instance.oldMedPackValue)
                {
                    if (medPackArrowGreen != null)
                        medPackArrowGreen.Play();
                }

                else
                {
                    if (medPackArrowRed != null)
                        medPackArrowRed.Play();
                }

                GameManager.Instance.oldMedPackValue = newVal;
                break;

            case "Firewood":
                if (newVal > GameManager.Instance.oldFirewoodValue)
                {
                    if (firewoodArrowGreen != null)
                        firewoodArrowGreen.Play();
                }
                  
                else
                {
                    if (firewoodArrowRed != null)
                        firewoodArrowRed.Play();
                }


                GameManager.Instance.oldFirewoodValue = newVal;
                break;
        }
    }
}
