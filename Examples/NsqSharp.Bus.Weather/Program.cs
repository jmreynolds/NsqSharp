﻿using System;
using Newtonsoft.Json;
using NsqSharp.Bus.Configuration;
using NsqSharp.Bus.Configuration.BuiltIn;
using NsqSharp.Bus.Weather.IoC;
using NsqSharp.Bus.Weather.Messages;

namespace NsqSharp.Bus.Weather
{
    class Program
    {
        static void Main()
        {
            var config = new BusConfiguration(
                new StructureMapObjectBuilder(ObjectFactory.Container),
                new NewtonsoftJsonSerializer(typeof(JsonConvert).Assembly),
                defaultThreadsPerHandler: 16,
                defaultNsqlookupdHttpEndpoints: new[] { "127.0.0.1:4161" },
                onStart: SendMessage
            );
            
            config.AddMessageHandlers(new[] { typeof(Program).Assembly });

            BusService.Start(config);
        }

        private static void SendMessage()
        {
            var bus = ObjectFactory.Container.GetInstance<IBus>();

            bus.Send(new GetWeather { City = "Austin" });
        }
    }
}