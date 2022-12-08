using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoofyLittleLinkChanger
{
    public class ConfigOptions
    {
        public string orgName { get; set; }
        public string projectName { get; set; }
        public string sourceWorkItemType { get; set; }
        public string targetWorkItemType { get; set; }
        public string sourceLinkType { get; set; }
        public string targetLinkType { get; set; }
        public string personalAccessToken { get; set; }
    }
}
