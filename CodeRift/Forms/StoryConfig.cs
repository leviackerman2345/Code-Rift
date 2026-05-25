using System;
using System.Collections.Generic;

namespace CodeRift.Forms
{
    public sealed class StoryStep
    {
        public StoryStep(string text, string imageKey)
        {
            Text = text;
            ImageKey = imageKey;
        }

        public string Text { get; }

        public string ImageKey { get; }
    }

    public sealed class StoryConfig
    {
        public StoryConfig(string title, string musicKey, bool showFinishButtonOnLastStep, string finishButtonText, Action<StoryForm> finishAction)
        {
            Title = title;
            MusicKey = musicKey;
            ShowFinishButtonOnLastStep = showFinishButtonOnLastStep;
            FinishButtonText = finishButtonText;
            FinishAction = finishAction;
        }

        public string Title { get; }

        public string MusicKey { get; }

        public bool ShowFinishButtonOnLastStep { get; }

        public string FinishButtonText { get; }

        public Action<StoryForm> FinishAction { get; }

        public List<StoryStep> Steps { get; } = new List<StoryStep>();

        public void AddStep(string text, string imageKey)
        {
            Steps.Add(new StoryStep(text, imageKey));
        }
    }
}
