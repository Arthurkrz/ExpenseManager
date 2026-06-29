using System.Collections.Generic;

namespace ExpenseManager.Core.Entities
{
    public class ServiceResponse
    {
        public bool Success { get; set; }

        public List<string> Errors { get; set; } = [];

        public ServiceResponse() 
        {
            Errors = [];
        }

        public ServiceResponse(bool success, List<string> errors = null)
        {
            Success = success;
            Errors = errors ?? [];
        }
    }
}
