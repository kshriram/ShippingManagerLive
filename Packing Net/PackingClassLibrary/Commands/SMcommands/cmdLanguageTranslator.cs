using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PackingClassLibrary.CustomEntity;
using System.IO;

namespace PackingClassLibrary.Commands
{
  public static class cmdLanguageTranslator
    {
      /// <summary>
      ///  static method to convert the inpute string to the language specified in the file
      /// </summary>
      /// <param name="ConvertString">String input string to convert</param>
      /// <param name="Filename">File for matching String</param>
      /// <returns>Converted string or default input value</returns>
      public static String Convert(String ConvertString, String Filename)
      {
          String _returnConvert = ConvertString;
          try
          {
               String[] Lines = File.ReadAllLines(Filename);
              foreach (var Line in Lines)
              {
                  string[] Word = Line.Split(new char[] { '|' });
                  if (Word[0].StartsWith("--") != true)//Comments..
                  {
                      if (Word[0].ToString() == ConvertString)
                      {
                          _returnConvert = Word[1].ToString();
                          break;
                      }
                      else
                      {
                          if (ConvertString.Contains(Word[0]))
                          {
                              ConvertString = ConvertString.Replace(Word[0], Word[1]);
                              _returnConvert = ConvertString;
                          }
                      }
                  }

              }
             
          }
          catch (Exception )
          {
             // Error_Loger.elAction.save("LanguageTranslator.Convert()", Ex.Message.ToString());
          }
          return _returnConvert;
      }
    }
}
