namespace CodeRift.Core
{
    // Holds player-specific animation and render state for the battle scene.
    public sealed class PlayerActorController : BattleActorController
    {
        public PlayerActorController(int attackFrameCount, int idleFrameCount)
            : base(attackFrameCount, idleFrameCount)
        {
        }
    }
}
