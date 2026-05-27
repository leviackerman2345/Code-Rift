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

        public string Text { get; private set; }

        public string ImageKey { get; private set; }
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
            Steps = new List<StoryStep>();
        }

        public string Title { get; private set; }

        public string MusicKey { get; private set; }

        public bool ShowFinishButtonOnLastStep { get; private set; }

        public string FinishButtonText { get; private set; }

        public Action<StoryForm> FinishAction { get; private set; }

        public List<StoryStep> Steps { get; private set; }

        public void AddStep(string text, string imageKey)
        {
            Steps.Add(new StoryStep(text, imageKey));
        }
    }
}
