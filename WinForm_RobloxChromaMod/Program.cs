using ChromaSDK;
using System;
using System.Threading;
using System.Windows.Forms;

namespace WinForm_RobloxChromaMod
{
    static class Program
    {
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


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

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
