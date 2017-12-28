using System;
using System.Collections.Generic;
using Managed.Adb;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using log4net;
using System.Xml;

namespace L2RBot
{
    //class ShellOutputReceiver : IShellOutputReceiver
    //{
    //    private static readonly ILog log = LogManager.GetLogger(typeof(ShellOutputReceiver));

    //    bool IShellOutputReceiver.IsCancelled { get; }

    //    public void AddOutput(byte[] data, int offset, int length)
    //    {
    //        log.Info(System.Text.Encoding.ASCII.GetString(data, offset, length));
    //    }

    //    public void Flush()
    //    {
    //    }
    //}

    //class FileMonitor : ISyncProgressMonitor
    //{
    //    private static readonly ILog log = LogManager.GetLogger(typeof(FileMonitor));

    //    public bool IsCanceled { get; }

    //    public void Advance(long work)
    //    {
    //        log.Info("work:" + work);
    //    }

    //    public void Start(long totalWork)
    //    {
    //        log.Info("totalWork:" + totalWork);
    //    }

    //    public void StartSubTask(string source, string destination)
    //    {
    //        log.Info("source:" + source + " dest:" + destination);
    //    }

    //    public void Stop()
    //    {
    //        log.Info("stop method called");
    //    }
    //}


    class ADBTest
    {

        //log object
        private static readonly ILog log = LogManager.GetLogger(typeof(ADBTest));

        public static void GetDevicesTest()
        {
            AndroidDebugBridge.Initialize(true);

            List<Device> devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);

            foreach (var item in devices)
            {
                //Monitor.
                FileMonitor monitor = new FileMonitor();

                //List of remote files to be downloaded.
                List<FileEntry> files = new List<FileEntry>();

                //First remote file.
                FileEntry file = FileEntry.Find(item, "/data/data/com.netmarble.lin2ws/shared_prefs/NetmarbleNeo_LineageS.xml");

                //Add remote file to list of remote files.
                files.Add(file);

                //Local filename string split character.
                string axe = ":";

                //Local filename.
                string[] f = Regex.Split(item.SerialNumber, axe);

                //Local path + filename.
                string localFilename = "C:\\test\\" + f[f.Length - 1] + ".xml";

                ShellOutputReceiver receiver = new ShellOutputReceiver();

                if (!item.CanSU())
                {
                    MainWindow.main.UpdateLog = "Root Android.";
                }

                try
                {
                    item.ExecuteShellCommand("input mouse tap 640 340", receiver);
                }
                catch(Exception e)
                {
                    MainWindow.main.UpdateLog = e.ToString();
                }



                //Pull file from remote emulator to local folder.
                item.SyncService.Pull(files, "C:\\test\\", monitor);

                //Rename file to adb port number.
                File.Delete(localFilename);
                File.Move("C:\\test\\NetmarbleNeo_LineageS.xml", localFilename);

                //sample code:
                //<string name="LOCAL_PUSH_TARGET">CharacterName</string>
                string text = Xml.FindInnerTextByTagAttribute(localFilename, "string", "name", "LOCAL_PUSH_TARGET");
                MainWindow.main.UpdateLog = text;

                ////testing shell commands
                //ShellOutPRec receiver = new ShellOutPRec( );
                //item.ExecuteShellCommand("ps", receiver);

                ////testing pull command
                //FileMonitor monitor = new FileMonitor();
                //List<FileEntry> files = new List<FileEntry>();
                //FileEntry file = FileEntry.Find(item, "/data/data/com.netmarble.lin2ws/shared_prefs/");
                //files.Add(file);
                //item.SyncService.Pull(files, "shared_prefs", monitor);

                ////testing screencap
                //PixelFormat format = PixelFormat.Format32bppArgb;
                //Image image = item.Screenshot.ToImage(format);

                //MainWindow.main.UpdateLog = item.SerialNumber + ", " + item.State + item.IsEmulator + " - Emulator" + receiver;
            }

        }
    }

    public class Book
    {
        public String title;

    }
}

