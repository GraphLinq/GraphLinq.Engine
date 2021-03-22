using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Engine.Enums
{
    public enum GraphStateEnum
    {
        STOPPED = 0,
        STARTING = 1,
        STARTED = 2,
        IN_ERROR = 3,
        RESTARTING = 4,
        IN_PAUSE = 5
    }
}
