using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DataBindHelper
/// </summary>
public class DataBindHelper
{
    string DataField;

    public DataBindHelper(string text)
    {
        DataField = text;
    }

    public string Text
    {
        get
        {
            return DataField;
        }
    }
}