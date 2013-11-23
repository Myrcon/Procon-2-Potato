﻿using System;
using System.Text;

namespace Procon.Net.Protocols.Frostbite {
    public class FrostbitePacketSerializer : IPacketSerializer {
        /// <summary>
        /// The minimum packet size requires to be passed into the packet serializer. Anything smaller
        /// and it the full header of a packet wouldn't be available, therefore we wouldn't know
        /// how many bytes the full packet is.
        /// </summary>
        public uint PacketHeaderSize { get; set; }

        public FrostbitePacketSerializer()
            : base() {
                this.PacketHeaderSize = 12;
        }

        /// <summary>
        /// Serializes a packet into an array of bytes to send to the server.
        /// </summary>
        /// <param name="packet">The packe to serialize</param>
        /// <returns>An array of bytes to send to the server.</returns>
        public byte[] Serialize(Packet packet) {
            FrostbitePacket frostbitePacket = packet as FrostbitePacket;
            byte[] serialized = null;

            if (frostbitePacket != null) {
                // Construct the header uint32
                UInt32 header = frostbitePacket.RequestId != null ? (UInt32)frostbitePacket.RequestId & 0x3fffffff : 0x3fffffff;

                if (frostbitePacket.Origin == PacketOrigin.Server) {
                    header |= 0x80000000;
                }

                if (frostbitePacket.Type == PacketType.Response) {
                    header |= 0x40000000;
                }

                // Construct the remaining packet headers
                UInt32 packetSize = this.PacketHeaderSize;
                UInt32 wordCount = Convert.ToUInt32(frostbitePacket.Words.Count);

                // Encode each word (WordLength, Word Bytes, Null Byte)
                byte[] encodedWords = new byte[] { };
                foreach (string word in frostbitePacket.Words) {

                    string convertedWord = word;

                    // Truncate words over 64 kbs (though the string is Unicode it gets converted below so this does make sense)
                    if (convertedWord.Length > UInt16.MaxValue - 1) {
                        convertedWord = convertedWord.Substring(0, UInt16.MaxValue - 1);
                    }

                    byte[] appendEncodedWords = new byte[encodedWords.Length + convertedWord.Length + 5];

                    encodedWords.CopyTo(appendEncodedWords, 0);

                    BitConverter.GetBytes(convertedWord.Length).CopyTo(appendEncodedWords, encodedWords.Length);
                    Encoding.GetEncoding(1252).GetBytes(convertedWord + Convert.ToChar(0x00)).CopyTo(appendEncodedWords, encodedWords.Length + 4);

                    encodedWords = appendEncodedWords;
                }

                // Get the full size of the packet.
                packetSize += Convert.ToUInt32(encodedWords.Length);

                // Now compile the whole packet.
                serialized = new byte[packetSize];

                BitConverter.GetBytes(header).CopyTo(serialized, 0);
                BitConverter.GetBytes(packetSize).CopyTo(serialized, 4);
                BitConverter.GetBytes(wordCount).CopyTo(serialized, 8);
                encodedWords.CopyTo(serialized, this.PacketHeaderSize);
            }

            return serialized;
        }

        /// <summary>
        /// Deserializes an array of bytes into a Packet of type P
        /// </summary>
        /// <param name="packetData">The array to deserialize to a packet. Must be exact length of bytes.</param>
        /// <returns>A new packet with data extracted from packetDate</returns>
        public Packet Deserialize(byte[] packetData) {

            FrostbitePacket packet = new FrostbitePacket();

            int header = BitConverter.ToInt32(packetData, 0);
            //this.PacketSize = BitConverter.ToInt32(packet, 4);
            int wordsTotal = BitConverter.ToInt32(packetData, 8);

            packet.Origin = Convert.ToBoolean(header & 0x80000000) == true ? PacketOrigin.Server : PacketOrigin.Client;

            packet.Type = Convert.ToBoolean(header & 0x40000000) == false ? PacketType.Request : PacketType.Response;
            packet.RequestId = header & 0x3fffffff;

            int iWordOffset = 0;

            for (UInt32 wordCount = 0; wordCount < wordsTotal; wordCount++) {
                UInt32 wordLength = BitConverter.ToUInt32(packetData, (int)this.PacketHeaderSize + iWordOffset);

                packet.Words.Add(Encoding.GetEncoding(1252).GetString(packetData, (int)this.PacketHeaderSize + iWordOffset + 4, (int)wordLength));

                iWordOffset += Convert.ToInt32(wordLength) + 5; // WordLength + WordSize + NullByte
            }

            return packet;
        }

        /// <summary>
        /// Fetches the full packet size by reading the header of a packet.
        /// </summary>
        /// <param name="packetData">The possibly incomplete packet data, or as much data as we have recieved from the server.</param>
        /// <returns>The total size, in bytes, that is requires for the header + data to be deserialized.</returns>
        public long ReadPacketSize(byte[] packetData) {
            long length = 0;

            if (packetData.Length >= this.PacketHeaderSize) {
                length = BitConverter.ToUInt32(packetData, 4);
            }

            return length;
        }
    }
}
