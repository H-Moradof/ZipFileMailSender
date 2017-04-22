using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Security
{
    public static class DataEncryptor
    {

        #region fields
        private const string TripleDesIv = @"wH1eFz6s9jQ=";
        private const string TripleDesKey = @"DT59VzTGuq2FNEQEMQ1MPQ==";
        private const string AESIv = @"KtAylJAg/BQG+eU0PHh114F0GKlIALWZbb3eUmc/wHE=";
        private const string AESKey = @"58kDwNgkWEPevGo07XoKHg==";
        private const string Salt = "yisojo?#@jrt9SDFBWR(SDFN@%)zxcIhdv";
        private const string SaltInsertIndexSeperator = "*";
        #endregion


        #region SHA

        public static string EncryptSha(string dataToEncrypt)
        {
            return Convert.ToBase64String(new SHA512Managed().ComputeHash(Encoding.Default.GetBytes(dataToEncrypt)));
        }

        #endregion


        #region Md5

        public static string EncryptMd5(string inputString)
        {
            var encoder = new UTF8Encoding();
            var md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(inputString));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sb.Append(hashedBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        #endregion


        #region MultiEncrypt

        public static string MultiEncrypt(string dataToEncrypt)
        {
            return EncryptMd5(EncryptSha(EncryptMd5(dataToEncrypt)));
        }

        #endregion


        #region EncryptWithSalt

        public static string EncryptWithSalt(string data)
        {
            int saltInsertIndex = data.Length / 2;
            string dataWithSalt = string.Format("{0}{1}{2}", data.Insert(saltInsertIndex, Salt), SaltInsertIndexSeperator, saltInsertIndex);
            string encryptedData = EncryptTpl(dataWithSalt);
            return encryptedData;
        }

        public static string DecryptWithSalt(string encryptedData)
        {
            string dataWithSalt = DecryptTpl(encryptedData);
            string saltInsertIndesStr = dataWithSalt.Substring(dataWithSalt.LastIndexOf(SaltInsertIndexSeperator) + SaltInsertIndexSeperator.Length);
            int saltInsertIndex = int.Parse(saltInsertIndesStr);
            string data = string.Format("{0}{1}", dataWithSalt.Substring(0, saltInsertIndex), dataWithSalt.Substring(saltInsertIndex + Salt.Length));
            data = data.Substring(0, data.LastIndexOf(SaltInsertIndexSeperator));
            return data;
        }

        #endregion


        #region TripleDes

        public static string EncryptTpl(string dataToEncrypt)
        {
            return TplEncryptor(dataToEncrypt);
        }

        public static string DecryptTpl(string encryptedData)
        {
            return TplDecryptor(encryptedData);
        }


        public static byte[] EncryptTpl(byte[] bytesToEncrypt)
        {
            return TplEncryptor(bytesToEncrypt);
        }

        public static byte[] DecryptTpl(byte[] encryptedBytes)
        {
            return TplDecryptor(encryptedBytes);
        }

        #endregion


        #region AES

        /// <summary>
        /// Use Default secret if set secret null
        /// </summary>
        /// <param name="dataToEncrypt"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string EncryptAES(string dataToEncrypt, string secret = null)
        {
            return AesEncryptor(dataToEncrypt, secret);
        }


        /// <summary>
        /// Use Default secret if set secret null
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string DecryptAES(string encryptedData, string secret = null)
        {
            return AesDecryptor(encryptedData, secret);
        }

        #endregion


        #region private methods

        private static string AesEncryptor(string dataToEncrypt, string secret)
        {
            byte[] keyBytes = string.IsNullOrEmpty(secret) ? Convert.FromBase64String(AESKey) : Convert.FromBase64String(secret);
            byte[] ivBytes = Convert.FromBase64String(AESIv);
            byte[] encrypted;

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.BlockSize = 256;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (var encryptor = aes.CreateEncryptor(keyBytes, ivBytes))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(dataToEncrypt);
                            }
                            encrypted = ms.ToArray();
                        }
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        private static string AesDecryptor(string base64EncryptedData, string secret)
        {
            byte[] keyBytes = string.IsNullOrEmpty(secret) ? Convert.FromBase64String(AESKey) : Convert.FromBase64String(secret);
            byte[] ivBytes = Convert.FromBase64String(AESIv);
            byte[] cipher = Convert.FromBase64String(base64EncryptedData);

            string result = string.Empty;

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.BlockSize = 256;
                aes.KeySize = 256;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(keyBytes, ivBytes))
                {
                    using (MemoryStream ms = new MemoryStream(cipher))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                result = sr.ReadToEnd();
                            }
                        }
                    }
                }
            }

            return result;
        }


        private static string TplEncryptor(string clearData)
        {
            var mStream = new MemoryStream();
            var binFormatter = new BinaryFormatter();
            binFormatter.Serialize(mStream, clearData);
            var mStream2 = new MemoryStream(mStream.ToArray());
            var tDesSp = new TripleDESCryptoServiceProvider
            {
                IV = Convert.FromBase64String(TripleDesIv),
                Key = Convert.FromBase64String(TripleDesKey)
            };

            new CryptoStream(mStream2, tDesSp.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] hashedArray = new byte[mStream2.Length];
            mStream2.Read(hashedArray, 0, hashedArray.Length);

            mStream.Dispose();
            mStream2.Dispose();

            string hashedData = Convert.ToBase64String(hashedArray);
            return hashedData;
        }

        private static string TplDecryptor(string hashedData)
        {
            var mStream = new MemoryStream(Convert.FromBase64String(hashedData));

            var tDesProvider = new TripleDESCryptoServiceProvider
            {
                IV = Convert.FromBase64String(TripleDesIv),
                Key = Convert.FromBase64String(TripleDesKey)
            };

            new CryptoStream(
                mStream,
                tDesProvider.CreateDecryptor(tDesProvider.Key, tDesProvider.IV),
                CryptoStreamMode.Read);

            byte[] hashedArray = new byte[mStream.Length];
            mStream.Read(hashedArray, 0, hashedArray.Length);

            MemoryStream mStream2 = new MemoryStream(hashedArray);
            BinaryFormatter binFormatter = new BinaryFormatter();
            string clearData = (string)binFormatter.Deserialize(mStream2);

            mStream.Dispose();
            mStream2.Dispose();

            return clearData;
        }



        private static byte[] TplEncryptor(byte[] clearData)
        {
            var ms = new MemoryStream(clearData);
            var tDesSp = new TripleDESCryptoServiceProvider
            {
                IV = Convert.FromBase64String(TripleDesIv),
                Key = Convert.FromBase64String(TripleDesKey)
            };

            new CryptoStream(ms, tDesSp.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] hashedArray = new byte[ms.Length];
            ms.Read(hashedArray, 0, hashedArray.Length);
            ms.Dispose();
            return hashedArray;
        }

        private static byte[] TplDecryptor(byte[] hashedData)
        {
            var ms = new MemoryStream(hashedData);
            var tDesProvider = new TripleDESCryptoServiceProvider
            {
                IV = Convert.FromBase64String(TripleDesIv),
                Key = Convert.FromBase64String(TripleDesKey)
            };

            new CryptoStream(
                ms,
                tDesProvider.CreateDecryptor(tDesProvider.Key, tDesProvider.IV),
                CryptoStreamMode.Read);

            byte[] dehashedArray = new byte[ms.Length];
            ms.Read(dehashedArray, 0, dehashedArray.Length);
            ms.Dispose();
            return dehashedArray;
        }

        #endregion

    }
}
