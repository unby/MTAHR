﻿using System;

namespace ManagementGui.View.TreeViewUserAndTasks.Common
{
    /// <summary>
    /// This attribute allows a method to be targeted as a recipient for a message.
    /// It requires that the Type is registered with the MessageMediator through the
    /// <seealso cref="MessageMediator.Register"/> method
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// [MessageMediatorTarget("DoBackgroundCheck")]
    /// void OnBackgroundCheck(object parameter) { ... }
    /// 
    /// [MessageMediatorTarget(typeof(SomeDataClass))]
    /// void OnNotifyDataRecieved(SomeDataClass parameter) { ... }
    /// ...
    /// 
    /// mediator.NotifyColleagues("DoBackgroundCheck", new CheckParameters());
    /// ...
    /// mediator.NotifyColleagues(new SomeDataClass(...));
    /// 
    /// ]]>
    /// </example>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MediatorMessageSinkAttribute : Attribute
    {
        /// <summary>
        /// Message key
        /// </summary>
        public object MessageKey { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MediatorMessageSinkAttribute()
        {
            MessageKey = null;
        }

        /// <summary>
        /// Constructor that takes a message key
        /// </summary>
        /// <param name="messageKey">Message Key</param>
        public MediatorMessageSinkAttribute(string messageKey)
        {
            MessageKey = messageKey;
        }

        public MediatorMessageSinkAttribute(Enum messageKey)
        {
            MessageKey = Enum.GetName(messageKey.GetType(), messageKey);
        }
    }

}
