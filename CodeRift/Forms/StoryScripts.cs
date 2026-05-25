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

            config.AddStep("In the age before the collapse, humanity lived in peace. Nations flourished, technology advanced rapidly, and the world believed its future was secure.", Constants.CG_01);
            config.AddStep("Then came the quake.", Constants.CG_02);
            config.AddStep("Without warning, the earth trembled violently across the globe. Cities crumbled, oceans roared, and the skies darkened. When the disaster finally ended, humanity witnessed something impossible.", Constants.CG_02);
            config.AddStep("Across different parts of the world, enormous fractures had appeared in reality itself.", Constants.CG_02);
            config.AddStep("They glowed with an eerie green light, filled with streams of floating binary digits and distorted symbols. Scientists could not explain them. Survivors would later give them a single name:", Constants.CG_03);
            config.AddStep("â€” The Digital Rift â€”", Constants.CG_03);
            config.AddStep("At first, people believed the rifts were harmless.", Constants.CG_04);
            config.AddStep("They were wrong.", Constants.CG_04);
            config.AddStep("From the depths of the rifts emerged terrifying creatures known as the Bugs â€” corrupted beings formed from broken code, failed logic, and digital corruption. These monsters spread rapidly across cities, destroying everything in their path.", Constants.CG_04);
            config.AddStep("Weapons had little effect against them, and entire civilizations fell within months.", Constants.CG_05);
            config.AddStep("Humanity was pushed to the brink of extinction.", Constants.CG_05);
            config.AddStep("The remaining survivors fled north, hiding deep within frozen mountains where the Bugs struggled to reach. There, humanity built its final sanctuary: the Stronghold.", Constants.CG_05);
            config.AddStep("Years passed.", Constants.CG_06);
            config.AddStep("During an expedition beneath an ancient ruined temple, survivors discovered a mysterious artifact unlike anything they had ever seen.", Constants.CG_06);
            config.AddStep("It was a black metallic device covered in glowing symbols and shifting lines of code.", Constants.CG_06);
            config.AddStep("â€” The Console â€”", Constants.CG_06);
            config.AddStep("Legends claimed the Console possessed the power to destroy the Bugs permanently. However, the artifact could not be activated by ordinary people.", Constants.CG_07);
            config.AddStep("According to ancient records left within the temple, only a chosen individual worthy of the 'Great Compiler' would one day wield its power.", Constants.CG_07);
            config.AddStep("Most believed it was only a myth.", Constants.CG_07);
            config.AddStep("A false hope created to comfort humanity during its final days.", Constants.CG_07);
            config.AddStep("But the Bugs feared the Console.", Constants.CG_07);
            config.AddStep("And deep within the Rift, something was watching.", Constants.CG_07);
            config.AddStep("â€” One Hundred Years Later â€”", Constants.CG_08);
            config.AddStep("For nearly a century, humanity remained hidden under the protection of the Great Compiler â€” a mysterious guardian who watched over the Stronghold and kept the Bugs away.", Constants.CG_08);
            config.AddStep("Peace, however, never lasts forever.", Constants.CG_08);
            config.AddStep("One night, the mountains shook once again.", Constants.CG_08);
            config.AddStep("The Bugs had returned.", Constants.CG_08);
            config.AddStep("This time, they came not as scattered monsters, but as an army.", Constants.CG_09);
            config.AddStep("The Stronghold was overwhelmed. Walls collapsed. Entire districts were consumed by corruption. Thousands were slaughtered while others were dragged away into the darkness beyond the mountains.", Constants.CG_09);
            config.AddStep("At the center of the invasion stood the Great Compiler himself, fighting endlessly to protect the last remnants of humanity.", Constants.CG_09);
            config.AddStep("But even he could not stop them forever.", Constants.CG_09);
            config.AddStep("The Bugs had finally discovered the truth behind the Console.", Constants.CG_10);
            config.AddStep("They had come to destroy it.", Constants.CG_10);
            config.AddStep("Because as long as the Console existed, the Bugs could never truly win.", Constants.CG_10);
            config.AddStep("Mortally wounded and nearing death, the Great Compiler made one final decision.", Constants.CG_10);
            config.AddStep("Using the last of his strength, he merged his spirit into the Console itself, sealing his knowledge, power, and consciousness within the artifact.", Constants.CG_11);
            config.AddStep("Before disappearing, he left behind a final message:", Constants.CG_11);
            config.AddStep("'One day, a worthy soul will compile the future humanity could not.'", Constants.CG_11);
            config.AddStep("As the Stronghold burned, the Console vanished into the chaos.", Constants.CG_11);
            config.AddStep("Lost. Forgotten. Waiting.", Constants.CG_11);
            config.AddStep("Far from the ruins of the capital, in a small hidden village near the northern mountains, a young boy named Elias lived an ordinary life.", Constants.CG_12);
            config.AddStep("Orphaned during earlier Bug attacks, Elias spent his days helping rebuild the village alongside the remaining survivors.", Constants.CG_12);
            config.AddStep("Like many others, he had grown up hearing stories about the Digital Rift, the Bugs, and the legendary Great Compiler.", Constants.CG_12);
            config.AddStep("Stories he never truly believed.", Constants.CG_12);
            config.AddStep("Until the day everything changed.", Constants.CG_13);
            config.AddStep("While searching through the remains of a destroyed caravan near the forest, Elias discovered a strange black device buried beneath the snow.", Constants.CG_13);
            config.AddStep("The moment he touched it, the Console activated.", Constants.CG_13);
            config.AddStep("Green symbols illuminated the darkness. Code flowed across its surface. And a voice echoed within his mind.", Constants.CG_13);
            config.AddStep("The spirit of the Great Compiler had awakened.", Constants.CG_13);
            config.AddStep("The Console recognized Elias as its new wielder.", Constants.CG_13);
            config.AddStep("Through the Console, Elias gained access to the ancient language capable of fighting the Bugs â€” the power of C#.", Constants.CG_13);
            config.AddStep("Under the guidance of the Great Compiler, Elias begins his journey across the corrupted world, learning to master code, battle the Bugs, and uncover the truth behind the Digital Rift before humanity disappears forever.", Constants.CG_13);

            return config;
        }

        public static StoryConfig CreateEpilogue()
        {
            StoryConfig config = new StoryConfig(
                title: "Epilogue",
                musicKey: Constants.MUSIC_EPILOGUE,
                showFinishButtonOnLastStep: true,
                finishButtonText: "Continue",
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

            config.AddStep("After a long and devastating battle, Elias finally defeats the supreme Bug known as: The Null King.", Constants.EP_01);
            config.AddStep("The origin of all corruption born from the Digital Rift.", Constants.EP_01);
            config.AddStep("As the Null King falls, its body begins to break apart into streams of green code and fragmented binary.", Constants.EP_02);
            config.AddStep("The entire Digital Rift Realm trembles violently as the corruption holding it together starts to collapse.", Constants.EP_02);
            config.AddStep("Thenâ€¦ Something unexpected happens.", Constants.EP_02);
            config.AddStep("The corrupted world does not explode. It begins to heal.", Constants.EP_03);
            config.AddStep("Across the Digital Rift, broken structures repair themselves. Glitched skies stabilize.", Constants.EP_03);
            config.AddStep("Distorted landscapes return to normal. The endless streams of corrupted code slowly reorganize into clean flowing data.", Constants.EP_03);
            config.AddStep("The Bugs â€” once terrifying monsters of destruction â€” begin disappearing one by one.", Constants.EP_04);
            config.AddStep("Not dying. But being fixed.", Constants.EP_04);
            config.AddStep("Their corrupted code was finally repaired.", Constants.EP_04);
            config.AddStep("For the first time in over a century, silence fills the world.", Constants.EP_05);
            config.AddStep("The Digital Rift itself starts closing the massive fissures scattered across reality.", Constants.EP_05);
            config.AddStep("One by one, the portals disappear, sealing the connection between the human world and the corrupted realm.", Constants.EP_05);
            config.AddStep("Far across the northern mountains, the remaining survivors watch the skies as the green light slowly fades away.", Constants.EP_06);
            config.AddStep("Humanity was finally free.", Constants.EP_06);
            config.AddStep("As Elias stands within the collapsing Rift Realm, the spirit of the Great Compiler appears before him one final time.", Constants.EP_07);
            config.AddStep("The Great Compiler smiles. â€œYou did not destroy the future, Eliasâ€¦ You corrected it.â€", Constants.EP_07);
            config.AddStep("The Console begins losing its glow. Its purpose had finally been fulfilled.", Constants.EP_08);
            config.AddStep("Before disappearing, the Great Compiler thanks Elias for giving humanity another chance.", Constants.EP_08);
            config.AddStep("Then, like fading code, his spirit vanishes peacefully into the light.", Constants.EP_08);
            config.AddStep("Moments later, Elias escapes the Digital Rift just before the final portal closes forever.", Constants.EP_09);
            config.AddStep("Years later, humanity slowly begins rebuilding civilization. Cities rise again.", Constants.EP_09);
            config.AddStep("The survivors no longer live in fear of the Bugs.", Constants.EP_09);
            config.AddStep("The story of Elias and the Great Compiler becomes a legend passed down across generations.", Constants.EP_09);
            config.AddStep("A reminder that even the most corrupted systems can still be repaired.", Constants.EP_09);
            config.AddStep("But deep beneath the ruins of the old worldâ€¦ A tiny green symbol suddenly flickers in the darkness.", Constants.EP_10);
            config.AddStep("System.Reboot();", Constants.EP_10);
            config.AddStep("The End", Constants.EP_11);

            return config;
        }
    }
}
