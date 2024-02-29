﻿using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace TooManyItems
{
    internal class HolyWater
    {
        public static ItemDef itemDef;

        // Gain 20% (+20% per stack) bonus experience.
        public static ConfigurableValue<float> experienceMultiplierPerStack = new(
            "Item: Holy Water",
            "XP Multiplier",
            20f,
            "Bonus experience generation as a percentage.",
            new List<string>()
            {
                "ITEM_HOLYWATER_DESC"
            }
        );
        public static float experienceMultiplierAsPercent = experienceMultiplierPerStack.Value / 100f;

        internal static void Init()
        {
            GenerateItem();
            AddTokens();

            var displayRules = new ItemDisplayRuleDict(null);
            ItemAPI.Add(new CustomItem(itemDef, displayRules));

            Hooks();
        }

        private static void GenerateItem()
        {
            itemDef = ScriptableObject.CreateInstance<ItemDef>();

            itemDef.name = "HOLY_WATER";
            itemDef.nameToken = "HOLY_WATER_NAME";
            itemDef.pickupToken = "HOLY_WATER_PICKUP";
            itemDef.descriptionToken = "HOLY_WATER_DESCRIPTION";
            itemDef.loreToken = "HOLY_WATER_LORE";

            ItemTierCatalog.availability.CallWhenAvailable(() =>
            {
                if (itemDef) itemDef.tier = ItemTier.Tier1;
            });

            itemDef.pickupIconSprite = TooManyItems.MainAssets.LoadAsset<Sprite>("HolyWater.png");
            itemDef.pickupModelPrefab = TooManyItems.MainAssets.LoadAsset<GameObject>("HolyWater.prefab");
            itemDef.canRemove = true;
            itemDef.hidden = false;
        }

        public static void Hooks()
        {
            On.RoR2.CharacterMaster.GiveExperience += (orig, self, amount) =>
            {
                if (self.inventory == null) return;

                int count = self.inventory.GetItemCount(itemDef);
                if (count > 0)
                {
                    float multiplier = 1 + count * experienceMultiplierAsPercent;
                    amount = (uint)(amount * multiplier);
                }

                orig(self, amount);
            };
        }

        private static void AddTokens()
        {
            LanguageAPI.Add("HOLY_WATER", "Holy Water");
            LanguageAPI.Add("HOLY_WATER_NAME", "Holy Water");
            LanguageAPI.Add("HOLY_WATER_PICKUP", "Gain bonus experience.");

            string desc = $"Gain <style=cIsUtility>{experienceMultiplierPerStack.Value}%</style> <style=cStack>(+{experienceMultiplierPerStack.Value}% per stack)</style> bonus experience.";
            LanguageAPI.Add("HOLY_WATER_DESCRIPTION", desc);

            string lore = "";
            LanguageAPI.Add("HOLY_WATER_LORE", lore);
        }
    }
}

// Styles
// <style=cIsHealth>" + exampleValue + "</style>
// <style=cIsDamage>" + exampleValue + "</style>
// <style=cIsHealing>" + exampleValue + "</style>
// <style=cIsUtility>" + exampleValue + "</style>
// <style=cIsVoid>" + exampleValue + "</style>
// <style=cHumanObjective>" + exampleValue + "</style>
// <style=cLunarObjective>" + exampleValue + "</style>
// <style=cStack>" + exampleValue + "</style>
// <style=cWorldEvent>" + exampleValue + "</style>
// <style=cArtifact>" + exampleValue + "</style>
// <style=cUserSetting>" + exampleValue + "</style>
// <style=cDeath>" + exampleValue + "</style>
// <style=cSub>" + exampleValue + "</style>
// <style=cMono>" + exampleValue + "</style>
// <style=cShrine>" + exampleValue + "</style>
// <style=cEvent>" + exampleValue + "</style>