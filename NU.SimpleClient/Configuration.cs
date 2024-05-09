using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NU.SimpleClient
{
    internal class Configuration
    {
        public abstract class BaseConfigData<TInternal>
        {
            protected string configurationPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDomain.CurrentDomain.FriendlyName, filePath);

            protected readonly string filePath;

            public event Action OnDataChanged = () => { };

            protected TInternal internalData { get; private set; }

            protected abstract TInternal Initialize();

            protected BaseConfigData(string filePath)
            {
                this.filePath = filePath;
            }

            protected void SaveFile()
            {
                var fi = new FileInfo(configurationPath);

                if (!fi.Directory.Exists)
                    fi.Directory.Create();

                File.WriteAllText(fi.FullName, JsonConvert.SerializeObject(internalData));
            }

            protected void LoadFile(bool initializeNoExists = true)
            {
                if (File.Exists(configurationPath))
                    internalData = JsonConvert.DeserializeObject<TInternal>(File.ReadAllText(configurationPath));

                if (internalData == null && initializeNoExists)
                {
                    internalData = Initialize();
                    SaveFile();
                }
            }

            protected void DataChanged() => OnDataChanged();
        }

        public class AppConfig
        {
            public class InternalData
            {
                public string LatestFolderBrowse;

                public string LatestFileBrowseDir;

                public string ApiUrl;

                public string PublishToken;

                public Guid? UID;

                public MainWindow.GridState GridState;
            }

            public class ConfigData : BaseConfigData<InternalData>
            {
                public ConfigData() : base(Path.Combine("Settings", "Config.cfg"))
                {
                    OnDataChanged += () => { base.SaveFile(); };
                    base.LoadFile(true);
                }

                protected override InternalData Initialize()
                {
                    return new InternalData();
                }

                public string LatestFolderBrowse { get => internalData.LatestFolderBrowse; set { if (internalData.LatestFolderBrowse == value) return; internalData.LatestFolderBrowse = value; DataChanged(); } }

                public string LatestFileBrowseDir { get => internalData.LatestFileBrowseDir; set { if (internalData.LatestFileBrowseDir == value) return; internalData.LatestFileBrowseDir = value; DataChanged(); } }

                public string ApiUrl { get => internalData.ApiUrl; set { if (internalData.ApiUrl == value) return; internalData.ApiUrl = value; DataChanged(); } }

                public string PublishToken { get => internalData.PublishToken; set { if (internalData.PublishToken == value) return; internalData.PublishToken = value; DataChanged(); } }

                public Guid? UID { get => internalData.UID; set { if (internalData.UID == value) return; internalData.UID = value; DataChanged(); } }

                public MainWindow.GridState GridState { get => internalData.GridState; set { if (internalData.GridState == value) return; internalData.GridState = value; DataChanged(); } }
            }

            public ConfigData Data { get; private set; } = default;

            public AppConfig()
            {
                Data = new ConfigData();
            }

        }

    }
}
