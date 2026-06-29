using System.Collections.Generic;

namespace ExpenseManager.Core.Entities
{
    public class ServiceResponseGeneric<T> : ServiceResponse
    {
        public T Data { get; set; }

        public ServiceResponseGeneric() { }

        public ServiceResponseGeneric(bool success, T data = default, List<string> errors = null) : base(success, errors)
        {
            Data = data;
        }
    }
}
