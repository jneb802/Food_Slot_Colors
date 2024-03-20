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
        // Manually set up our alpha value to be the same as vanilla food HUD
        const float ALPHA = 0.503f;

        // Set our colors using vanilla food icon colors but replacing alpha with HUD alpha
        private static Color SetFoodColor(Color color, float alpha = ALPHA) => new(color.r, color.g, color.b, alpha);
        static readonly Color healthFoodColor = SetFoodColor(InventoryGui.instance.m_playerGrid.m_foodHealthColor);
        static readonly Color staminaFoodColor = SetFoodColor(InventoryGui.instance.m_playerGrid.m_foodStaminaColor);
        static readonly Color eitrFoodColor = SetFoodColor(InventoryGui.instance.m_playerGrid.m_foodEitrColor);
        static readonly Color defaultColor = SetFoodColor(Color.black);

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
                    // Decide which color to assign using the same logic as for vanilla food icon colors
                    if (food.m_eitr / 2f > food.m_health && food.m_eitr / 2f > food.m_stamina)
                    {
                        foodColor = eitrFoodColor;
                    }
                    else if (food.m_health / 2f > food.m_stamina)
                    {
                        foodColor = healthFoodColor;
                    }
                    else if (food.m_stamina / 2f > food.m_health)
                    {
                        foodColor = staminaFoodColor;
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
