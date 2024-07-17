using System.Collections.Generic;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace ExpeditionPogbooks
{
    public class ExpeditionPogbooksSettings : ISettings
    {


        public ToggleNode DebugMode { get; set; }

       
        public RangeNode<float> level68 { get; set; }
        public RangeNode<float> level69 { get; set; }
        public RangeNode<float> level70 { get; set; }
        public RangeNode<float> level71 { get; set; }
        public RangeNode<float> level72 { get; set; }
        public RangeNode<float> level73 { get; set; }
        public RangeNode<float> level74 { get; set; }
        public RangeNode<float> level75 { get; set; }
        public RangeNode<float> level76 { get; set; }
        public RangeNode<float> level77 { get; set; }
        public RangeNode<float> level78 { get; set; }
        public RangeNode<float> level79 { get; set; }
        public RangeNode<float> level80 { get; set; }
        public RangeNode<float> level81 { get; set; }
        public RangeNode<float> level82 { get; set; }
        public RangeNode<float> level83 { get; set; }
        public RangeNode<float> level84 { get; set; }
        public RangeNode<float> level85 { get; set; }




        public RangeNode<float> ContainsBoss { get; set; } 
        public RangeNode<float> PlacementRange { get; set; }
        public RangeNode<float> ExplosiveNumber { get; set; }
        public RangeNode<float> ExplosionRadius { get; set; } 
        public RangeNode<float> RunicMonsterMarks { get; set; } 
        public RangeNode<float> ArtifactQuantity { get; set; } 
        public RangeNode<float> NumberOfRemnants { get; set; } 
        public RangeNode<float> MapExpeditionChestCount { get; set; } 
        public RangeNode<float> ExtraRemnantChance { get; set; } 
        public RangeNode<float> ChestDoubleDrop { get; set; }
        public RangeNode<float> UndergroundArea { get; set; } 
        public RangeNode<float> NumberOfMonsterMarkers { get; set; }




        public RangeNode<float> GwennenBoss { get; set; } 
        public RangeNode<float> RogBoss { get; set; } 
        public RangeNode<float> DannigBoss { get; set; } 
        public RangeNode<float> TujenBoss { get; set; }


        public RangeNode<float> ForestRuins { get; set; } 
        public RangeNode<float> DriedRiverbed { get; set; }
        public RangeNode<float> VaalTemple { get; set; } 
        public RangeNode<float> ShipwreckReef { get; set; } 
        public RangeNode<float> Bluffs { get; set; } 
        public RangeNode<float> KaruiWargraves { get; set; } 
        public RangeNode<float> BattlegroundGraves { get; set; } 
        public RangeNode<float> UtzaalOutskirts { get; set; } 
        public RangeNode<float> Cemetery { get; set; } 
        public RangeNode<float> DesertRuins { get; set; } 
        public RangeNode<float> Scrublands { get; set; } 
        public RangeNode<float> VolcanicIsland { get; set; }
        public RangeNode<float> RottingTemple { get; set; } 
        public RangeNode<float> SarnSlums { get; set; } 
        public RangeNode<float> Mountainside { get; set; } 



        public RangeNode<float> DruidsoftheBrokenCircle { get; set; }
        public RangeNode<float> OrderoftheChalice { get; set; } 
        public RangeNode<float> KnightsoftheSun { get; set; }
        public RangeNode<float> BlackScytheMercenaries { get; set; }



        public ToggleNode Enable { get; set; } = new ToggleNode(false);

        public ExpeditionPogbooksSettings()
        {
            DebugMode = new ToggleNode(false);
            level68 = new RangeNode<float>(0.24f, 0, 3);
            level69 = new RangeNode<float>(0.3f, 0, 3);
            level70 = new RangeNode<float>(0.3f, 0, 3);
            level71 = new RangeNode<float>(0.3f, 0, 3);
            level72 = new RangeNode<float>(0.4f, 0, 3);
            level73 = new RangeNode<float>(0.4f, 0, 3);
            level74 = new RangeNode<float>(0.4f, 0, 3);
            level75 = new RangeNode<float>(0.5f, 0, 3);
            level76 = new RangeNode<float>(0.5f, 0, 3);
            level77 = new RangeNode<float>(0.6f, 0, 3);
            level78 = new RangeNode<float>(0.6f, 0, 3);
            level79 = new RangeNode<float>(0.7f, 0, 3);
            level80 = new RangeNode<float>(0.8f, 0, 3);
            level81 = new RangeNode<float>(0.8f, 0, 3);
            level82 = new RangeNode<float>(0.9f, 0, 3);
            level83 = new RangeNode<float>(1f, 0, 3);
            level84 = new RangeNode<float>(1f, 0, 3);
            level85 = new RangeNode<float>(1f, 0, 3);

            ContainsBoss = new RangeNode<float>(1f, 0, 3);
            PlacementRange = new RangeNode<float>(1.5f, 0, 3);
            ExplosiveNumber = new RangeNode<float>(1.5f, 0, 3);
            ExplosionRadius = new RangeNode<float>(1.5f, 0, 3);
            RunicMonsterMarks = new RangeNode<float>(1.5f, 0, 3);
            ArtifactQuantity = new RangeNode<float>(1.5f, 0, 3);
            NumberOfRemnants = new RangeNode<float>(1.15f, 0, 3);
            MapExpeditionChestCount = new RangeNode<float>(1.1f, 0, 3);
            ExtraRemnantChance = new RangeNode<float>(1.1f, 0, 3);
            ChestDoubleDrop = new RangeNode<float>(1.1f, 0, 3);
            UndergroundArea = new RangeNode<float>(1.1f, 0, 3);
            NumberOfMonsterMarkers = new RangeNode<float>(1.05f, 0, 3);




            GwennenBoss = new RangeNode<float>(1f, 0, 10);
            RogBoss = new RangeNode<float>(1.5f, 0, 10);
            DannigBoss = new RangeNode<float>(7.5f, 0, 10);
            TujenBoss = new RangeNode<float>(5.5f, 0, 10);


            ForestRuins = new RangeNode<float>(4.5f, 0, 10);
            DriedRiverbed = new RangeNode<float>(4.5f, 0, 10);
            VaalTemple = new RangeNode<float>(4.5f, 0, 10);
            ShipwreckReef = new RangeNode<float>(4.5f, 0, 10);
            Bluffs = new RangeNode<float>(2.4f, 0, 10);
            KaruiWargraves = new RangeNode<float>(2.5f, 0, 10);
            BattlegroundGraves = new RangeNode<float>(2.1f, 0, 10);
            UtzaalOutskirts = new RangeNode<float>(2.1f, 0, 10);
            Cemetery = new RangeNode<float>(2.1f, 0, 10);
            DesertRuins = new RangeNode<float>(2f, 0, 10);
            Scrublands = new RangeNode<float>(1.8f, 0, 10);
            VolcanicIsland = new RangeNode<float>(1.8f, 0, 10);
            RottingTemple = new RangeNode<float>(1.5f, 0, 10);
            SarnSlums = new RangeNode<float>(1.5f, 0, 10);
            Mountainside = new RangeNode<float>(1.6f, 0, 10);



            DruidsoftheBrokenCircle = new RangeNode<float>(2f, 0, 20);
            OrderoftheChalice = new RangeNode<float>(3f, 0, 20);
            KnightsoftheSun = new RangeNode<float>(15f, 0, 20);
            BlackScytheMercenaries = new RangeNode<float>(11f, 0, 20);

        }
    }

}