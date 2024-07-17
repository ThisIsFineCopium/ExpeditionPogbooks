using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ExileCore;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.Elements;
using ExileCore.PoEMemory.Elements.InventoryElements;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ExileCore.Shared.Helpers;
using ImGuiNET;
using SharpDX;




namespace ExpeditionPogbooks
{
    public class ExpeditionPogbooks : BaseSettingsPlugin<ExpeditionPogbooksSettings>
    {
        private Dictionary<int, float> areaLevelMultiplier = new Dictionary<int, float>
        {
            {55,0.03f},
            {56,0.04f},
            {57,0.04f},
            {58,0.05f},
            {59,0.06f},
            {60,0.07f},
            {61,0.08f},
            {62,0.09f},
            {63,0.10f},
            {64,0.12f},
            {65,0.15f},
            {66,0.18f},
            {67,0.2f},
            {68,0.24f},
            {69,0.3f},
            {70,0.3f},
            {71,0.3f},
            {72,0.4f},
            {73,0.4f},
            {74,0.4f},
            {75,0.5f},
            {76,0.5f},
            {77,0.6f},
            {78,0.6f},
            {79,0.7f},
            {80,0.8f},
            {81,0.8f},
            {82,0.9f},
            {83,1f},
            {84,1f},
            {85,1f},
            {86,1f}

        };


        

        private Dictionary<string, float> modScoreMultiplier = new Dictionary<string, float>
        {
            {"ExpeditionLogbookMapExpeditionSagaContainsBoss",1f},
            {"ExpeditionLogbookMapExpeditionMaximumPlacementDistance",1.5f},
            {"ExpeditionLogbookMapExpeditionExplosives",1.5f},
            {"ExpeditionLogbookMapExpeditionExplosionRadius",1.5f},
            {"MapExpeditionEliteMonsterQuantity",1.5f},
            {"MapExpeditionArtifactQuantity",1.5f},
            {"ExpeditionLogbookMapExpeditionRelics",1.15f},
            {"MapExpeditionChestCount",1.1f},
            {"ExpeditionLogbookMapExpeditionExtraRelicSuffixChance",1.1f},
            {"ExpeditionLogbookMapExpeditionChestDoubleDropsChance",1.1f},
            {"ExpeditionLogbookMapExpeditionSagaAdditionalTerrainFeatures",1.1f},
            {"ExpeditionLogbookMapExpeditionNumberOfMonsterMarkers",1.05f}
        };

        private  Dictionary<string, float> bossScore = new Dictionary<string, float>
        {
            {"ExpeditionLogbookMapExpeditionSagaContainsBoss|Druids of the Broken Circle",1f},
            {"ExpeditionLogbookMapExpeditionSagaContainsBoss|Order of the Chalice",1.5f},
            {"ExpeditionLogbookMapExpeditionSagaContainsBoss|Knights of the Sun",7.5f},
            {"ExpeditionLogbookMapExpeditionSagaContainsBoss|Black Scythe Mercenaries",5.5f}
        };

        private Dictionary<string, float> zoneScore = new Dictionary<string, float>
        {
            {"Forest Ruins",4.5f},
            {"Dried Riverbed",4.5f},
            {"Vaal Temple",4.5f},
            {"Shipwreck Reef",4.5f},
            {"Bluffs",2.4f},
            {"Karui Wargraves",2.5f},
            {"Battleground Graves",2.1f},
            {"Utzaal Outskirts",2.1f},
            {"Cemetery",2.1f},
            {"Desert Ruins",2f},
            {"Scrublands",1.8f},
            {"Volcanic Island",1.8f},
            {"Rotting Temple",1.5f},
            {"Sarn Slums",1.5f},
            {"Mountainside",1.6f}
        };

        private  Dictionary<string, float> factionScore = new Dictionary<string, float>
        {
            {"Druids of the Broken Circle",2f},
            {"Order of the Chalice",3f},
            {"Knights of the Sun",15f},
            {"Black Scythe Mercenaries",11f},



        };

        private readonly Dictionary<string, string> factionShort = new Dictionary<string, string>
        {
            {"Druids of the Broken Circle","Gwennen"},
            {"Order of the Chalice","Rog"},
            {"Knights of the Sun","Dannig"},
            {"Black Scythe Mercenaries","Tujen"},



        };

        


        /* ExpeditionLogbookMapExpeditionExplosionRadius
 * ExpeditionLogbookMapExpeditionMaximumPlacementDistance
 * MapExpeditionArtifactQuantity
 * ExpeditionLogbookMapExpeditionExplosives
 * MapExpeditionChestCount
 * ExpeditionLogbookMapExpeditionNumberOfMonsterMarkers
 * ExpeditionLogbookMapExpeditionSagaAdditionalTerrainFeatures
 * ExpeditionLogbookMapExpeditionSagaContainsBoss
 * ExpeditionLogbookMapExpeditionExtraRelicSuffixChance
 * ExpeditionLogbookMapExpeditionChestDoubleDropsChance
 * ExpeditionLogbookMapExpeditionRelics
 * MapExpeditionEliteMonsterQuantity
 */


        private readonly Color[] _atlasInventLayerColors = new[]
        {
            Color.Gray,
            Color.White,
            Color.Yellow,
            Color.OrangeRed,
            Color.Red,
        };

        class ExpeditionZone
        {
            public int AreaIndex { get; set; }

            public string Zone { get; set; }
            public string Faction { get; set; }
            public string Mod1 { get; set; }
            public string Mod2 { get; set; }
            public string Mod3 { get; set; }
            public float ZoneScore { get; set; }
            public float FactionScore { get; set; }
            public float Mod1Score { get; set; }
            public float Mod2Score { get; set; }
            public float Mod3Score { get; set; }
            public float BossScore { get; set; }
            public float FinalScore { get; set; }

            public ExpeditionZone(int areaIndex, string zone, string faction, string mod1, string mod2, string mod3, float zoneScore, float factionScore, float mod1Score, float mod2Score, float mod3Score, float bossScore, float finalScore)
            {
                AreaIndex = areaIndex;
                Zone = zone;
                Faction = faction;
                Mod1 = mod1;
                Mod2 = mod2;
                Mod3 = mod3;
                ZoneScore = zoneScore;
                FactionScore = factionScore;
                Mod1Score = mod1Score;
                Mod2Score = mod2Score;
                Mod3Score = mod3Score;
                BossScore = bossScore;
                FinalScore = finalScore;
            }
        }


