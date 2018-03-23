using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

namespace sdy.HotFix
{

    public class HotFixResources : MonoBehaviour
    {
        public static string VERSION_FILE = "version.txt";

        private string LOCAL_RES_URL = "";
        private string SERVER_RES_URL = "";
        private string PERSISTENT_RES_PATH = "";


        private Dictionary<string, string> PersistentVersion;
        private Dictionary<string, string> ServerVersion;

        private List<string> MissingFiles;
        private List<string> NeedDownFiles;

        private bool NeedUpdateLocalVersionFile;
        private bool LoadPersistentVersionDone;

        private bool LoadMissingFileDone;
        private bool DownLoadFileDone;

        public Texture T;
        public TextAsset Txt;


        private void Start()
        {
            Init();
            StartCoroutine(FixResources());
        }


        /// <summary>
        /// 初始化参数
        /// </summary>
        private void Init()
        {
            LOCAL_RES_URL = "file://" + Application.dataPath + "/LocalResources/";
            SERVER_RES_URL = "file://" + Application.dataPath + "/ServerResources/";
            PERSISTENT_RES_PATH = Application.persistentDataPath + "/Resources/";

            PersistentVersion = new Dictionary<string, string>();
            ServerVersion = new Dictionary<string, string>();

            MissingFiles = new List<string>();
            NeedDownFiles = new List<string>();

            NeedUpdateLocalVersionFile = false;
            LoadPersistentVersionDone = false;

            LoadMissingFileDone = false;
            DownLoadFileDone = false;
        }


        /// <summary>
        /// 比较版本差异，更新本地资源
        /// </summary>
        /// <returns></returns>
        private IEnumerator FixResources()
        {
            yield return null;

            //本地不存在资源文件夹，则创建
            if (!Directory.Exists(PERSISTENT_RES_PATH))
            {
                Directory.CreateDirectory(PERSISTENT_RES_PATH);
            }

            //本地不存在版本，则拷贝
            if (!File.Exists(PERSISTENT_RES_PATH + VERSION_FILE))
            {
                StartCoroutine(DownLoad(LOCAL_RES_URL + VERSION_FILE, delegate (WWW localVersion)
                {
                    FileStream stream = File.Create(PERSISTENT_RES_PATH + VERSION_FILE);
                    stream.Write(localVersion.bytes, 0, localVersion.bytes.Length);
                    ParseVersionFile(localVersion.text, PersistentVersion);
                    LoadPersistentVersionDone = true;
                }));
            }
            else
            {
                StartCoroutine(DownLoad(PERSISTENT_RES_PATH + VERSION_FILE, delegate (WWW persistentVersion)
                {
                    ParseVersionFile(persistentVersion.text, PersistentVersion);
                    LoadPersistentVersionDone = true;
                }));
            }

            while (!LoadPersistentVersionDone)
            {
                yield return null;
            }

            StartCoroutine(DownLoad(SERVER_RES_URL, delegate (WWW serverVersion)
            {
                ParseVersionFile(serverVersion.text, ServerVersion);

                CompareVersion();

                LoadMissingRes();

                DownLoadRes();

            }));

            while (!LoadMissingFileDone || !DownLoadFileDone)
            {
                yield return null;
            }

            AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(PERSISTENT_RES_PATH + "common");
            yield return abcr;

            AssetBundle ab = abcr.assetBundle;

            AssetBundleRequest abq = ab.LoadAssetAsync("2");
            yield return abq;

            T = abq.asset as Texture;
        }


