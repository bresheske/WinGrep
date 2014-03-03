using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinGrep.Core.Models;

namespace WinGrep.Core.Services
{
    public class ResultFormatter
    {

        public string FormatResult(ContentsResult result)
        {
            var sb = new StringBuilder();
            sb.Append(result.FileName);
            if (result.FileLine > 0)
                sb.AppendFormat(":{0}{1}", result.FileLine, Environment.NewLine);
            else
                sb.AppendLine();
            if (result.CaptureGroups != null && result.CaptureGroups.Count() > 1)
                for (int i = 0; i < result.CaptureGroups.Count(); i++ )
                    sb.AppendFormat("\t[{0}]:{1}{2}", i, result.CaptureGroups.ElementAt(i), Environment.NewLine);
            return sb.ToString();
        }
    }
}
