﻿using System;

namespace Procon.Net.Models {
    /// <summary>
    /// A group identifier to group players by
    /// </summary>
    [Serializable]
    public class Grouping : NetworkModel {

        /// <summary>
        /// A majority of games have teams, teams have squads. So we have these variables
        /// where to simply avoid debugging string issues later.
        /// </summary>
        public static readonly String Team = "Team";

        /// <summary>
        /// A majority of games have teams, teams have squads. So we have these variables
        /// where to simply avoid debugging string issues later.
        /// </summary>
        public static readonly String Squad = "Squad";

        /// <summary>
        /// A majority of games have teams, teams have squads. So we have these variables
        /// where to simply avoid debugging string issues later.
        /// </summary>
        public static readonly String Player = "Player";

        /// <summary>
        /// The type of group this signifies, such as "Team", "Squad" or "Player"
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// The unique identifier for the group
        /// </summary>
        public String Uid { get; set; }

        /// <summary>
        /// A friendly name for the team, potentially used to lookup a localized name.
        /// </summary>
        public String FriendlyName { get; set; }

        /// <summary>
        /// Initializes default values.
        /// </summary>
        public Grouping() {
            this.Type = String.Empty;
            this.Uid = null;
            this.FriendlyName = String.Empty;
        }
    }
}