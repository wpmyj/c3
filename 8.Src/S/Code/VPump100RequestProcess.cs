using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using Xdgk.Common.Protocol;
using C3.Communi;
using VPump100Common;


namespace S
{
    internal class VPump100RequestProcess : IRequestProcess
    {
        /// <summary>
        /// 
        /// </summary>
        public VPump100RequestProcess()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private ReceivePart GetReceivePart()
        {
            if (_rp == null)
            {
                string xmlPath = Application.StartupPath + @"\Config\PumpDeviceDefine\DeviceDefine.xml";
                ReceivePart rp = ReceivePartFacotry.Create(xmlPath, "vPump100", "receive");
                _rp = rp;
            }
            return _rp;
        } private ReceivePart _rp;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="bs"></param>
        public bool Process(Client client, byte[] bs)
        {
            StringBuilder sb = new StringBuilder();

            IParseResult pr = this.GetReceivePart().ToValues(bs);
            if (pr.IsSuccess)
            {
                string name = Convert.ToString(pr.Results["name"]);
                name = name.Trim();

                DateTime dt = (DateTime)pr.Results["dt"];

                sb.AppendLine(string.Format(
                    Strings.PumpDataRequest,
                    name, dt));

                byte[] bsReply = null;
                bsReply = CreatePumpReplyBytes(name, dt, sb);

                bool r = client.CommuniPort.Write(bsReply);

            }

            LogItem log = new LogItem(DateTime.Now, sb.ToString());
            client.LogItems.Add(log);

            return pr.IsSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dt"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        private static byte[] CreatePumpReplyBytes(string name, DateTime dt, StringBuilder logContentBuilder)
        {
            bool existPump = DB.ExistPump(name);
            if (!existPump)
            {
                logContentBuilder.AppendLine(string.Format(
                    Strings.NameNotExist,
                    name));
                return new PumpFailResponse(ResponseStatusEnum.NotExistName).ToBytes();
            }

            DataTable tbl = DB.GetPumpDataTable(name, dt);
            if (tbl.Rows.Count == 0)
            {
                logContentBuilder.AppendLine(
                    string.Format (
                    Strings.HasNotNewDatas,
                    name));
                return new PumpFailResponse(ResponseStatusEnum.NotNewDatas).ToBytes();
            }

            int createdCount = 0;
            VPump100Data[] datas = ConvertToVPump100Datas(tbl, out createdCount);

            logContentBuilder.AppendLine(string.Format(Strings.GetNewDataWithCount, createdCount));
            return new PumpDataResponse(name, datas, (byte)createdCount).ToBytes();
        }

        static private VPump100Data[] ConvertToVPump100Datas(DataTable datatable, out int createCount)
        {
            NullOrDBNullConverter nc = new NullOrDBNullConverter(0f);

            createCount = 0;
            List<VPump100Data> list = new List<VPump100Data>();
            foreach (DataRow row in datatable.Rows)
            {
                DateTime dt = Convert.ToDateTime(row[ColumnNamesForPump.StrTime]);
                float instantFlux = Convert.ToSingle(nc.Convert(row[ColumnNamesForPump.Flux]));
                float efficienty = Convert.ToSingle(nc.Convert(row[ColumnNamesForPump.Efficiency]));
                float sum = Convert.ToSingle(nc.Convert(row[ColumnNames.TuWater]));
                float remain = Convert.ToSingle(nc.Convert(row[ColumnNames.ReWater]));

                PumpStatus pumpStatus = VPumpStatusParser.ParsePumpStatus(row[ColumnNamesForPump.PumpStatus].ToString());
                ForceStartStatus forceStatus = VPumpStatusParser.ParseForceStartStatus(row[ColumnNamesForPump.ForceRun].ToString());
                VibrateStatus vibrateStatus = VPumpStatusParser.ParseVibrateStatus(row[ColumnNamesForPump.Vibrate].ToString());
                PumpPowerStatus powerStatus = VPumpStatusParser.ParsePowerStatus(row[ColumnNamesForPump.Power].ToString());


                VPump100Data data = new VPump100Data();
                data.DT = dt;
                data.InstantFlux = instantFlux;
                data.Efficiency = efficienty;
                data.TotalAmount = sum;
                data.RemainAmount = remain;
                data.PumpStatus = pumpStatus;
                data.ForceStartStatus = forceStatus;
                data.VibrateStatus = vibrateStatus;
                data.PowerStatus = powerStatus;

                list.Add(data);
                if (list.Count >= 5)
                {
                    break;
                }
            }
            createCount = list.Count;
            while (list.Count < 5)
            {
                list.Add(new VPump100Data());
            }
            return list.ToArray();
        }
    }

}
