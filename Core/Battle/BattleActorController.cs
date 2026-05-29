using System;
using System.Drawing;

namespace CodeRift.Core
{
    public abstract class BattleActorController
    {
        protected BattleActorController(int attackFrameCount, int idleFrameCount)
        {
            RunFrames = new Image[attackFrameCount];
            IdleFrames = new Image[idleFrameCount];
            AttackFrames = new Image[attackFrameCount];
            HurtFrames = new Image[attackFrameCount];
        }

        public Image[] RunFrames { get; private set; }
        public Image[] IdleFrames { get; private set; }
        public Image[] AttackFrames { get; private set; }
        public Image[] HurtFrames { get; private set; }

        public Point Position { get; set; }
        public int IdleX { get; set; }
        public int IdleY { get; set; }
        public int ContactX { get; set; }
        public Size RenderSize { get; set; }
        public Image CurrentImage { get; private set; }

        public void SetRunFrame(int frameIndex)
        {
            CurrentImage = RunFrames[frameIndex];
        }

        public void SetIdleFrame(int frameIndex)
        {
            CurrentImage = IdleFrames[frameIndex];
        }

        public void SetAttackFrame(int frameIndex)
        {
            CurrentImage = AttackFrames[frameIndex];
        }

        public void SetHurtFrame(int frameIndex)
        {
            CurrentImage = HurtFrames[frameIndex];
        }

        public void SetCurrentImage(Image image)
        {
            CurrentImage = image;
        }

        public void SetPosition(int x, int y)
        {
            Position = new Point(x, y);
        }

        public void SetPositionX(int x)
        {
            Position = new Point(x, Position.Y);
        }

        public void MoveXTowards(int targetX, int speed)
        {
            if (speed <= 0)
            {
                return;
            }

            if (Position.X < targetX)
            {
                Position = new Point(Math.Min(targetX, Position.X + speed), Position.Y);
                return;
            }

            Position = new Point(Math.Max(targetX, Position.X - speed), Position.Y);
        }
    }
}
