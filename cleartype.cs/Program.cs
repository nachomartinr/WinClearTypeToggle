using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace FontSmoothingUtility
{
    class FontSmoothing
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni);

        const int SPI_GETFONTSMOOTHING = 0x004A;
        const int SPI_GETFONTSMOOTHINGTYPE = 0x200A;

        const int SPI_SETFONTSMOOTHING = 0x004B;
        const int SPI_SETFONTSMOOTHINGTYPE = 0x200B;

        const int SPI_GETCLEARTYPE = 0x1048;
        const int SPI_SETCLEARTYPE = 0x1049;

        const int FE_FONTSMOOTHINGSTANDARD = 0x1;
        const int FE_FONTSMOOTHINGCLEARTYPE = 0x2;

        const int SPIF_UPDATEINIFILE = 0x1;
        const int SPIF_SENDCHANGE = 0x2;

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
            SystemParametersInfo(SPI_SETCLEARTYPE, 0, ref pv, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            
            pv = 0;
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 0, ref pv, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            Console.WriteLine("Font Smoothing Disabled: {0}", iResult);

        }

        private static string GetFontSmoothingTypeStr(int value)
        {
            string typeStr;

            switch(value)
            {
                case FE_FONTSMOOTHINGSTANDARD:
                    typeStr = "Standard";
                    break;
                case FE_FONTSMOOTHINGCLEARTYPE:
                    typeStr = "ClearType";
                    break;
                default:
                    typeStr = "Unknown";
                    break;
            }

            return typeStr;
        }

        public static void EnableFontSmoothing()
        {
            bool iResult;

            /* Set the font smoothing value to enabled. */
            // Turn on font smoothing            
            int pv = 0;
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHING, 1, ref pv, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            Console.WriteLine("Font Smoothing Enabled: {0}", iResult);

            pv = 1;
            iResult = SystemParametersInfo(SPI_SETCLEARTYPE, 0, ref pv, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
 
            pv = FE_FONTSMOOTHINGCLEARTYPE;
            iResult = SystemParametersInfo(SPI_SETFONTSMOOTHINGTYPE, 0, ref pv, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);

            int smoothingType = 0;
            if (SystemParametersInfo(SPI_GETFONTSMOOTHINGTYPE, 0, ref smoothingType, 0))
            {
                Console.WriteLine("Smoothing type: {0}", GetFontSmoothingTypeStr(smoothingType));
            }

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
