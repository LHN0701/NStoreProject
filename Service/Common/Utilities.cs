using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common
{
    public class Utilities
    {
        public class PropertyCopier<TParent, TChild> where TParent : class
                                                     where TChild : class
        {
            public static void Copy(TParent parent, TChild child)
            {
                var parentProperties = parent.GetType().GetProperties();
                var childProperties = child.GetType().GetProperties();

                foreach (var parentProperty in parentProperties)
                {
                    //if (parentProperty.Name.ToLower() == "id") continue;
                    foreach (var childProperty in childProperties)
                    {
                        if (parentProperty.Name == childProperty.Name &&
                            parentProperty.PropertyType == childProperty.PropertyType &&
                            childProperty.SetMethod != null)
                        {
                            if (parentProperty.GetValue(parent) != null)
                                childProperty.SetValue(child, parentProperty.GetValue(parent));
                            break;
                        }
                    }
                }
            }
        }

        public static string RandomPassword()
        {
            string[] _password = new string[6];
            string charSet = "abcdefghijklmnopqursuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            System.Random _random = new Random();
            int counter;

            for (counter = 0; counter < 6; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)].ToString();
            }
            return string.Join("", _password);
        }
    }
}