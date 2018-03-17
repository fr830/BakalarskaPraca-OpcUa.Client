﻿namespace OpcUA.Client.Core
{
    /// <summary>
    /// Data model for attribute data grid
    /// </summary>
    public class AttributeDataGridViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Attribute of ReferenceDescription object
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// Value of ReferenceDescription object Attribute
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Constructor

        public AttributeDataGridViewModel(string attribute, object value)
        {
            Attribute = attribute;
            Value = value;
        } 

        #endregion

    }
}