using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MarkMpn.SecurityDebugger
{
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Security Debugger"),
        ExportMetadata("Description", "Understand and resolve errors in permissions error messages"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAABgUExURQAAADw8PKioqB4eHvb29vjrufzZTffx1//MAPvebLi4uP7PD9fX1/7RH+bm5v3ULsfHx/zcXP3WPomJifvhe0xMTPnpqVtbWw4ODvfz5/rkipqamvjuyGxsbC0tLXp6egbjaL0AAAAJcEhZcwAADsMAAA7DAcdvqGQAAAEVSURBVDhPjZPrdoMgEIQTO4moeKmJxktM3/8ty26mCkTP6fxaZj5ZQDiFOifJF8s9nRM4HSISX67X9AB5x8ZpF9li0QcSxqIA0ThjsspDchcXtH05xCpgUdIyRsYsjSlQEdi+DwBDoDoG8D/AoqYRASVbNEdAzV3k+KYTAS1yBW6404mADDcFOvR0IqDHQ4EBI50ISNEpEBylp5qbkEVM9ALdMRPoth4yZGmefx2kR0tTRixb2EFTpwdSujJiecGiocqCF0YG7yrzJtBVRBspRh4CNeMZXKqi5ymuypFuv8yUKRqvgWhoMK5E7fLPt+Pu9qQLKacRefS9anHvsi9bF1feBn29Znm7qOYXjR0t1v74s59OvxR+FgdnF2W5AAAAAElFTkSuQmCC"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAMAAAC5zwKfAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAB1UExURQAAAA4ODnp6etfX14mJifb29vfz5/npqf3WPvnmmv3ULv/MAP7RH/jruS0tLVtbW2xsbJqamvvebI19PT47Lp6KOvzcXKioqPzZTW1kPMfHx+bm5ri4uPvhe0xMTPfx1x4eHv7PD62VNc6sJfjuyDw8PPrkio3P0DwAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAMVSURBVFhH5ZjbeuogEEY9RW08VG2r1ag1u+r7P+JmJj9KOAwk8a7rqgFcH2SGIbSXRn8wHOHPV6B0meJVSuiIVygNHdFVaemILsqnbjyZvuHP9kpTl89ms3k3pa0jOih9OqKl0tAtTB2RN1dKOqKhMqYjGiifuiyoIxKVqToiQdlER0SUTXWEoGyjI0LKZTsdYSiXsCne0dRYRzyUK9gUK25opSOgHMCmWNPzBt0OH5+aL7Q4bEmwhk2xo+dvdDrsMarX26PFYUqCHUYpWDhFp0OCMCfBAaMUB3oOvsEE4ZwEQ4xScNZ0FhYYpfiDwpcH5UjPr0ybzkJObEPIW2+LTodU4RGjFCN6Du7lBKG9l1l4QqdDgnBjCQf0/IZOhwThiQRGzeZ6OEGnQ4JwQgKjHp7peYxOhwThggRnjFL0uwp/SHDBKKKghjl6beJCe+dFMjsutPM6kohxoZ01iEoozHEhB9k4RdVJTy2hqMSFHBMjyAhzKCpRYcm/NoOMEhsoD1HhN/3YqIYERyWw+f7tNYGDnjeecSoT8l6J4L7CyEuMwOXaeoVI7WBJFOEsrKU1wV907dbMlcEoNRXCmn+/NL9oqeFfMT6YvHGOpA3H2FmxtGZZWHKMnRXrNftyWxZyVhfuipHbvinKQg5JrdJoglMUhTxB8wJgwMe9Z4qi8Eo/srad5kJ9nilKwmqCN/TbcOa4UxSEpTRB/Radwi0IOQcL/xskONDO/eJ5T/lAi4YPp+D1VtHnOjsuMT5GteCij1/74LIY/iixuPPo2uHkwHEJfyrW4LLlz+kn1aKTjNsxjRxKCyZuLAx/Hz/I2WdXfg/VVTd8JwDwCRF+wDtQuJwy03SfDox4wGwq3zr2AgEfWEL2lPinwCHR95jj1X+s5vxtpK4RyT69B7PsDodBea+Wm40a+HAvUIytjCy3fIQoPKeIyI2/khVXQ1lOefcqineMS6eP9FGzvFfvMj/p2WVH36EUBSlOLDblVk9OTU+uB2GekzQpdq2mV3FxlMUuXJ+TWNaUxbGjjjkjz4vjKpZ6vd5/ylOhvZDr0B0AAAAASUVORK5CYII="),
        ExportMetadata("BackgroundColor", "DarkMagenta"),
        ExportMetadata("PrimaryFontColor", "White"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class SecurityDebuggerPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new SecurityDebuggerPluginControl();
        }
    }
}
