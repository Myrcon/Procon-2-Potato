#region Copyright
// Copyright 2014 Myrcon Pty. Ltd.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.IO;
using System.Net;

namespace Potato.Net.Shared.Geolocation.Maxmind {

    /// <summary>
    /// Summary description for CountryLookup.
    /// </summary>
    public class CountryLookup
    {
        private FileStream fileInput;
        private static long COUNTRY_BEGIN = 16776960;
        private static string[] countryCode = 
                                { "--","AP","EU","AD","AE","AF","AG","AI","AL","AM","AN","AO","AQ","AR","AS","AT","AU","AW","AZ","BA","BB","BD","BE","BF","BG","BH","BI","BJ","BM","BN","BO","BR","BS","BT","BV","BW","BY","BZ","CA","CC","CD","CF","CG","CH","CI","CK","CL","CM","CN","CO","CR","CU","CV","CX","CY","CZ","DE","DJ","DK","DM","DO","DZ",
                                    "EC","EE","EG","EH","ER","ES","ET","FI","FJ","FK","FM","FO","FR","FX","GA","GB","GD","GE","GF","GH","GI","GL","GM","GN","GP","GQ","GR","GS","GT","GU","GW","GY","HK","HM","HN","HR","HT","HU","ID","IE","IL","IN","IO","IQ","IR","IS","IT","JM","JO","JP","KE","KG","KH","KI","KM","KN","KP","KR","KW","KY","KZ",
                                    "LA","LB","LC","LI","LK","LR","LS","LT","LU","LV","LY","MA","MC","MD","MG","MH","MK","ML","MM","MN","MO","MP","MQ","MR","MS","MT","MU","MV","MW","MX","MY","MZ","NA","NC","NE","NF","NG","NI","NL","NO","NP","NR","NU","NZ","OM","PA","PE","PF","PG","PH","PK","PL","PM","PN","PR","PS","PT","PW","PY","QA",
                                    "RE","RO","RU","RW","SA","SB","SC","SD","SE","SG","SH","SI","SJ","SK","SL","SM","SN","SO","SR","ST","SV","SY","SZ","TC","TD","TF","TG","TH","TJ","TK","TM","TN","TO","TL","TR","TT","TV","TW","TZ","UA","UG","UM","US","UY","UZ","VA","VC","VE","VG","VI","VN","VU","WF","WS","YE","YT","RS","ZA","ZM","ME","ZW","A1","A2",
                                    "O1","AX","GG","IM","JE","BL","MF"
                                    };
        private static string[] countryName = 
                                {"N/A","Asia/Pacific Region","Europe","Andorra","United Arab Emirates","Afghanistan","Antigua and Barbuda","Anguilla","Albania","Armenia","Netherlands Antilles","Angola","Antarctica","Argentina","American Samoa","Austria","Australia","Aruba","Azerbaijan","Bosnia and Herzegovina","Barbados","Bangladesh","Belgium",
                                    "Burkina Faso","Bulgaria","Bahrain","Burundi","Benin","Bermuda","Brunei Darussalam","Bolivia","Brazil","Bahamas","Bhutan","Bouvet Island","Botswana","Belarus","Belize","Canada","Cocos (Keeling) Islands","Congo, The Democratic Republic of the","Central African Republic","Congo","Switzerland","Cote D'Ivoire",
                                    "Cook Islands","Chile","Cameroon","China","Colombia","Costa Rica","Cuba","Cape Verde","Christmas Island","Cyprus","Czech Republic","Germany","Djibouti","Denmark","Dominica","Dominican Republic","Algeria","Ecuador","Estonia","Egypt","Western Sahara","Eritrea","Spain","Ethiopia","Finland","Fiji","Falkland Islands (Malvinas)",
                                    "Micronesia, Federated States of","Faroe Islands","France","France, Metropolitan","Gabon","United Kingdom","Grenada","Georgia","French Guiana","Ghana","Gibraltar","Greenland","Gambia","Guinea","Guadeloupe","Equatorial Guinea","Greece","South Georgia and the South Sandwich Islands","Guatemala","Guam","Guinea-Bissau","Guyana",
                                    "Hong Kong","Heard Island and McDonald Islands","Honduras","Croatia","Haiti","Hungary","Indonesia","Ireland","Israel","India","British Indian Ocean Territory","Iraq","Iran, Islamic Republic of","Iceland","Italy","Jamaica","Jordan","Japan","Kenya","Kyrgyzstan","Cambodia","Kiribati","Comoros","Saint Kitts and Nevis",
                                    "Korea, Democratic People's Republic of","Korea, Republic of","Kuwait","Cayman Islands","Kazakstan","Lao People's Democratic Republic","Lebanon","Saint Lucia","Liechtenstein","Sri Lanka","Liberia","Lesotho","Lithuania","Luxembourg","Latvia","Libyan Arab Jamahiriya","Morocco","Monaco","Moldova, Republic of","Madagascar",
                                    "Marshall Islands","Macedonia","Mali","Myanmar","Mongolia","Macau","Northern Mariana Islands","Martinique","Mauritania","Montserrat","Malta","Mauritius","Maldives","Malawi","Mexico","Malaysia","Mozambique","Namibia","New Caledonia","Niger","Norfolk Island","Nigeria","Nicaragua","Netherlands",
                                    "Norway","Nepal","Nauru","Niue","New Zealand","Oman","Panama","Peru","French Polynesia","Papua New Guinea","Philippines","Pakistan","Poland","Saint Pierre and Miquelon","Pitcairn Islands","Puerto Rico","Palestinian Territory","Portugal","Palau","Paraguay","Qatar","Reunion","Romania","Russian Federation","Rwanda","Saudi Arabia",
                                    "Solomon Islands","Seychelles","Sudan","Sweden","Singapore","Saint Helena","Slovenia","Svalbard and Jan Mayen","Slovakia","Sierra Leone","San Marino","Senegal","Somalia","Suriname","Sao Tome and Principe","El Salvador","Syrian Arab Republic","Swaziland","Turks and Caicos Islands","Chad","French Southern Territories","Togo",
                                    "Thailand","Tajikistan","Tokelau","Turkmenistan","Tunisia","Tonga","Timor-Leste","Turkey","Trinidad and Tobago","Tuvalu","Taiwan","Tanzania, United Republic of","Ukraine","Uganda","United States Minor Outlying Islands","United States","Uruguay","Uzbekistan","Holy See (Vatican City State)","Saint Vincent and the Grenadines",
                                    "Venezuela","Virgin Islands, British","Virgin Islands, U.S.","Vietnam","Vanuatu","Wallis and Futuna","Samoa","Yemen","Mayotte","Serbia","South Africa","Zambia","Montenegro","Zimbabwe","Anonymous Proxy","Satellite Provider",
                                    "Other","Aland Islands","Guernsey","Isle of Man","Jersey","Saint Barthelemy","Saint Martin"};
        
        
        public CountryLookup(string fileName)
        {
            try
            {
                fileInput = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File " + fileName + " not found.");
            }
        }

