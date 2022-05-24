using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace Farsight
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public bool Enabled { get; set; } = false;

        public List<IPlayerTableEntry> Players { get; set; } = new List<IPlayerTableEntry>();

        public IPlayerTableEntry CurrentInputPlayer = new PlayerTableEntry();

        // the below exist just to make saving less cumbersome

        [NonSerialized]
        private DalamudPluginInterface? pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
        }

        public void ResetCurrentInputPlayer()
        {
            this.CurrentInputPlayer = new PlayerTableEntry();
        }

        public void Save()
        {
            this.pluginInterface!.SavePluginConfig(this);
        }
    }
}
