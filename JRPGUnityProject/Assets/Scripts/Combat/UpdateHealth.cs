using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateHealth : MonoBehaviour
{
    public Image healthBar;
    public Text healthText;

    public void UpdateBar(float percentHpLeft)
    {
        healthBar.fillAmount = percentHpLeft;
        Canvas.ForceUpdateCanvases();
    }
    public void UpdateText(int hp, int maxHp)
    {
        healthText.text = string.Format("HP: {0}/{1}", hp, maxHp);
        Canvas.ForceUpdateCanvases();
    }
}
