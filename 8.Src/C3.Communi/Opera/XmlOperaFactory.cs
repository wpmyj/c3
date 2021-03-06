
using System;
using Xdgk.Common;

namespace C3.Communi
{
    public class XmlOperaFactory : IOperaFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defineDirectory"></param>
        public XmlOperaFactory(string defineDirectory)
        {
            string path = System.IO.Path.Combine(defineDirectory, "DeviceDefine");
            this.DefineDirectory = path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operaName"></param>
        /// <returns></returns>
        public IOpera Create(string deviceType, string operaName)
        {
            IOpera op = this.OperaDefines.Create(deviceType, operaName);
            return op;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DefineDirectory
        {
            get { return _defineDirectory; }
            set { _defineDirectory = value; }
        } private string _defineDirectory;

        /// <summary>
        /// 
        /// </summary>
        public OperaDefineCollection OperaDefines
        {
            get 
            {
                if (_operaDefines == null)
                {
                    OperaDefineFactory f = new OperaDefineFactory();
                    f.LoadFromPath(DefineDirectory);
                    _operaDefines = f.DeviceDefineCollection;
                    
                }
                return _operaDefines; 
            }
            set { _operaDefines = value; }
        } private OperaDefineCollection _operaDefines;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public PickerCollection CreatePickers(string deviceType)
        {       
            PickerCollection uploadParsers = new PickerCollection();
            OperaDefineFactory f = new OperaDefineFactory();
            f.LoadFromPath(DefineDirectory);
            foreach ( UploadDefine ud in f.UploadDefines )
            {
                IPicker p = ud.Create("aaa");
                uploadParsers.Add(p);
            }
            return uploadParsers;
        }
    }

}
