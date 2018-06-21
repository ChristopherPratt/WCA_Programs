using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WcaInterfaceLibrary;

namespace WcaFlashApplicationManaged
{
    public enum PD_HARDWARE_SIGNALS
    {
        HS_TEL_ON_TRIGGER_BUTTON = 0,
        HS_TEL_ON_TRIGGER_EXTERN,
        HS_KESSY_TRIGGER_BUTTON,
        HS_KESSY_LED1,
        HS_KESSY_LED2,
        HS_KESSY_LED3,
        HS_KESSY_LED4,
        HS_GENERAL_LED1,
        HS_GENERAL_LED2,
        HS_GENERAL_LED3,
        HS_GENERAL_LED4,
        HS_UNIVERSAL_OUTPUT,
        HS_EXTERN_TEL_ON_ENABLE,
        HS_ENCODER_A,
        HS_ENCODER_B,
        HS_KESSY_SIGNAL_OUTPUT,
        HS_ENCODER_PUSH_BUTTON,
        HS_TEL_ON_SIGNAL,//TelOn_Mc
        HS_SELECT_RXD_TXD,
        HS_SELECT_TEL_ON_RXD,
        HS_12V_SWITCH,
        HS_ENABLE_PWM,
        HS_PWM_DATA_MONITOR,
        HS_MAX_SIGNAL
    };


    public class ProgrammingDevice
    {




        public bool SimulatePressStartButton(int press_time_ms)
        {
            return true;
        }

        public bool SimulatePressProgStartButton(int press_time_ms)
        {
            return true;
        }
    }
}
