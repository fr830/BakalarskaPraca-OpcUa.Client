﻿using System;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows.Input;
using OpcUa.Client.Core;

namespace OpcUa.Client.WPF
{
    public class WelcomeViewModel : BaseViewModel
    {
        private readonly Messenger _messenger;
        private readonly UaClientApi _uaClientApi;
        private readonly IUnitOfWork _unitOfWork;

        #region Public Properties

        public string WelcomeText { get; set; } = "Lorem ipsum dolor, lorem isum dolor, Lorem ipsum dolor sit amet, lorem ipsum dolor sit amet";
        public ObservableCollection<ProjectModel> Projects { get; set; }
        public ProjectModel SelectedProject { get; set; }

        #endregion

        #region Commands

        public ICommand LoadProjectCommand { get; set; }
        public ICommand DeleteProjectCommand { get; set; }
        public ICommand CreateProjectCommand { get; set; }
        
        #endregion

        public WelcomeViewModel()
        {
            _messenger = IoC.Messenger;
            // TODO vsetkych pages predavat IoC veci cey konstruktor z Page Convertora !
            _uaClientApi = IoC.UaClientApi;
            _unitOfWork = IoC.UnitOfWork;

            OnLoad();

            LoadProjectCommand = new MixRelayCommand(LoadProject);
            DeleteProjectCommand = new MixRelayCommand(DeleteProject);
            CreateProjectCommand = new MixRelayCommand((obj) => IoC.Application.GoToPage(ApplicationPage.Endpoints));

            _messenger.Register<SendCredentials>(msg => Login(msg.UserName, msg.Password));
        }

        private void LoadProject(object parameter)
        {
            try
            {
                var endpoint = _unitOfWork.Endpoints.SingleOrDefault(x => x.Id == SelectedProject.EndpointId);
                if (SelectedProject.UserId == null)
                {
                    _uaClientApi.ConnectAnonymous(Mapper.CreateEndpointDescription(endpoint), SelectedProject.SessionName);
                    IoC.AppManager.ProjectId = SelectedProject.Id;
                    IoC.Application.GoToPage(ApplicationPage.Main);
                }
                else
                    IoC.Ui.ShowLogIn(new LogInDialogViewModel(_messenger));
            }
            catch (Exception e)
            {
                IoC.Ui.ShowMessage(new MessageBoxDialogViewModel()
                {
                    Title = "Error",
                    Message = e.Message,
                    OkText = "ok",
                });
            }
        }

        private void DeleteProject(object parameter)
        {
            // TODO opztat sa uyivatela ci si je isty
            var project = _unitOfWork.Projects.SingleOrDefault(x => x.Id == SelectedProject.Id);
            _unitOfWork.Endpoints.Remove(project.Endpoint);
            if(project.UserId != null)
                //todo remove user

            _unitOfWork.Projects.Remove(project);

            var notifications = _unitOfWork.Notifications.Find(x => x.ProjectId == SelectedProject.Id);
            _unitOfWork.Notifications.RemoveRange(notifications);

            var variables = _unitOfWork.Variables.Find(x => x.ProjectId == SelectedProject.Id);

            if (variables == null) return;

            foreach (var variable in variables)
                _unitOfWork.Records.RemoveRange(variable.Records);

            _unitOfWork.Variables.RemoveRange(variables);

            
        }

        private void Login(string userName, SecureString password)
        {
            try
            {
                var user = _unitOfWork.Auth.Login(userName, password);
                if (user == null)
                {
                    IoC.Ui.ShowMessage(new MessageBoxDialogViewModel()
                    {
                        Title = "Error",
                        Message = "Zadali ste zlé prihlasovacie údaje!",
                        OkText = "ok"
                    });
                }
                var endpoint = _unitOfWork.Endpoints.SingleOrDefault(x => x.Id == SelectedProject.EndpointId);
                _uaClientApi.Connect(Mapper.CreateEndpointDescription(endpoint), userName, SecureStringHelpers.Unsecure(password), SelectedProject.SessionName);
            }
            catch (Exception e)
            {
                IoC.Ui.ShowMessage(new MessageBoxDialogViewModel()
                {
                    Title = "Error",
                    Message = e.Message,
                    OkText = "ok"
                });
            }
            IoC.Application.GoToPage(ApplicationPage.Main);
        }

        private void OnLoad()
        {
            var projectsEntities = _unitOfWork.Projects.GetAllWithEndpoints();

            if (projectsEntities == null) return;
            Projects = new ObservableCollection<ProjectModel>(Mapper.ProjectToListModel(projectsEntities));
        }
    }
}