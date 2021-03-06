﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using NUnit.Core;
using NLog;
using Xdgk.Common;
using System.Collections ;

namespace C3.Communi
{
    using Path = System.IO.Path;

    /// <summary>
    /// 
    /// </summary>
    public class BytesConverterManager
    {
        /// <summary>
        /// 
        /// </summary>
        static public BytesConverterManager Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new BytesConverterManager();
                }
                return _default;
            }
        } static private BytesConverterManager _default;

        static private Logger log = LogManager.GetCurrentClassLogger();
        static private string BytesConverterAddinDir = "bc";
        private List<Assembly> _assemblyList= new List<Assembly>();

        //#region BytesConverters
        ///// <summary>
        ///// 
        ///// </summary>
        //public BytesConverterCollection BytesConverters
        //{
        //    get { return _bytesConverterCollection; }
        //} private BytesConverterCollection _bytesConverterCollection = new BytesConverterCollection();
        //#endregion //BytesConverters

        #region BytesConverterManager
        /// <summary>
        /// 
        /// </summary>
        private BytesConverterManager() 
        {
            RegisterAddins();
        }
        #endregion //BytesConverterManager

        #region RegisterAddins
        /// <summary>
        /// 
        /// </summary>
        private void RegisterAddins()
        {
            // Load any extensions in the addins directory
            string addinDirectory = Path.Combine(Application.StartupPath, BytesConverterAddinDir );
            DirectoryInfo dir = new DirectoryInfo(addinDirectory);

            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles("*.dll"))
                {
                    Register(file.FullName);
                }
            }
        }
        #endregion //RegisterAddins


        #region Register
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void Register(string path)
        {
            try
            {
                AssemblyName assemblyName = new AssemblyName();
                assemblyName.Name = Path.GetFileNameWithoutExtension(path);
                assemblyName.CodeBase = path;
                Assembly assembly = Assembly.Load(assemblyName);
                log.Debug("Loaded " + Path.GetFileName(path));
                this._assemblyList.Add(assembly);

                foreach (Type type in assembly.GetExportedTypes())
                {
                    if (type.IsClass &&
                        typeof(IBytesConverter).IsAssignableFrom(type))
                    {
                        if (!type.IsAbstract)
                        {
                            this.RegisteredByteConverterDict.Add(type.FullName, type);
                            log.Info("Registered byte converter addin: {0}", type.FullName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to load bc assembly: {0}\r\n{1}", path, ex);
            }
        }
        #endregion //Register


        public Dictionary<string, Type> RegisteredByteConverterDict
        {
            get 
            {
                if (_registeredByteConverterDict == null)
                {
                    _registeredByteConverterDict = new Dictionary<string, Type>();
                }
                return _registeredByteConverterDict;
            }
        } private Dictionary<string, Type> _registeredByteConverterDict;

        #region GetType
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private Type GetType(string typeName)
        {
            foreach (Assembly a in _assemblyList)
            {
                Type t = a.GetType(typeName);
                if (t != null)
                    return t;
            }
            return null;
        }
        #endregion //GetType


        #region CreateBytesConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public IBytesConverter CreateBytesConverter(string typeName, object[] args)
        {
            Type t = this.GetType(typeName);
            if (t == null)
            {
                string s = string.Format("Cannot create bytes converter by '{0}'", typeName);
                throw new ArgumentException(s);
            }

            return (IBytesConverter)Activator.CreateInstance(t, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public IBytesConverter CreateBytesConverter(BytesConverterConfig cfg)
        {
            if (cfg == null)
            {
                throw new ArgumentNullException("cfg");
            }

            IBytesConverter bc = CreateBytesConverter(cfg.Name, null);
            foreach (object key in cfg.Propertys.Keys)
            {
                string value = cfg.Propertys[key].ToString();
                SetValue(bc, key.ToString(), value);
            }

            if (cfg.HasInner)
            {
                IBytesConverter innerBc = CreateBytesConverter(cfg.InnerBytesConverterConfig);
                bc.InnerBytesConverter = innerBc;
            }

            return bc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        private void SetValue(object obj, string propertyName, string value)
        {
            bool isSet = false;
            PropertyInfo[] pis = obj.GetType().GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (StringHelper.Equal(pi.Name, propertyName))
                {
                    object objValue = Convert.ChangeType(value, pi.PropertyType);
                    pi.SetValue(obj, objValue, null);
                    isSet = true;
                    break;
                }
            }

            if (!isSet)
            {
                string s = string.Format("not set property '{0}'", propertyName);
                throw new ArgumentException(s);
            }
        }

        #endregion //CreateBytesConverter
    }
}
