﻿using System;
using System.Diagnostics;
using System.Globalization;
using Opc.Ua;

namespace OpcUa.Client.WPF
{
    /// <inheritdoc />
    /// <summary>
    /// Converts a full path to a specific image type of a drive, folder or file
    /// </summary>
    public class HeaderToImageConverter : BaseValueConverter<HeaderToImageConverter>
    { 
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((NodeClass)value)
            {
                case NodeClass.Object:
                    return "archive";
                case NodeClass.Unspecified:
                    Debugger.Break();
                    return "help-circle";
                case NodeClass.Variable:
                    return "tag";
                case NodeClass.Method:
                    return "cube";
                case NodeClass.ObjectType:
                    return "flag";
                case NodeClass.VariableType:
                    return "database";
                case NodeClass.ReferenceType:
                    return "flask";
                case NodeClass.DataType:
                    return "beer";
                case NodeClass.View:
                    return "eye";
                default:
                    Debugger.Break();
                    return "bug";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
