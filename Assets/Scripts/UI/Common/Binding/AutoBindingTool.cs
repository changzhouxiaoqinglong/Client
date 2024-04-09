using System;
using System.Collections.Generic;
using System.Reflection;

namespace Spore.DataBinding
{
    /// <summary>
    /// 监听绑定数据。
    /// </summary>
    public struct BindingData
    {
        /// <summary>
        /// 此数据是否合法。只有通过构造方法传入有效参数的实例合法。
        /// </summary>
        public bool IsValid { get; }
        /// <summary>
        /// 事件信息。
        /// </summary>
        public EventInfo ValueChangedEventInfo { get; }
        /// <summary>
        /// 事件宿主。
        /// </summary>
        public object BindableProperty { get; }
        /// <summary>
        /// 事件监听。
        /// </summary>
        public Delegate ValueChangedHandlerDelegate { get; }

        /// <summary>
        /// 监听绑定数据。
        /// </summary>
        /// <param name="valueChangedEventInfo">事件信息</param>
        /// <param name="bindableProperty">事件宿主</param>
        /// <param name="valueChangedHandlerDelegate">事件监听</param>
        public BindingData(EventInfo valueChangedEventInfo, object bindableProperty, Delegate valueChangedHandlerDelegate)
        {
            IsValid = true;
            ValueChangedEventInfo = valueChangedEventInfo;
            BindableProperty = bindableProperty;
            ValueChangedHandlerDelegate = valueChangedHandlerDelegate;
        }
    }

