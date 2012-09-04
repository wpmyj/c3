﻿using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Xdgk.Common;

namespace C3.Remote
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ExecuteArgs
    {
        #region StationName
        /// <summary>
        /// 
        /// </summary>
        public string StationName
        {
            get
            {
                if (_stationName == null)
                {
                    _stationName = string.Empty;
                }
                return _stationName;
            }
            set
            {
                _stationName = value;
            }
        } private string _stationName;
        #endregion //StationName

        #region DeviceAddress
        /// <summary>
        /// 
        /// </summary>
        public UInt64 DeviceAddress
        {
            get
            {
                return _deviceAddress;
            }
            set
            {
                _deviceAddress = value;
            }
        } private UInt64 _deviceAddress;
        #endregion //DeviceAddress

        #region ExecuteName
        /// <summary>
        /// 
        /// </summary>
        public string ExecuteName
        {
            get
            {
                if (_executeName == null)
                {
                    _executeName = string.Empty;
                }
                return _executeName;
            }
            set
            {
                _executeName = value;
            }
        } private string _executeName;
        #endregion //ExecuteName

        #region HashTable
        /// <summary>
        /// 
        /// </summary>
        public Hashtable HashTable
        {
            get
            {
                if (_hashTable == null)
                {
                    _hashTable = new Hashtable();
                }
                return _hashTable;
            }
            set
            {
                _hashTable = value;
            }
        } private Hashtable _hashTable;
        #endregion //HashTable
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ResultArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public ExecuteArgs ExecuteArgs
        {
            get { return _executeArgs; }
            set { _executeArgs = value; }
        } private ExecuteArgs _executeArgs;

        #region IsComplete
        public bool IsComplete
        {
            get { return _isComplete; }
            set { _isComplete = value; }
        } private bool _isComplete;
        #endregion //IsComplete

        #region IsSuccess
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }
            set
            {
                _isSuccess = value;
            }
        } private bool _isSuccess;
        #endregion //IsSuccess

        #region Message
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get
            {
                if (_message == null)
                {
                    _message = string.Empty;
                }
                return _message;
            }
            set
            {
                _message = value;
            }
        } private string _message;
        #endregion //Message

        /// <summary>
        /// 
        /// </summary>
        public KeyValueCollection KeyValues
        {
            get { return _keyValues;  }
            set { _keyValues = value; }
        } private KeyValueCollection _keyValues;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="status"></param>
    public delegate void ResultDelegate(ResultArgs args);

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class CallbackWrapper : MarshalByRefObject
    {
        private ResultDelegate _callbackDelegate;

        public CallbackWrapper(ResultDelegate cbTarget)
        {
            if (cbTarget == null)
            {
                throw new ArgumentNullException("cbTarget");
            }

            this._callbackDelegate = cbTarget;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Callback(ResultArgs args)
        {
            if (_callbackDelegate != null)
            {
                this._callbackDelegate(args);
            }
        }
    }


    [Serializable]
    public enum ResultEnum
    {
        Success,
        Fail,
        Unsure,
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Result
    {

        /// <summary>
        /// 
        /// </summary>
        public ResultEnum ResultEnum
        {
            get { return _resultEnum; }
            set { _resultEnum = value; }
        } private ResultEnum _resultEnum;

        /// <summary>
        /// 
        /// </summary>
        public string FailMessage
        {
            get { return _failMessage; }
            set { _failMessage = value; }
        } private string _failMessage;

        /// <summary>
        /// 
        /// </summary>
        public object Values
        {
            get { return _values; }
            set { _values = value; }
        } private object _values;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExecuteEventArgs : EventArgs
    {
        public ExecuteArgs Parameter;
        public Result Result;
        public CallbackWrapper CallbackWrapper;
    }

    public delegate void ExecuteEventHandler(object sender, ExecuteEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public class RemoteObject : MarshalByRefObject
    {
        static public event ExecuteEventHandler Executeing;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="cbWrapper"></param>
        /// <returns></returns>
        public Result Execute(ExecuteArgs parameter, CallbackWrapper cbWrapper)
        {
            if (Executeing != null)
            {
                ExecuteEventArgs e = new ExecuteEventArgs();
                e.Parameter = parameter;
                if (cbWrapper != null)
                {
                    e.CallbackWrapper = cbWrapper;
                }

                Executeing(this, e);

                return e.Result;
            }
            else
            {
                return null;
            }
        }
    }
}
