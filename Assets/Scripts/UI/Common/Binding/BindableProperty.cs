using System.Collections.Generic;

namespace Spore.DataBinding
{
    /// <summary>
    /// 可绑定ValueChanged事件监听的属性。
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    public class BindableProperty<TProperty>
    {
        public delegate void ValueChangedEventHandler(TProperty oldValue, TProperty newValue);
        /// <summary>
        /// 当Value发生变化时触发此事件。
        /// </summary>
        public event ValueChangedEventHandler ValueChanged;

        private TProperty _value;
        public TProperty Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }

        public BindableProperty(TProperty value = default)
        {
            _value = value;
        }

        public void ClearListeners()
        {
            ValueChanged = null;
        }

        /// <summary>
        /// 如果 `Value` 不为 `null` ，返回 `Value.ToString()` ；
        /// 否则返回 `null` 。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value?.ToString();
        }


        /// <summary>
        /// 为字段赋值，并在值发生改变时触发事件。
        /// 如果字段的值改变，则返回 `true` ，否则返回 `false` 。
        /// </summary>
        /// <param name="field">目标字段</param>
        /// <param name="value">目标值</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        protected bool SetField(ref TProperty field, TProperty value)
        {
            if (!EqualityComparer<TProperty>.Default.Equals(field, value))
            {
                var oldValue = field;
                field = value;
                ValueChanged?.Invoke(oldValue, value);
                return true;
            }
            return false;
        }
    }
}
