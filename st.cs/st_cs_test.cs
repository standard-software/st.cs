using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;   //Debug.Assert

namespace st_cs
{
    public static class st_cs_test
    {
        public static void test_01()
        {
            st_cs_Core.test_Max();
            st_cs_Extensions.test_ExtIsRange();
            st_cs_Core.test_List_SaveToFile();
            st_cs_Core.test_List_LoadFromFile();
            st_cs_Core.test_IncludeStart();
            st_cs_Core.test_ExcludeStart();
            st_cs_Core.test_IncludeEnd();
            st_cs_Core.test_ExcludeEnd();
            st_cs_Core.test_FirstStrFirstDelim();
            st_cs_Core.test_FirstStrLastDelim();
            st_cs_Core.test_LastStrFirstDelim();
            st_cs_Core.test_LastStrLastDelim();

            st_cs_Core.Path.test_AppExePath();
            st_cs_Core.Config.test_Config();

            st_cs_Core.SQL.test_InsertStatement();
            st_cs_Core.SQL.test_UpdateSentence();
            st_cs_Core.SQL.test_MergeStatement();
            st_cs_Core.SQL.test_ConditionClause();

            st_cs_Core.test_String_SaveToFile();
            st_cs_Core.test_String_LoadFromFile();

            st_cs_Core.test_ParseDefault();
            st_cs_Core.test_TryParse();
        }
    }
}
