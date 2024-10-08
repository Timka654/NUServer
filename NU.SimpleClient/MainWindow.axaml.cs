using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Interactivity;
using Avalonia;
using Newtonsoft.Json;
using NU.Core;
using NU.SimpleClient.Models.Request;
using NU.SimpleClient.Models.Response;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static NU.SimpleClient.Configuration;
using MsBox.Avalonia;

namespace NU.SimpleClient
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        Configuration.AppConfig data = new Configuration.AppConfig();
        WindowNotificationManager notifyManager;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            notifyManager = new WindowNotificationManager(this);

            SetContentGrid(data.Data.GridState);

            ResetSettingsForm();

            ProcessArgs();
        }

        private async void ProcessArgs()
        {
            await Task.Delay(1_000);

            var cmdArgs = Environment.GetCommandLineArgs();

            string ckey = default;

            bool cvalued = false;

            Dictionary<string, string?> args = new Dictionary<string, string?>();


            foreach (var item in cmdArgs.Skip(1))
            {
                if (cvalued)
                {
                    args.Add(ckey, item);
                    ckey = default;
                    cvalued = default;
                    continue;
                }

                ckey = item.TrimStart('-');

                cvalued = item.StartsWith("--");

                if (!cvalued)
                    args.Add(ckey, default);
            }

            if (args.TryGetValue("d", out var dir))
            {
                if (Directory.Exists(dir))
                {
                    foreach (var item in Directory.GetFiles(dir, "*.nupkg", SearchOption.AllDirectories))
                    {
                        AddPackage(item);
                    }
                }
                else
                    return;
            }


            if (args.ContainsKey("upload"))
            {
                if (await UploadPackages())
                {
                    if (args.ContainsKey("closeOnSuccess"))
                        Environment.Exit(0);
                }
            }
        }

        public ObservableCollection<UploadListItem> UploadFileList { get; set; } = new ObservableCollection<UploadListItem>()
        {
            //new UploadListItem() { Name = "NSL.TCP", OldVersion = "2021.12.12", NewVersion = "2021.12.21" }, 
            //new UploadListItem() { Name = "NSL.TCP.Client", OldVersion = "2021.12.12", NewVersion = "2021.12.21" }, 
        };

        #region LeftBarHandles

        private void LeftBarSettingsButtonClick(object sender, RoutedEventArgs @event)
            => SetContentGrid(GridState.Settings);

        private void LeftBarSignUpButtonClick(object sender, RoutedEventArgs @event)
            => SetContentGrid(GridState.SignUp);

        private void LeftBarUploadButtonClick(object sender, RoutedEventArgs @event)
            => SetContentGrid(GridState.Upload);

        #endregion

        #region SettingsFormHandles

        private void SettingsFormUpdateButtonClick(object sender, RoutedEventArgs @event)
        {
            data.Data.ApiUrl = SettingsApiUrlField.Text;
            data.Data.PublishToken = SettingsPublishTokenField.Text;

            if (string.IsNullOrWhiteSpace(SettingsUIDField.Text))
                data.Data.UID = null;
            else if (Guid.TryParse(SettingsUIDField.Text, out var newUid))
                data.Data.UID = newUid;
            else
                notifyManager.Show(new Notification("Ошибка", "Поле \"UID\" имеет не корректный формат", NotificationType.Error));
        }

        private Task ShowMessageBox(string title, string content)
        {
            var msgw = MessageBoxManager.GetMessageBoxStandard(new MsBox.Avalonia.Dto.MessageBoxStandardParams()
            {
                ContentTitle = title,
                ContentMessage = content
            });

            return msgw.ShowWindowDialogAsync(this);
        }

        private async void SettingsFormGetShareUrlClick(object sender, RoutedEventArgs @event)
        {
            if (string.IsNullOrEmpty(data.Data.ApiUrl))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Api Url\" не заполнено", NotificationType.Error));
                return;
            }

            if (!data.Data.UID.HasValue)
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"UID\" не заполнено", NotificationType.Error));
                return;
            }

            if (string.IsNullOrEmpty(data.Data.PublishToken))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Publish Token\" не заполнено", NotificationType.Error));
                return;
            }

            await RestHelper.PostRequest(data.Data.ApiUrl, "api/User/GetSharedUrl",
                request => Task.CompletedTask,
                async response =>
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        await ShowMessageBox("Подробности", content);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadGateway && response.Content is StringContent sc)
                    {
                        var content = await sc.ReadAsStringAsync();

                        await ShowMessageBox($"Подробности {System.Net.HttpStatusCode.BadGateway}({Enum.GetName(System.Net.HttpStatusCode.BadGateway)})", content);
                    }
                }, new Dictionary<string, string>() {
                    { "uid", data.Data.UID.ToString() },
                    { "signToken", data.Data.PublishToken }
            });
        }

        private void SettingsFormResetButtonClick(object sender, RoutedEventArgs @event)
            => ResetSettingsForm();

        private async void SettingsFormExportClick(object sender, RoutedEventArgs @event)
        {
            if (string.IsNullOrEmpty(data.Data.ApiUrl))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Api Url\" не заполнено", NotificationType.Error));
                return;
            }

            if (!data.Data.UID.HasValue)
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"UID\" не заполнено", NotificationType.Error));
                return;
            }

            if (string.IsNullOrEmpty(data.Data.PublishToken))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Publish Token\" не заполнено", NotificationType.Error));
                return;
            }

            var sfd = new SaveFileDialog();

            sfd.Filters.Add(new FileDialogFilter() { Extensions = new List<string>() { "json" } });
            sfd.InitialFileName = "data.json";

            var path = await sfd.ShowAsync(this);

            if (path != null)
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(new { data.Data.ApiUrl, data.Data.UID, data.Data.PublishToken }));
                notifyManager.Show(new Notification("Инфо", "Сохранено", NotificationType.Success));
            }
        }

        private async void SettingsFormImportClick(object sender, RoutedEventArgs @event)
        {

            var ofd = new OpenFileDialog();

            ofd.Filters.Add(new FileDialogFilter() { Extensions = new List<string>() { "json" } });
            ofd.InitialFileName = "data.json";
            ofd.AllowMultiple = false;

            var path = await ofd.ShowAsync(this);

            if (path != null)
            {
                var d = JsonConvert.DeserializeObject<AppConfig.InternalData>(File.ReadAllText(path.First()));

                data.Data.ApiUrl = d.ApiUrl;
                data.Data.UID = d.UID;
                data.Data.PublishToken = d.PublishToken;

                notifyManager.Show(new Notification("Инфо", "Сохранено", NotificationType.Success));
            }
        }

        public void ResetSettingsForm()
        {
            SettingsApiUrlField.Text = data.Data.ApiUrl;
            SettingsPublishTokenField.Text = data.Data.PublishToken;
            SettingsUIDField.Text = data.Data.UID?.ToString() ?? String.Empty;
        }

        #endregion

        #region SignUpForm

        private SignUpResponseModel latestSignUpResponse;

        private async void SignUpSubmitButtonClick(object sender, RoutedEventArgs @event)
        {
            if (string.IsNullOrEmpty(data.Data.ApiUrl))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Api Url\" не заполнено", NotificationType.Error));
                return;
            }

            if (string.IsNullOrEmpty(SignUpNameField.Text))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Name\" не заполнено", NotificationType.Error));
                return;
            }

            if (string.IsNullOrEmpty(SignUpEmailField.Text))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Email\" не заполнено", NotificationType.Error));
                return;
            }


            SignUpSubmitButton.IsEnabled = false;

            await RestHelper.PostRequest(data.Data.ApiUrl, "api/User/SignUp",
                request =>
                {
                    request.SetJsonContent(new SignUpRequestModel
                    {
                        Name = SignUpNameField.Text,
                        Email = SignUpEmailField.Text,
                    });

                    return Task.CompletedTask;
                },
                async response =>
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        SignUpMessageText.Text = content;

                        latestSignUpResponse = JsonConvert.DeserializeObject<SignUpResponseModel>(content);

                        SignUpForm.IsVisible = false;
                        SignUpMessage.IsVisible = true;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        notifyManager.Show(new Notification("Ошибка", "Произошла ошибка\r\nНажмите сюда для просмотра подробностей", NotificationType.Error, onClick: () =>
                        {
                            var msgw = MessageBoxManager.GetMessageBoxStandard(new MsBox.Avalonia.Dto.MessageBoxStandardParams()
                            {
                                ContentTitle = "Подробности",
                                ContentMessage = content
                            });

                            msgw.ShowWindowDialogAsync(this);
                        }));
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadGateway && response.Content is StringContent sc)
                    {
                        var content = await sc.ReadAsStringAsync();

                        await ShowMessageBox($"Подробности {System.Net.HttpStatusCode.BadGateway}({Enum.GetName(System.Net.HttpStatusCode.BadGateway)})", content);
                    }
                });

            SignUpSubmitButton.IsEnabled = true;
        }

        private void SignUpBackButtonClick(object sender, RoutedEventArgs @event)
        {
            SignUpForm.IsVisible = true;
            SignUpMessage.IsVisible = false;
        }

        private void SignUpSetUpButtonClick(object sender, RoutedEventArgs @event)
        {
            if (latestSignUpResponse == null)
            {
                SignUpBackButtonClick(sender, @event);
                return;
            }

            SettingsUIDField.Text = (data.Data.UID = latestSignUpResponse.UID).ToString();
            SettingsPublishTokenField.Text = data.Data.PublishToken = latestSignUpResponse.PublishToken;
        }

        #endregion

        #region UploadFormHandles

        private void RemoveUploadItemButtonClick(object sender, RoutedEventArgs @event)
        {
            var btn = (sender as Button);

            if (btn.DataContext is UploadListItem item)
            {
                UploadFileList.Remove(item);
            }
        }

        private async void UploadFileAddButtonClick(object sender, RoutedEventArgs @event)
        {
            OpenFileDialog f = new OpenFileDialog();

            f.Directory = data.Data.LatestFileBrowseDir;

            f.AllowMultiple = true;

            f.Filters = new List<FileDialogFilter>() { new FileDialogFilter() { Extensions = new List<string>() { "nupkg" }, Name = "Nuget Package" } };

            var files = await f.ShowAsync(this);

            if (files != null)
            {
                foreach (var item in files)
                {
                    AddPackage(item);
                }

                data.Data.LatestFileBrowseDir = f.Directory;
            }
        }

        private async void UploadFolderAddButtonClick(object sender, RoutedEventArgs @event)
        {
            OpenFolderDialog f = new OpenFolderDialog();

            f.Directory = data.Data.LatestFolderBrowse;

            var directory = await f.ShowAsync(this);

            if (directory != null)
            {
                foreach (var item in Directory.GetFiles(directory, "*.nupkg", SearchOption.AllDirectories))
                {
                    AddPackage(item);
                }

                data.Data.LatestFolderBrowse = directory;
            }
        }

        private async void UploadClearListButtonClick(object sender, RoutedEventArgs @event)
        {
            UploadFileList.Clear();
        }

        private async void UploadButtonClick(object sender, RoutedEventArgs @event)
        {
            await UploadPackages();
        }

        private async Task<bool> UploadPackages()
        {

            if (string.IsNullOrEmpty(data.Data.ApiUrl))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Api Url\" не заполнено", NotificationType.Error));
                return false;
            }

            if (data.Data.UID.HasValue == false)
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"UID\" не заполнено", NotificationType.Error));
                return false;
            }

            if (string.IsNullOrEmpty(data.Data.PublishToken))
            {
                notifyManager.Show(new Notification("Ошибка", "Поле \"Publish Token\" не заполнено", NotificationType.Error));
                return false;
            }

            if (!UploadFileList.Any())
            {
                notifyManager.Show(new Notification("Ошибка", "Добавьте файлы", NotificationType.Error));
                return false;
            }

            bool success = false;

            UploadSubmitButton.IsEnabled = false;

            await RestHelper.PostRequest(data.Data.ApiUrl, "api/Package/Publish", request =>
            {
                var sfiles = UploadFileList.Where(x => File.Exists(x.Path)).Select(x => x.Path).ToArray();

                if (sfiles.Length != UploadFileList.Count)
                    throw new Exception($"Invalid file patches {Environment.NewLine}{string.Join(Environment.NewLine, UploadFileList.Where(x => !sfiles.Contains(x.Path)).Select(x => x.Path))}");

                request.InitializeMultipart()
                .SetFileArrayContent("package", sfiles);

                return Task.CompletedTask;
            },
            async response =>
            {
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    UploadFileList.Clear();

                    string content = await response.Content.ReadAsStringAsync();

                    notifyManager.Show(new Notification("Уведомление", "Успешная загрузка\r\nНажмите сюда для просмотра подробностей", onClick: () =>
                    {
                        var msgw = MessageBoxManager.GetMessageBoxStandard(new MsBox.Avalonia.Dto.MessageBoxStandardParams()
                        {
                            ContentTitle = "Подробности",
                            ContentMessage = content
                        });

                        msgw.ShowWindowDialogAsync(this);
                    }));
                }
                else
                {
                    var statusCode = response.StatusCode;

                    if (statusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        string scontent = await response.Content.ReadAsStringAsync();

                        notifyManager.Show(new Notification("Ошибка", "Произошла ошибка загрузки\r\nНажмите сюда для просмотра подробностей", NotificationType.Error, onClick: () =>
                        {
                            var msgw = MessageBoxManager.GetMessageBoxStandard(new MsBox.Avalonia.Dto.MessageBoxStandardParams()
                            {
                                ContentTitle = "Подробности",
                                ContentMessage = scontent
                            });

                            msgw.ShowWindowDialogAsync(this);
                        }));
                    }
                    else if (statusCode == System.Net.HttpStatusCode.BadGateway && response.Content is StringContent sc)
                    {
                        var content = await sc.ReadAsStringAsync();

                        await ShowMessageBox($"Подробности {System.Net.HttpStatusCode.BadGateway}({Enum.GetName(System.Net.HttpStatusCode.BadGateway)})", content);
                    }
                    else
                    {

                        notifyManager.Show(new Notification("Ошибка", "Произошла ошибка загрузки\r\nНажмите сюда для просмотра подробностей", NotificationType.Error, onClick: () =>
                        {
                            var msgw = MessageBoxManager.GetMessageBoxStandard(new MsBox.Avalonia.Dto.MessageBoxStandardParams()
                            {
                                ContentTitle = "Подробности",
                                ContentMessage = $"Status code {(int)statusCode}({Enum.GetName(statusCode)})"
                            });

                            msgw.ShowWindowDialogAsync(this);
                        }));
                    }
                }

            }, new Dictionary<string, string>() {
                    { "uid", data.Data.UID.ToString() },
                    { "signToken", data.Data.PublishToken }
            });

            UploadSubmitButton.IsEnabled = true;

            return success;
        }

        private bool AddPackage(string path)
        {
            using var nuFile = new NuGetFile(path);

            var file = new UploadListItem { Path = path, Name = nuFile.Id, NewVersion = nuFile.Version };

            var exFile = UploadFileList.FirstOrDefault(x => x.Name.Equals(nuFile.Id));

            nuFile.Dispose();

            if (exFile != null)
            {
                if (!NuGetVersion.TryParse(file.NewVersion, out var nver) || !NuGetVersion.TryParse(exFile.NewVersion, out var exVer))
                    return false;

                if (nver > exVer)
                {
                    string msg = $"Duplicate package - {file.Name} replace with new version = {nver}\r\nReplace \"{exFile.Path}\" > \"{file.Path}\"";

                    notifyManager.Show(new Notification("Ошибка", msg, NotificationType.Warning, onClick: () =>
                    {
                        var msgw = MessageBoxManager.GetMessageBoxStandard(new MsBox.Avalonia.Dto.MessageBoxStandardParams()
                        {
                            ContentTitle = "Подробности",
                            ContentMessage = msg
                        });

                        msgw.ShowWindowDialogAsync(this);
                    }));

                    UploadFileList.Remove(exFile);
                }
                else
                {
                    string msg = $"Duplicate package - {file.Name} have lower or equals version = {nver}\r\nIgnoring\"{file.Path}\"";

                    notifyManager.Show(new Notification("Ошибка", msg, NotificationType.Warning, onClick: () =>
                    {
                        var msgw = MessageBoxManager.GetMessageBoxStandard(new MsBox.Avalonia.Dto.MessageBoxStandardParams()
                        {
                            ContentTitle = "Подробности",
                            ContentMessage = msg
                        });

                        msgw.ShowWindowDialogAsync(this);
                    }));
                    return false;
                }

            }

            UploadFileList.Add(file);

            return true;
        }

        #endregion


        #region HttpUtils


        #endregion

        private void SetContentGrid(GridState state)
        {
            SettingsFormGrid.IsVisible = state == GridState.Settings;
            SignUpFormGrid.IsVisible = state == GridState.SignUp;
            UploadFormGrid.IsVisible = state == GridState.Upload;

            data.Data.GridState = state;
        }

        public enum GridState
        {
            Settings,
            SignUp,
            Upload
        }
    }

    public class UploadListItem
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string OldVersion { get; set; }

        public string NewVersion { get; set; }
    }
}
