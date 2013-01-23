using System;

namespace Xdgk.GR.Common
{
    public class PumpStatus
    {

        #region Members
        /// <summary>
        /// 
        /// </summary>
        static public PumpStatus Run = new PumpStatus(PumpStatusEnum.Run, "����");
        /// <summary>
        /// 
        /// </summary>
        static public PumpStatus Stop = new PumpStatus(PumpStatusEnum.Stop, "ֹͣ");
        #endregion //Members

        #region Find
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public PumpStatus Find(PumpStatusEnum value)
        {
            if (value == PumpStatusEnum.Run)
            {
                return Run;
            }
            else if (value == PumpStatusEnum.Stop)
            {
                return Stop;
            }
            else
            {
                throw new ArgumentException(value.ToString());
            }
        }
        #endregion //Find

        #region PumpStatus
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private PumpStatus(PumpStatusEnum value, string text)
        {
            this.PumpStatusEnum = value;
            this.Text = text;
        }
        #endregion //PumpStatus

        #region PumpStatusEnum
        /// <summary>
        /// 
        /// </summary>
        public PumpStatusEnum PumpStatusEnum
        {
            get
            {
                return _pumpStatusEnum;
            }
            private set
            {
                _pumpStatusEnum = value;
            }
        } private PumpStatusEnum _pumpStatusEnum;
        #endregion //PumpStatusEnum

        #region Text
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get
            {
                if (_text == null)
                {
                    _text = string.Empty;
                }
                return _text;
            }
            private set
            {
                _text = value;
            }
        } private string _text;
        #endregion //Text

        #region ToString
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Text;
        }
        #endregion //ToString
    }


    /// <summary>
    /// 
    /// </summary>
    public class PumpData
    {
        public PumpData(string pumpName, int runningFrequency)
        {
            this.PumpName = pumpName;
            this.RunningFrequency = runningFrequency;
        }
        #region PumpName
        /// <summary>
        /// 
        /// </summary>
        public string PumpName
        {
            get
            {
                if (_pumpName == null)
                {
                    _pumpName = string.Empty;
                }
                return _pumpName;
            }
            set
            {
                _pumpName = value;
            }
        } private string _pumpName;
        #endregion //PumpName

        #region PumpStatus
        /// <summary>
        /// 
        /// </summary>
        public PumpStatus PumpStatus
        {
            get
            {
                return this.RunningFrequency > 0 ? PumpStatus.Run : PumpStatus.Stop;
            }
        } 
        #endregion //PumpStatus

        #region RunningFrequency
        /// <summary>
        /// 
        /// </summary>
        public int RunningFrequency
        {
            get
            {
                return _runningFrequency;
            }
            set
            {
                _runningFrequency = value;
            }
        } private int _runningFrequency;
        #endregion //RunningFrequency

        public override string ToString()
        {
            return string.Format("{0}: {1}Hz", this.PumpName, this.RunningFrequency);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PumpDataCollection : Xdgk.Common.Collection<PumpData>
    {
        public override string ToString()
        {
            string s = string.Empty;
            for (int i = 0; i < this.Count; i++)
            {
                s += this[i].ToString() + ((i == Count - 1) ? "" : ", ");
            }
            return s; 
        }
    }

}
