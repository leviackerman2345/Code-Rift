using System.Windows.Forms;
using CodeRift.Core;
using CodeRift.Managers;
using CodeRift.Utils;

namespace CodeRift.Forms
{
    public static class StoryScripts
    {
        public static StoryConfig CreatePrologue()
        {
            var lang = LanguageManager.Instance;
            StoryConfig config = new StoryConfig(
                title: "Prologue",
                musicKey: Constants.MUSIC_PROLOGUE,
                showFinishButtonOnLastStep: false,
                finishButtonText: "Continue",
                finishAction: form =>
                {
                    LevelsMenuForm levelsMenu = new LevelsMenuForm();
                    if (!FormTransitionManager.ShowChild(form, levelsMenu, () =>
                    {
                        form.Close();
                        return false;
                    }))
                    {
                        levelsMenu.Dispose();
                    }
                });

            config.AddStep(lang.Get("prologue_step_1"), Constants.CG_01);
            config.AddStep(lang.Get("prologue_step_2"), Constants.CG_02);
            config.AddStep(lang.Get("prologue_step_3"), Constants.CG_02);
            config.AddStep(lang.Get("prologue_step_4"), Constants.CG_02);
            config.AddStep(lang.Get("prologue_step_5"), Constants.CG_03);
            config.AddStep(lang.Get("prologue_step_6"), Constants.CG_03);
            config.AddStep(lang.Get("prologue_step_7"), Constants.CG_04);
            config.AddStep(lang.Get("prologue_step_8"), Constants.CG_04);
            config.AddStep(lang.Get("prologue_step_9"), Constants.CG_04);
            config.AddStep(lang.Get("prologue_step_10"), Constants.CG_05);
            config.AddStep(lang.Get("prologue_step_11"), Constants.CG_05);
            config.AddStep(lang.Get("prologue_step_12"), Constants.CG_05);
            config.AddStep(lang.Get("prologue_step_13"), Constants.CG_06);
            config.AddStep(lang.Get("prologue_step_14"), Constants.CG_06);
            config.AddStep(lang.Get("prologue_step_15"), Constants.CG_06);
            config.AddStep(lang.Get("prologue_step_16"), Constants.CG_06);
            config.AddStep(lang.Get("prologue_step_17"), Constants.CG_07);
            config.AddStep(lang.Get("prologue_step_18"), Constants.CG_07);
            config.AddStep(lang.Get("prologue_step_19"), Constants.CG_07);
            config.AddStep(lang.Get("prologue_step_20"), Constants.CG_07);
            config.AddStep(lang.Get("prologue_step_21"), Constants.CG_07);
            config.AddStep(lang.Get("prologue_step_22"), Constants.CG_07);
            config.AddStep(lang.Get("prologue_step_23"), Constants.CG_08);
            config.AddStep(lang.Get("prologue_step_24"), Constants.CG_08);
            config.AddStep(lang.Get("prologue_step_25"), Constants.CG_08);
            config.AddStep(lang.Get("prologue_step_26"), Constants.CG_08);
            config.AddStep(lang.Get("prologue_step_27"), Constants.CG_08);
            config.AddStep(lang.Get("prologue_step_28"), Constants.CG_09);
            config.AddStep(lang.Get("prologue_step_29"), Constants.CG_09);
            config.AddStep(lang.Get("prologue_step_30"), Constants.CG_09);
            config.AddStep(lang.Get("prologue_step_31"), Constants.CG_09);
            config.AddStep(lang.Get("prologue_step_32"), Constants.CG_10);
            config.AddStep(lang.Get("prologue_step_33"), Constants.CG_10);
            config.AddStep(lang.Get("prologue_step_34"), Constants.CG_10);
            config.AddStep(lang.Get("prologue_step_35"), Constants.CG_10);
            config.AddStep(lang.Get("prologue_step_36"), Constants.CG_11);
            config.AddStep(lang.Get("prologue_step_37"), Constants.CG_11);
            config.AddStep(lang.Get("prologue_step_38"), Constants.CG_11);
            config.AddStep(lang.Get("prologue_step_39"), Constants.CG_11);
            config.AddStep(lang.Get("prologue_step_40"), Constants.CG_11);
            config.AddStep(lang.Get("prologue_step_41"), Constants.CG_12);
            config.AddStep(lang.Get("prologue_step_42"), Constants.CG_12);
            config.AddStep(lang.Get("prologue_step_43"), Constants.CG_12);
            config.AddStep(lang.Get("prologue_step_44"), Constants.CG_12);
            config.AddStep(lang.Get("prologue_step_45"), Constants.CG_13);
            config.AddStep(lang.Get("prologue_step_46"), Constants.CG_13);
            config.AddStep(lang.Get("prologue_step_47"), Constants.CG_13);
            config.AddStep(lang.Get("prologue_step_48"), Constants.CG_13);
            config.AddStep(lang.Get("prologue_step_49"), Constants.CG_13);
            config.AddStep(lang.Get("prologue_step_50"), Constants.CG_13);
            config.AddStep(lang.Get("prologue_step_51"), Constants.CG_13);
            config.AddStep(lang.Get("prologue_step_52"), Constants.CG_13);

            return config;
        }

