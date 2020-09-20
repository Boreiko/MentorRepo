using PerformanceCounterHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Infrastructure
{
    [PerformanceCounterCategory("MvcMusicStore", PerformanceCounterCategoryType.MultiInstance, "MvcMusicStore")]
    public enum  Counters
    {
        [PerformanceCounter("Login", "Login number", PerformanceCounterType.NumberOfItems32)]
        Login,

        [PerformanceCounter("Logoff", "Logoff  number", PerformanceCounterType.NumberOfItems32)]
        Logoff,
       
        [PerformanceCounter("Registration", "Registration  number", PerformanceCounterType.NumberOfItems32)]
        Registration,
    }
}