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

        public string Name { get; private set; }

        public string AssetFolder { get; private set; }

        public string PortraitFileName { get; private set; }

        public float RenderScale { get; private set; }

        public bool CenterActorsByWidth { get; private set; }

        public int ActorIdleGap { get; private set; }

        public int PlayerAttackContactOverlap { get; private set; }

        public int EnemyAttackContactOverlap { get; private set; }

        public int GroundYOffset { get; private set; }
    }

    public sealed class LevelConfig
    {
        private static readonly Dictionary<int, LevelConfig> ConfigByLevel = new Dictionary<int, LevelConfig>()
        {
            { 1, new LevelConfig(1, new EnemyConfig("LOOPBUG", "enemy1", "enemy_level_1.jpeg", 1.12f)) },
            { 2, new LevelConfig(2, new EnemyConfig("VOID_CRAWLER", "enemy2", "enemy_level_2.jpeg", 1.75f, true, 40, 300, 340)) },
            { 3, new LevelConfig(3, new EnemyConfig("STRING_CORRUPTOR", "enemy3", "enemy_level_3.jpeg", 1.75f, true, 40, 300, 340)) },
            { 4, new LevelConfig(4, new EnemyConfig("ARRAY_CRASHER", "enemy4", "enemy_level_4.jpeg", 1.75f, true, 40, 300, 340)) },
            { 5, new LevelConfig(5, new EnemyConfig("NULL_KING", "enemy5", "enemy_level_5.jpeg", 1.75f, true, 40, 300, 340, 115), opensEpilogueOnWin: true) }
        };

        public int Level { get; private set; }

        public EnemyConfig Enemy { get; private set; }

        public bool OpensEpilogueOnWin { get; private set; }

        public string EnemyName { get { return Enemy.Name; } }

        public string EnemyAssetFolder { get { return Enemy.AssetFolder; } }

        public float EnemyRenderScale { get { return Enemy.RenderScale; } }

        public bool CenterActorsByWidth { get { return Enemy.CenterActorsByWidth; } }

        public int ActorIdleGap { get { return Enemy.ActorIdleGap; } }

        public int PlayerAttackContactOverlap { get { return Enemy.PlayerAttackContactOverlap; } }

        public int EnemyAttackContactOverlap { get { return Enemy.EnemyAttackContactOverlap; } }

        public int EnemyGroundYOffset { get { return Enemy.GroundYOffset; } }

        private LevelConfig(int level, EnemyConfig enemy, bool opensEpilogueOnWin = false)
        {
            Level = level;
            Enemy = enemy;
            OpensEpilogueOnWin = opensEpilogueOnWin;
        }

        public static LevelConfig ForLevel(int level)
        {
            LevelConfig config;
            if (ConfigByLevel.TryGetValue(level, out config))
                return config;
            return new LevelConfig(level, new EnemyConfig("UNKNOWN", "enemy1", "enemy_level_1.jpeg"));
        }
    }
}
