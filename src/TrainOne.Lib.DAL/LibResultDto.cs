using System;
using System.Collections.Generic;
using System.Text;

namespace TrainOne.Lib.DAL
{
    internal class LibResultDto
    {
        public bool State { get; set; } = true;
        public string Message { get; set; } = "ok";
        public dynamic Data { get; set; } = 0;
    }
}
