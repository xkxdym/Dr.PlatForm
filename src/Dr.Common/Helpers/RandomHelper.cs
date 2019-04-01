
#region RandomHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Helpers
* 类 名 称 ：RandomHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 13:57:20
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
using System.Text;

namespace Dr.Common.Helpers
{
    /// <summary>
    /// 随机数 帮助类
    /// </summary>
    public class RandomHelper
    {
        private static Random rand;
        private static readonly char[] RandChar;
        private static int s_RoCount;

        private RandomHelper()
        {
        }

        static RandomHelper()
        {
            RandChar = new char[] {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f',
                'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
                'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
             };
            rand = new Random((int)DateTime.Now.Ticks);
            s_RoCount = 1;
        }
        /// <summary>
        /// 获取一般的随机数字
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int GetRandNumeric(int min, int max)
        {
            int num = 0;
            num = new Random(s_RoCount * ((int)DateTime.Now.Ticks)).Next(min, max);
            s_RoCount++;
            return num;
        }
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetRandString(int length)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(GetRandChar());
            }
            return builder.ToString();
        }

        /// <summary>
        /// 根据模型获取随机字符串
        /// </summary>
        /// <param name="pattern">模型##*#?#*（#=数字;*=字母；?=#+*）</param>
        /// <returns></returns>
        public static string GetRandStringByPattern(string pattern)
        {
            if ((!pattern.Contains("#") && !pattern.Contains("?")) && !pattern.Contains("*"))
            {
                return pattern;
            }
            char[] chArray = pattern.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < chArray.Length; i++)
            {
                switch (chArray[i])
                {
                    case '#':
                        chArray[i] = GetRandNum();
                        goto Label_0069;

                    case '*':
                        chArray[i] = GetRandChar();
                        goto Label_0069;

                    case '?':
                        chArray[i] = GetRandWord();
                        break;
                }
                Label_0069:
                builder.Append(chArray[i]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取指定长度的随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRadomString(int length)
        {
            string rs = "abcdefghigklmnopqrstuvwxyz0123456789";
            return GetRadomString(rs, length);
        }

        /// <summary>
        /// 获取指定长度的随机字符串
        /// </summary>
        /// <param name="rs">指定的字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetRadomString(string rs, int length)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(rs) && length > 0)
            {
                int r = rs.Length;
                Random random = new Random();
                for (int i = 0; i < length; ++i)
                {
                    sb.Append(rs[random.Next(r)]);
                }
            }

            return sb.ToString();
        }

        private static char GetRandWord()
        {
            return RandChar[rand.Next(10, 0x3e)];
        }

        private static char GetRandChar()
        {
            return RandChar[rand.Next(0x3e)];
        }

        private static char GetRandNum()
        {
            return RandChar[rand.Next(0, 10)];
        }
    }

}
