using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;

namespace Food_Slot_Tweaks;

public class FoodSlots
{
    [HarmonyPatch(typeof(Hud), "UpdateFood", new[] { typeof(Player) })]
    static class FoodColorPatch
    {
        static readonly Color healthFoodColor = new Color(1f, 0.333f, 0.333f, 0.35f); // Red color for health food
        static readonly Color staminaFoodColor = new Color(1f, 0.9513f, 0.2941f, 0.3f); // Orange color for stamina food
        static readonly Color defaultColor = new Color(0f, 0f, 0f, 0.5375f); // Default color when food is inactive

        static void Postfix(Hud __instance)
        {
            // Access the Player's food list
            List<Player.Food> foods = Player.m_localPlayer.GetFoods();

            // Access the m_healthPanel field of the Hud instance
            RectTransform healthPanel = __instance.m_healthPanel;

            // Iterate over the food slots and change their colors based on the food type
            for (int i = 0; i < foods.Count; i++)
            {
                Player.Food food = foods[i];
                Image foodImage = healthPanel.Find($"food{i}").GetComponent<Image>();

                // Only change the color if the food is still active
                if (foodImage != null && food.m_time > 0)
                {
                    Color foodColor = defaultColor;
                    if (food.m_health > food.m_stamina)
                    {
                        foodColor = healthFoodColor; // Red color for health food
                    }
                    else if (food.m_health < food.m_stamina)
                    {
                        foodColor = staminaFoodColor; // Orange color for stamina food
                    }

                    foodImage.color = foodColor;
                }
                else if (foodImage != null)
                {
                    // If the food is not active, revert to the default color
                    foodImage.color = defaultColor;
                }
            }

            // Handle any slots that don't have food in them
            for (int i = foods.Count; i < healthPanel.childCount; i++)
            {
                Transform slotTransform = healthPanel.Find($"food{i}");
                if (slotTransform != null)
                {
                    Image slotImage = slotTransform.GetComponent<Image>();
                    if (slotImage != null)
                    {
                        // Set to the default color
                        slotImage.color = defaultColor;
                    }
                }
            }
        }
    }
}
