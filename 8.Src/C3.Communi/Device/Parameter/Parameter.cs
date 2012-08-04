using System;
using System.Collections.Generic;
using System.Text;
using Xdgk.Common;
using System.Diagnostics;


namespace C3.Communi
{
    public class Parameter : IParameter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="orderNumber"></param>
        public Parameter(string name, object value, int orderNumber)
            : this(name, value, null, orderNumber, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="orderNumber"></param>
        public Parameter(string name, object value, Unit unit, int orderNumber)
            : this(name, value, unit, orderNumber, null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valueType"></param>
        /// <param name="value"></param>
        /// <param name="orderNumber"></param>
        public Parameter(string name, Type valueType, object value, int orderNumber)
            : this(name, value, null, orderNumber, null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="valueType"></param>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="orderNumber"></param>
        /// <param name="description"></param>
        public Parameter(string name, object value, Unit unit, int orderNumber, string description)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.Name = name;
            this.ValueType = value.GetType();
            this.Value = value;
            this.Unit = unit;
            this.OrderNumber = orderNumber;
            this.Description = description;
        }
        #region OrderNumber
        /// <summary>
        /// 
        /// </summary>
        public int OrderNumber
        {
            get
            {
                return _orderNumber;
            }
            set
            {
                _orderNumber = value;
            }
        } private int _orderNumber;
        #endregion //OrderNumber

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
                if (value == null || value.Trim().Length == 0)
                {
                    throw new ArgumentException("Name is null or empty");
                }
                _name = value;
            }
        } private string _name;
        #endregion //Name

        #region Value
        /// <summary>
        /// 
        /// </summary>
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                SetValue(value);
            }
        } private object _value;
        #endregion //Value

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(string stringValue)
        {
            object r = null;
            if (this.ValueType.IsEnum)
            {
                r = Enum.Parse(this.ValueType, stringValue);
            }
            else if (this.ValueType.IsValueType)
            {
                r = Convert.ChangeType(stringValue, this.ValueType);
            }

            SetValue(r);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected void SetValue(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Value");
            }

            if (value != _value)
            {
                this.VerifyValue(value);
                this._value = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        virtual protected void VerifyValue(object value)
        {
            if (value.GetType() != this.ValueType)
            {
                string s = string.Format("value type is '{0}', but expect is '{1}'",
                        value.GetType().Name,
                        this.ValueType.GetType().Name);
                throw new InvalidOperationException(s);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="conversionType"></param>
        /// <returns></returns>
        private object ChangeValueType(object value, Type conversionType)
        {
            object result = null;
            if (conversionType.IsValueType)
            {
                result = Convert.ChangeType(value, conversionType);
            }

            // TODO:
            //
            return result;
        }

        #region Description
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                if (_description == null)
                {
                    _description = string.Empty;
                }
                return _description;
            }
            set
            {
                _description = value;
            }
        } private string _description;
        #endregion //Description

        #region Unit
        /// <summary>
        /// 
        /// </summary>
        public Unit Unit
        {
            get
            {
                if (_unit == null)
                {
                    _unit = Unit.FindByName(Unit.None);
                }
                return _unit;
            }
            set
            {
                _unit = value;
            }
        } private Unit _unit;
        #endregion //Unit

        #region ValueType
        /// <summary>
        /// 
        /// </summary>
        public Type ValueType
        {
            get
            {
                return _valueType;
            }
            set
            {
                _valueType = value;
            }
        } private Type _valueType;
        #endregion //ValueType


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
                    _text = this.Name;
                }
                return _text;
            }
            set
            {
                _text = value;
            }
        } private string _text;

        #endregion

        #region Group
        /// <summary>
        /// 
        /// </summary>
        public ParameterGroup Group
        {
            get
            {
                return _group;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Group");
                }
                _group = value;
            }
        } private ParameterGroup _group;


        /// <summary>
        /// 
        /// </summary>
        public IParameterUI ParameterUI
        {
            get
            {
                // TODO: 2012-08-04
                //
                return _parameterUI;
            }
            set
            {
                _parameterUI = value;
            }
        } private IParameterUI _parameterUI;

        #endregion
    }

}