
#region TExtension 声明
/**************************************************************
* 命名空间 ：Dr.Common.Extensions
* 类 名 称 ：TExtension
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-29 11:55:00
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dr.Common.Extensions
{
    /// <summary>
    /// T 泛型扩展
    /// </summary>
    public static class TExtension
    {

        /// <summary>
        /// 模型对象比较
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newModel">新的模型</param>
        /// <param name="oldModel">旧的模型</param>
        /// <param name="prefix">前缀</param>
        /// <param name="propDes">模型属性描述</param>
        /// <param name="exceptProps">不进行比较的属性</param>
        /// <returns></returns>
        public static string CompareModel<T>(this T newModel, T oldModel, string prefix = "修改信息:", Dictionary<string, string> propDes = null, List<string> exceptProps = null)
            where T : class, new()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (newModel != null && oldModel != null)
                {
                    Type type = typeof(T);

                    var props = type.GetProperties();

                    if (props != null)
                    {
                        if (exceptProps == null)
                        {
                            exceptProps = new List<string>();
                        }
                        foreach (var prop in props)
                        {
                            try
                            {
                                var name = prop.Name;
                                if (exceptProps.Exists(a => a.ToLower() == name.ToLower()))
                                {
                                    continue;
                                }
                                var propType = prop.PropertyType;
                                var newValue = prop.GetValue(newModel,null);
                                var oldValue = prop.GetValue(oldModel,null);

                                if (newValue == null && oldValue == null)
                                {
                                    continue;
                                }
                                if (propType.IsValueType || propType.FullName == typeof(String).FullName)
                                {
                                    if (newValue.Equals(oldValue))
                                    {
                                        continue;
                                    }

                                    if (propDes != null && propDes.ContainsKey(name))
                                    {
                                        name = propDes[prop.Name];
                                    }

                                    if (newValue == null)
                                    {
                                        //删除
                                        sb.Append($"删除[{name}]({oldValue}),");
                                    }
                                    else
                                    {
                                        if (oldValue == null)
                                        {
                                            //添加
                                            sb.Append($"添加[{name}]({newValue}),");
                                        }
                                        else
                                        {
                                            //更新
                                            sb.Append($"更新[{name}]由({oldValue})到({newValue}),");
                                        }
                                    }
                                }
                                else
                                {
                                    sb.Append(CompareModel(newValue, oldValue));
                                }
                            }
                            catch
                            { }
                        }
                    }
                }
            }
            catch
            {}
            var changeStr = sb.ToString().Trim(',');
            if (string.IsNullOrEmpty(changeStr))
            {
                changeStr = "没有字段更新！";
            }
            return $"{prefix}{changeStr}";
        }
    }
}

