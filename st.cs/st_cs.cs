/*----------------------------------------
st_cs
----------------------------------------
ModuleName:     Core Module
FileName:       st_cs.cs
----------------------------------------
License:        MIT License
All Right Reserved:
    Name:       Standard Software
    URL:        https://www.facebook.com/stndardsoftware/
--------------------------------------
Version:        2017/06/13
//----------------------------------------*/
using System;
using System.Collections.Generic;

using System.Diagnostics;       //Debug.Assert
using System.Text;              //Encoding

//using System.Windows.Forms;
//MessageBox    WinForms
//[参照の追加][アセンブリ][フレームワーク][System.Windows.Forms]を選択

using System.Windows;
//MessageBox    WinForms
//[参照の追加][アセンブリ][フレームワーク][PresentationFramework]を選択

namespace st_cs
{
    public static class st_cs_Core
    {
        //----------------------------------------
        //◆アプリケーション設定
        //----------------------------------------
        public static string Application_ExePath()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        public static string Application_FolderPath()
        {
            return System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        //----------------------------------------
        //◆条件判断
        //----------------------------------------
        public static bool Check<T>(T valueA, T valueB) where T : IComparable
        {
            bool result = valueA.Equals(valueB);
            if (!result)
            {
                MessageBox.Show(
                    "A != B\n" +
                    "A = " + valueA.ToString() + "\n" +
                    "B = " + valueB.ToString());
            }
            return result;
        }

        public static bool OrValue<T>(T value, params T[] compares) 
        {
            foreach (var item in compares)
            {
                if (value.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool OrValue(string value, params string[] compares)
        {
            foreach (var item in compares)
            {
                if (value == item)
                {
                    return true;
                }
            }
            return false;
        }

        //値がValueならchangeValueに変更して出力する関数
        //空白のときだけ何かに変更という時などに使える
        public static T ValueChange<T>(T target, T value, T changeValue)
            where T : IComparable
        {
            if (target.Equals(value))
            {
                return changeValue;
            }
            else
            {
                return target;
            }
        }


        //----------------------------------------
        //◆型・型変換
        //----------------------------------------
        public static bool TryParse<TIn, TOut>(TIn input)
        {
            //TOutのコンバーターを作成
            System.ComponentModel.TypeConverter converter =
                System.ComponentModel.TypeDescriptor.GetConverter(typeof(TOut));

            //TInから変換不可能な場合は規定値を返す
            if (!converter.CanConvertFrom(typeof(TIn)))
            {
                return false;
            }

            try
            {
                // 変換した値を返す
                var outValue = (TOut)converter.ConvertFrom(input);
                return true;
            }
            catch
            {
                // 変換に失敗したら規定値を返す
                return false;
            }
        }

        public static void test_TryParse()
        {
            Check<bool>(true, TryParse<string, int>("5"));
            Check<bool>(true, TryParse<string, int>("10"));
            Check<bool>(false, TryParse<string, int>("A"));

            Check<bool>(true, TryParse<string, bool>("true"));
            Check<bool>(true, TryParse<string, bool>("false"));
            Check<bool>(true, TryParse<string, bool>("TRUE"));
            Check<bool>(true, TryParse<string, bool>("FALSE"));
            Check<bool>(false, TryParse<string, bool>("OK"));
            Check<bool>(false, TryParse<string, bool>("NG"));
        }

        //----------------------------------------
        //・デフォルト値を指定できるParseメソッド
        //----------------------------------------
        public static TOut ParseDefault<TIn, TOut>(TIn input, TOut defaultValue)
        {
            //TOutのコンバーターを作成
            System.ComponentModel.TypeConverter converter =
                System.ComponentModel.TypeDescriptor.GetConverter(typeof(TOut));

            //TInから変換不可能な場合は規定値を返す
            if (!converter.CanConvertFrom(typeof(TIn)))
            {
                return defaultValue;
            }

            try
            {
                // 変換した値を返す
                return (TOut)converter.ConvertFrom(input);
            }
            catch
            {
                // 変換に失敗したら規定値を返す
                return defaultValue;
            }
        }

        public static void test_ParseDefault()
        {
            Check<int>(5, ParseDefault<string, int>("5", 5));
            Check<int>(10, ParseDefault<string, int>("10", 5));
            Check<int>(5, ParseDefault<string, int>("A", 5));

            Check<bool>(true, ParseDefault<string, bool>("true", true));
            Check<bool>(false, ParseDefault<string, bool>("false", true));
            Check<bool>(true, ParseDefault<string, bool>("TRUE", true));
            Check<bool>(false, ParseDefault<string, bool>("FALSE", true));
            Check<bool>(true, ParseDefault<string, bool>("OK", true));
            Check<bool>(true, ParseDefault<string, bool>("NG", true));
        }

        //----------------------------------------
        //◆数値処理
        //----------------------------------------

        public static bool IsRange(int a, int from, int to)
        {
            return (from <= a && a <= to);
        }

        public static Type Max<Type>(Type a, Type b) where Type : IComparable
        {
            return a.CompareTo(b) > 0 ? a : b;
        }

        public static void test_Max()
        {
            Debug.Assert(5 == Max(1, 5));
            Debug.Assert(5 == Max(5, 1));
            Debug.Assert(1 == Max(1, -5));
            Debug.Assert(1 == Max(-5, 1));
        }



        //----------------------------------------
        //◆文字列処理
        //----------------------------------------

        //----------------------------------------
        //◇Include/Exclude
        //----------------------------------------

        //----------------------------------------
        //Include/Exclude Start
        //----------------------------------------
        public static string IncludeStart(string str, string subStr)
        {
            if (string.IsNullOrEmpty(str)) return subStr;
            if (str.StartsWith(subStr))
            {
                return str;
            }
            return subStr + str;
        }

        public static void test_IncludeStart()
        {
            Debug.Assert("12345" == IncludeStart("12345", "1"));
            Debug.Assert("12345" == IncludeStart("12345", "12"));
            Debug.Assert("12345" == IncludeStart("12345", "123"));
            Debug.Assert("2312345" == IncludeStart("12345", "23"));
        }

        public static string ExcludeStart(string str, string subStr)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (str.StartsWith(subStr))
            {
                return str.Substring(subStr.Length);
            }
            return str;
        }

        public static void test_ExcludeStart()
        {
            Debug.Assert("2345" == ExcludeStart("12345", "1"));
            Debug.Assert("345" == ExcludeStart("12345", "12"));
            Debug.Assert("45" == ExcludeStart("12345", "123"));
            Debug.Assert("12345" == ExcludeStart("12345", "23"));
        }

        //----------------------------------------
        //Include/Exclude End
        //----------------------------------------
        public static string IncludeEnd(string str, string subStr)
        {
            if (string.IsNullOrEmpty(str)) return subStr;
            if (str.EndsWith(subStr))
            {
                return str;
            }
            return str + subStr;
        }

        public static void test_IncludeEnd()
        {
            Debug.Assert("12345" == IncludeEnd("12345", "5"));
            Debug.Assert("12345" == IncludeEnd("12345", "45"));
            Debug.Assert("12345" == IncludeEnd("12345", "345"));
            Debug.Assert("1234534" == IncludeEnd("12345", "34"));

        }

        public static string ExcludeEnd(string str, string subStr)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (str.EndsWith(subStr))
            {
                return str.Remove(str.Length - subStr.Length);
            }
            return str;
        }

        public static void test_ExcludeEnd()
        {
            Debug.Assert("1234" == ExcludeEnd("12345", "5"));
            Debug.Assert("123" == ExcludeEnd("12345", "45"));
            Debug.Assert("12" == ExcludeEnd("12345", "345"));
            Debug.Assert("12345" == ExcludeEnd("12345", "34"));
        }

        //----------------------------------------
        //Include/Exclude StartEnd
        //----------------------------------------
        public static string IncludeStartEnd(string str, string subStr)
        {
            if (string.IsNullOrEmpty(str)) return subStr + subStr; 
            return IncludeStart(IncludeEnd(str, subStr), subStr);
        }

        public static string ExcludeStartEnd(string str, string subStr)
        {
            if (string.IsNullOrEmpty(str)) return subStr + subStr;
            return ExcludeStart(ExcludeEnd(str, subStr), subStr);
        }

        //----------------------------------------
        //◇FirstStr / LastStr 
        //----------------------------------------

        //----------------------------------------
        //・FirstStrFirstDelim
        //----------------------------------------
        //   ・  先頭で見つかれば空文字を返す
        //   ・  見つからなければ文字をそのまま返す
        //----------------------------------------
        public static string FirstStrFirstDelim(string str, string delimiter)
        {
            int index = str.IndexOf(delimiter);
            if (0 <= index)
            {
                return str.Substring(0, index);
            }
            else
            {
                return str;
            }
        }

        public static void test_FirstStrFirstDelim()
        {
            Debug.Assert("123" == FirstStrFirstDelim("123,456", ","));
            Debug.Assert("123" == FirstStrFirstDelim("123,456,789", ","));
            Debug.Assert("123" == FirstStrFirstDelim("123ttt456", "ttt"));
            Debug.Assert("123" == FirstStrFirstDelim("123ttt456", "tt"));
            Debug.Assert("123" == FirstStrFirstDelim("123ttt456", "t"));
            Debug.Assert("123ttt456" == FirstStrFirstDelim("123ttt456", ","));
            Debug.Assert("" == FirstStrFirstDelim(",123,", ","));
        }


        //----------------------------------------
        //・FirstStrLastDelim
        //----------------------------------------
        public static string FirstStrLastDelim(string str, string delimiter)
        {
            int index = str.LastIndexOf(delimiter);
            if (0 <= index)
            {
                return str.Substring(0, index);
            }
            else
            {
                return str;
            }
        }

        public static void test_FirstStrLastDelim()
        {
             Debug.Assert("123" == FirstStrLastDelim("123,456", ","));
             Debug.Assert("123,456" == FirstStrLastDelim("123,456,789", ","));
             Debug.Assert("123" == FirstStrLastDelim("123ttt456", "ttt"));
             Debug.Assert("123t" == FirstStrLastDelim("123ttt456", "tt"));
             Debug.Assert("123tt" == FirstStrLastDelim("123ttt456", "t"));
             Debug.Assert("123ttt456" == FirstStrLastDelim("123ttt456", ","));
             Debug.Assert(",123" == FirstStrLastDelim(",123,", ","));
        }


        //----------------------------------------
        //・LastStrFirstDelim
        //----------------------------------------
        public static string LastStrFirstDelim(string str, string delimiter)
        {
            int index = str.IndexOf(delimiter);
            if (0 <= index)
            {
                return str.Substring(index + delimiter.Length);
            }
            else
            {
                return str;
            }
        }        

        public static void test_LastStrFirstDelim()
        {
            Debug.Assert("456" == LastStrFirstDelim("123,456", ","));
            Debug.Assert("456,789" == LastStrFirstDelim("123,456,789", ","));
            Debug.Assert("456" == LastStrFirstDelim("123ttt456", "ttt"));
            Debug.Assert("t456" == LastStrFirstDelim("123ttt456", "tt"));
            Debug.Assert("tt456" == LastStrFirstDelim("123ttt456", "t"));
            Debug.Assert("123ttt456" == LastStrFirstDelim("123ttt456", ","));
            Debug.Assert("123," == LastStrFirstDelim(",123,", ","));
        }

        //----------------------------------------
        //・LastStrLastDelim
        //----------------------------------------
        public static string LastStrLastDelim(string str, string delimiter)
        {
            int index = str.LastIndexOf(delimiter);
            if (0 <= index)
            {
                return str.Substring(index + delimiter.Length);
            }
            else
            {
                return str;
            }
        }
        
        public static void test_LastStrLastDelim()
        {
            Debug.Assert("456" == LastStrLastDelim("123,456", ","));
            Debug.Assert("789" == LastStrLastDelim("123,456,789", ","));
            Debug.Assert("456" == LastStrLastDelim("123ttt456", "ttt"));
            Debug.Assert("456" == LastStrLastDelim("123ttt456", "tt"));
            Debug.Assert("456" == LastStrLastDelim("123ttt456", "t"));
            Debug.Assert("123ttt456" == LastStrLastDelim("123ttt456", ","));
            Debug.Assert("" == LastStrLastDelim(",123,", ","));
        }

        //----------------------------------------
        //エンコード指定して指定バイト数で切り取る関数
        //----------------------------------------
        public static string LeftByte(string s, Encoding encoding, int maxByteCount)
        {
            var bytes = encoding.GetBytes(s);
            if (bytes.Length <= maxByteCount) return s;

            var result = s.Substring(0,
                encoding.GetString(bytes, 0, maxByteCount).Length);

            while (encoding.GetByteCount(result) > maxByteCount)
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result;
        }


        //----------------------------------------
        //◆ファイル入出力
        //----------------------------------------

        //----------------------------------------
        //◇Stringのファイル入出力
        //----------------------------------------
        //  ・  EncodeはDefault=ShiftJISになる
        //----------------------------------------

        static public string String_LoadFromFile(string filePath, Encoding encode)
        {
            Debug.Assert(System.IO.File.Exists(filePath));

            using (var sr = new System.IO.StreamReader(filePath, encode))
            {
                return sr.ReadToEnd();
            }
        }

        static public void test_String_LoadFromFile()
        {
            Check<string>("abc\ndef\n123\n456", String_LoadFromFile(
                System.IO.Path.Combine(
                    st_cs_Core.Application_FolderPath(),
                    "test_StringFileIO.txt"), Encoding.Default));
        }

        static public void String_SaveToFile(string value, string filePath, Encoding encode)
        {
            using (var sw = new System.IO.StreamWriter(filePath, false, encode))
            {
                sw.Write(value);
            }
        }

        static public void test_String_SaveToFile()
        {
            String_SaveToFile("abc\ndef\n123\n456",
                System.IO.Path.Combine(
                    st_cs_Core.Application_FolderPath(),
                    "test_StringFileIO.txt"), Encoding.Default);
        }

        //----------------------------------------
        //◇List<T>でのファイル入出力
        //----------------------------------------

        //ToStringとFromStringを実装するための抽象クラス
        public abstract class ItemConvertableString
        {
            public abstract override string ToString();
            public abstract void FromString(string value);
        }

        static public void List_SaveToFile<type>(string filePath, List<type> list)
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath))
            {
                foreach (var itm in list)
                {
                    writer.WriteLine(itm.ToString());
                }
                writer.Flush();
            }
        }

