﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Opc.Ua;
using Opc.Ua.Client;

namespace OpcUa.Client.Core
{
    //public class NotificationViewModel : BaseViewModel
    //{
    //    #region Private Fields

    //    private readonly UaClientApi _uaClientApi;
    //    private readonly IUnitOfWork _unitOfWork;
    //    private ReferenceDescription _selectedNode;
    //    private readonly Subscription _subscription; 

    //    #endregion

    //    #region Public Properties

    //    public ObservableCollection<ExtendedNotificationModel> Notifications { get; set; } = new ObservableCollection<ExtendedNotificationModel>();
    //    public ExtendedNotificationModel SelectedNotification { get; set; }

    //    #endregion

    //    #region Commands

    //    public ICommand AddNotificationCommand { get; set; }
    //    public ICommand RemoveNotificationCommand { get; set; }

    //    #endregion

    //    #region Constructor

    //    // TODO Prerobit WriteValue v opcuaApi
    //    public NotificationViewModel(IUnitOfWork unitOfWork, UaClientApi uaClientApi)
    //    {
    //        _uaClientApi = uaClientApi;
    //        _unitOfWork = unitOfWork;

    //        _subscription = _uaClientApi.Subscribe(300, "Notifications");
    //        LoadDataFromDataBase();

    //        //AddNotificationCommand = new RelayCommand(AddNotification);
    //        AddNotificationCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(AddNotification, AddNotificationCanUse);
    //        //RemoveNotificationCommand = new RelayCommand(RemoveNotification);
    //        RemoveNotificationCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(RemoveNotification, RemoveNotificationCanUse);

    //        MessengerInstance.Register<SendSelectedRefNode>(
    //            this,
    //            msg => _selectedNode = msg.ReferenceNode);

    //        MessengerInstance.Register<SendNewNotification>(
    //            this,
    //            msg => AddNotificationToSubscription(msg.Notification));
    //    }

    //    #endregion

    //    #region Command Methods

    //    private void AddNotification()
    //    {
    //        IoC.Ui.ShowAddNotification(new AddNotificationDialogViewModel()
    //        {
    //            NodeId = _selectedNode.NodeId.ToString()
    //        });
    //    }

    //    private void RemoveNotification()
    //    {
    //        _uaClientApi.RemoveMonitoredItem(_subscription, SelectedNotification.NodeId);
    //        Notifications.Remove(SelectedNotification);
    //    }

    //    //private void SaveSubscription()
    //    //{
    //    //    // TODO save do priecinka /Subscriptions a ukladat s mneom ako datum v stringu
    //    //    _uaClientApi.SaveSubsciption();
    //    //}

    //    //private void LoadSubscription()
    //    //{
    //    //    // TODO opytat sa uzivatela ci chce prepisat existujucu subscription
    //    //    if (_subscription != null) return;
    //    //    _subscription = _uaClientApi.LoadSubsciption();

    //    //    if (_subscription == null) return;

    //    //    // TODO private metoda opakovany kod
    //    //    foreach (var item in _subscription.MonitoredItems)
    //    //    {
    //    //        var tmp = new ExtendedNotificationModel()
    //    //        {
    //    //            NodeId = item.StartNodeId.ToString(),
    //    //            Name = item.DisplayName
    //    //            // TODO set up type here
    //    //        };

    //    //        item.Notification += Notification_MonitoredItem;
    //    //        Notifications.Add(tmp);
    //    //    }
    //    //    _subscription.ApplyChanges();
    //    //}

    //    #endregion

    //    #region Can use methods

    //    public bool AddNotificationCanUse()
    //    {
    //        if (_selectedNode == null)
    //            return false;
    //        else if (_selectedNode.NodeClass != NodeClass.Variable)
    //            return false;
    //        else return true;
    //    }

    //    public bool RemoveNotificationCanUse()
    //    {
    //        return SelectedNotification != null;
    //    }

    //    #endregion

    //    #region Helpers

    //    private void AddNotificationToSubscription(ExtendedNotificationModel notification)
    //    {
    //        notification.DataType = _uaClientApi.GetBuiltInTypeOfVariableNodeId(notification.NodeId);

    //        MonitoredItem item;

    //       var notificationEntity = new NotificationEntity()
    //        {
    //            Name = notification.Name,
    //            NodeId = notification.NodeId,
    //            ProjectId = IoC.AppManager.ProjectId,
    //        };

    //        // TODO inak vymysliet notification model
    //        if (notification is DigitalNotificationModel)
    //        {
    //            item = _uaClientApi.CreateMonitoredItem(notification.Name, notification.NodeId, 300);
    //            notificationEntity.IsDigital = true;
    //            notificationEntity.IsZeroDescription = (notification as DigitalNotificationModel).IsZeroDescription;
    //            notificationEntity.IsOneDescription = (notification as DigitalNotificationModel).IsOneDescription;
    //        }
    //        else
    //        {
    //            var analog = (notification as AnalogNotificationModel);

    //            item = _uaClientApi.CreateMonitoredItem(notification.Name,
    //                notification.NodeId,
    //                300,
    //                new DataChangeFilter()
    //                {
    //                    DeadbandType = (uint)analog.DeadbandType,
    //                    DeadbandValue = analog.FilterValue,
    //                    Trigger = DataChangeTrigger.StatusValue
    //                });
    //            notificationEntity.FilterValue = analog.FilterValue;
    //            notificationEntity.DeadbandType = analog.DeadbandType;
    //        }

    //        _uaClientApi.AddMonitoredItem(item, _subscription);
    //        item.Notification += Notification_MonitoredItem;

    //        _unitOfWork.Notifications.Add(notificationEntity);

    //        Notifications.Add(notification);
    //    }

    //    private void LoadDataFromDataBase()
    //    {
    //        Notifications.Add(new AnalogNotificationModel());
    //        ArchiveVariables = new ObservableCollection<VariableEntity>(_unitOfWork.Variables.Find(x => x.ProjectId == IoC.AppManager.ProjectId));
    //    }

    //    #endregion

    //    #region CallBack Methods

    //    /// <summary>
    //    /// Callback method for updating values of subscibed nodes
    //    /// </summary>
    //    /// <param name="monitoredItem"></param>
    //    /// <param name="e"></param>
    //    private void Notification_MonitoredItem(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
    //    {
    //        if (!(e.NotificationValue is MonitoredItemNotification notification))
    //            return;

    //        var value = notification.Value;

    //        var variable = Notifications.FirstOrDefault(x => x.Name == monitoredItem.DisplayName);

    //        if (variable == null) return;

    //        variable.Value = value.Value;
    //        variable.StatusCode = value.StatusCode;
    //        variable.SourceDateTime = value.SourceTimestamp;
    //    }

    //    #endregion
    //}
}