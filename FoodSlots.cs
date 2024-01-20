using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine;

namespace Food_Slot_Tweaks;

public class FoodSlots
{
    /*[HarmonyPatch(typeof(Hud), nameof(Hud.))]
    static class SlotPatch
    {
        public static Color defaultColor = new Color(0f, 0f, 0f, 0.5375f);
        public static Color redColor = new Color(1f, 0f, 0f, 1f);
        
        public static void Postfix(Hud __instance)
        {
            GameObject healthpanel = __instance.m_healthPanel.gameObject;
            if (healthpanel == null) {
                Debug.LogError("Health panel is null");
                return;
            }
            
            Graphic food0 = healthpanel.GetComponentInChildren<Graphic>();
            if (food0 == null) {
                Debug.LogError("Food0 image is null");
                return;
            }

            // If you intend to change the color, apply it here
            food0.color = redColor; // Example of applying a new color
            

            Debug.LogError("The color has been changed to red");
        }
    }*/
    
    /*[HarmonyPatch(typeof(Hud), "UpdateFood", new[] { typeof(Player) })]
    static class FoodColorPatch
    {
        static void Postfix(Hud __instance, Player player)
        {
            // Your logic to change the color of food icons
            Image[] foodIcons = __instance.m_foodIcons;
            Color newColor = new Color(1f, 0f, 0f, 1f); // Red color
            foreach (var icon in foodIcons)
            {
                if (icon != null)
                {
                    icon.color = newColor;
                }
            }
        }
    }*/
    
    /*HarmonyPatch(typeof(Hud), "UpdateFood", new[] { typeof(Player) })]
    static class FoodColorPatch
    {
        static void Postfix(Hud __instance, Player player)
        {
            // Your logic to change the color of food icons
            RectTransform healthpanel = __instance.m_healthPanel;
            Image food0 = healthpanel.GetComponentInChildren<Image>();
            Graphic foodGraphic = food0.GetComponent<Graphic>();
            
            Color newColor = new Color(1f, 0f, 0f, 1f); // Red color

            foodGraphic.color = newColor;
        }
    }*/
    
    /*[HarmonyPatch(typeof(Hud), "UpdateFood", new[] { typeof(Player) })]
    static class FoodColorPatch
    {
        static void Postfix(Hud __instance)
        {
            // Define the new color you want to apply to the food slots
            Color healthFood = new Color(1f, 0f, 0f, 0.5375f); // Red color
            Color staminaFood = new Color(1f, 0f, 0f, 0.5375f); // Red color

            // Access the m_healthPanel field of the Hud instance
            RectTransform healthPanel = __instance.m_healthPanel;

            // Change color for food0
            Image food0Image = healthPanel.Find("food0").GetComponent<Image>();
            if (food0Image != null) food0Image.color = newColor;

            // Change color for food1
            Image food1Image = healthPanel.Find("food1").GetComponent<Image>();
            if (food1Image != null) food1Image.color = newColor;

            // Change color for food2
            Image food2Image = healthPanel.Find("food2").GetComponent<Image>();
            if (food2Image != null) food2Image.color = newColor;
        }
    }*/

    /*[HarmonyPatch(typeof(Hud), "UpdateFood", new[] { typeof(Player) })]
    static class FoodColorPatch
    {
        static readonly Color healthFoodColor = new Color(1f, 0.333f, 0.333f, 0.4f); // Red color for health food
        static readonly Color staminaFoodColor = new Color(1f, 0.9513f, 0.2941f, 0.4f); // Green color for stamina food
            
        static void Postfix(Hud __instance)
        {
            // Access the Player's food list
            List<Player.Food> foods = Player.m_localPlayer.GetFoods();

            // Access the m_healthPanel field of the Hud instance
            RectTransform healthPanel = __instance.m_healthPanel;

            // Iterate over the food slots and change their colors based on the food type
            for (int i = 0; i < foods.Count; i++)
            {
                // Assume that the food type can be determined by a property or method on the food object
                // For example, you might check a property like 'food.m_foodType' or 'food.IsHealthFood()'
                Player.Food food = foods[i];

                // Find the corresponding food slot by name, e.g., "food0", "food1", "food2"
                Image foodImage = healthPanel.Find($"food{i}").GetComponent<Image>();

                // Determine the color based on whether m_health is greater than or equal to m_stamina
                Color foodColor = food.m_health >= food.m_stamina ? healthFoodColor : staminaFoodColor;

                if (foodImage != null)
                {
                    foodImage.color = foodColor;
                }
            }
        }
    }*/
    
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
                // Determine the color based on whether m_health is greater than or equal to m_stamina
                Color foodColor = food.m_health >= food.m_stamina ? healthFoodColor : staminaFoodColor;
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