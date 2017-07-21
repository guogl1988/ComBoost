﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Wodsoft.ComBoost
{
    /// <summary>
    /// 领域服务事件路由。
    /// </summary>
    public sealed class DomainServiceEventRoute
    {
        /// <summary>
        /// 注册同步事件。
        /// </summary>
        /// <returns></returns>
        public static DomainServiceEventRoute RegisterEvent(string name, Type ownerType)
        {
            DomainServiceEventRoute route = new DomainServiceEventRoute(name, ownerType, typeof(DomainServiceEventHandler));
            return route;
        }

        /// <summary>
        /// 注册同步事件。
        /// </summary>
        /// <typeparam name="T">事件参数类型。</typeparam>
        /// <returns></returns>
        public static DomainServiceEventRoute RegisterEvent<T>(string name, Type ownerType)
            where T : EventArgs
        {
            DomainServiceEventRoute route = new DomainServiceEventRoute(name, ownerType, typeof(DomainServiceEventHandler<T>));
            return route;
        }

        /// <summary>
        /// 注册异步事件。
        /// </summary>
        /// <returns></returns>
        public static DomainServiceEventRoute RegisterAsyncEvent(string name, Type ownerType)
        {
            DomainServiceEventRoute route = new DomainServiceEventRoute(name, ownerType, typeof(DomainServiceAsyncEventHandler));
            return route;
        }

        /// <summary>
        /// 注册异步事件。
        /// </summary>
        /// <typeparam name="T">事件参数类型。</typeparam>
        /// <returns></returns>
        public static DomainServiceEventRoute RegisterAsyncEvent<T>(string name, Type ownerType)
            where T : EventArgs
        {
            DomainServiceEventRoute route = new DomainServiceEventRoute(name, ownerType, typeof(DomainServiceAsyncEventHandler<T>));
            return route;
        }

        private DomainServiceEventRoute(string name, Type ownerType, Type handlerType)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (ownerType == null)
                throw new ArgumentNullException(nameof(ownerType));
            if (handlerType == null)
                throw new ArgumentNullException(nameof(handlerType));
            Name = name;
            OwnerType = ownerType;
            HandlerType = handlerType;
            DomainServiceEventManager.GlobalEventManager.RegisterEventRoute(this);
        }

        /// <summary>
        /// 获取事件名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取事件注册类类型。
        /// </summary>
        public Type OwnerType { get; private set; }

        /// <summary>
        /// 获取事件委托类型。
        /// </summary>
        public Type HandlerType { get; private set; }

        /// <summary>
        /// 获取路由名称。 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetTypeName(OwnerType) + " - " + Name;
        }

        private string GetTypeName(Type type)
        {
            if (type.GetTypeInfo().IsGenericType)
            {
                string name = type.Name.Substring(0, type.Name.IndexOf('`'));
                name += "<" + string.Join(",", OwnerType.GetGenericArguments().Select(t => GetTypeName(t))) + ">";
                return name;
            }
            else
                return type.Name;
        }
    }
}