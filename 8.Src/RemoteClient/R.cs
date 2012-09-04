﻿using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xdgk.Common;
//using ControllerIn;
//using C3.Remote;

namespace RemoteClient
{
    public class RemoteController : _1100ControllerInterface
    {
        #region _1100ControllerInterface 成员

        /// <summary>
        /// 
        /// </summary>
        public void Doit(ExecuteArgs args)
        {
            RemoteObject obj = this.GetRemoteObject();
            CallbackWrapper w = new CallbackWrapper(new ResultDelegate(Target));
            obj.Execute(args, w);
        }

        #region Dispose
        public void Dispose()
        {
            if (_tcpChannel != null)
            {
                ChannelServices.UnregisterChannel(_tcpChannel);
            }
        }
        #endregion //Dispose
        #endregion

        TcpChannel _tcpChannel;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private RemoteObject GetRemoteObject()
        {
            if (_remoteObject == null)
            {

                IDictionary tcpProperties = new Hashtable();

                BinaryClientFormatterSinkProvider tcpClientSinkProvider =
                    new BinaryClientFormatterSinkProvider();

                BinaryServerFormatterSinkProvider tcpServerSinkProvider =
                    new BinaryServerFormatterSinkProvider();

                tcpServerSinkProvider.TypeFilterLevel =
                    System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;

                tcpProperties["timeout"] = 5000;
                tcpProperties["port"] = 0;

                _tcpChannel = new TcpChannel(
                    tcpProperties,
                    tcpClientSinkProvider,
                    tcpServerSinkProvider);

                ChannelServices.RegisterChannel(_tcpChannel, false);


                _remoteObject = (RemoteObject)Activator.GetObject(
                               typeof(RemoteObject),
                               "tcp://127.0.0.1:9000/RO"
                               );
            }

            return _remoteObject;
        } private RemoteObject _remoteObject;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        private void Target(ResultArgs args)
        {
            //Console.WriteLine("RemoteController Target: {0} {1} {2} {3} ",
            //    args.IsComplete,
            //    args.IsSuccess,
            //    args.Message,
            //    args.ExecuteName);

            this._resultArgs = args;
            if (this.CallbackEvent != null)
            {
                this.CallbackEvent(this, EventArgs.Empty);
            }
        }

        #region _1100ControllerInterface 成员


        public event EventHandler CallbackEvent;

        #endregion

        #region _1100ControllerInterface 成员


        /// <summary>
        /// 
        /// </summary>
        public ResultArgs ResultArgs
        {
            get { return _resultArgs; }
        } private ResultArgs _resultArgs;

        #endregion

        #region IDisposable 成员

        #endregion
    }
}