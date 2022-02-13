using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Button button;
    public Image skillIcon;

    private bool isCooldown = false;
    private float cooldownTime;


    public void SetCooldownTime(float time)
    {
        cooldownTime = time;
    }

    public Button GetButton()
    {
        return button;
    }


    public void StartCooldown()
    {
        if (isCooldown)
            return;

        StartCoroutine(CheckCooldownTime());
    }


    private IEnumerator CheckCooldownTime()
    {
        float elapsedTime = 0f;

        isCooldown = true;
        skillIcon.fillAmount = 0f;

        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            skillIcon.fillAmount = elapsedTime / cooldownTime;

            yield return null;
        }

        isCooldown = false;
    }


}
