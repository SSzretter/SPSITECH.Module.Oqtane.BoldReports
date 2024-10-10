using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Shared;
using System.Collections.Generic;

namespace SPSITECH.Module.BoldReports
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "BoldReports",
            Description = "SPSITECH.BoldReports",
            Version = "1.0.0",
            ServerManagerType = "SPSITECH.Module.BoldReports.Manager.BoldReportsManager,SPSITECH.Module.BoldReports.Server.Oqtane",
            ReleaseVersions = "1.0.0",
            Dependencies = "SPSITECH.Module.BoldReports.Shared.Oqtane,Bold.Licensing,BoldReports.Net.Core,BoldReports.Data.WebData,Newtonsoft.Json",
            PackageName = "SPSITECH.Module.BoldReports",
            Resources =  new List<Resource>()
            {
                new Resource { ResourceType = ResourceType.Stylesheet, Url =  "~/Module.css" },
                new Resource { ResourceType = ResourceType.Script, Url =  "~/Module.js" },

                new Resource { ResourceType = ResourceType.Stylesheet, Url =  "https://cdn.boldreports.com/6.2.37/content/v2.0/tailwind-light/bold.report-designer.min.css" },
                new Resource { ResourceType = ResourceType.Stylesheet, Url =  "https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.37.0/codemirror.min.css" },
                new Resource { ResourceType = ResourceType.Stylesheet, Url =  "https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.37.0/addon/hint/show-hint.min.css" },

                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.37.0/codemirror.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.37.0/addon/hint/show-hint.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.37.0/addon/hint/sql-hint.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.37.0/mode/sql/sql.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.37.0/mode/vb/vb.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdn.boldreports.com/6.2.37/scripts/v2.0/common/bold.reports.common.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdn.boldreports.com/6.2.37/scripts/v2.0/common/bold.reports.widgets.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdn.boldreports.com/6.2.37/scripts/v2.0/bold.report-viewer.min.js" },
                new Resource { ResourceType = ResourceType.Script, Url =  "https://cdn.boldreports.com/6.2.37/scripts/v2.0/bold.report-designer.min.js" },
            }
    };
    }
}
