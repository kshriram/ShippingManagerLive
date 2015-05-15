using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Packing_Net.validations
{
  public static  class AllValidations
    {
      public static Boolean CharOnly(this String input)
      {
          Boolean _return = false;
          if (Regex.IsMatch(input, @"^[a-zA-Z]*$"))
          {
              _return = true;
          }
          return _return;
      }
    }
}
