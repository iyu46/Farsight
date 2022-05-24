using ImGuiNET;
using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;
using Lumina.Excel.GeneratedSheets;

namespace Farsight
{
    // It is good to have this be disposable in general, in case you ever need it
    // to do any cleanup
    class PluginUI : IDisposable
    {
        private Configuration configuration;

        private ImGuiScene.TextureWrap goatImage;

        // this extra bool exists for ImGui, since you can't ref a property
        private bool visible = false;
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        private bool settingsVisible = false;
        public bool SettingsVisible
        {
            get { return this.settingsVisible; }
            set { this.settingsVisible = value; }
        }

        // passing in the image here just for simplicity
        public PluginUI(Configuration configuration, ImGuiScene.TextureWrap goatImage)
        {
            this.configuration = configuration;
            this.goatImage = goatImage;
        }

        public void Dispose()
        {
            this.goatImage.Dispose();
        }

        public void Draw()
        {
            // This is our only draw handler attached to UIBuilder, so it needs to be
            // able to draw any windows we might have open.
            // Each method checks its own visibility/state to ensure it only draws when
            // it actually makes sense.
            // There are other ways to do this, but it is generally best to keep the number of
            // draw delegates as low as possible.

            //DrawMainWindow();
            DrawSettingsWindow();
        }

        public void DrawMainWindow()
        {
            //if (!Visible)
            //{
            //    return;
            //}

            //ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
            //ImGui.SetNextWindowSizeConstraints(new Vector2(375, 330), new Vector2(float.MaxValue, float.MaxValue));
            //if (ImGui.Begin("Farsight Config", ref this.visible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            //{
            //    ImGui.Text($"The enabled bool is {this.configuration.Enabled}");

            //    var configValue = this.configuration.Enabled;
            //    if (ImGui.Checkbox("Enabled", ref configValue))
            //    {
            //        this.configuration.Enabled = configValue;
            //        // can save immediately on change, if you don't want to provide a "Save and Close" button
            //        this.configuration.Save();
            //    }

            //    if (ImGui.Button("Show Settings"))
            //    {
            //        SettingsVisible = true;
            //    }

            //    ImGui.Spacing();

            //    ImGui.Text("Have a goat:");
            //    ImGui.Indent(55);
            //    ImGui.Image(this.goatImage.ImGuiHandle, new Vector2(this.goatImage.Width, this.goatImage.Height));
            //    ImGui.Unindent(55);
            //}
            //ImGui.End();
        }

        public void DrawSettingsWindow()
        {
            if (!SettingsVisible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(375, 330), ImGuiCond.FirstUseEver);
            if (ImGui.Begin("Farsight Config", ref this.settingsVisible, ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse))
            {
                ImGui.Text($"The enabled bool is {this.configuration.Enabled}");

                string currentHomeworld = Plugin.ClientState.LocalPlayer?.CurrentWorld?.GameData?.Name;
                var currentDCEnum = Plugin.DataManager.GetExcelSheet<World>()!
                                        .First(row => row.Name == currentHomeworld).DataCenter.Row;
                string currentDC = Plugin.DataManager.GetExcelSheet<WorldDCGroupType>()!
                                    .First(row => row.RowId == currentDCEnum).Name.ToString();
                var currentDCHomeworlds = Plugin.DataManager.GetExcelSheet<World>()!
                                .Where(row => row.IsPublic == true &&
                                       row.DataCenter.Row == currentDCEnum)
                                .Select(row => row.Name.ToString())
                                .OrderBy(name => name)
                                .ToList();
                ImGui.Text($"The current world is {currentHomeworld}");
                ImGui.Text($"The current dc is {currentDC}");

                var configValue = this.configuration.Enabled;
                if (ImGui.Checkbox("Enabled", ref configValue))
                {
                    this.configuration.Enabled = configValue;
                    // can save immediately on change, if you don't want to provide a "Save and Close" button
                    this.configuration.Save();
                }

                if (ImGui.BeginTable("PlayerTable", 4))
                {
                    /* table headers */
                    ImGui.TableSetupColumn("Player Name");
                    ImGui.TableSetupColumn("Homeworld");
                    ImGui.TableSetupColumn("Colour");
                    ImGui.TableSetupColumn("Active?  ", ImGuiTableColumnFlags.NoSort | ImGuiTableColumnFlags.WidthFixed);
                    ImGui.TableHeadersRow();
                    ImGui.TableNextRow();

                    //for (int row = 1; row < 4; row++)
                    //{
                    //    //ImGui::TableNextRow();
                    //    //for (int column = 0; column < 3; column++)
                    //    //{
                    //    //    ImGui::TableSetColumnIndex(column);
                    //    //    ImGui::Text("Row %d Column %d", row, column);
                    //    //}
                    //}

                    /* new entry submission */
                    
                    this.configuration.CurrentInputPlayer.World = currentHomeworld;
                    ImGui.TableSetColumnIndex(1);
                    //ImGui.InputText("playerName", ref inputPlayerName, 33);
                    ImGui.Text($"The current selected homeworld is {this.configuration.CurrentInputPlayer.World}");
                    if (ImGui.BeginCombo("ServerSelectorCombo", this.configuration.CurrentInputPlayer.World))
                    {
                        for (int i = 0; i < currentDCHomeworlds.Count; i++)
                        {
                            bool is_selected = (this.configuration.CurrentInputPlayer.World == currentDCHomeworlds[i]);
                            if (ImGui.Selectable(currentDCHomeworlds[i], is_selected))
                                this.configuration.CurrentInputPlayer.World = currentDCHomeworlds[i];
                                this.configuration.Save();
                        }
                        ImGui.EndCombo();
                    }

                    ImGui.EndTable();
                }
            }
            ImGui.End();
        }
    }
}
