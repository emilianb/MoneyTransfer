using System;

namespace MoneyTransfer.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class NotNullAttribute
        : Attribute
    { }
}
