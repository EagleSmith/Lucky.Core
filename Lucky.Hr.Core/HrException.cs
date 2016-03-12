using System;
using System.Runtime.Serialization;

namespace Lucky.Hr.Core
{
    public class HrException: Exception
    {
        /// <summary>
        /// 初始化Exception类的新实例
        /// </summary>
        public HrException()
        {
        }

        /// <summary>
        /// 使用指定错误信息的Exception类的新实例
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public HrException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 使用指定错误信息的Exception类的新实例
        /// </summary>
        /// <param name="messageFormat">异常消息的格式</param>
        /// <param name="args">异常消息参数</param>
        public HrException(string messageFormat, params object[] args)
			: base(string.Format(messageFormat, args))
		{
		}

        /// <summary>
        /// 初始化序列化数据的异常类的新实例
        /// </summary>
        /// <param name="info">保存有关异常的序列化对象数据的SerializationInfo中被抛出</param>
        /// <param name="context">它包含有关源或目标的上下文信息的StreamingContext</param>
        protected HrException(SerializationInfo
            info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// 使用指定错误信息和参考的内部例外此异常原因的异常类的新实例
        /// </summary>
        /// <param name="message">该解释异常原因的错误消息</param>
        /// <param name="innerException">这是当前异常的原因，或空引用，如果没有指定内部异常的异常</param>
        public HrException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
