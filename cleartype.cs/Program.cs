using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace FontSmoothingUtility
{
    class FontSmoothing
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni);

        const uint SPI_GETFONTSMOOTHING = 74;
        const uint SPI_SETFONTSMOOTHING = 75;
        const uint SPI_UPDATEINI = 0x1;
        const UInt32 SPIF_UPDATEINIFILE = 0x1;
        const UInt32 SPIF_SENDCHANGE = 0x1;        
        const uint SPI_GETFONTSMOOTHINGTYPE = 0x200B;
        const uint FE_FONTSMOOTHINGCLEARTYPE = 0x2;

        public static bool GetFontSmoothing()
        {
            bool iResult;
            int pv = 0;
            /* Get the font smoothing value. */
            iResult = SystemParametersInfo(SPI_GETFONTSMOOTHING, 0, ref pv, 0);
            if (pv > 0)
            {
                // font smoothing is on.
                return true;
            }
            else
            {
                // font smoothing is off.
                return false;
            }
        }

        public static void DisableFontSmoothing()
        {
            bool iResult;
            int pv = 0;
            /* Set the font smoothing value to disabled. */
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 0, ref pv, SPIF_UPDATEINIFILE|SPIF_SENDCHANGE);
            Console.WriteLine("Disabled: {0}", iResult);
        }

        public static void EnableFontSmoothing()
        {
            bool iResult;
            int pv = 0;
            /* Set the font smoothing value to enabled. */
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 1, ref pv, SPIF_UPDATEINIFILE|SPIF_SENDCHANGE);
            Console.WriteLine("Smoothing Enabled: {0}", iResult);
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHINGTYPE, 0, FE_FONTSMOOTHINGCLEARTYPE, SPIF_UPDATEINIFILE|SPIF_SENDCHANGE)
            Console.WriteLine("Cleartype Enabled: {0}", iResult);

        }

        public static void ToggleFontSmoothing()
        {
            /* Toggle the font smoothing value */
            bool enabled = GetFontSmoothing();

            if (enabled)
            {
                Console.WriteLine("Disabling ClearType...");
                DisableFontSmoothing();
            }
            else
            {
                Console.WriteLine("Enabling ClearType...");
                EnableFontSmoothing();
            }
        }



    }

    class Program
    {

        static int Main(string[] args)
        {

            FontSmoothing.ToggleFontSmoothing();

            DateTime beginWait = DateTime.Now;
            while (!Console.KeyAvailable && DateTime.Now.Subtract(beginWait).TotalMilliseconds < 1500)
            {
                Thread.Sleep(250);
            }

            return 0;

        }
    }
}
