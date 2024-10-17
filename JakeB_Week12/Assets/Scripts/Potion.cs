using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {
    public PotionData potionEffect;
    private SpriteRenderer spriteRenderer;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            spriteRenderer.color = potionEffect.potionColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerMovement player = other.GetComponent<playerMovement>();
            if (player != null) {
                ApplyPotionEffect(player);
                Destroy(gameObject);
            }
        }
    }

    void ApplyPotionEffect(playerMovement player) {
        switch (potionEffect.effectType) {
            case PotionData.EffectType.ScaleIncrease:
                player.StartCoroutine(player.ScaleIncrease(potionEffect.effectValue, potionEffect.effectDuration));
                break;
            case PotionData.EffectType.SpeedIncrease:
                player.StartCoroutine(player.SpeedIncrease(potionEffect.effectValue, potionEffect.effectDuration));
                break;
            case PotionData.EffectType.HealthRestore:
                player.RestoreHealth(potionEffect.effectValue);  // Add health restoration effect
                break;
        }
    }
}