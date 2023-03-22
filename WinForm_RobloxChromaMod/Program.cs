using ChromaSDK;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WinForm_RobloxChromaMod
{
    static class Program
    {
        const string CONFIG_NAME = "config.json";

        #region Load Configuration

        private static void LoadConfiguration()
        {
            try
            {
                FileInfo fi = new FileInfo(CONFIG_NAME);
                if (!fi.Exists)
                {
                    return;
                }
                using (FileStream fs = File.Open(CONFIG_NAME, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        using (JsonTextReader reader = new JsonTextReader(sr))
                        {
                            JObject json = (JObject)JToken.ReadFrom(reader);
                            Form1._sMouseMoveStart = new Point(
                                json.GetValue("start-x").ToObject<int>(),
                                json.GetValue("start-y").ToObject<int>());

                            Form1._sMouseMoveEnd = new Point(
                                json.GetValue("end-x").ToObject<int>(),
                                json.GetValue("end-y").ToObject<int>());

                            Form1._sMouseMoveOffset = new Point(
                                json.GetValue("offset-x").ToObject<int>(),
                                json.GetValue("offset-y").ToObject<int>());
                        }
                    }
                }
            }
            catch
            {

            }
        }

        #endregion

        #region Save Configuration

        private static void SaveConfiguration()
        {
            try
            {
                using (FileStream fs = File.Open(CONFIG_NAME, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        using (JsonTextWriter writer = new JsonTextWriter(sw))
                        {
                            JObject json = new JObject(
                                new JProperty("start-x", Form1._sMouseMoveStart.X),
                                new JProperty("start-y", Form1._sMouseMoveStart.Y),
                                new JProperty("end-x", Form1._sMouseMoveEnd.X),
                                new JProperty("end-y", Form1._sMouseMoveEnd.Y),
                                new JProperty("offset-x", Form1._sMouseMoveOffset.X),
                                new JProperty("offset-y", Form1._sMouseMoveOffset.Y));
                            json.WriteTo(writer);
                            writer.Flush();
                        }
                        sw.Flush();
                    }
                }
            }
            catch
            {

            }
        }

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ChromaSDK.APPINFOTYPE appInfo = new APPINFOTYPE();
            appInfo.Title = "Razer Chroma CSharp Game Loop Sample Application";
            appInfo.Description = "A sample application using Razer Chroma SDK";

            appInfo.Author_Name = "Razer";
            appInfo.Author_Contact = "https://developer.razer.com/chroma";

            //appInfo.SupportedDevice = 
            //    0x01 | // Keyboards
            //    0x02 | // Mice
            //    0x04 | // Headset
            //    0x08 | // Mousepads
            //    0x10 | // Keypads
            //    0x20   // ChromaLink devices
            appInfo.SupportedDevice = (0x01 | 0x02 | 0x04 | 0x08 | 0x10 | 0x20);
            //    0x01 | // Utility. (To specifiy this is an utility application)
            //    0x02   // Game. (To specifiy this is a game);
            appInfo.Category = 1;
            int result = ChromaAnimationAPI.InitSDK(ref appInfo);
            switch (result)
            {
                case RazerErrors.RZRESULT_DLL_NOT_FOUND:
                    Console.Error.WriteLine("Chroma DLL is not found! {0}", RazerErrors.GetResultString(result));
                    return;
                case RazerErrors.RZRESULT_DLL_INVALID_SIGNATURE:
                    Console.Error.WriteLine("Chroma DLL has an invalid signature! {0}", RazerErrors.GetResultString(result));
                    return;
                case RazerErrors.RZRESULT_SUCCESS:
                    Thread.Sleep(100);
                    break;
                default:
                    Console.Error.WriteLine("Failed to initialize Chroma! {0}", RazerErrors.GetResultString(result));
                    return;
            }

            LoadConfiguration();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            SaveConfiguration();

            if (ChromaAnimationAPI.IsInitialized())
            {
                ChromaAnimationAPI.StopAll();
                ChromaAnimationAPI.CloseAll();
                result = ChromaAnimationAPI.Uninit();
                ChromaAnimationAPI.UninitAPI();
                if (result != RazerErrors.RZRESULT_SUCCESS)
                {
                    Console.Error.WriteLine("Failed to uninitialize Chroma!");
                }
            }
        }
    }
}
