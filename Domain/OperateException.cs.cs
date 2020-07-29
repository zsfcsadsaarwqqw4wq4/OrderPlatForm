using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// 自定义异常,用于返回自定义错误,防止catch到未知错误
    /// </summary>
    public class OperateException : Exception
    {
        public override string Message { get; }

        public OperateException() : base() { }

        public OperateException(string message) : base(message) => Message = message;

        public OperateException(string message, Exception innerException) : base(message, innerException) => Message = message;
    }


    /// <summary>
    /// Token过期异常
    /// </summary>
    public class TokenException : Exception
    {
        public override string Message { get; }

        public TokenException() : base() { }

        public TokenException(string message) : base(message) => Message = message;

    }
}
