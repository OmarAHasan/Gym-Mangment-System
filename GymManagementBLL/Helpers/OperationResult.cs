using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Helpers
{
    public class OperationResult
    {
        public bool Success { get; private set; }
        public string ErrorMessage { get; private set; }
        public static OperationResult Ok() => new OperationResult { Success = true };
        public static OperationResult Fail(string message) => new OperationResult { Success = false, ErrorMessage = message };
    }

}

