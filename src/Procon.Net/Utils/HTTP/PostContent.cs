﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Procon.Net.Utils.HTTP {

    public class PostContent {

        public String Boundry { get; protected set; }

        public List<PostParameter> Params { get; protected set; }

        public PostContent() {
            this.Params = new List<PostParameter>();

            this.Boundry = "----" + MD5.String("Ike says I'm special!" + DateTime.Now.ToLongDateString());
        }

        /// <summary>
        /// Returns the parameters array formatted for multi-part/form data
        /// </summary>
        /// <returns></returns>
        public byte[] BuildPostData() {
            MemoryStream stream = new MemoryStream();

            foreach (PostParameter param in this.Params) {
                stream.WriteLine("--{0}", this.Boundry);

                stream.Write(param.BuildHeader());
            }

            // Output the footer
            stream.Write("--{0}--", this.Boundry);

            return stream.ToArray();
        }

    }
}