        public static StoryConfig CreateEpilogue()
        {
            var lang = LanguageManager.Instance;
            StoryConfig config = new StoryConfig(
                title: "Epilogue",
                musicKey: Constants.MUSIC_EPILOGUE,
                showFinishButtonOnLastStep: true,
                finishButtonText: "System.Reboot();",
                finishAction: form =>
                {
                    CreditsForm creditsForm = new CreditsForm();
                    if (!FormTransitionManager.ShowChild(form, creditsForm, () =>
                    {
                        foreach (Form openForm in Application.OpenForms)
                        {
                            if (openForm is LevelsMenuForm)
                            {
                                openForm.Tag = "EXIT_TO_MENU";
                                break;
                            }
                        }

                        form.Close();
                        return false;
                    }))
                    {
                        creditsForm.Dispose();
                    }
                });

            config.AddStep(lang.Get("epilogue_step_1"), Constants.EP_01);
            config.AddStep(lang.Get("epilogue_step_2"), Constants.EP_01);
            config.AddStep(lang.Get("epilogue_step_3"), Constants.EP_02);
            config.AddStep(lang.Get("epilogue_step_4"), Constants.EP_02);
            config.AddStep(lang.Get("epilogue_step_5"), Constants.EP_02);
            config.AddStep(lang.Get("epilogue_step_6"), Constants.EP_03);
            config.AddStep(lang.Get("epilogue_step_7"), Constants.EP_03);
            config.AddStep(lang.Get("epilogue_step_8"), Constants.EP_03);
            config.AddStep(lang.Get("epilogue_step_9"), Constants.EP_04);
            config.AddStep(lang.Get("epilogue_step_10"), Constants.EP_04);
            config.AddStep(lang.Get("epilogue_step_11"), Constants.EP_04);
            config.AddStep(lang.Get("epilogue_step_12"), Constants.EP_05);
            config.AddStep(lang.Get("epilogue_step_13"), Constants.EP_05);
            config.AddStep(lang.Get("epilogue_step_14"), Constants.EP_05);
            config.AddStep(lang.Get("epilogue_step_15"), Constants.EP_06);
            config.AddStep(lang.Get("epilogue_step_16"), Constants.EP_06);
            config.AddStep(lang.Get("epilogue_step_17"), Constants.EP_07);
            config.AddStep(lang.Get("epilogue_step_18"), Constants.EP_07);
            config.AddStep(lang.Get("epilogue_step_19"), Constants.EP_08);
            config.AddStep(lang.Get("epilogue_step_20"), Constants.EP_08);
            config.AddStep(lang.Get("epilogue_step_21"), Constants.EP_08);
            config.AddStep(lang.Get("epilogue_step_22"), Constants.EP_09);
            config.AddStep(lang.Get("epilogue_step_23"), Constants.EP_09);
            config.AddStep(lang.Get("epilogue_step_24"), Constants.EP_09);
            config.AddStep(lang.Get("epilogue_step_25"), Constants.EP_09);
            config.AddStep(lang.Get("epilogue_step_26"), Constants.EP_09);
            config.AddStep(lang.Get("epilogue_step_27"), Constants.EP_10);
            config.AddStep(lang.Get("epilogue_step_28"), Constants.EP_10);
            config.AddStep(lang.Get("epilogue_step_29"), Constants.EP_11);

            return config;
        }
    }
}
