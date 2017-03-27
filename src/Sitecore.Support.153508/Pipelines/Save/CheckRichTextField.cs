using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Save;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Support.Pipelines.Save
{
  public class CheckRichTextField
  {
    public void Process(SaveArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      foreach (SaveArgs.SaveItem item in args.Items)
      {
        Item item2 = Client.ContentDatabase.Items[item.ID, item.Language, item.Version];
        if (item2 != null)
        {
          foreach (var field in item.Fields)
          {
            var templateField = item2.Fields[field.ID];
            if (templateField != null && templateField.TypeKey == "rich text")
            {
              if (!string.IsNullOrEmpty(field.Value))
              {
                field.Value = this.ClearTags(field.Value);
              }
            }
          }
        }
      }
    }

    private string ClearTags(string content)
    {
      string[] lines = content.Split(new string[] { "<p>", "</p>" }, StringSplitOptions.None);
      if (lines != null && lines.Length == 3)
      {
        content = content.Replace("<p>", "").Replace("</p>", "");
      }
      return content;
    }
  }
}
