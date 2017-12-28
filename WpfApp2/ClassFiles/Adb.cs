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
    class ShellOutputReceiver : IShellOutputReceiver
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShellOutputReceiver));

        bool IShellOutputReceiver.IsCancelled { get; }

        public void AddOutput(byte[] data, int offset, int length)
        {
            log.Info(System.Text.Encoding.ASCII.GetString(data, offset, length));
        }

        public void Flush()
        {
        }
    }

    class FileMonitor : ISyncProgressMonitor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FileMonitor));

        public bool IsCanceled { get; }

        public void Advance(long work)
        {
            log.Info("work:" + work);
        }

        public void Start(long totalWork)
        {
            log.Info("totalWork:" + totalWork);
        }

        public void StartSubTask(string source, string destination)
        {
            log.Info("source:" + source + " dest:" + destination);
        }

        public void Stop()
        {
            log.Info("stop method called");
        }
    }

    public class Adb
    {
        //Monitor.
        private FileMonitor monitor = new FileMonitor();

        //Object that receives Adb shell STDOUT
        private ShellOutputReceiver receiver = new ShellOutputReceiver();

        public Device[] GetADBDevices()
        {
            AndroidDebugBridge.Initialize(true);

            List<Device> Devices = AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress);

            Device[] DevicesArray = Devices.ToArray();

            return DevicesArray;
        }
    }
}
