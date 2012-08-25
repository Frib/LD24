using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD24
{
    public static class Ext
    {
        public static T Random<T>(this List<T> list)
        {
            return list[G.r.Next(list.Count)];
        }
    }
}