        static public void List_LoadFromFile<Type>(string filePath, List<Type> list)
            where Type : ItemConvertableString, new()
        {
            using (System.IO.StreamReader reader = new System.IO.StreamReader(filePath, Encoding.Default))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var item = new Type();
                    item.FromString(line);
                    list.Add(item);
                }
            }
        }

        private class TestItem : ItemConvertableString
        {
            public string Id;
            public string Name;
            public string Mail;

            public override string ToString()
            {
                return System.Text.RegularExpressions.Regex.Escape(
                    String.Join(",",
                    new string[] {
                    this.Id,
                    this.Name,
                    this.Mail,
                    }));
            }
            public override void FromString(string value)
            {
                string[] s = System.Text.RegularExpressions.Regex.Unescape(value).Split(',');

                int i = 0;
                this.Id = s[i++];
                this.Name = s[i++];
                this.Mail = s[i++];
            }
        }

        static public void test_List_SaveToFile()
        {
            var listTest = new List<TestItem>();

            TestItem Item;
            Item = new TestItem()
            {
                Id = @"sy01",
                Name = @"satoshi.yamamoto.01",
                Mail = @"standard.software.net+01@gmail.com",
            };
            listTest.Add(Item);

            Item = new TestItem()
            {
                Id = @"sy02",
                Name = @"satoshi.yamamoto.02",
                Mail = @"standard.software.net+02@gmail.com",
            };
            listTest.Add(Item);

            st_cs_Core.List_SaveToFile<TestItem>(
                System.IO.Path.Combine(
                    st_cs_Core.Application_FolderPath(),
                    "test_List.csv"), listTest);

            Debug.Assert(2 == listTest.Count);
        }

        static public void test_List_LoadFromFile()
        {
            var listTest = new List<TestItem>();
            Debug.Assert(0 == listTest.Count);

            st_cs_Core.List_LoadFromFile<TestItem>(
                System.IO.Path.Combine(
                    st_cs_Core.Application_FolderPath(),
                    "test_List.csv"), listTest);

            Debug.Assert(2 == listTest.Count);

            Debug.Assert(listTest[0].Id == "sy01");
            Debug.Assert(listTest[0].Name == "satoshi.yamamoto.01");
            Debug.Assert(listTest[1].Id == "sy02");
            Debug.Assert(listTest[1].Name == "satoshi.yamamoto.02");

        }

        //----------------------------------------
        //◆SQL
        //----------------------------------------
        public static class SQL
        {
            public class FieldValueItem
            {
                public string Field;
                public string Value;
            }

            //----------------------------------------
            //・ INSERT文を生成する
            //----------------------------------------
            public static string InsertStatement(string tableName, List<FieldValueItem> list)
            {
                var fields = new List<string>();
                var values = new List<string>();
                foreach (var item in list)
                {
                    fields.Add(item.Field);
                    values.Add(item.Value);
                }

                return
                    "INSERT INTO " + tableName +" ( " +
                    String.Join(", ", fields.ToArray()) +
                    " ) VALUES ( " + 
                     String.Join(", ", values.ToArray()) + 
                    " )";
            }

            public static void test_InsertStatement()
            {
                var listFieldValue = new List<FieldValueItem>();
                listFieldValue.Add(new FieldValueItem() { Field = "FIELD01", Value = IncludeStartEnd("0123", "'") , });
                Check("INSERT INTO TESTTABLE01 ( FIELD01 ) VALUES ( '0123' )",
                    InsertStatement("TESTTABLE01", listFieldValue));

                listFieldValue.Add(new FieldValueItem() { Field = "FIELD02", Value = IncludeStartEnd("0456", "'"), });
                listFieldValue.Add(new FieldValueItem() { Field = "FIELD03", Value = "sysdate", });

                Check("INSERT INTO TESTTABLE02 ( FIELD01, FIELD02, FIELD03 ) VALUES ( '0123', '0456', sysdate )",
                    InsertStatement("TESTTABLE02", listFieldValue));
            }

            //----------------------------------------
            //・ UPDATE文を生成する
            //----------------------------------------
            public static string UpdateStatement(string tableName, List<FieldValueItem> list)
            {
                var fieldEqualValue = new List<string>();
                foreach (var item in list)
                {
                    fieldEqualValue.Add(item.Field + "=" + item.Value);
                }

                return
                    "UPDATE " + tableName + " SET " +
                    String.Join(", ", fieldEqualValue.ToArray());
            }

            public static void test_UpdateSentence()
            {
                var listFieldValue = new List<FieldValueItem>();
                listFieldValue.Add(new FieldValueItem() { Field = "FIELD01", Value = IncludeStartEnd("0123", "'"), });
                Check("UPDATE TESTTABLE01 SET FIELD01='0123'",
                    UpdateStatement("TESTTABLE01", listFieldValue));

                listFieldValue.Add(new FieldValueItem() { Field = "FIELD02", Value = IncludeStartEnd("0456", "'"), });
                listFieldValue.Add(new FieldValueItem() { Field = "FIELD03", Value = "sysdate", });

                Check("UPDATE TESTTABLE02 SET FIELD01='0123', FIELD02='0456', FIELD03=sysdate",
                    UpdateStatement("TESTTABLE02", listFieldValue));
            }

            //----------------------------------------
            //・ ANDやORで接続された条件文を生成する
            //----------------------------------------
            //  ・   論理条件節は英語でLogicalConditionClause
            //  ・   WHERE句は英語でWHERE Clause
            //----------------------------------------
            public static string ConditionClause(List<FieldValueItem> list, 
                string conditionAndOr,
                string startBracket = "(", string endBracket = ")",
                string startInsideBracket = "(", string endInsideBracket = ")", 
                string equalOperator = "=")
            {
                Debug.Assert(0 <= list.Count);

                var fieldEqualValue = new List<string>();
                foreach (var item in list)
                {
                    fieldEqualValue.Add(item.Field + equalOperator + item.Value);
                }

                if (2 <= list.Count)
                {
                    return 
                        startBracket + startInsideBracket +
                        String.Join(endInsideBracket + conditionAndOr + startInsideBracket, fieldEqualValue.ToArray()) +
                        endInsideBracket + endBracket;
                }
                else
                {
                    return
                        startBracket + fieldEqualValue[0].ToString() + endBracket;
                }
            }

            public static void test_ConditionClause()
            {
                var listFieldValue = new List<FieldValueItem>();
                listFieldValue.Add(new FieldValueItem() {
                    Field = "FIELD01", Value = IncludeStartEnd("0123", "'"), });
                Check("(FIELD01='0123')",
                    ConditionClause(listFieldValue, "AND"));
                //条件が一つのときは括弧が一重、ANDかORは無視される

                listFieldValue.Add(new FieldValueItem() {
                    Field = "FIELD02", Value = IncludeStartEnd("0456", "'"), });
                listFieldValue.Add(new FieldValueItem() {
                    Field = "FIELD03", Value = "sysdate", });

                Check("((FIELD01='0123') AND (FIELD02='0456') AND (FIELD03=sysdate))",
                    ConditionClause(listFieldValue, " AND "));
                //条件が複数のときは括弧が二重

                Check(" ( (FIELD01='0123') OR (FIELD02='0456') OR (FIELD03=sysdate) ) ",
                    ConditionClause(listFieldValue, " OR ", " ( ", " ) "));
                //前後の括弧はスペースもあけて指定できる

                Check(" <!- <FIELD01=='0123'>, <FIELD02=='0456'>, <FIELD03==sysdate> --> ",
                    ConditionClause(listFieldValue, ", ", " <!- ", " --> ", "<", ">", "=="));
                //フル機能を使えばこんなこともできる

                Check("FIELD01='0123', FIELD02='0456', FIELD03=sysdate",
                    ConditionClause(listFieldValue, ", ", "", "", "", ""));
                //空文字も指定可能
            }


            //----------------------------------------
            //・ SQL MERGE文を生成するメソッド
            //----------------------------------------
            //  ・   MERGE文はOracleDBのみの機能だと思う
            //----------------------------------------
            public static string MergeStatement(string tableName, string usingDualOn, 
                List<FieldValueItem> listUpdate, List<FieldValueItem> listInsert)
            {
                var fieldEqualValue = new List<string>();
                foreach (var item in listUpdate)
                {
                    fieldEqualValue.Add(item.Field + "=" + item.Value);
                }

                //Update句
                var updateClauses = 
                    "UPDATE SET " +
                    String.Join(", ", fieldEqualValue.ToArray());

                var fields = new List<string>();
                var values = new List<string>();
                foreach (var item in listInsert)
                {
                    fields.Add(item.Field);
                    values.Add(item.Value);
                }

                //Insert句
                var insertClauses =
                    "INSERT ( " +
                    String.Join(", ", fields.ToArray()) +
                    " ) VALUES ( " +
                     String.Join(", ", values.ToArray()) +
                    " )";

                return
                    "MERGE INTO " + tableName + 
                    " USING DUAL ON ( " + usingDualOn + " )" +
                    " WHEN MATCHED THEN " + updateClauses +
                    " WHEN NOT MATCHED THEN " + insertClauses;
            }

            public static void test_MergeStatement()
            {
                var listFieldValueUpdadte = new List<FieldValueItem>();
                listFieldValueUpdadte.Add(new FieldValueItem() { Field = "FIELD01", Value = IncludeStartEnd("0456", "'"), });
                var listFieldValueInsert = new List<FieldValueItem>();
                listFieldValueInsert.Add(new FieldValueItem() { Field = "FIELD01", Value = IncludeStartEnd("0123", "'"), });

                Check("MERGE INTO TESTTABLE01 USING DUAL ON ( FIELD01='0123' ) " + 
                    "WHEN MATCHED THEN UPDATE SET FIELD01='0456' " + 
                    "WHEN NOT MATCHED THEN INSERT ( FIELD01 ) VALUES ( '0123' )",
                    MergeStatement("TESTTABLE01", "FIELD01='0123'", listFieldValueUpdadte, listFieldValueInsert));

                listFieldValueUpdadte.Add(new FieldValueItem() { Field = "FIELD02", Value = IncludeStartEnd("0456", "'"), });
                listFieldValueUpdadte.Add(new FieldValueItem() { Field = "FIELD03", Value = "sysdate", });

                listFieldValueInsert.Add(new FieldValueItem() { Field = "FIELD02", Value = IncludeStartEnd("0456", "'"), });
                listFieldValueInsert.Add(new FieldValueItem() { Field = "FIELD03", Value = "sysdate", });

                Check("MERGE INTO TESTTABLE02 USING DUAL ON ( FIELD01='0123' ) " +
                    "WHEN MATCHED THEN UPDATE SET FIELD01='0456', FIELD02='0456', FIELD03=sysdate " +
                    "WHEN NOT MATCHED THEN INSERT ( FIELD01, FIELD02, FIELD03 ) VALUES ( '0123', '0456', sysdate )",
                    MergeStatement("TESTTABLE02", "FIELD01='0123'", listFieldValueUpdadte, listFieldValueInsert));
                
            }



        }

    }

    //----------------------------------------
    //■拡張メソッド用クラス
    //----------------------------------------

    public static class st_cs_Extensions
    {
        //----------------------------------------
        //◆比較範囲
        //----------------------------------------

        //CompareToがひどく可読性が低いので作った
        public static bool GreaterThan<T>(this T value1, T value2) where T : IComparable
        { return (value1.CompareTo(value2) < 0); }

        public static bool GreaterThanEquals<T>(this T value1, T value2) where T : IComparable
        { return (value1.CompareTo(value2) <= 0); }

        public static bool LessThan<T>(this T value1, T value2) where T : IComparable
        { return value1.CompareTo(value2) > 0; }

        public static bool LessThanEquals<T>(this T value1, T value2) where T : IComparable
        { return value1.CompareTo(value2) >= 0; }

        public static bool IsRange<T>(this T value, T from, T to) where T : IComparable
        {
            return (
                (from.GreaterThanEquals(value))    //(from < value)
                &&
                (value.GreaterThanEquals(to))      //(value < to);
            );
        }

        public static void test_ExtIsRange()
        {
            Debug.Assert(true == 5.IsRange(1, 10));
            Debug.Assert(true == 1.IsRange(1, 10));
            Debug.Assert(true == 10.IsRange(1, 10));
            Debug.Assert(false == 0.IsRange(1, 10));
            Debug.Assert(false == 11.IsRange(1, 10));
        }
    }
}

/*----------------------------------------
◇   ver 2017/03/19
・   作成
◇   ver 2017/04/01
・  Checkメソッド追加
・  SQLクラス 
    InsertSentenceメソッド
    UpdateSentenceメソッド
    MergeSentenceメソッド、追加
◇  ver 2017/04/08
・  TryParse/ParseDefault 追加
◇  ver 2017/06/13
・  GitHubに登録のために調整
    WPFとWinFormsへの参照設定記載
//----------------------------------------*/