        public string lookupCountryCode(string str) 
        {
            IPAddress addr;
            try 
            {
                addr = IPAddress.Parse(str);
            }
            catch (FormatException e) 
            {
                return "--";
            }

            return lookupCountryCode(addr);
        }

        private long addrToNum(IPAddress addr) 
        {
            long ipnum = 0;
            var b = addr.GetAddressBytes();
            for (var i = 0; i < 4; ++i) 
            {
                long y = b[i];
                if (y < 0) 
                {
                    y+= 256;
                }
                ipnum += y << ((3-i)*8);
            }
            //Console.WriteLine(ipnum);
            return ipnum;
        }

        public string lookupCountryCode(IPAddress addr) 
        {
            var lookupCountryCode = "";

            lock (this) {
                lookupCountryCode = "" + (countryCode[(int)seekCountry(0, addrToNum(addr), 31)]);
            }

            return lookupCountryCode;
        }

        public string lookupCountryName(string str) 
        {
            IPAddress addr;
            try 
            {
                addr = IPAddress.Parse(str);
            }
            catch (FormatException e) 
            {
                return "N/A";
            }
            return lookupCountryName(addr);
        }

        public string lookupCountryName(IPAddress addr) 
        {
            return(countryName[(int)seekCountry(0, addrToNum(addr), 31)]);
        }

        private long seekCountry(long offset, long ipnum, int depth) 
        {
            var buf = new byte[6];
            var x = new long[2];
            if (depth < 0) 
            {
                Console.WriteLine("Error seeking country.");
                return 0; // N/A
            }
            try 
            {
                fileInput.Seek(6 * offset, 0);
                fileInput.Read(buf, 0, 6);
            }
            catch (IOException e) 
            {
                Console.WriteLine("IO Exception");
            }
            for (var i = 0; i<2; i++) 
            {
                x[i] = 0;
                for (var j = 0; j<3; j++) 
                {
                    int y = buf[i*3+j];
                    if (y < 0) 
                    {
                        y+= 256;
                    }
                    x[i] += (y << (j * 8));
                }
            }

            if ((ipnum & (1 << depth)) > 0) 
            {
                if (x[1] >= COUNTRY_BEGIN) 
                {
                    return x[1] - COUNTRY_BEGIN;
                }
                return seekCountry(x[1], ipnum, depth-1);
            } 
            else 
            {
                if (x[0] >= COUNTRY_BEGIN) 
                {
                    return x[0] - COUNTRY_BEGIN;
                }
                return seekCountry(x[0], ipnum, depth-1);
            }
        }
    }
}
