using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRageMeter : MonoBehaviour
{
    public int maxRage = 100;
    public int currentRage;

    public Sprite Nhuman;
    public Sprite RHuman;

    public Image HumanUI;

    public Slider rageSlider;

    private bool FullRaged = false;

    private void Start()
    {
        currentRage = 0;
        if(rageSlider != null )
        {
            rageSlider.maxValue = maxRage;
            rageSlider.value = currentRage;
        }
    }
    public void TakeRage(int rage)
    {
        currentRage = Mathf.Clamp(currentRage, 0, maxRage);

        
        if( rageSlider != null && rageSlider.value != maxRage)
        {
            currentRage += rage;
            rageSlider.value=currentRage;
        }
        if(rageSlider.value == maxRage)
        {
            FullRaged = true;
            HumanUI.sprite= RHuman;
            FullRagedPL();
            Debug.Log("Fully Corrupted");
        }
        
        Debug.Log("rage increase: " + rage + " cureent rage" + currentRage);
    }


    public void DecreaseRage(int rage)
    {
        currentRage = Mathf.Clamp(currentRage, 0, maxRage);
       
        if (rageSlider != null)
        {
            currentRage -= rage;
            rageSlider.value = currentRage;
        }
        if (rageSlider.value < maxRage)
        {
            FullRaged = false;
            HumanUI.sprite = Nhuman;
            FullRagedPL();

        }
        Debug.Log("rage decrease: " + rage + " cureent rage" + currentRage);
    }
    
    void FullRagedPL()
    {
        if(FullRaged)
        {
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<PlayerRageAttack>().enabled = true;
        }
        else if(!FullRaged)
        {
            GetComponent<PlayerAttack>().enabled = true;
            GetComponent<PlayerRageAttack>().enabled = false;
        }
    }
}
