using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCO.Cryptography
{
    public class GameCryptography
    {
        private byte[] _cryptKey1, _cryptKey2, _cryptKey3, _cryptKey4;
        private CryptCounter _decryptCounter;
        private CryptCounter _encryptCounter;
        bool _alternate = false;

        public GameCryptography()
        {
            _decryptCounter = new CryptCounter();
            _encryptCounter = new CryptCounter();

            CreateKeys();
        }

        class CryptCounter
        {
            UInt16 m_Counter = 0;

            public byte Key2
            {
                get { return (byte)(m_Counter >> 8); }
            }

            public byte Key1
            {
                get { return (byte)(m_Counter & 0xFF); }
            }

            public void Increment()
            {
                m_Counter++;
            }

            public void Reset()
            {
                m_Counter = 0;
            }
        }

        public void Encrypt(ref byte[] Data)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                Data[i] ^= 0xab; Data[i] = (byte)(Data[i] >> 4 | Data[i] << 4);
                Data[i] ^= (byte)(_cryptKey1[_encryptCounter.Key1] ^ _cryptKey2[_encryptCounter.Key2]);
                _encryptCounter.Increment();
            }
        }

        public void Decrypt(ref byte[] Data)
        {
            if (!_alternate)
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    Data[i] ^= 0xab; Data[i] = (byte)(Data[i] >> 4 | Data[i] << 4);
                    Data[i] ^= (byte)(_cryptKey2[_decryptCounter.Key2] ^ _cryptKey1[_decryptCounter.Key1]);
                    _decryptCounter.Increment();
                }
            }
            else
            {
                for (int i = 0; i < Data.Length; i++)
                {
                    Data[i] ^= 0xab; Data[i] = (byte)(Data[i] >> 4 | Data[i] << 4);
                    Data[i] ^= (byte)(_cryptKey4[_decryptCounter.Key2] ^ _cryptKey3[_decryptCounter.Key1]);
                    _decryptCounter.Increment();
                }
            }
        }

        public void CreateKeys()
        {
            _cryptKey1 = new byte[0x100];
            _cryptKey2 = new byte[0x100];
            Byte iKey1 = 0x9D;
            Byte iKey2 = 0x62;
            for (int i = 0; i < 0x100; i++)
            {
                _cryptKey1[i] = iKey1;
                _cryptKey2[i] = iKey2;
                iKey1 = (byte)((0x0F + (byte)(iKey1 * 0xFA)) * iKey1 + 0x13);
                iKey2 = (byte)((0x79 - (byte)(iKey2 * 0x5C)) * iKey2 + 0x6D);
            }
        }

        public void GenerateKeys(Int32 accountId, Int32 token)
        {
            Int32 tmpkey1 = (Int32)((((token) + accountId) ^ 0x4321) ^ (token));
            Int32 tmpkey2 = (Int32)(tmpkey1 * tmpkey1);
            _cryptKey3 = new byte[0x100]; _cryptKey4 = new byte[0x100];
            byte[] tmp1 = BitConverter.GetBytes(tmpkey1);
            byte[] tmp2 = BitConverter.GetBytes(tmpkey2);
            for (int i = 0; i < 256; i++)
            {
                _cryptKey3[i] = (byte)(_cryptKey1[i] ^ tmp1[i % 4]);
                _cryptKey4[i] = (byte)(_cryptKey2[i] ^ tmp2[i % 4]);
            }
            _alternate = true;
        }
    }
}
