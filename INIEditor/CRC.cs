﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INIEditor
{
    class CRC
    {
        private static CRC inst = null;

        public static CRC GetInstance()
        {
            if (inst != null)
                return inst;
            else
            {
                inst = new CRC();
                return inst;
            }
        }

        private CRC()
        {
            InitFLHashContext();
        }

        //CRC32 table for Freelancer
        private static uint[] flcrc32tbl =
	     {
		     0x0,	0x9073096,	0x120E612C,	0x1B0951BA,	
		     0xFF6DC419,	0xF66AF48F,	0xED63A535,	0xE46495A3,	
		     0xFEDB8832,	0xF7DCB8A4,	0xECD5E91E,	0xE5D2D988,	
		     0x1B64C2B,	0x8B17CBD,	0x13B82D07,	0x1ABF1D91,	
		     0xFDB71064,	0xF4B020F2,	0xEFB97148,	0xE6BE41DE,	
		     0x2DAD47D,	0xBDDE4EB,	0x10D4B551,	0x19D385C7,	
		     0x36C9856,	0xA6BA8C0,	0x1162F97A,	0x1865C9EC,	
		     0xFC015C4F,	0xF5066CD9,	0xEE0F3D63,	0xE7080DF5,	
		     0xFB6E20C8,	0xF269105E,	0xE96041E4,	0xE0677172,	
		     0x403E4D1,	0xD04D447,	0x160D85FD,	0x1F0AB56B,	
		     0x5B5A8FA,	0xCB2986C,	0x17BBC9D6,	0x1EBCF940,	
		     0xFAD86CE3,	0xF3DF5C75,	0xE8D60DCF,	0xE1D13D59,	
		     0x6D930AC,	0xFDE003A,	0x14D75180,	0x1DD06116,	
		     0xF9B4F4B5,	0xF0B3C423,	0xEBBA9599,	0xE2BDA50F,	
		     0xF802B89E,	0xF1058808,	0xEA0CD9B2,	0xE30BE924,	
		     0x76F7C87,	0xE684C11,	0x15611DAB,	0x1C662D3D,	
		     0xF6DC4190,	0xFFDB7106,	0xE4D220BC,	0xEDD5102A,	
		     0x9B18589,	0xB6B51F,	0x1BBFE4A5,	0x12B8D433,	
		     0x807C9A2,	0x100F934,	0x1A09A88E,	0x130E9818,	
		     0xF76A0DBB,	0xFE6D3D2D,	0xE5646C97,	0xEC635C01,	
		     0xB6B51F4,	0x26C6162,	0x196530D8,	0x1062004E,	
		     0xF40695ED,	0xFD01A57B,	0xE608F4C1,	0xEF0FC457,	
		     0xF5B0D9C6,	0xFCB7E950,	0xE7BEB8EA,	0xEEB9887C,	
		     0xADD1DDF,	0x3DA2D49,	0x18D37CF3,	0x11D44C65,	
		     0xDB26158,	0x4B551CE,	0x1FBC0074,	0x16BB30E2,	
		     0xF2DFA541,	0xFBD895D7,	0xE0D1C46D,	0xE9D6F4FB,	
		     0xF369E96A,	0xFA6ED9FC,	0xE1678846,	0xE860B8D0,	
		     0xC042D73,	0x5031DE5,	0x1E0A4C5F,	0x170D7CC9,	
		     0xF005713C,	0xF90241AA,	0xE20B1010,	0xEB0C2086,	
		     0xF68B525,	0x66F85B3,	0x1D66D409,	0x1461E49F,	
		     0xEDEF90E,	0x7D9C998,	0x1CD09822,	0x15D7A8B4,	
		     0xF1B33D17,	0xF8B40D81,	0xE3BD5C3B,	0xEABA6CAD,	
		     0xEDB88320,	0xE4BFB3B6,	0xFFB6E20C,	0xF6B1D29A,	
		     0x12D54739,	0x1BD277AF,	0xDB2615,	0x9DC1683,	
		     0x13630B12,	0x1A643B84,	0x16D6A3E,	0x86A5AA8,	
		     0xEC0ECF0B,	0xE509FF9D,	0xFE00AE27,	0xF7079EB1,	
		     0x100F9344,	0x1908A3D2,	0x201F268,	0xB06C2FE,	
		     0xEF62575D,	0xE66567CB,	0xFD6C3671,	0xF46B06E7,	
		     0xEED41B76,	0xE7D32BE0,	0xFCDA7A5A,	0xF5DD4ACC,	
		     0x11B9DF6F,	0x18BEEFF9,	0x3B7BE43,	0xAB08ED5,	
		     0x16D6A3E8,	0x1FD1937E,	0x4D8C2C4,	0xDDFF252,	
		     0xE9BB67F1,	0xE0BC5767,	0xFBB506DD,	0xF2B2364B,	
		     0xE80D2BDA,	0xE10A1B4C,	0xFA034AF6,	0xF3047A60,	
		     0x1760EFC3,	0x1E67DF55,	0x56E8EEF,	0xC69BE79,	
		     0xEB61B38C,	0xE266831A,	0xF96FD2A0,	0xF068E236,	
		     0x140C7795,	0x1D0B4703,	0x60216B9,	0xF05262F,	
		     0x15BA3BBE,	0x1CBD0B28,	0x7B45A92,	0xEB36A04,	
		     0xEAD7FFA7,	0xE3D0CF31,	0xF8D99E8B,	0xF1DEAE1D,	
		     0x1B64C2B0,	0x1263F226,	0x96AA39C,	0x6D930A,	
		     0xE40906A9,	0xED0E363F,	0xF6076785,	0xFF005713,	
		     0xE5BF4A82,	0xECB87A14,	0xF7B12BAE,	0xFEB61B38,	
		     0x1AD28E9B,	0x13D5BE0D,	0x8DCEFB7,	0x1DBDF21,	
		     0xE6D3D2D4,	0xEFD4E242,	0xF4DDB3F8,	0xFDDA836E,	
		     0x19BE16CD,	0x10B9265B,	0xBB077E1,	0x2B74777,	
		     0x18085AE6,	0x110F6A70,	0xA063BCA,	0x3010B5C,	
		     0xE7659EFF,	0xEE62AE69,	0xF56BFFD3,	0xFC6CCF45,	
		     0xE00AE278,	0xE90DD2EE,	0xF2048354,	0xFB03B3C2,	
		     0x1F672661,	0x166016F7,	0xD69474D,	0x46E77DB,	
		     0x1ED16A4A,	0x17D65ADC,	0xCDF0B66,	0x5D83BF0,	
		     0xE1BCAE53,	0xE8BB9EC5,	0xF3B2CF7F,	0xFAB5FFE9,	
		     0x1DBDF21C,	0x14BAC28A,	0xFB39330,	0x6B4A3A6,	
		     0xE2D03605,	0xEBD70693,	0xF0DE5729,	0xF9D967BF,	
		     0xE3667A2E,	0xEA614AB8,	0xF1681B02,	0xF86F2B94,	
		     0x1C0BBE37,	0x150C8EA1,	0xE05DF1B,	0x702EF8D,	
	     };

        //Many thanks to Anton's CRCTool source for the algorithm and the CRC table!
        public uint ComputeCRC(String str)
        {
            uint crc;

            crc = 0xFFFFFFFF;

            char[] cstr = str.ToCharArray();

            for (uint i = 0; i < (uint)(str.Length); i++)
                crc = ((crc >> 8) & 0x00FFFFFF) ^ flcrc32tbl[(crc ^ (cstr[i])) & 0xFF];

            crc = crc ^ 0xFFFFFFFF;

            return crc;
        }

        private uint[] hashContext;

        private static readonly uint FLHASH_POLYNOMIAL = 0xA001u;

        private static readonly uint LOGICAL_BITS = 30;
        private static readonly uint PHYSICAL_BITS = 32;
        private static readonly uint SHIFTED_POLY = (FLHASH_POLYNOMIAL << (int)(LOGICAL_BITS - 16));

        void InitFLHashContext()
        {
            hashContext = new uint[256];

            uint i, bit;
            for (i = 0; i < 256; i++)
            {
                uint x = i;
                for (bit = 0; bit < 8; bit++)
                    x = (x & 1) == 1 ? (x >> 1) ^ SHIFTED_POLY : x >> 1;
                hashContext[i] = x;
            }
        }

        private uint BSwap32(uint x)
        {
            return (x >> 24)
                | ((x >> 8) & 0x0000FF00u)
                | ((x << 8) & 0x00FF0000u)
                | (x << 24);
        }

        public uint ComputeHash(String text)
        {
            char[] cstr = text.ToLower().ToCharArray();
            uint hash = 0;
            for (uint i = 0; i < text.Length; i++)
                hash = (hash >> 8) ^ hashContext[(byte)hash ^ (byte)(cstr[i])];
            /* b0rken because byte swapping is not the same as bit reversing, but 
            that's just the way it is; two hash bits are shifted out and lost */
            return (BSwap32(hash) >> (int)(PHYSICAL_BITS - LOGICAL_BITS)) | 0x80000000u;
        }
    }
}
