﻿using System;
using System.Collections;
using System.Linq;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.FakeDb.Data.Engines;

namespace Sitecore.FakeDb
{
  public class DbTemplate : DbItem
  {

    public DbTemplate() : this("Noname")
    {
    }

    public DbTemplate(string name) : this(name, ID.NewID)
    {
    }

    public DbTemplate(string name, ID id) : this(name, id, new ID[] {})
    {
    }

    public DbTemplate(string name, ID id, ID[] baseTemplates) : base(name, id, TemplateIDs.Template)
    {
      //ToDo: [__Base template] is not technically a "standard field". Will revisit once we imlement auto-creating fields on the items from the templates
      this.Fields.Add(new DbField(FieldIDs.BaseTemplate)
      {
        Value = string.Join("|", (baseTemplates ?? new ID[] { }).AsEnumerable())
      });
    }

    public override void Add(string fieldName, string fieldValue)
    {
      throw new InvalidOperationException("Template Item does not support adding fields with values");
    }

    public override void Add(DbItem child)
    {
      base.Add(child);

      if (IsStandardValuesItem(child))
      {
        Fields[FieldIDs.StandardValues].Value = child.ID.ToString();
      }
    }

    // Sitecore.Data.Managers.TemplateProvider.IsStandardValuesHolder()
    public bool IsStandardValuesItem(DbItem child)
    {
      Assert.ArgumentNotNull(child, "Child");

      return child.TemplateID == ID &&
             string.Equals(child.Name, "__Standard Values", StringComparison.InvariantCultureIgnoreCase);
    }

    public void Add(string fieldName)
    {
      Fields.Add(fieldName);
    }

    public IEnumerator GetEnumerator()
    {
      return Fields.GetEnumerator();
    }
  }
}