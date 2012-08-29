﻿using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections.Generic;
using System.Text;
using C3.Remote;

namespace RemoteClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpChannel _tcpChannel;
            RemoteObject _remoteObject;


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

            ExecuteParameter p = new ExecuteParameter();
            p.StationName = "st";
            p.DeviceAddress = 123;
            p.ExecuteName = "exe_nn";
            p.HashTable["a"] = "bbb";

            CallbackWrapper w = new CallbackWrapper(Func);
            //w._callbackDelegate = Func;

            try
            {
                Result r = _remoteObject.Execute(p, w);
                if (r != null)
                {
                    Console.WriteLine(r.ResultEnum +" : "+ r.FailMessage );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("key...");
            Console.Read();

        }

        static void Func(object status)
        {
            Console.WriteLine("Func() . " + status );
        }

    }
}