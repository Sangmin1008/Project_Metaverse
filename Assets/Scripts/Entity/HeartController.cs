using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private ResourceController resourceController;

    [SerializeField] private List<HeartAnimationHandler> hearts = new List<HeartAnimationHandler>();
    private int previousHealth;
    private int maxHeartCount;

    private void Start()
    {
        if (resourceController == null)
            resourceController = FindObjectOfType<ResourceController>();

        previousHealth = resourceController.MaxHealth;
        maxHeartCount = Mathf.CeilToInt(resourceController.MaxHealth / 2f);

        StartCoroutine(WatchHealth());
    }

    private IEnumerator WatchHealth()
    {
        while (true)
        {
            yield return null;

            int currentHealth = resourceController.CurrentHealth;
            if (currentHealth == previousHealth) continue;

            int delta = currentHealth - previousHealth;

            int changedHeartIndex = (previousHealth + 1) / 2 - 1;
            
            if (delta > 0)
            {
                hearts[changedHeartIndex].StartHealing();
                yield return new WaitForSeconds(0.3f);
                hearts[changedHeartIndex].EndHealing();
            }
            else
            {
                hearts[changedHeartIndex].StartDamage();
                yield return new WaitForSeconds(0.3f);
                hearts[changedHeartIndex].EndDamage();
            }

            previousHealth = currentHealth;
        }
    }


}