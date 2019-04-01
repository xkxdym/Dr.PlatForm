
#region EncryptsHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Helpers
* 类 名 称 ：EncryptsHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-4-1 10:59:46
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dr.Common.Helpers
{
    /// <summary> 
    ///常用加密方法
    /// </summary> 
    public class EncryptsHelper
    {
        #region MD5 加密
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public static string Md5(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            using (MD5 md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty).ToLower();
            }
        }

        #endregion

        #region SHA1 加密

        /// <summary>
        ///  SHA1 加密
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <returns>返回加密后的字符串</returns>
        public string SHA1(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }
            using(SHA1 sha1 = System.Security.Cryptography.SHA1.Create())
            {
                return BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", string.Empty);
            }
        }
        #endregion

        #region DES 加密 解密

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8个字符</param>
        /// <returns>返回加密后的字符串</returns>
        public static string DESEncrypt(string str, string key,CipherMode model = CipherMode.CBC,PaddingMode padding = PaddingMode.PKCS7)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 8)
            {
                throw new ArgumentException("DES密钥要求为8个字符");
            }
            //默认密钥向量
            byte[] ivBytes = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] toEncryptBytes = Encoding.UTF8.GetBytes(str);

            using (DES des = DES.Create())
            {
                des.Key = keyBytes;
                des.IV = ivBytes;
                des.Mode = model;
                des.Padding = padding;
                using (var encryptor = des.CreateEncryptor())
                {
                    return Convert.ToBase64String(encryptor.TransformFinalBlock(toEncryptBytes, 0, toEncryptBytes.Length));
                }
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="str">待解密的字符串</param>
        /// <param name="key">解密密钥，要求为8个字符</param>
        /// <returns>返回解密后的字符串</returns>
        public static string DESDecrypt(string str, string key, CipherMode model = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 8)
            {
                throw new ArgumentException("DES密钥要求为8个字符");
            }
            //默认密钥向量
            byte[] ivBytes = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] toDecryptBytes = Convert.FromBase64String(str);

            using (DES des = DES.Create())
            {
                des.Key = keyBytes;
                des.IV = ivBytes;
                des.Mode = model;
                des.Padding = padding;
                using (var decryptor = des.CreateDecryptor())
                {
                    return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(toDecryptBytes, 0, toDecryptBytes.Length));
                }
            }
        }

        #endregion

        #region AES 加密 解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">待加密字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>返回加密后字符串</returns>
        public static string AESEncrypt(string str, string key, CipherMode model = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            {
                throw new ArgumentException("AES密钥要求为16/24/32个字符");
            }

            byte[] toEncryptBytes = Encoding.UTF8.GetBytes(str);

            using (Aes aes=Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = model;
                aes.Padding = padding;

                using (var encryptor = aes.CreateEncryptor())
                {
                    return Convert.ToBase64String(encryptor.TransformFinalBlock(toEncryptBytes, 0, toEncryptBytes.Length));
                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">待解密的字符串</param>
        /// <param name="key">密钥</param>
        /// <returns>返回解密后字符串</returns>
        public static string AESDecrypt(string str, string key, CipherMode model = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            if (keyBytes.Length != 16 && keyBytes.Length != 24 && keyBytes.Length != 32)
            {
                throw new ArgumentException("AES密钥要求为16/24/32个字符");
            }

            byte[] toDecryptBytes = Convert.FromBase64String(str);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode =model;
                aes.Padding = padding;

                using (var decryptor = aes.CreateDecryptor())
                {
                    return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(toDecryptBytes, 0, toDecryptBytes.Length));
                }
            }
        }

        #endregion

        #region  Base64 加密 解密 

        /// <summary>
        /// Base64 加密
        /// </summary>
        /// <param name="str">待加密的字符串</param>
        /// <param name="encoding">编码方式默认ASCII</param>
        /// <returns></returns>
        public static string Base64Encrypt(string str,Encoding encoding=null)
        {
            return Convert.ToBase64String((encoding??Encoding.ASCII).GetBytes(str));
        }

        /// <summary>
        /// Base64 解密
        /// </summary>
        /// <param name="base64str">待解密的字符串</param>
        /// <param name="encoding">编码方式默认ASCII</param>
        /// <returns></returns>
        public static string Base64Decrypt(string base64str, Encoding encoding = null)
        {
            return (encoding ?? Encoding.ASCII).GetString(Convert.FromBase64String(base64str));
        }
        
        #endregion
    }
}
