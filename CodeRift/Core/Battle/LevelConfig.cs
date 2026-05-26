using System.Collections.Generic;

namespace CodeRift.Core
{
    public sealed class EnemyConfig
    {
        public EnemyConfig(
            string name,
            string assetFolder,
            string portraitFileName,
            float renderScale = 1.0f,
            bool centerActorsByWidth = false,
            int actorIdleGap = 20,
            int playerAttackContactOverlap = 150,
            int enemyAttackContactOverlap = 150,
            int groundYOffset = 0)
        {
            Name = name;
            AssetFolder = assetFolder;
            PortraitFileName = portraitFileName;
            RenderScale = renderScale;
            CenterActorsByWidth = centerActorsByWidth;
            ActorIdleGap = actorIdleGap;
            PlayerAttackContactOverlap = playerAttackContactOverlap;
            EnemyAttackContactOverlap = enemyAttackContactOverlap;
            GroundYOffset = groundYOffset;
        }

        public string Name { get; }

        public string AssetFolder { get; }

        public string PortraitFileName { get; }

        public float RenderScale { get; }

        public bool CenterActorsByWidth { get; }

        public int ActorIdleGap { get; }

        public int PlayerAttackContactOverlap { get; }

        public int EnemyAttackContactOverlap { get; }

        public int GroundYOffset { get; }
    }

    public sealed class LevelConfig
    {
        private static readonly Dictionary<int, LevelConfig> ConfigByLevel = new()
        {
            { 1, new LevelConfig(1, new EnemyConfig("LOOPBUG", "enemy1", "enemy_level_1.jpeg", 1.12f)) },
            { 2, new LevelConfig(2, new EnemyConfig("VOID_CRAWLER", "enemy2", "enemy_level_2.jpeg", 1.75f, true, 40, 300, 340)) },
            { 3, new LevelConfig(3, new EnemyConfig("STRING_CORRUPTOR", "enemy3", "enemy_level_3.jpeg", 1.75f, true, 40, 300, 340)) },
            { 4, new LevelConfig(4, new EnemyConfig("ARRAY_CRASHER", "enemy4", "enemy_level_4.jpeg", 1.75f, true, 40, 300, 340)) },
            { 5, new LevelConfig(5, new EnemyConfig("NULL_KING", "enemy5", "enemy_level_5.jpeg", 1.75f, true, 40, 300, 340, 115), opensEpilogueOnWin: true) }
        };

        public int Level { get; }

        public EnemyConfig Enemy { get; }

        public bool OpensEpilogueOnWin { get; }

        public string EnemyName => Enemy.Name;

        public string EnemyAssetFolder => Enemy.AssetFolder;

        public float EnemyRenderScale => Enemy.RenderScale;

        public bool CenterActorsByWidth => Enemy.CenterActorsByWidth;

        public int ActorIdleGap => Enemy.ActorIdleGap;

        public int PlayerAttackContactOverlap => Enemy.PlayerAttackContactOverlap;

        public int EnemyAttackContactOverlap => Enemy.EnemyAttackContactOverlap;

        public int EnemyGroundYOffset => Enemy.GroundYOffset;

        private LevelConfig(int level, EnemyConfig enemy, bool opensEpilogueOnWin = false)
        {
            Level = level;
            Enemy = enemy;
            OpensEpilogueOnWin = opensEpilogueOnWin;
        }

        public static LevelConfig ForLevel(int level)
        {
            return ConfigByLevel.TryGetValue(level, out LevelConfig? config)
                ? config
                : new LevelConfig(level, new EnemyConfig("UNKNOWN", "enemy1", "enemy_level_1.jpeg"));
        }
    }
}
