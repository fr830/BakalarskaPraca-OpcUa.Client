﻿using System.Collections.ObjectModel;
using System.Linq;

namespace OpcUA.Client.Core
{
    /// <inheritdoc />
    /// <summary>
    /// The view model for the address space tab item
    /// </summary>
    public class NodeStructureViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// A list of root nodes in the address space
        /// </summary>
        public ObservableCollection<NodeItemViewModel> Items { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public NodeStructureViewModel()
        {
            // Get the root nodes
            var children = IoC.Get<UAClientHelperAPI>().BrowseRoot();

            // Create the view models from the root ndoes
            //Items = new ObservableCollection<NodeItemViewModel>(
            //    children.Select(content => new NodeItemViewModel(content)));
        }

        #endregion
    }
}