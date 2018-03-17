﻿using System;
using Opc.Ua;
using Opc.Ua.Client;

namespace OpcUA.Client.Core
{
    public class Variable : BaseViewModel
    {
        public MonitoredItem MonitoredItem { get; set; }

        public string Name => MonitoredItem.DisplayName;

        public object Value { get; set; }

        public Type DataType => Value?.GetType();

        public StatusCode StatusCode { get; set; }

        public DateTime DateTime { get; set; }
    }
}