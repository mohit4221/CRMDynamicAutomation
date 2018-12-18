using System;

namespace CRMDynamicTestFramework
{

    public enum AppType
    {
        Web,
        POS
    }

    public class Constant
    {
        public static TimeSpan INIT_TIMEOUT_SEC = TimeSpan.FromSeconds(180);
        public static TimeSpan IMPLICIT_TIMEOUT_SEC = TimeSpan.FromSeconds(10);
        public static TimeSpan WAIT_FOR_UPDATE_CHECK = TimeSpan.FromSeconds(45);
        public static TimeSpan WAIT_FOR_EXIST_SEC = TimeSpan.FromSeconds(20);

        public static readonly string ChromeDriverPath = @"..\..\..\packages\Selenium.WebDriver.ChromeDriver.2.45.0\driver\win32\";

        public static readonly string MicrosoftEmailText = "i0116";
        public static readonly string MicrosoftNextButton = "idSIButton9";

    }
}
