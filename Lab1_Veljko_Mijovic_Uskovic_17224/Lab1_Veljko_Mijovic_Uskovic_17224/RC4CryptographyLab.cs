using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_Veljko_Mijovic_Uskovic_17224
{
    public class RC4CryptographyLab
    {
        //Singleton klasa za rc4 kriptografiju

        private static RC4CryptographyLab instance = null;
        private string key;
        private string latestData;
        private byte[] s = new byte[256];

        private RC4CryptographyLab()
        {
            key = null;
            latestData = null;

            for (int i = 0; i < 256; i++)
                s[i] = (byte)i;
        }

        public static RC4CryptographyLab GetInstance() 
        {
            if (instance != null)
                return instance;
            instance = new RC4CryptographyLab();
            return instance;
        }

        public string Key
        {
            get { return key; }
            set
            {
                byte[] asciiByteKey = Encoding.Convert(Encoding.Unicode, Encoding.ASCII, Encoding.Unicode.GetBytes(value));
                char[] asciiCharsKey = new char[Encoding.ASCII.GetCharCount(asciiByteKey)];
                Encoding.ASCII.GetChars(asciiByteKey, 0, asciiCharsKey.Length);
                key = value;

                for(int i =0; i < 256; i++)
                {
                    s[0] = (byte)i;
                }


                Shuffle(asciiCharsKey);
            }
        }

        public string LatestData
        {
            get { return latestData; }
        }

        private void Shuffle(char[] key)
        {
            int j = 0;

            for (int i = 0; i < 256; i++)
            {
                j = (j + s[i] + key[i % key.Length]) % 256;

                byte temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }
        }

        public byte[] Run(byte[] data)
        {
            if (key == null)
                return null;



            int j = 0;
            byte[] results = new byte[data.Length];

            byte[] s = new byte[256];
            this.s.CopyTo(s, 0);
            int i = 0;
            byte keyCurrent;
            byte temp;
            for (int iData = 0; iData < data.Length; iData++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;

                temp = s[i];
                s[i] = s[j];
                s[j] = temp;


                keyCurrent = s[(s[i] + s[j]) % 256];

                results[iData] = (byte)((int)data[iData] ^ (int)keyCurrent);
                //Console.WriteLine((char)(data[iData] ^ keyCurrent));

            }

            // Console.WriteLine(results.ToString());

            char[] resultChars = new char[Encoding.ASCII.GetCharCount(results, 0, results.Length)];
            Encoding.ASCII.GetChars(results, 0, results.Length, resultChars, 0);
            this.latestData = new string(resultChars);
            return results;
        }

        public string Dencrypt(byte[] cypherText)
        {
            //byte[] cypherBytesDef = Encoding.Default.GetBytes(cypherText);
            //byte[] cypherBytes = Encoding.Convert(Encoding.Default, Encoding.ASCII, cypherBytesDef);

            //   byte[] cypherBytes = Encoding.ASCII.GetBytes(cypherText);

            byte[] r = Run(cypherText);

            char[] resultChars = new char[Encoding.ASCII.GetCharCount(r, 0, r.Length)];
            Encoding.ASCII.GetChars(r, 0, r.Length, resultChars, 0);

            string plainText = new string(resultChars);
            return plainText;
        }

        public byte[] Encrypt(string plainText)
        {
            byte[] plainBytesDef = Encoding.Default.GetBytes(plainText);
            byte[] plainBytes = Encoding.Convert(Encoding.Default, Encoding.ASCII, plainBytesDef);

            //byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            byte[] r = Run(plainBytes);

            return r;
        }
    }
}
