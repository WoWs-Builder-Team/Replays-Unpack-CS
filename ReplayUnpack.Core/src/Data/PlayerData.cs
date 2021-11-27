using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ReplaysUnpackCS.Data
{
    /// <summary>
    /// Represents a single player data entry from a replay containing the data for a single player including his name, ID, selected ship, etc.
    /// </summary>
    /// <param name="Properties"></param>
    public record PlayerData(Dictionary<PlayerProperty, object> Properties) : IReplayData
    {
        /// <summary>
        /// Gets the name of the player.
        /// </summary>
        public string Name => Properties[PlayerProperty.Name] as string ?? string.Empty;

        /// <summary>
        /// Gets the selected ship components of the current ship of the player.
        /// </summary>
        public Dictionary<string, string?> ShipComponents => ((Hashtable)Properties[PlayerProperty.ShipComponents])
            .Cast<DictionaryEntry>()
            .ToDictionary(entry => (string)entry.Key, entry => entry.Value as string);
    }

    public record PlayerDataList(List<PlayerData> DataList) : IReplayData;
}