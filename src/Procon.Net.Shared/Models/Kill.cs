﻿using System;
using System.Collections.Generic;

namespace Procon.Net.Shared.Models {
    [Serializable]
    public sealed class Kill : NetworkModel {

        /// <summary>
        /// The location of the target 
        /// </summary>
        public HumanHitLocation HumanHitLocation { get; set; }

        public Kill() : base() {
            // Target
            this.Scope.Content = new List<String>();
            this.Scope.Players = new List<Player>();
            this.Scope.Items = new List<Item>();

            // Killer
            this.Now.Players = new List<Player>();
            this.Now.Items = new List<Item>();
        }
    }
}