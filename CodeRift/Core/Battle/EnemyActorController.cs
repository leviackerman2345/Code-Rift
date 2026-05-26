namespace CodeRift.Core
{
    // Holds enemy-specific animation and render state for the battle scene.
    public sealed class EnemyActorController : BattleActorController
    {
        public EnemyActorController(int attackFrameCount, int idleFrameCount)
            : base(attackFrameCount, idleFrameCount)
        {
        }
    }
}
