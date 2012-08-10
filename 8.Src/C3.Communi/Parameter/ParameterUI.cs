using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Xdgk.Common;
using System.Diagnostics;

namespace C3.Communi
{
    public class StringParameterUI : ParameterUIBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        protected override void OnSetParameter(IParameter parameter)
        {
            UCStringParameterUI paramCtrl = new UCStringParameterUI();
            paramCtrl.Parameter = parameter;
            this.Control = paramCtrl;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnApplyNewValue()
        {
            UCStringParameterUI paramCtrl = this.Control as UCStringParameterUI;
            paramCtrl.ApplyNewValue();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EnumParameterUI : ParameterUIBase
    {
        private UCComboBoxParameterUI ComboxParameterUI
        {
            get { return (UCComboBoxParameterUI)this.Control; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        protected override void OnSetParameter(IParameter parameter)
        {
            UCComboBoxParameterUI c = new UCComboBoxParameterUI();
            c.Parameter = parameter;

            this.Control = c;
        }

        protected override void OnApplyNewValue()
        {
            this.ComboxParameterUI.ApplyNewValue();
        }
    }

    public class NumberParameterUI : ParameterUIBase
    {
        /// <summary>
        /// 
        /// </summary>
        private UCNumberParameterUI UC 
        {
            get { return (UCNumberParameterUI)this.Control; }
        }

        protected override void OnSetParameter(IParameter parameter)
        {
            UCNumberParameterUI c = new UCNumberParameterUI();
            c.Parameter = parameter;
            this.Control = c;
        }

        protected override void OnApplyNewValue()
        {
            this.UC.ApplyNewValue();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommuniPortConfigUI : ParameterUIBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        protected override void OnSetParameter(IParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("Parameter");
            }

            Debug.Assert(parameter.Value != null);

            Debug.Assert(parameter.Value is ICommuniPortConfig);

            UCCommuniPortConfigUI c = new UCCommuniPortConfigUI();
            //c.CommuniPortConfig = (ICommuniPortConfig)parameter.Value;
            c.Parameter = parameter;
            this.Control = c;
        }

        /// <summary>
        /// 
        /// </summary>
        private UCCommuniPortConfigUI UIControl
        {
            get
            {
                return (UCCommuniPortConfigUI)this.Control;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnApplyNewValue()
        {
            //throw new NotImplementedException();
            this.UIControl.ApplyNewValue();
        }
    }

}
