using System;
using Xdgk.Common;

namespace C3.Communi
{
    abstract public class DeviceBase : IDevice
    {
        #region Address
        /// <summary>
        /// 
        /// </summary>
        public Int64 Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        } private Int64 _address;
        #endregion //Address

        #region Name
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    _name = string.Empty;
                }
                return _name;
            }
            set
            {
                _name = value;
            }
        } private string _name;
        #endregion //Name

        #region Station
        /// <summary>
        /// 
        /// </summary>
        public IStation Station
        {
            get
            {
                return _station;
            }
            set
            {
                _station = value;
            }
        } private IStation _station;
        #endregion //Station

        #region LastData
        /// <summary>
        /// 
        /// </summary>
        public IDeviceData LastData
        {
            get
            {
                return _lastData;
            }
            set
            {
                _lastData = value;
            }
        } private IDeviceData _lastData;
        #endregion //LastData

        #region DeviceDatas
        /// <summary>
        /// 
        /// </summary>
        public DeviceDataCollection DeviceDatas
        {
            get
            {
                if (_deviceDatas == null)
                {
                    _deviceDatas = new DeviceDataCollection();
                }
                return _deviceDatas;
            }
        } private DeviceDataCollection _deviceDatas;
        #endregion //DeviceDatas

        #region DeviceSource
        /// <summary>
        /// 
        /// </summary>
        public IDeviceSource DeviceSource
        {
            get
            {
                return _deviceSource;
            }
            set
            {
                _deviceSource = value;
            }
        } private IDeviceSource _deviceSource;
        #endregion //DeviceSource

        #region Tasks
        /// <summary>
        /// 
        /// </summary>
        public TaskQueue Tasks
        {
            get
            {
                if (_tasks == null)
                {
                    _tasks = new TaskQueue();
                }
                return _tasks;
            }
            set
            {
                _tasks = value;
            }
        } private TaskQueue _tasks;
        #endregion //Tasks

        #region CurrentTask
        /// <summary>
        /// 
        /// </summary>
        public ITask CurrentTask
        {
            get
            {
                return _currentTask;
            }
            set
            {
                _currentTask = value;
            }
        } private ITask _currentTask;
        #endregion //CurrentTask

        #region Dpu
        /// <summary>
        /// 
        /// </summary>
        public IDPU Dpu
        {
            get
            {
                return _dpu;
            }
            set
            {
                _dpu = value;
            }
        } private IDPU _dpu;
        #endregion //Dpu

        #region CommuniDetails
        /// <summary>
        /// 
        /// </summary>
        public CommuniDetailCollection CommuniDetails
        {
            get
            {
                if (_communiDetails == null)
                {
                    _communiDetails = new CommuniDetailCollection();
                }
                return _communiDetails;
            }
            set
            {
                _communiDetails = value;
            }
        } private CommuniDetailCollection _communiDetails;
        #endregion //CommuniDetails


        #region Guid
        /// <summary>
        /// 
        /// </summary>
        public Guid Guid
        {
            get
            {
                if (_guid == null)
                {
                    _guid = new Guid();
                }
                return _guid;
            }
            set
            {
                _guid = value;
            }
        } private Guid _guid;
        #endregion //Guid

        #region StationGuid
        /// <summary>
        /// 
        /// </summary>
        public Guid StationGuid
        {
            get
            {
                if (_stationGuid == null)
                {
                    _stationGuid = new Guid();
                }
                return _stationGuid;
            }
            set
            {
                _stationGuid = value;
            }
        } private Guid _stationGuid;
        #endregion //StationGuid

    }
}