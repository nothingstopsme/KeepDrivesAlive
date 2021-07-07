using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeepDrivesAlive
{
    

    public class DriveChecker
    {
        public class OnRefreshedEventArgument
        {
            public OnRefreshedEventArgument(string group, IEnumerable<string> deviceIds)
            {
                _group = group;
                _deviceIds = deviceIds.ToList().AsReadOnly();
            }

            public string Group
            {
                get { return _group; }
            }

            public ReadOnlyCollection<string> Drives
            {
                get { return _deviceIds; }
            }

            private string _group;
            private ReadOnlyCollection<string> _deviceIds;
        }

        public delegate void OnRefreshedEventHandler(object sender, OnRefreshedEventArgument ea);


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct DRIVE_PERFORMANCE
        {
            public Int64 BytesRead;
            public Int64 BytesWritten;
            public Int64 ReadTime;
            public Int64 WriteTime;
            public Int64 IdleTime;
            public UInt32 ReadCount;
            public UInt32 WriteCount;
            public UInt32 QueueDepth;
            public UInt32 SplitCount;
            public Int64 QueryTime;
            public UInt32 StorageDeviceNumber;

            
            public UInt16 StorageManagerName0;
            public UInt16 StorageManagerName1;
            public UInt16 StorageManagerName2;
            public UInt16 StorageManagerName3;
            public UInt16 StorageManagerName4;
            public UInt16 StorageManagerName5;
            public UInt16 StorageManagerName6;
            public UInt16 StorageManagerName7;
            public UInt16 StorageManagerName8;
            public UInt16 StorageManagerName9;


        }

        private class DriveRecord
        {
            

            public string deviceID = null;
            public string caption = null;            
            public DRIVE_PERFORMANCE performance = new DRIVE_PERFORMANCE {QueryTime = -1};
            
            
        }

        private class TaskRecord
        {

            [DllImport("Kernel32.dll", SetLastError = true)]
            private static extern bool DeviceIoControl(
                SafeFileHandle hDevice,
                uint IoControlCode,
                IntPtr InBuffer,
                int nInBufferSize,
                byte[] OutBuffer,
                int nOutBufferSize,
                out int pBytesReturned,
                IntPtr Overlapped
            );

            [DllImport("Kernel32.dll", SetLastError = true)]
            private static extern bool DeviceIoControl(
                SafeFileHandle hDevice,
                uint IoControlCode,
                IntPtr InBuffer,
                int nInBufferSize,
                out DRIVE_PERFORMANCE OutBuffer,
                int nOutBufferSize,
                out int pBytesReturned,
                IntPtr Overlapped
            );

            [DllImport("Kernel32.dll", SetLastError = true)]
            private static extern bool DeviceIoControl(
                SafeFileHandle hDevice,
                uint IoControlCode,
                IntPtr InBuffer,
                int nInBufferSize,
                IntPtr OutBuffer,
                int nOutBufferSize,
                out int pBytesReturned,
                IntPtr Overlapped
            );


            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern int CreateFile(
                string lpFileName,
                uint dwDesiredAccess,
                uint dwShareMode,
                uint lpSecurityAttributes,
                uint dwCreationDisposition,
                uint dwFlagsAndAttributes,
                uint hTemplateFile
                );

            /*[DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool CloseHandle(
                SafeFileHandle hObject
                );
            */

            private const uint IOCTL_DISK_UPDATE_PROPERTIES = 0x00000007 << 16 | 0x0 << 14 | 0x0050 << 2 | 0x0;
            private const uint IOCTL_DISK_PERFORMANCE = 0x00000007 << 16 | 0x0 << 14 | 0x0008 << 2 | 0x0;
            private const uint IOCTL_DISK_PERFORMANCE_OFF = 0x00000007 << 16 | 0x0 << 14 | 0x0018 << 2 | 0x0;

            
            public TaskRecord()
            {
                _isAutoDetecting = true;
                _name = AUTO_DETECTING_CAPTION;               
                _query = new ManagementObjectSearcher(@"SELECT * FROM Win32_DiskDrive WHERE MediaType='External hard disk media'");


                _task = Do();
                
                
            }

            public TaskRecord(string caption)
            {
                _isAutoDetecting = false;
                _name = caption;                                               
                _query = new ManagementObjectSearcher(String.Format(@"SELECT * FROM Win32_DiskDrive WHERE Caption='{0}'", caption));

                _task = Do();
                
                
            }

                        

            private async Task Do()
            {
                int nextInterval = FOREVER_INTERVAL, elapsed = 0;
                CancellationToken runningToken = _runningTokenSource.Token;
                _waitingTokenSource = CancellationTokenSource.CreateLinkedTokenSource(runningToken);
                CancellationToken waitingToken = _waitingTokenSource.Token;
                
                

                //setting the first startTime to 0 so that it always trigger checking for the first waking up
                long startTime = 0;                
                while (!runningToken.IsCancellationRequested)
                {                                       
                    while (!waitingToken.IsCancellationRequested)
                    {                                                
                        try
                        {                            
                            await Task.Delay(nextInterval, waitingToken).ConfigureAwait(false);                            
                        }
                        catch (TaskCanceledException e)
                        {

                        }                         
                        long endTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                        elapsed += (int)Math.Min(endTime - startTime, (long)(int.MaxValue - elapsed));


                        if (!runningToken.IsCancellationRequested)
                            Do(ref elapsed, out nextInterval);
                        
                                                

                        startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    }

                    CancellationTokenSource oldWaitingTokenSource = null;
                    lock (_waitingTokenSourceLock)
                    {
                        if (_cancellationPending)
                            _cancellationPending = false;
                        else
                        {
                            oldWaitingTokenSource = _waitingTokenSource;
                            _waitingTokenSource = CancellationTokenSource.CreateLinkedTokenSource(runningToken); ;
                            waitingToken = _waitingTokenSource.Token;
                        }
                    }

                    if (oldWaitingTokenSource != null)
                        oldWaitingTokenSource.Dispose();
                    
                    
                }

                
            }

            private void Do(ref int elapsed, out int nextInterval)
            {

                nextInterval = refreshDriveRecords();
                
                bool due = false;                
                if (nextInterval != TaskRecord.FOREVER_INTERVAL)
                {

                    if (elapsed >= nextInterval)
                    {
                        due = true;
                        elapsed = 0;
                    }
                    else
                        nextInterval -= elapsed;
                }


                if (due)
                {
                    foreach (DriveRecord driveRecord in _driveRecordTable.Values)
                    {
                        SafeFileHandle handle = null;
                        try
                        {
                            /*
                             * dwDesiredAccess: NONE (0), 
                             * dwShareMode: FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE, 
                             * dwCreationDisposition: OPEN_EXISTING
                            */
                            IntPtr handlePtr = (IntPtr)CreateFile(driveRecord.deviceID, 0x00000000, 0x00000007, 0, 3, 0, 0);
                            handle = new SafeFileHandle(handlePtr, true);
                            TryToNudge(handle, driveRecord);
                            Checkpoint(handle, driveRecord);

                        }
                        finally
                        {
                            if (handle != null && !handle.IsClosed)
                                handle.Close();
                        }
                    }
                }

            }


            private int refreshDriveRecords()
            {
                DrivesSection drivesSection = (DrivesSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection(DrivesSection.Name);
                HashSet<string> unusedKeys = new HashSet<string>(_driveRecordTable.Keys);
                int nextInterval = TaskRecord.FOREVER_INTERVAL;
                bool enabled = true;
                if (IsAutoDetecting)
                {
                    if (drivesSection.Auto.Enabled)
                        nextInterval = drivesSection.Auto.Interval * 1000;
                    else
                        enabled = false;

                }
                else
                {
                    //in this case, "Name" will be equal to the caption for the corresponding drive
                    DriveConfiguration driveConfig = drivesSection.Drives[Name];
                    if (driveConfig != null && driveConfig.Enabled)
                    {
                        nextInterval = driveConfig.Interval * 1000;
                    }
                    else
                        enabled = false;
                }

                if (enabled)
                {

                    foreach (ManagementObject device in Query.Get())
                    {

                        string deviceID = (string)device.GetPropertyValue("DeviceID");
                        string caption = (string)device.GetPropertyValue("Caption");


                        DriveConfiguration foundDriveConfig = drivesSection.Drives[caption];

                        if ((IsAutoDetecting && foundDriveConfig == null) || (!IsAutoDetecting && foundDriveConfig != null))
                        {                                                        
                            if (!_driveRecordTable.ContainsKey(deviceID))
                            {
                                DriveRecord drive = new DriveRecord();
                                drive.deviceID = deviceID;
                                drive.caption = caption;
                                _driveRecordTable.Add(deviceID, drive);

                            }                            

                            unusedKeys.Remove(deviceID);


                        }

                    }
                }
                foreach (string key in unusedKeys)
                {
                    _driveRecordTable.Remove(key);
                }
                



                //raising the refreshed event and auto unregistering any handlers being notified
                OnRefreshedEventHandler drivesRefreshedHandler = _drivesRefreshed;                
                if (drivesRefreshedHandler != null)
                {
                    _drivesRefreshed -= drivesRefreshedHandler;
                    
                    drivesRefreshedHandler(this, new OnRefreshedEventArgument(Name, _driveRecordTable.Select(item => item.Value.caption + ", " + item.Value.deviceID)));
                }


                return nextInterval;
            }

            

            private void TryToNudge(SafeFileHandle handle, DriveRecord driveRecord)
            {
                DRIVE_PERFORMANCE performanceNow = new DRIVE_PERFORMANCE();
                int written = 0;
                bool nudgeDrive = false;

                if (driveRecord.performance.QueryTime >= 0)
                {
                    if (!DeviceIoControl(handle, IOCTL_DISK_PERFORMANCE, IntPtr.Zero, 0, out performanceNow, Marshal.SizeOf(performanceNow), out written, IntPtr.Zero))
                    {
                        Trace.WriteLine(this, String.Format("Something wrong when querying the performance of {0} after waking up, lastError = {1}", driveRecord.caption, Marshal.GetLastWin32Error()));                        
                    }

                    nudgeDrive = (driveRecord.performance.ReadCount == performanceNow.ReadCount && driveRecord.performance.WriteCount == performanceNow.WriteCount);

                    //driveRecord.performance = performanceNow;
                }
                else
                    nudgeDrive = true;



                if (nudgeDrive)
                {
                    if (!DeviceIoControl(handle, IOCTL_DISK_UPDATE_PROPERTIES, IntPtr.Zero, 0, IntPtr.Zero, 0, out written, IntPtr.Zero))
                    {
                        Trace.WriteLine(this, String.Format("Something wrong when waking up {0}, lastError = {1}", driveRecord.caption, Marshal.GetLastWin32Error()));
                    }                    
                }
            }


            private void Checkpoint(SafeFileHandle handle, DriveRecord driveRecord)
            {
                int written = 0;

                if (!DeviceIoControl(handle, IOCTL_DISK_PERFORMANCE, IntPtr.Zero, 0, out driveRecord.performance, Marshal.SizeOf(driveRecord.performance), out written, IntPtr.Zero))
                {
                    Trace.WriteLine(this, String.Format("Something wrong when querying the performance of {0} before going to sleep, lastError = {1}", driveRecord.caption, Marshal.GetLastWin32Error()));
                }


            }


            public void Invalidate()
            {
                lock (_waitingTokenSourceLock)
                {
                    if (_waitingTokenSource.IsCancellationRequested)
                        _cancellationPending = true;
                    else
                        _waitingTokenSource.Cancel();
                    
                }
                
            }

            

            public void Close()
            {
                _runningTokenSource.Cancel();                                
            }

            


            public string Name
            {
                get { return _name; }
            }
            public bool IsAutoDetecting
            {
                get { return _isAutoDetecting; }
            }
            public ManagementObjectSearcher Query
            {
                get { return _query; }
            }

            public Task Task
            {
                get { return _task; }
            }

            public event OnRefreshedEventHandler DrivesRefreshed{
                add
                {
                    _drivesRefreshed += value;
                }
                remove
                {
                    _drivesRefreshed -= value;
                }
            }

            

            public const string AUTO_DETECTING_CAPTION = "AutoDetection";            
            public const int FOREVER_INTERVAL = -1;

            private readonly object _waitingTokenSourceLock = new object();
            private bool _cancellationPending = false;
            
            private CancellationTokenSource _runningTokenSource = new CancellationTokenSource();
            private CancellationTokenSource _waitingTokenSource = null;                        

            private event OnRefreshedEventHandler _drivesRefreshed;
            
            private bool _isAutoDetecting = false;            
            private readonly ManagementObjectSearcher _query = null;
            private string _name = null;                        
            private readonly Dictionary<string, DriveRecord> _driveRecordTable = new Dictionary<string, DriveRecord>();

            private Task _task = null;
        }

        

        
        private readonly Dictionary<string, TaskRecord> _taskTable = new Dictionary<string, TaskRecord>();
        private TaskRecord _autoTask = null;        

        

        public void Reload()
        {
            DrivesSection drivesSection = (DrivesSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection(DrivesSection.Name);
            
            
            _autoTask.Invalidate();



            HashSet<string> unusedKeys = new HashSet<string>(_taskTable.Keys);
            foreach (DriveConfiguration driveConfig in drivesSection.Drives)
            {
                if (_taskTable.ContainsKey(driveConfig.Caption))
                {
                    unusedKeys.Remove(driveConfig.Caption);
                    _taskTable[driveConfig.Caption].Invalidate();
                }
                else 
                {
                    TaskRecord taskRecord = new TaskRecord(driveConfig.Caption);
                    _taskTable.Add(driveConfig.Caption, taskRecord);
                    taskRecord.Invalidate();
                }                

            }

            foreach (string unusedKey in unusedKeys)
            {
                TaskRecord taskRecord = _taskTable[unusedKey];                
                _taskTable.Remove(unusedKey);
                
                taskRecord.Close();
            }
           

        }

        

        public void Refresh(OnRefreshedEventHandler onceHandler)
        {
            _autoTask.DrivesRefreshed -= onceHandler;
            _autoTask.DrivesRefreshed += onceHandler;
            _autoTask.Invalidate();

            foreach(TaskRecord taskRecord in _taskTable.Values)
            {
                taskRecord.DrivesRefreshed -= onceHandler;
                taskRecord.DrivesRefreshed += onceHandler;
                taskRecord.Invalidate();
            }
           
        }

        public Task Close(int timeout=10*1000)
        {                        
            List<Task> taskList = new List<Task>(_taskTable.Count+1);
            _autoTask.Close();
            taskList.Add(_autoTask.Task);
            foreach (TaskRecord taskRecord in _taskTable.Values)
            {
                taskRecord.Close();
                taskList.Add(taskRecord.Task);
            }

            return Task.Run(() =>
            {
                Task.WaitAll(taskList.ToArray(), timeout);
            });
            
        }

        public DriveChecker()
        {

            _autoTask = new TaskRecord();
            _autoTask.Invalidate();

            DrivesSection drivesSection = (DrivesSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection(DrivesSection.Name);
            foreach (DriveConfiguration driveConfig in drivesSection.Drives)
            {
                
                TaskRecord taskRecord = new TaskRecord(driveConfig.Caption);
                _taskTable.Add(driveConfig.Caption, taskRecord);
                taskRecord.Invalidate();                
                
            }

                                    
            

        }

        
    }
}
