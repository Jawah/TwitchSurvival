using UnityEngine;

public class ArrowHandler : MonoBehaviour {

    public ParticleSystem m_FoodArrowGreen;
    public ParticleSystem m_FoodArrowRed;
    public ParticleSystem m_FirewoodArrowGreen;
    public ParticleSystem m_FirewoodArrowRed;
    public ParticleSystem m_MedPackArrowGreen;
    public ParticleSystem m_MedPackArrowRed;

    public void ArrowDisplayRessources(int newVal, string valueName)
    {
        switch (valueName)
        {
            case "Food":
                if (newVal > GameManager.Instance.m_OldFoodValue)
                {
                    if (m_FoodArrowGreen != null)
                        m_FoodArrowGreen.Play();
                }

                else
                {
                    if (m_FoodArrowRed != null)
                        m_FoodArrowRed.Play();
                }

                GameManager.Instance.m_OldFoodValue = newVal;
                break;

            case "MedPack":
                if (newVal > GameManager.Instance.m_OldMedPackValue)
                {
                    if (m_MedPackArrowGreen != null)
                        m_MedPackArrowGreen.Play();
                }

                else
                {
                    if (m_MedPackArrowRed != null)
                        m_MedPackArrowRed.Play();
                }

                GameManager.Instance.m_OldMedPackValue = newVal;
                break;

            case "Firewood":
                if (newVal > GameManager.Instance.m_OldFirewoodValue)
                {
                    if (m_FirewoodArrowGreen != null)
                        m_FirewoodArrowGreen.Play();
                }
                  
                else
                {
                    if (m_FirewoodArrowRed != null)
                        m_FirewoodArrowRed.Play();
                }


                GameManager.Instance.m_OldFirewoodValue = newVal;
                break;
        }
    }
}