    /// <summary>
    /// 自动绑定事件监听工具，该工具会利用 `AutoBindingAttribute` 特性和反射。
    /// </summary>
    public static class AutoBindingTool
    {
        /// <summary>
        /// 通过反射，自动为两个对象中带有 `AutoBindingAttribute` 特性的方法和字段进行事件监听绑定。
        /// </summary>
        /// <param name="bindObj1">绑定对象</param>
        /// <param name="bindObj2">绑定对象</param>
        /// <param name="bindingDatas">用于存储绑定数据，在解绑时需要此数据</param>
        /// <param name="bindingFlags">PropertyInfo和MethodInfo的BindingFlags</param>
        public static void Binding(object bindObj1, object bindObj2, List<BindingData> bindingDatas, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        {
            if (bindObj1 == null)
            {
                return;
            }

            if (bindingDatas == null)
            {
                throw new ArgumentNullException(nameof(bindingDatas), $"A non-null {nameof(bindingDatas)} argument must be passed.");
            }
            BindingFromObserver(bindObj1, bindObj2, bindingDatas, bindingFlags);
            BindingFromObserver(bindObj2, bindObj1, bindingDatas, bindingFlags);
        }

        /// <summary>
        /// 解绑参数列表中传入的所有监听绑定，并清空参数列表。
        /// </summary>
        /// <param name="bindingDatas">监听绑定列表</param>
        public static void Unbinding(List<BindingData> bindingDatas)
        {
            if (bindingDatas != null && bindingDatas.Count > 0)
            {
                foreach (var bindingData in bindingDatas)
                {
                    bindingData.ValueChangedEventInfo.RemoveEventHandler(bindingData.BindableProperty, bindingData.ValueChangedHandlerDelegate);
                }
                bindingDatas.Clear();
            }
        }

        /// <summary>
        /// 通过反射，使用观察者中的方法的 `AutoBindingAttribute` 特性参数进行自动绑定。
        /// </summary>
        /// <param name="publisher">发布源</param>
        /// <param name="observer">观察者</param>
        /// <param name="bindingFlags">PropertyInfo和MethodInfo的BindingFlags</param>
        private static void BindingFromObserver(object publisher, object observer, List<BindingData> bindingDatas, BindingFlags bindingFlags)
        {
            var observerMethodAndAttributeDict = GetMethodAndAttributeDict(observer, bindingFlags);
            var publisherNameAndPropertyDict = GetPropertyAndNameDict(publisher, bindingFlags);

            foreach (var methodAndAttribute in observerMethodAndAttributeDict)
            {
                var bindablePropertyName = methodAndAttribute.Value.PropertyName;
                if (publisherNameAndPropertyDict.TryGetValue(bindablePropertyName, out var bindablePropertyInfo))
                {
                    // 方法信息和其特性数据
                    var valueChangedHandlerInfo = methodAndAttribute.Key;
                    var autoBindingAttribute = methodAndAttribute.Value;
                    var bindingData = BindEventHandler(publisher, observer, bindablePropertyInfo, valueChangedHandlerInfo, autoBindingAttribute, bindingFlags);
                    if (bindingData.IsValid)
                    {
                        bindingDatas.Add(bindingData);
                    }
                }
                else
                {
                    Logger.LogError($"Cannot found property with name {bindablePropertyName} in {publisher.GetType().ToString()}.");
                }
            }
        }

        // 绑定事件监听
        private static BindingData BindEventHandler(object publisher, object observer, PropertyInfo bindablePropertyInfo, MethodInfo valueChangedHandlerInfo, AutoBindingAttribute autoBindingAttribute, BindingFlags bindingFlags)
        {
            // 事件信息
            var valueChangedEventInfo = bindablePropertyInfo.PropertyType.GetEvent(autoBindingAttribute.PropertyEvName, bindingFlags);
            if (valueChangedEventInfo == null)
            {
                Logger.LogError($"Cannot find event {autoBindingAttribute.PropertyEvName} in {bindablePropertyInfo.DeclaringType}.{autoBindingAttribute.PropertyName} with BindingFlags: {bindingFlags}.");
                return default;
            }

            // 从方法信息创建委托并添加为事件监听者
            var bindableProperty = bindablePropertyInfo.GetValue(publisher);
            try
            {
                var valueChangedHandlerDelegate = valueChangedHandlerInfo.CreateDelegate(valueChangedEventInfo.EventHandlerType, observer);
                valueChangedEventInfo.AddEventHandler(bindableProperty, valueChangedHandlerDelegate);

                return new BindingData(valueChangedEventInfo, bindableProperty, valueChangedHandlerDelegate);
            }
            catch (Exception e)
            {
                Logger.LogError($"Exception occurred when CreateDelegate: {e.Message}. EventInfo: {valueChangedEventInfo}, MethodInfo: {valueChangedHandlerInfo}.");
                return default;
            }
        }

        // 根据AutoBindingAttribute获取观察者中的PropertyChanged事件监听方法，并组成Dictionary
        private static Dictionary<MethodInfo, AutoBindingAttribute> GetMethodAndAttributeDict<TObserver>(TObserver observer, BindingFlags bindingFlags)
        {
            var observerMethods = observer.GetType().GetMethods(bindingFlags);
            var observerMethodAndAttributeDict = new Dictionary<MethodInfo, AutoBindingAttribute>();
            foreach (var method in observerMethods)
            {
                var autoBindingAttribute = method.GetCustomAttribute<AutoBindingAttribute>(true);
                if (autoBindingAttribute != null)
                {
                    observerMethodAndAttributeDict.Add(method, autoBindingAttribute);
                }
            }

            return observerMethodAndAttributeDict;
        }

        // 根据AutoBindingAttribute获取发布源中的BindableProperity及其名称，并组成Dictionary
        private static Dictionary<string, PropertyInfo> GetPropertyAndNameDict(object publisher, BindingFlags bindingFlags)
        {
            var publisherProperties = publisher.GetType().GetProperties(bindingFlags);
            var publisherNameAndPropertyDict = new Dictionary<string, PropertyInfo>();
            foreach (var properity in publisherProperties)
            {
                var autoBindingAttribute = properity.GetCustomAttribute<AutoBindingAttribute>(true);
                if (autoBindingAttribute != null)
                {
                    publisherNameAndPropertyDict.Add(autoBindingAttribute.PropertyName, properity);
                }
            }

            return publisherNameAndPropertyDict;
        }
    }
}