        private static byte ToByte(float component)
        {
            var value = (int)(component * 255.0f);
            return ToByte(value);
        }

        public static byte ToByte(int value)
        {
            return (byte)(value < 0 ? 0 : value > 255 ? 255 : value);
        }


        public override bool Initialise()
        {

            



            /*
            { "Forest Ruins",4.5f},
            { "Dried Riverbed",4.5f},
            { "Vaal Temple",4.5f},
            { "Shipwreck Reef",4.5f},
            { "Bluffs",2.4f},
            { "Karui Wargraves",2.5f},
            { "Battleground Graves",2.1f},
            { "Utzaal Outskirts",2.1f},
            { "Cemetery",2.1f},
            { "Desert Ruins",2f},
            { "Scrublands",1.8f},
            { "Volcanic Island",1.8f},
            { "Rotting Temple",1.5f},
            { "Sarn Slums",1.5f},
            { "Mountainside",1.6f}    
            
            {"Druids of the Broken Circle",2f},
            {"Order of the Chalice",3f},
            {"Knights of the Sun",15f},
            {"Black Scythe Mercenaries",11f},
            */


            return true;
        }

        public override void Render()
        {
            var ingameState = GameController.Game.IngameState;
            if (ingameState.IngameUi.InventoryPanel.IsVisible)
            {
                updateSettings();
                DrawPlayerInvLogbooks();
                RelicLockerLogbooks();
                stashLogbooks();
                VendorLogbooks();
                DrawHaggleLogbooks();
                DrawHaggleCurrency();

            }

        }

        private void updateSettings()
        {
            areaLevelMultiplier[68] = Settings.level68;
            areaLevelMultiplier[69] = Settings.level69;
            areaLevelMultiplier[70] = Settings.level70;
            areaLevelMultiplier[71] = Settings.level71;
            areaLevelMultiplier[72] = Settings.level72;
            areaLevelMultiplier[73] = Settings.level73;
            areaLevelMultiplier[74] = Settings.level74;
            areaLevelMultiplier[75] = Settings.level75;
            areaLevelMultiplier[76] = Settings.level76;
            areaLevelMultiplier[77] = Settings.level77;
            areaLevelMultiplier[78] = Settings.level78;
            areaLevelMultiplier[79] = Settings.level79;
            areaLevelMultiplier[80] = Settings.level80;
            areaLevelMultiplier[81] = Settings.level81;
            areaLevelMultiplier[82] = Settings.level82;
            areaLevelMultiplier[83] = Settings.level83;
            areaLevelMultiplier[84] = Settings.level84;
            areaLevelMultiplier[85] = Settings.level85;


            modScoreMultiplier["ExpeditionLogbookMapExpeditionSagaContainsBoss"] = Settings.ContainsBoss;
            modScoreMultiplier["ExpeditionLogbookMapExpeditionMaximumPlacementDistance"] = Settings.PlacementRange;
            modScoreMultiplier["ExpeditionLogbookMapExpeditionExplosives"] = Settings.ExplosiveNumber;
            modScoreMultiplier["ExpeditionLogbookMapExpeditionExplosionRadius"] = Settings.ExplosionRadius;
            modScoreMultiplier["MapExpeditionEliteMonsterQuantity"] = Settings.RunicMonsterMarks;
            modScoreMultiplier["MapExpeditionArtifactQuantity"] = Settings.ArtifactQuantity;
            modScoreMultiplier["ExpeditionLogbookMapExpeditionRelics"] = Settings.NumberOfRemnants;
            modScoreMultiplier["MapExpeditionChestCount"] = Settings.MapExpeditionChestCount;
            modScoreMultiplier["ExpeditionLogbookMapExpeditionExtraRelicSuffixChance"] = Settings.ExtraRemnantChance;
            modScoreMultiplier["ExpeditionLogbookMapExpeditionChestDoubleDropsChance"] = Settings.ChestDoubleDrop;
            modScoreMultiplier["ExpeditionLogbookMapExpeditionSagaAdditionalTerrainFeatures"] = Settings.UndergroundArea;
            modScoreMultiplier["ExpeditionLogbookMapExpeditionNumberOfMonsterMarkers"] = Settings.NumberOfMonsterMarkers;


            bossScore["ExpeditionLogbookMapExpeditionSagaContainsBoss|Druids of the Broken Circle"] = Settings.GwennenBoss;
            bossScore["ExpeditionLogbookMapExpeditionSagaContainsBoss|Order of the Chalice"] = Settings.RogBoss;
            bossScore["ExpeditionLogbookMapExpeditionSagaContainsBoss|Knights of the Sun"] = Settings.DannigBoss;
            bossScore["ExpeditionLogbookMapExpeditionSagaContainsBoss|Black Scythe Mercenaries"] = Settings.TujenBoss;

            zoneScore["Forest Ruins"] = Settings.ForestRuins;
            zoneScore["Dried Riverbed"] = Settings.DriedRiverbed;
            zoneScore["Vaal Temple"] = Settings.VaalTemple;
            zoneScore["Shipwreck Reef"] = Settings.ShipwreckReef;
            zoneScore["Bluffs"] = Settings.Bluffs;
            zoneScore["Karui Wargraves"] = Settings.KaruiWargraves;
            zoneScore["Battleground Graves"] = Settings.BattlegroundGraves;
            zoneScore["Utzaal Outskirts"] = Settings.UtzaalOutskirts;
            zoneScore["Cemetery"] = Settings.Cemetery;
            zoneScore["Desert Ruins"] = Settings.DesertRuins;
            zoneScore["Scrublands"] = Settings.Scrublands;
            zoneScore["Volcanic Island"] = Settings.VolcanicIsland;
            zoneScore["Rotting Temple"] = Settings.RottingTemple;
            zoneScore["Sarn Slums"] = Settings.SarnSlums;
            zoneScore["Mountainside"] = Settings.Mountainside;


            factionScore["Druids of the Broken Circle"] = Settings.DruidsoftheBrokenCircle;
            factionScore["Order of the Chalice"] = Settings.OrderoftheChalice;
            factionScore["Knights of the Sun"] = Settings.KnightsoftheSun;
            factionScore["Black Scythe Mercenaries"] = Settings.BlackScytheMercenaries;

        }


