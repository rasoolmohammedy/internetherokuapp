using log4net;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace TestExecutor
{
    public interface iExecutor
    {
        ILog logger { get; set; }
        void Initializer();
    }
}
