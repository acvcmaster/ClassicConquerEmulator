using System;
using System.IO;
using System.Text;

namespace CCO.Cryptography
{
    public class PasswordCryptography
    {
        private static uint[] _key = new uint[] {
                                        0xEBE854BC, 0xB04998F7, 0xFFFAA88C, 0x96E854BB, 
                                        0xA9915556, 0x48E44110, 0x9F32308F, 0x27F41D3E, 
                                        0xCF4F3523, 0xEAC3C6B4, 0xE9EA5E03, 0xE5974BBA, 
                                        0x334D7692, 0x2C6BCF2E, 0xDC53B74,  0x995C92A6, 
                                        0x7E4F6D77, 0x1EB2B79F, 0x1D348D89, 0xED641354, 
                                        0x15E04A9D, 0x488DA159, 0x647817D3, 0x8CA0BC20, 
                                        0x9264F7FE, 0x91E78C6C, 0x5C9A07FB, 0xABD4DCCE, 
                                        0x6416F98D, 0x6642AB5B };

        private static uint LeftRotate(uint dwVar, uint dwOffset)
        {
            uint dwTemp1, dwTemp2;

            dwOffset = dwOffset & 0x1F;
            dwTemp1 = dwVar >> (int)(32 - dwOffset);
            dwTemp2 = dwVar << (int)dwOffset;
            dwTemp2 = dwTemp2 | dwTemp1;

            return dwTemp2;
        }

        private static uint RightRotate(uint dwVar, uint dwOffset)
        {
            uint dwTemp1, dwTemp2;

            dwOffset = dwOffset & 0x1F;
            dwTemp1 = dwVar << (int)(32 - dwOffset);
            dwTemp2 = dwVar >> (int)dwOffset;
            dwTemp2 = dwTemp2 | dwTemp1;

            return dwTemp2;
        }

        public static byte[] Encrypt(string password)
        {
            byte[] result = new byte[16];
            Encoding.ASCII.GetBytes(password).CopyTo(result, 0);
            BinaryReader reader = new BinaryReader(new MemoryStream(result, false));
            uint[] passInts = new uint[4];
            for (uint i = 0; i < 4; i++)
                passInts[i] = (uint)reader.ReadInt32();

            uint temp1, temp2;
            for (int i = 1; i >= 0; i--)
            {
                temp1 = _key[5] + passInts[(i * 2) + 1];
                temp2 = _key[4] + passInts[i * 2];
                for (int j = 0; j < 12; j++)
                {
                    temp2 = LeftRotate(temp1 ^ temp2, temp1) + _key[j * 2 + 6];
                    temp1 = LeftRotate(temp1 ^ temp2, temp2) + _key[j * 2 + 7];
                }
                passInts[i * 2] = temp2;
                passInts[i * 2 + 1] = temp1;
            }

            BinaryWriter writer = new BinaryWriter(new MemoryStream(result, true));
            for (uint i = 0; i < 4; i++)
                writer.Write((int)passInts[i]);
            return result;
        }

        public static string Decrypt(byte[] bytes)
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(bytes, false));
            uint[] passInts = new uint[4];
            for (uint i = 0; i < 4; i++)
                passInts[i] = (uint)reader.ReadInt32();

            uint temp1, temp2;
            for (int i = 1; i >= 0; i--)
            {
                temp1 = passInts[(i * 2) + 1];
                temp2 = passInts[i * 2];
                for (int j = 11; j >= 0; j--)
                {
                    temp1 = RightRotate(temp1 - _key[j * 2 + 7], temp2) ^ temp2;
                    temp2 = RightRotate(temp2 - _key[j * 2 + 6], temp1) ^ temp1;
                }
                passInts[i * 2 + 1] = temp1 - _key[5];
                passInts[i * 2] = temp2 - _key[4];
            }

            BinaryWriter writer = new BinaryWriter(new MemoryStream(bytes, true));
            for (uint i = 0; i < 4; i++)
                writer.Write((int)passInts[i]);
            for (int i = 0; i < 16; i++)
                if (bytes[i] == 0)
                    return Encoding.ASCII.GetString(bytes, 0, i);
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
