using System;
using Xunit;
using IIG.PasswordHashingUtils;
using System.Collections.Generic;

namespace KPI_LAB3
{
    public class UnitTest1
    {
        private static string GenerateReallyLongString()
        {
            string returnstring = "";
            for (uint i = 0; i < /* uint.MaxValue / */ 100000; i++) returnstring += "a";
            return returnstring;
        }

        public static IEnumerable<object[]> GetData()
        {
            string longstring = GenerateReallyLongString();
            yield return new object[] { "", "", null };
            yield return new object[] { "", "", (uint)10 };
            yield return new object[] { "", "soul", null };
            yield return new object[] { "", "soul", (uint)10 };

            yield return new object[] { "password", "", null };
            yield return new object[] { "password", "", (uint)10 };
            yield return new object[] { "password", "soul", null };
            yield return new object[] { "password", "soul", (uint)10 };

            // Long string for getting into catch statement
            yield return new object[] { longstring, "", null };
            yield return new object[] { longstring, "", (uint)10 };
            yield return new object[] { longstring, "soul", null };
            yield return new object[] { longstring, "soul", (uint)10 };
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public void TestGetHashPassword(string password, string salt, uint? adlerMod32)
        {
            Assert.Equal(
                PasswordHasher.GetHash(password, salt, adlerMod32),
                PasswordHasher.GetHash(password, salt, adlerMod32)
            );
            Assert.NotEqual(
                PasswordHasher.GetHash(password, salt, adlerMod32),
                PasswordHasher.GetHash(password+"1", salt, adlerMod32)
            );
        }
    }
}