        /// <summary>
        /// 和服务端的版本进行对比
        /// </summary>
        private void CompareVersion()
        {
            if (ServerVersion.Count > 0)
            {
                foreach (var version in ServerVersion)
                {
                    string fileName = version.Key;
                    string serverMd5 = version.Value;
                    //新增的资源    
                    if (!PersistentVersion.ContainsKey(fileName))
                    {
                        NeedDownFiles.Add(fileName);
                    }
                    else
                    {
                        //需要替换的资源    
                        string localMd5;
                        PersistentVersion.TryGetValue(fileName, out localMd5);
                        if (!serverMd5.Equals(localMd5))
                        {
                            NeedDownFiles.Add(fileName);
                        }
                        else
                        {
                            if (!File.Exists(PERSISTENT_RES_PATH + fileName))
                            {
                                MissingFiles.Add(fileName);
                            }
                        }
                    }
                }
                //本次有更新，同时更新本地的version.txt    
                NeedUpdateLocalVersionFile = NeedDownFiles.Count > 0;
            }
            else
            {
                foreach (var version in PersistentVersion)
                {
                    string fileName = version.Key;
                    string serverMd5 = version.Value;

                    //检查本地资源是否完整
                    if (!File.Exists(PERSISTENT_RES_PATH + fileName))
                    {
                        MissingFiles.Add(fileName);
                    }
                }
            }
        }


        /// <summary>
        /// 导入本地丢失的资源
        /// </summary>
        private void LoadMissingRes()
        {
            if (MissingFiles.Count == 0)
            {
                LoadMissingFileDone = true;
                return;
            }

            string fileName = MissingFiles[0];
            MissingFiles.RemoveAt(0);

            StartCoroutine(DownLoad(LOCAL_RES_URL + fileName, delegate (WWW www)
            {
                ReplacePersistentResource(fileName, www.bytes);
                LoadMissingRes();
            }));
        }


        /// <summary>
        /// 下载服务端的资源
        /// </summary>
        private void DownLoadRes()
        {
            if (NeedDownFiles.Count == 0)
            {
                UpdatePersistentVersion();
                DownLoadFileDone = true;
                return;
            }

            string fileName = NeedDownFiles[0];
            NeedDownFiles.RemoveAt(0);

            StartCoroutine(DownLoad(SERVER_RES_URL + fileName, delegate (WWW www)
            {
                ReplacePersistentResource(fileName, www.bytes);
                DownLoadRes();
            }));
        }


        /// <summary>
        /// 更新本地的版本
        /// </summary>
        private void UpdatePersistentVersion()
        {
            if (NeedUpdateLocalVersionFile)
            {
                Debug.Log("有版本的更新");
                string persistentPath = PERSISTENT_RES_PATH + VERSION_FILE;

                if (File.Exists(persistentPath))
                {
                    File.Delete(persistentPath);
                }

                StringBuilder versions = new StringBuilder();
                foreach (var item in ServerVersion)
                {
                    versions.Append(item.Key).Append(",").Append(item.Value).Append("\n");
                }

                FileStream stream = File.Create(persistentPath);
                byte[] data = Encoding.UTF8.GetBytes(versions.ToString());
                stream.Write(data, 0, data.Length);
                stream.Flush();
                stream.Close();
            }
        }


        /// <summary>
        /// 替换本地资源，如果本地资源存在则先删除
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        private void ReplacePersistentResource(string name, byte[] content)
        {
            string filePath = PERSISTENT_RES_PATH + name;

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            FileStream stream = File.Create(filePath);
            stream.Write(content, 0, content.Length);
            stream.Flush();
            stream.Close();
        }


        /// <summary>
        /// 解析版本配置文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="dic"></param>
        private void ParseVersionFile(string content, Dictionary<string, string> dic)
        {
            if (content == null || content.Length == 0)
            {
                return;
            }
            string[] items = content.Split(new char[] { '\n' });
            foreach (string item in items)
            {
                string[] info = item.Split(new char[] { ',' });
                if (info != null && info.Length == 2)
                {
                    dic.Add(info[0], info[1]);
                }
            }
        }


        private IEnumerator DownLoad(string url, HandFinishDownLoad finishLoad)
        {
            WWW www = new WWW(url);
            yield return www;

            if (finishLoad != null)
            {
                finishLoad(www);
            }

            www.Dispose();
        }


        private delegate void HandFinishDownLoad(WWW www);
    }


}