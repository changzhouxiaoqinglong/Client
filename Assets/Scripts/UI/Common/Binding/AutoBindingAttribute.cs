using System;

namespace Spore.DataBinding
{
    /// <summary>
    /// 标记属性或方法将会用于事件的自动绑定。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, Inherited = true)]
    public class AutoBindingAttribute : Attribute
    {
        /// <summary>
        /// 属性名称。
        /// </summary>
        public string PropertyName { get; private set; }
        /// <summary>
        /// 属性变化事件名称。
        /// </summary>
        public string PropertyEvName { get; private set; }

        /// <summary>
        /// 标记属性或方法将会用于事件的自动绑定。
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyEvName">事件名称，默认值为 `ValueChanged`</param>
        public AutoBindingAttribute(string propertyName, string propertyEvName = "ValueChanged")
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException($"Argument {nameof(propertyName)} cannot be empty.", nameof(propertyName));
            }

            PropertyName = propertyName;

            PropertyEvName = propertyEvName;
        }
    }
}
