﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace SophiApp.Helpers
{
    internal class WindowsDefenderHelper
    {
        private const string AME_WRONG_VERSION = "0.0.0.0";
        private static readonly List<string> DEFENDER_SERVICES = new List<string>() { "WinDefend", "SecurityHealthService", "wscsvc" };

        internal static bool DisabledByGroupPolicy()
        {
            const string DISABLE_RTM_MONITORING = "DisableRealtimeMonitoring";
            const string DISABLE_BEHAVIOR_MONITORING = "DisableBehaviorMonitoring";
            const string DISABLE_ANTI_SPYWARE = "DisableAntiSpyware";
            const string DEFENDER_REAL_TIME_PATH = @"SOFTWARE\Policies\Microsoft\Windows Defender\Real-Time Protection";
            const string DEFENDER_PATH = @"SOFTWARE\Policies\Microsoft\Windows Defender";
            const int DISABLED_VALUE = 1;

            return RegHelper.GetNullableIntValue(RegistryHive.LocalMachine, DEFENDER_PATH, DISABLE_ANTI_SPYWARE) == DISABLED_VALUE
                    || RegHelper.GetNullableIntValue(RegistryHive.LocalMachine, DEFENDER_REAL_TIME_PATH, DISABLE_RTM_MONITORING) == DISABLED_VALUE
                        || RegHelper.GetNullableIntValue(RegistryHive.LocalMachine, DEFENDER_REAL_TIME_PATH, DISABLE_BEHAVIOR_MONITORING) == DISABLED_VALUE;
        }

        internal static bool IsCorrupted()
        {
            try
            {
                //var servicesRunning = AllServicesIsRunning();
                //var protectionDisabled = WmiHelper.DefenderProtectionIsDisabled();
                //var productEnabled = WmiHelper.DefenderProductStatus() != 1;
                //var amEngineDisabled = WmiHelper.GetDefenderAMEngineVersion() == AME_WRONG_VERSION;
                //var disabledByGroupPolicy = DisabledByGroupPolicy();

                //return servicesRunning || protectionDisabled 
                //    || amEngineDisabled || disabledByGroupPolicy || !productEnabled;
            }
            catch (Exception)
            {
                return true;
            }
        }

        internal static bool AllServicesIsRunning()
        {
            try
            {
                return DEFENDER_SERVICES.TrueForAll(service => ServiceHelper.Get(service).Status == System.ServiceProcess.ServiceControllerStatus.Running);
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool AllServicesExist()
        {
            return DEFENDER_SERVICES.TrueForAll(service => ServiceHelper.ServiceExist(service));
        }
    }
}