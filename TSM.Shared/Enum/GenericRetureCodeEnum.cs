
using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Shared.Enum
{
    public enum GenericRetureCodeEnum
    {
        Success = 1,
        failure =0,
        Duplicate = -1,
        NotFound = -2,
        AnotherProcessWorking = -3
    }
}