        private void DrawPlayerInvLogbooks()
        {
            var ingameState = GameController.Game.IngameState;

            if (ingameState.IngameUi.InventoryPanel.IsVisible)
            {
                var inventoryZone = ingameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory].VisibleInventoryItems;
                HiglightAllLookbooks(inventoryZone);
            }
        }

        private void DrawHaggleLogbooks()
        {
            var ingameState = GameController.Game.IngameState;

            if (ingameState.IngameUi.HaggleWindow.IsVisible)
            {
                //var inventoryZone = ingameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory].VisibleInventoryItems;
                var inventoryZone = ingameState?.IngameUi.HaggleWindow.InventoryItems;
                if(inventoryZone is not null)
                    HiglightAllLookbooks(inventoryZone);
            }
        }

        private void DrawHaggleCurrency()
        {
            var ingameState = GameController.Game.IngameState;

            if (ingameState.IngameUi.HaggleWindow.IsVisible)
            {
                //var inventoryZone = ingameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory].VisibleInventoryItems;
                var inventoryZone = ingameState.IngameUi.HaggleWindow.InventoryItems;
                HiglightCurrencyToBuy(inventoryZone);
            }
        }

        private void VendorLogbooks()
        {
            var ingameState = GameController.Game.IngameState;

            if (ingameState.ServerData.NPCInventories.Count > 0)
            {
                //LogMessage("ya", 5, Color.Red);
                foreach (var inventory in ingameState.ServerData.NPCInventories)
                {
                    if (inventory.Inventory.CountItems > 0)
                    {
                        var auxaux = inventory.Inventory.InventorySlotItems;
                        //HiglightAllLogbooksShop(auxaux);
                    }

                }
                var inventoryZone = ingameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory].VisibleInventoryItems;

            }
        }

      

        private Color getColorScore(float score, string faction)
        {
            var maxZone = zoneScore.Values.Max();
            var minZone = zoneScore.Values.Min();
            var auxMultiplierScore = zoneScore.ToArray();
            var minLevelMultiplier = areaLevelMultiplier.Values.Min();
            var maxLevelMultiplier= areaLevelMultiplier.Values.Max();

            var auxMultiplierScoreOrdered = modScoreMultiplier.OrderByDescending(x => x.Value);
            var bestMultipliers = auxMultiplierScoreOrdered.ElementAt(0).Value * auxMultiplierScoreOrdered.ElementAt(1).Value * auxMultiplierScoreOrdered.ElementAt(2).Value;

            var auxMultiplierScoreOrderedBad = modScoreMultiplier.OrderBy(x => x.Value);
            var worstMultipliers = auxMultiplierScoreOrderedBad.ElementAt(0).Value * auxMultiplierScoreOrderedBad.ElementAt(1).Value;

            var rogFactionScore = 1f;
            var gwennenFactionScore = 1f;
            var tujenFactionScore = 1f;
            var dannigFactionScore = 1f; 
            factionScore.TryGetValue("Druids of the Broken Circle", out gwennenFactionScore);
            factionScore.TryGetValue("Order of the Chalice", out rogFactionScore);
            factionScore.TryGetValue("Black Scythe Mercenaries", out tujenFactionScore);
            factionScore.TryGetValue("Knights of the Sun", out dannigFactionScore);

            var rogBossScore = 1f;
            var gwennenBossScore = 1f;
            var tujenBossScore = 1f; 
             var dannigBossScore = 1f;
            bossScore.TryGetValue("ExpeditionLogbookMapExpeditionSagaContainsBoss|Druids of the Broken Circle", out gwennenBossScore);
            bossScore.TryGetValue("ExpeditionLogbookMapExpeditionSagaContainsBoss|Order of the Chalice", out rogBossScore);
            bossScore.TryGetValue("ExpeditionLogbookMapExpeditionSagaContainsBoss|Black Scythe Mercenaries", out tujenBossScore);
            bossScore.TryGetValue("ExpeditionLogbookMapExpeditionSagaContainsBoss|Knights of the Sun", out dannigBossScore);


            var maxRogFinalScore = ((maxZone * rogFactionScore) + rogBossScore) * bestMultipliers * maxLevelMultiplier;
            var maxGwennenFinalScore = ((maxZone * gwennenFactionScore) + gwennenBossScore) * bestMultipliers * maxLevelMultiplier;
            var maxTujenFinalScore = ((maxZone * tujenFactionScore) + tujenBossScore) * bestMultipliers * maxLevelMultiplier;
            var maxDannigFinalScore = ((maxZone * dannigFactionScore) + dannigBossScore) * bestMultipliers * maxLevelMultiplier;


            var minRogFinalScore = ((minZone * rogFactionScore)) * worstMultipliers * minLevelMultiplier;
            var minGwennenFinalScore = ((minZone * gwennenFactionScore)) * worstMultipliers * minLevelMultiplier;
            var minTujenFinalScore = ((minZone * tujenFactionScore)) * worstMultipliers * minLevelMultiplier;
            var minDannigFinalScore = ((minZone * dannigFactionScore)) * worstMultipliers * minLevelMultiplier;
            /*
            LogMessage("maxRogFinalScore: " + maxRogFinalScore, 5, Color.Green);
            LogMessage("maxGwennenFinalScore: " + maxGwennenFinalScore, 5, Color.Green);
            LogMessage("maxTujenFinalScore: " + maxTujenFinalScore, 5, Color.Green);
            LogMessage("maxDannigFinalScore: " + maxDannigFinalScore, 5, Color.Green);
            LogMessage("bestMultipliers: " + bestMultipliers, 5, Color.Green);

            LogMessage("minRogFinalScore: " + minRogFinalScore, 5, Color.Red);
            LogMessage("minGwennenFinalScore: " + minGwennenFinalScore, 5, Color.Red);
            LogMessage("minTujenFinalScore: " + minTujenFinalScore, 5, Color.Red);
            LogMessage("minDannigFinalScore: " + minDannigFinalScore, 5, Color.Red);
            LogMessage("worstMultipliers: " + worstMultipliers, 5, Color.Green);

            */

            var min = 0f;
            var max = 0f;
           
            switch (faction)
            { 
                case "Druids of the Broken Circle":
                    // code block
                    min = minGwennenFinalScore;
                    max = maxGwennenFinalScore;
                    break; 
                case "Order of the Chalice":
                    // code block
                    min = minRogFinalScore;
                    max = maxRogFinalScore;
                    break;
                case "Black Scythe Mercenaries":
                    min = minTujenFinalScore;
                    max = maxTujenFinalScore;
                    break;
                case "Knights of the Sun":
                    // code block
                    min = minDannigFinalScore;
                    max = maxDannigFinalScore;
                    break;
                default:
                    min = 0f;
                    max = 0f;
                    break;

            }

            var dif = max - min;
            var scale = 256f / dif;
            var valueAux = score - min;
            var colorValue = valueAux * scale;

            var red = 255- colorValue;
            if ((255 - colorValue) < 0)
                red = 0;

            var green = colorValue;
            if ((colorValue) > 255)
                green = 255;



            Color auxColor = Color.Red;

            //var auxColor = new Color(red, green, 0);
            auxColor.B = ToByte(0);
            auxColor.G = ToByte((int)Math.Ceiling(green));
            auxColor.R = ToByte((int)Math.Ceiling(red));

            return auxColor;
        }
    
        private void HiglightAllLookbooks(IList<NormalInventoryItem> items)
        {
            var ingameState = GameController.Game.IngameState;
            var serverData = ingameState.ServerData;
            var bonusComp = serverData.BonusCompletedAreas;
            var comp = serverData.CompletedAreas;
            var shEld = serverData.ShaperElderAreas;

            var disableOnHover = false;
            var disableOnHoverRect = new RectangleF();

            var inventoryItemIcon = ingameState.UIHover.AsObject<HoverItemIcon>();
            var UIHoverEntity = ingameState.UIHover.Entity;

            var tooltip = inventoryItemIcon?.Tooltip;

            //LogMessage("tooltip" + " is visible local: " + tooltip.IsVisibleLocal + " is visible: " + tooltip.IsVisible, 5, Color.Red);
            //LogMessage("tooltip item" + " is visible : " + tooltip?.IsVisible, 5, Color.Red);
            //LogMessage("tooltip item" + " is visible local: " + tooltip?.IsVisibleLocal, 5, Color.Red);


            if (tooltip != null)
            {
                disableOnHover = true;
                disableOnHoverRect = tooltip.GetClientRect();
            }

            foreach (var item in items)
            {
                var entity = item?.Item;
                var itemIsHovered = false;
                
                if (UIHoverEntity.Address == entity.Address)
                    itemIsHovered = true;
                //LogMessage("UIHoverEntity" + " is visible : " + , 5, Color.Red);
                //LogMessage("tooltip item" + " is visible local: " + , 5, Color.Red);
                
                if (entity == null) continue;
                var bit = GameController.Files.BaseItemTypes.Translate(entity.Path);
                if (bit == null) continue;
                
                if (bit.ClassName != "ExpeditionLogbook" && bit.ClassName != "ExpeditionLogbooks" && bit.ClassName != "Expedition Logbooks") continue;
                
                var expeditionComponent = entity.GetComponent<ExpeditionSaga>();

                var drawRect = item.GetClientRect();

                if (disableOnHover && disableOnHoverRect.Intersects(drawRect))
                    continue;
               
                var offset = 3;
                drawRect.Top += offset;
                drawRect.Bottom -= offset;
                drawRect.Right -= offset;
                drawRect.Left += offset;

                var i = 1;

                List<ExpeditionZone> expeditionZonesList = new List<ExpeditionZone>();
                var level = expeditionComponent.AreaLevel;
                foreach (var expeditionArea in expeditionComponent.Areas)

                {
                    var zone = expeditionArea.Name;
                    var faction = expeditionArea.Faction;

                    var implicitMod1 = expeditionArea.ImplicitMods[0];
                    var implicitMod2 = expeditionArea.ImplicitMods[1];
                    var implicitMod3 = expeditionArea.ImplicitMods[1];

                    var implicitModCount = expeditionArea.ImplicitMods.Count();
                    if (implicitModCount == 3)
                        implicitMod3 = expeditionArea.ImplicitMods[2];

                    var implicitMod1ScoreMultiplier = 1f;
                    var implicitMod2ScoreMultiplier = 1f;
                    var implicitMod3ScoreMultiplier = 1f;

                    modScoreMultiplier.TryGetValue(implicitMod1.Name, out implicitMod1ScoreMultiplier);
                    modScoreMultiplier.TryGetValue(implicitMod2.Name, out implicitMod2ScoreMultiplier);
                    if (implicitModCount == 3)
                        modScoreMultiplier.TryGetValue(implicitMod3.Name, out implicitMod3ScoreMultiplier);

                    var zoneBaseScore = 0f;
                    var factionBaseScore = 0f;

                    zoneScore.TryGetValue(zone, out zoneBaseScore);
                    factionScore.TryGetValue(faction, out factionBaseScore);


                    var bossMod1Score = 0f;
                    var bossMod2Score = 0f;
                    var bossMod3Score = 0f;

                    bossScore.TryGetValue(implicitMod1.Name + "|" + faction, out bossMod1Score);
                    bossScore.TryGetValue(implicitMod2.Name + "|" + faction, out bossMod2Score);
                    bossScore.TryGetValue(implicitMod3.Name + "|" + faction, out bossMod3Score);
                    var levelMultiplier = 1f;
                    areaLevelMultiplier.TryGetValue(level, out levelMultiplier);

                    var bossScoreBase = bossMod1Score + bossMod2Score + bossMod3Score;
                    var baseScore = (zoneBaseScore * factionBaseScore) + bossScoreBase;
                    var scoreMultiplier = implicitMod3ScoreMultiplier * implicitMod2ScoreMultiplier * implicitMod1ScoreMultiplier;
                    var finalScore = baseScore * scoreMultiplier * levelMultiplier;

                    if (itemIsHovered && Settings.DebugMode)
                    {



                        
                    if (implicitModCount == 3)
                    {
                        LogMessage(zone + " : " + faction + " - " + scoreMultiplier.ToString() + " - " + implicitMod1.Name + ": " + implicitMod1ScoreMultiplier.ToString() + " - " + implicitMod2.Name + ": " + implicitMod2ScoreMultiplier.ToString() + " - " + implicitMod3.Name + ": " + implicitMod3ScoreMultiplier.ToString(), 5, Color.Red);
                    }
                    else
                    {
                        LogMessage(zone + " : " + faction + " - " + scoreMultiplier.ToString() + " - " + implicitMod1.Name + ": " + implicitMod1ScoreMultiplier.ToString() + " - " + implicitMod2.Name + ": " + implicitMod2ScoreMultiplier.ToString(), 5, Color.Red);
                    }

                    LogMessage(zone + " : " + faction + " zone base - " + zoneBaseScore.ToString() + " faction- " + factionBaseScore.ToString() + " boss- " + bossScoreBase.ToString(), 5, Color.Red);
                    LogMessage(zone + " : " + faction + " final: " + finalScore.ToString(), 5, Color.Red);
                    }
                    expeditionZonesList.Add(new ExpeditionZone(i, zone, faction, implicitMod1.Name, implicitMod2.Name,
                        implicitMod3.Name, zoneBaseScore, factionBaseScore, implicitMod1ScoreMultiplier, implicitMod2ScoreMultiplier, implicitMod3ScoreMultiplier, bossScoreBase, finalScore));
                    i = i + 1;
                }

                ExpeditionZone bestZone = expeditionZonesList.OrderByDescending(x => x.FinalScore).First();
                if (itemIsHovered && Settings.DebugMode)
                {
                    LogMessage("Best Zone: " + bestZone.AreaIndex + " : " +
                    "" + bestZone.Zone + " : " + bestZone.Faction + " final: " + bestZone.FinalScore.ToString(), 5, Color.Green);
                }
                
                var auxNPC ="";
                factionShort.TryGetValue(bestZone.Faction, out auxNPC);
                var drawBox = new RectangleF(drawRect.X, drawRect.Y +15, drawRect.Width, -15);
                var position = new Vector2(drawBox.Center.X-20, drawBox.Center.Y-7);

                var drawBox2 = new RectangleF(drawRect.X, drawRect.Y + 35, drawRect.Width, -15);
                var position2 = new Vector2(drawBox2.Center.X - 16, drawBox2.Center.Y - 7);


                var backColor = getColorScore(bestZone.FinalScore, bestZone.Faction);
                Graphics.DrawText(auxNPC , position, Color.Black,10);
                
                //LogMessage("hex:" + backColor.ToHex(), 5, Color.Green);

                Graphics.DrawBox(drawBox, backColor);

                Graphics.DrawText(bestZone.FinalScore.ToString("0") + "|" + bestZone.AreaIndex, position2, Color.Black, 10);
                Graphics.DrawBox(drawBox2, backColor);

                //Graphics.DrawText(bestZone.FinalScore.ToString(), new Vector2(drawRect.X - 2, drawRect.Y - 2), Color.White);



            }
        }



        
        private void HiglightCurrencyToBuy(IList<NormalInventoryItem> items)
        {
            var ingameState = GameController.Game.IngameState;
            var serverData = ingameState.ServerData;
            var bonusComp = serverData.BonusCompletedAreas;
            var comp = serverData.CompletedAreas;
            var shEld = serverData.ShaperElderAreas;

            var disableOnHover = false;
            var disableOnHoverRect = new RectangleF();

            var inventoryItemIcon = ingameState.UIHover.AsObject<HoverItemIcon>();
            var UIHoverEntity = ingameState.UIHover.Entity;

            var tooltip = inventoryItemIcon?.Tooltip;

            //LogMessage("tooltip" + " is visible local: " + tooltip.IsVisibleLocal + " is visible: " + tooltip.IsVisible, 5, Color.Red);
            //LogMessage("tooltip item" + " is visible : " + tooltip?.IsVisible, 5, Color.Red);
            //LogMessage("tooltip item" + " is visible local: " + tooltip?.IsVisibleLocal, 5, Color.Red);


            if (tooltip != null)
            {
                disableOnHover = true;
                disableOnHoverRect = tooltip.GetClientRect();
            }

            

            foreach (var item in items)
            {
                var entity = item?.Item;
                var itemIsHovered = false;
                
                if (UIHoverEntity.Address == entity.Address)
                    itemIsHovered = true;
                //LogMessage("UIHoverEntity" + " is visible : " + , 5, Color.Red);
                //LogMessage("tooltip item" + " is visible local: " + , 5, Color.Red);


                if (entity == null) continue;
          
                if (!entity.Path.Contains("ExpeditionVendorCurrency") && !entity.Path.Contains("CurrencyRefresh")) continue;
                string type = "";

                if (entity.Path.Contains("ExpeditionVendorCurrency"))
                {
                    type = "vendorCurrency";
                } else

                {
                    type = "refreshCurrency";
                }


                //var expeditionComponent = entity.GetComponent<ExpeditionSaga>();

                var drawRect = item.GetClientRect();

                if (disableOnHover && disableOnHoverRect.Intersects(drawRect))
                    continue;

                var offset = 3;
                drawRect.Top += offset;
                drawRect.Bottom -= offset;
                drawRect.Right -= offset;
                drawRect.Left += offset;

                var auxsize = 15;
                var drawBox = new RectangleF(drawRect.X, drawRect.Y, drawRect.Width, drawRect.Height);
                var position = new Vector2(drawBox.X + 5, drawRect.Y + drawRect.Height - auxsize);
                var position2 = new Vector2(drawBox.X + 5, drawRect.Y + drawRect.Height - auxsize * 2);

                var backColor = Color.Orange;


                var stackSize = entity.GetComponent<ExileCore.PoEMemory.Components.Stack>().Size;
                var bit = GameController.Files.BaseItemTypes.Translate(entity.Path);
                var basename = bit.BaseName;
                if (type == "vendorCurrency")
                    {

                
                    var currentcount = 0;
                    switch (basename)
                    {
                        case "Lesser Broken Circle Artifact":
                            currentcount = ingameState.ServerData.LesserBrokenCircleArtifacts;
                            break;
                        case "Lesser Black Scythe Artifact":
                            currentcount = ingameState.ServerData.LesserBlackScytheArtifacts;
                            break;
                        case "Lesser Order Artifact":
                            currentcount = ingameState.ServerData.LesserOrderArtifacts;
                            break;
                        case "Greater Broken Circle Artifact":
                            currentcount = ingameState.ServerData.GreaterBrokenCircleArtifacts;
                            break;
                        case "Greater Black Scythe Artifact":
                            currentcount = ingameState.ServerData.GreaterBlackScytheArtifacts;
                            break;
                        case "Greater Order Artifact":
                            currentcount = ingameState.ServerData.GreaterOrderArtifacts;
                            break;
                        case "Grand Broken Circle Artifact":
                            currentcount = ingameState.ServerData.GrandBrokenCircleArtifacts;
                            break;
                        case "Grand Black Scythe Artifact":
                            currentcount = ingameState.ServerData.GrandBlackScytheArtifacts;
                            break;
                        case "Grand Order Artifact":
                            currentcount = ingameState.ServerData.GrandOrderArtifacts;
                            break;
                        case "Exceptional Broken Circle Artifact":
                            currentcount = ingameState.ServerData.ExceptionalBrokenCircleArtifacts;
                            break;
                        case "Exceptional Black Scythe Artifact":
                            currentcount = ingameState.ServerData.ExceptionalBlackScytheArtifacts;
                            break;
                        case "Exceptional Order Artifact":
                            currentcount = ingameState.ServerData.ExceptionalOrderArtifacts;
                            break;
                        default:
                            currentcount = ingameState.ServerData.LesserBlackScytheArtifacts;
                            break;
                    }
                    var vendorName = "";
                    switch (basename)
                    {
                        case var s when basename.Contains("Broken Circle"):
                            vendorName = "Gwennen";
                            break;
                        case var s when basename.Contains("Black Scythe"):
                            vendorName = "Tujen";
                            break;
                        case var s when basename.Contains("Order"):
                            vendorName = "Rog";
                            break;
                        default:
                            vendorName = "none";
                            break;




                    }
                    if (stackSize != 1)
                    {
                   

                        switch (vendorName)
                        {
                            case "Rog":
                                backColor = Color.Orange;
                                break;
                            case "Gwennen":
                                backColor = Color.Orange;
                                break;
                            case "Tujen":
                                backColor = Color.Green;
                                break;
                            default:
                                backColor = Color.Orange;
                                break;
                        }


                        // Graphics.DrawBox(drawBox, backColor);
                        Graphics.DrawFrame(drawBox, backColor, 3);
                        Graphics.DrawText(currentcount.ToString(), position, backColor, 10);
                        Graphics.DrawText(vendorName, position2, backColor, 10);

                    }
                } else if ((type == "refreshCurrency") && (basename == "Exotic Coinage")) {

                    Graphics.DrawFrame(drawBox, Color.Green, 5);
          

                }







                //Graphics.DrawText(bestZone.FinalScore.ToString(), new Vector2(drawRect.X - 2, drawRect.Y - 2), Color.White);



            }
        }
        private void RelicLockerLogbooks()
        {
            var ingameState = GameController.Game.IngameState;
            var serverData = ingameState.ServerData;
            var bonusComp = serverData.BonusCompletedAreas;
            var comp = serverData.CompletedAreas;
            var shEld = serverData.ShaperElderAreas;

            var disableOnHover = false;
            var disableOnHoverRect = new RectangleF();

            var inventoryItemIcon = ingameState.UIHover.AsObject<HoverItemIcon>();
            var UIHoverEntity = ingameState.UIHover.Entity;

            var tooltip = inventoryItemIcon?.Tooltip;

            //LogMessage("tooltip" + " is visible local: " + tooltip.IsVisibleLocal + " is visible: " + tooltip.IsVisible, 5, Color.Red);
            //LogMessage("tooltip item" + " is visible : " + tooltip?.IsVisible, 5, Color.Red);
            //LogMessage("tooltip item" + " is visible local: " + tooltip?.IsVisibleLocal, 5, Color.Red);

            var relicLockerOffset = 1f;
            if (GameController.Game.IngameState.IngameUi.ExpeditionLockerElement.IsVisible)
            {

                var locker = GameController.Game.IngameState.IngameUi.ExpeditionLockerElement;
                var auxLocker = locker?.GetChildFromIndices(26).Children;

                // 26


                foreach (var item in auxLocker.Skip(1))
                {
                    var entity = item?.Entity;
                    var itemIsHovered = false;

                    if (UIHoverEntity.Address == entity.Address)
                        itemIsHovered = true;
                    //LogMessage("UIHoverEntity" + " is visible : " + , 5, Color.Red);
                    //LogMessage("tooltip item" + " is visible local: " + , 5, Color.Red);


                    if (entity == null) continue;
                    var bit = GameController.Files.BaseItemTypes.Translate(entity.Path);

                    if (bit == null) continue;
                    if (bit.ClassName != "ExpeditionLogbook" && bit.ClassName != "ExpeditionLogbooks" && bit.ClassName != "Expedition Logbooks") continue;

                    var expeditionComponent = entity.GetComponent<ExpeditionSaga>();

                    var drawRect = item.GetClientRect();
                    relicLockerOffset = drawRect.Width * 4;
                    if (disableOnHover && disableOnHoverRect.Intersects(drawRect))
                        continue;

                    var offset = 3;
                    drawRect.Top += offset;
                    drawRect.Bottom -= offset;

                    //drawRect.Right -= offset + relicLockerOffset; // width / 90.5 ---- 212
                    //drawRect.Left += offset - relicLockerOffset; //212
                    
                    
                    drawRect.Right -= offset ; // width / 90.5 ---- 212
                    drawRect.Left += offset ; //212

                    var i = 1;

                    List<ExpeditionZone> expeditionZonesList = new List<ExpeditionZone>();
                    var level = expeditionComponent.AreaLevel;
                    foreach (var expeditionArea in expeditionComponent.Areas)

                    {
                        var zone = expeditionArea.Name;
                        var faction = expeditionArea.Faction;
                        var implicitMod1 = expeditionArea.ImplicitMods[0];
                        var implicitMod2 = expeditionArea.ImplicitMods[1];
                        var implicitMod3 = expeditionArea.ImplicitMods[1];

                        var implicitModCount = expeditionArea.ImplicitMods.Count();
                        if (implicitModCount == 3)
                            implicitMod3 = expeditionArea.ImplicitMods[2];

                        var implicitMod1ScoreMultiplier = 1f;
                        var implicitMod2ScoreMultiplier = 1f;
                        var implicitMod3ScoreMultiplier = 1f;

                        modScoreMultiplier.TryGetValue(implicitMod1.Name, out implicitMod1ScoreMultiplier);
                        modScoreMultiplier.TryGetValue(implicitMod2.Name, out implicitMod2ScoreMultiplier);
                        if (implicitModCount == 3)
                            modScoreMultiplier.TryGetValue(implicitMod3.Name, out implicitMod3ScoreMultiplier);

                        var zoneBaseScore = 0f;
                        var factionBaseScore = 0f;

                        zoneScore.TryGetValue(zone, out zoneBaseScore);
                        factionScore.TryGetValue(faction, out factionBaseScore);


                        var bossMod1Score = 0f;
                        var bossMod2Score = 0f;
                        var bossMod3Score = 0f;

                        bossScore.TryGetValue(implicitMod1.Name + "|" + faction, out bossMod1Score);
                        bossScore.TryGetValue(implicitMod2.Name + "|" + faction, out bossMod2Score);
                        bossScore.TryGetValue(implicitMod3.Name + "|" + faction, out bossMod3Score);
                        var levelMultiplier = 1f;
                        areaLevelMultiplier.TryGetValue(level, out levelMultiplier);

                        var bossScoreBase = bossMod1Score + bossMod2Score + bossMod3Score;
                        var baseScore = (zoneBaseScore * factionBaseScore) + bossScoreBase;
                        var scoreMultiplier = implicitMod3ScoreMultiplier * implicitMod2ScoreMultiplier * implicitMod1ScoreMultiplier;
                        var finalScore = baseScore * scoreMultiplier * levelMultiplier;

                        if (itemIsHovered)
                        {


                            if (implicitModCount == 3)
                            {
                                //LogMessage(zone + " : " + faction + " - " + scoreMultiplier.ToString() + " - " + implicitMod1.Name + ": " + implicitMod1ScoreMultiplier.ToString() + " - " + implicitMod2.Name + ": " + implicitMod2ScoreMultiplier.ToString() + " - " + implicitMod3.Name + ": " + implicitMod3ScoreMultiplier.ToString(), 5, Color.Red);
                            }
                            else
                            {
                                //LogMessage(zone + " : " + faction + " - " + scoreMultiplier.ToString() + " - " + implicitMod1.Name + ": " + implicitMod1ScoreMultiplier.ToString() + " - " + implicitMod2.Name + ": " + implicitMod2ScoreMultiplier.ToString(), 5, Color.Red);
                            }

                            //LogMessage(zone + " : " + faction + " zone base - " + zoneBaseScore.ToString() + " faction- " + factionBaseScore.ToString() + " boss- " + bossScoreBase.ToString(), 5, Color.Red);
                            //LogMessage(zone + " : " + faction + " final: " + finalScore.ToString(), 5, Color.Red);
                        }
                        expeditionZonesList.Add(new ExpeditionZone(i, zone, faction, implicitMod1.Name, implicitMod2.Name,
                            implicitMod3.Name, zoneBaseScore, factionBaseScore, implicitMod1ScoreMultiplier, implicitMod2ScoreMultiplier, implicitMod3ScoreMultiplier, bossScoreBase, finalScore));
                        i = i + 1;
                    }

                    ExpeditionZone bestZone = expeditionZonesList.OrderByDescending(x => x.FinalScore).First();
                    if (itemIsHovered)
                    {
                        //LogMessage("Best Zone: " + bestZone.AreaIndex + " : " +
                        //"" + bestZone.Zone + " : " + bestZone.Faction + " final: " + bestZone.FinalScore.ToString(), 5, Color.Green);
                    }
                    // acabei de iterar as zonas
                    var auxNPC = "";
                    factionShort.TryGetValue(bestZone.Faction, out auxNPC);
                    var backColor = getColorScore(bestZone.FinalScore, bestZone.Faction);
                    var drawBox = new RectangleF(drawRect.X, drawRect.Y + 15, drawRect.Width, -15);
                    var position = new Vector2(drawBox.Center.X - 20, drawBox.Center.Y - 7);

                    var drawBox2 = new RectangleF(drawRect.X, drawRect.Y + 35, drawRect.Width, -15);
                    var position2 = new Vector2(drawBox2.Center.X - 16, drawBox2.Center.Y - 7);


                    Graphics.DrawText(auxNPC, position, Color.Black, 10);
                    Graphics.DrawBox(drawBox, backColor);

                    Graphics.DrawText(bestZone.FinalScore.ToString("0"), position2, Color.Black, 10);
                    Graphics.DrawBox(drawBox2, backColor);

                    //Graphics.DrawText(bestZone.FinalScore.ToString(), new Vector2(drawRect.X - 2, drawRect.Y - 2), Color.White);

                }

            }
        }

        private void stashLogbooks()
        {
            var stashElement = GameController.IngameState.IngameUi.StashElement.StashInventoryPanel;
            var ingameState = GameController.Game.IngameState;
            var serverData = ingameState.ServerData;
            var bonusComp = serverData.BonusCompletedAreas;
            var comp = serverData.CompletedAreas;
            var shEld = serverData.ShaperElderAreas;

            var disableOnHover = false;
            var disableOnHoverRect = new RectangleF();

            var inventoryItemIcon = ingameState.UIHover.AsObject<HoverItemIcon>();
            var UIHoverEntity = ingameState.UIHover.Entity;

            var tooltip = inventoryItemIcon?.Tooltip;

            //LogMessage("tooltip" + " is visible local: " + tooltip.IsVisibleLocal + " is visible: " + tooltip.IsVisible, 5, Color.Red);
            //LogMessage("tooltip item" + " is visible : " + tooltip?.IsVisible, 5, Color.Red);
            //LogMessage("tooltip item" + " is visible local: " + tooltip?.IsVisibleLocal, 5, Color.Red);


            if (stashElement.IsVisible)
            {
                


                var auxStash = stashElement?.GetChildFromIndices(GameController.IngameState.IngameUi.StashElement.IndexVisibleStash, 0,0)?.Children;
               var auxStash2 = GameController.IngameState.IngameUi.StashElement.VisibleStash;
                var auxStashItems = auxStash2?.GetChildFromIndices(0).Children;
                

                // 26

                if(auxStashItems?.Count > 1)
                { 
                foreach (var item in auxStashItems.Skip(1))
                {
                    var entity = item?.Entity;
                    var itemIsHovered = false;

                    if (UIHoverEntity.Address == entity.Address)
                        itemIsHovered = true;
                    //LogMessage("UIHoverEntity" + " is visible : " + , 5, Color.Red);
                    //LogMessage("tooltip item" + " is visible local: " + , 5, Color.Red);


                    if (entity == null) continue;
                    var bit = GameController.Files.BaseItemTypes.Translate(entity.Path);

                    if (bit == null) continue;
                        if (bit.ClassName != "ExpeditionLogbook" && bit.ClassName != "ExpeditionLogbooks" && bit.ClassName != "Expedition Logbooks") continue;

                        var expeditionComponent = entity.GetComponent<ExpeditionSaga>();

                    var drawRect = item.GetClientRect();

                    if (disableOnHover && disableOnHoverRect.Intersects(drawRect))
                        continue;

                    var offset = 3;
                    drawRect.Top += offset;
                    drawRect.Bottom -= offset;

                    drawRect.Right -= offset;
                    drawRect.Left += offset;

                    var i = 1;

                    List<ExpeditionZone> expeditionZonesList = new List<ExpeditionZone>();
                    var level = expeditionComponent.AreaLevel;
                    foreach (var expeditionArea in expeditionComponent.Areas)

                    {
                        var zone = expeditionArea.Name;
                        var faction = expeditionArea.Faction;
                        var implicitMod1 = expeditionArea.ImplicitMods[0];
                        var implicitMod2 = expeditionArea.ImplicitMods[1];
                        var implicitMod3 = expeditionArea.ImplicitMods[1];

                        var implicitModCount = expeditionArea.ImplicitMods.Count();
                        if (implicitModCount == 3)
                            implicitMod3 = expeditionArea.ImplicitMods[2];

                        var implicitMod1ScoreMultiplier = 1f;
                        var implicitMod2ScoreMultiplier = 1f;
                        var implicitMod3ScoreMultiplier = 1f;

                        modScoreMultiplier.TryGetValue(implicitMod1.Name, out implicitMod1ScoreMultiplier);
                        modScoreMultiplier.TryGetValue(implicitMod2.Name, out implicitMod2ScoreMultiplier);
                        if (implicitModCount == 3)
                            modScoreMultiplier.TryGetValue(implicitMod3.Name, out implicitMod3ScoreMultiplier);

                        var zoneBaseScore = 0f;
                        var factionBaseScore = 0f;

                        zoneScore.TryGetValue(zone, out zoneBaseScore);
                        factionScore.TryGetValue(faction, out factionBaseScore);


                        var bossMod1Score = 0f;
                        var bossMod2Score = 0f;
                        var bossMod3Score = 0f;

                        bossScore.TryGetValue(implicitMod1.Name + "|" + faction, out bossMod1Score);
                        bossScore.TryGetValue(implicitMod2.Name + "|" + faction, out bossMod2Score);
                        bossScore.TryGetValue(implicitMod3.Name + "|" + faction, out bossMod3Score);
                        var levelMultiplier = 1f;
                        areaLevelMultiplier.TryGetValue(level, out levelMultiplier);

                        var bossScoreBase = bossMod1Score + bossMod2Score + bossMod3Score;
                        var baseScore = (zoneBaseScore * factionBaseScore) + bossScoreBase;
                        var scoreMultiplier = implicitMod3ScoreMultiplier * implicitMod2ScoreMultiplier * implicitMod1ScoreMultiplier;
                        var finalScore = baseScore * scoreMultiplier * levelMultiplier;

                        if (itemIsHovered)
                        {


                            if (implicitModCount == 3)
                            {
                                //LogMessage(zone + " : " + faction + " - " + scoreMultiplier.ToString() + " - " + implicitMod1.Name + ": " + implicitMod1ScoreMultiplier.ToString() + " - " + implicitMod2.Name + ": " + implicitMod2ScoreMultiplier.ToString() + " - " + implicitMod3.Name + ": " + implicitMod3ScoreMultiplier.ToString(), 5, Color.Red);
                            }
                            else
                            {
                                //LogMessage(zone + " : " + faction + " - " + scoreMultiplier.ToString() + " - " + implicitMod1.Name + ": " + implicitMod1ScoreMultiplier.ToString() + " - " + implicitMod2.Name + ": " + implicitMod2ScoreMultiplier.ToString(), 5, Color.Red);
                            }

                            //LogMessage(zone + " : " + faction + " zone base - " + zoneBaseScore.ToString() + " faction- " + factionBaseScore.ToString() + " boss- " + bossScoreBase.ToString(), 5, Color.Red);
                            //LogMessage(zone + " : " + faction + " final: " + finalScore.ToString(), 5, Color.Red);
                        }
                        expeditionZonesList.Add(new ExpeditionZone(i, zone, faction, implicitMod1.Name, implicitMod2.Name,
                            implicitMod3.Name, zoneBaseScore, factionBaseScore, implicitMod1ScoreMultiplier, implicitMod2ScoreMultiplier, implicitMod3ScoreMultiplier, bossScoreBase, finalScore));
                        i = i + 1;
                    }

                    ExpeditionZone bestZone = expeditionZonesList.OrderByDescending(x => x.FinalScore).First();
                    if (itemIsHovered)
                    {
                        //LogMessage("Best Zone: " + bestZone.AreaIndex + " : " +
                        //"" + bestZone.Zone + " : " + bestZone.Faction + " final: " + bestZone.FinalScore.ToString(), 5, Color.Green);
                    }
                    // acabei de iterar as zonas
                    var auxNPC = "";
                    factionShort.TryGetValue(bestZone.Faction, out auxNPC);
                    var backColor = getColorScore(bestZone.FinalScore, bestZone.Faction);
                    if (auxStash2.InvType == InventoryType.QuadStash )
                    {
                        var drawBox = new RectangleF(drawRect.X, drawRect.Y + 8, drawRect.Width, -12);
                        var position = new Vector2(drawBox.Center.X - 10, drawBox.Center.Y - 5);

                    


                        Graphics.DrawText(bestZone.FinalScore.ToString("0"), position, Color.Black, 3);
                        Graphics.DrawBox(drawBox, backColor);

                     
                        //Graphics.DrawBox(drawBox2, Color.Green);
                    }
                    else
                    {
                        var drawBox = new RectangleF(drawRect.X, drawRect.Y + 15, drawRect.Width, -15);
                        var position = new Vector2(drawBox.Center.X - 20, drawBox.Center.Y - 7);

                        var drawBox2 = new RectangleF(drawRect.X, drawRect.Y + 35, drawRect.Width, -15);
                        var position2 = new Vector2(drawBox2.Center.X - 16, drawBox2.Center.Y - 7);


                        Graphics.DrawText(auxNPC, position, Color.Black, 10);
                        Graphics.DrawBox(drawBox, backColor);

                        Graphics.DrawText(bestZone.FinalScore.ToString("0"), position2, Color.Black, 10);
                        Graphics.DrawBox(drawBox2, backColor);

                    }



                    //Graphics.DrawText(bestZone.FinalScore.ToString(), new Vector2(drawRect.X - 2, drawRect.Y - 2), Color.White);

                }
                }
            }
        }





    }
}
