﻿// Copyright 2011 Geoffrey 'Phogue' Green
// 
// http://www.phogue.net
//  
// This file is part of Potato 2.
// 
// Potato 2 is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Potato 2 is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Potato 2.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Potato.Net.Utils {
    public class Format {

        public static string Bytes(int bytes) {
            const int scale = 1024;

            string[] orders = new string[] { "TiB", "GiB", "MiB", "KiB", "Bytes" };
            int max = (int)Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders) {
                if (bytes > max) {
                    return string.Format("{0:##.##} {1}", Decimal.Divide(bytes, max), order);
                }

                max /= scale;
            }

            return "0 Bytes";
        }

    }
}
