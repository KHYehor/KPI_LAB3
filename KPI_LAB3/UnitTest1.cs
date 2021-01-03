using System;
using Xunit;
using IIG.PasswordHashingUtils;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace KPI_LAB3
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        private static string GenerateReallyLongString()
        {
            string returnstring = "";
            for (uint i = 0; i < 100000; i++) returnstring += "a";
            return returnstring;
        }

        public static IEnumerable<object[]> GetDataHash()
        {
            string longstring = GenerateReallyLongString();
            yield return new object[] { "'' 'password' + '' 'salt' + null 'adlerMod32'", "", "", null };
            yield return new object[] { "'' 'password' + '' 'salt' + 10 'adlerMod32'", "", "", (uint)10 };
            yield return new object[] { "'' 'password' + 'soul' 'salt' + null 'adlerMod32'", "", "soul", null };
            yield return new object[] { "'' 'password' + 'soul' 'salt' + 10 'adlerMod32'", "", "soul", (uint)10 };

            yield return new object[] { "'password' 'password' + '' 'salt' + null 'adlerMod32'", "password", "", null };
            yield return new object[] { "'password' 'password' + '' 'salt' + 10 'adlerMod32'", "password", "", (uint)10 };
            yield return new object[] { "'password' 'password' + 'soul' 'salt' + null 'adlerMod32'", "password", "soul", null };
            yield return new object[] { "'password' 'password' + 'soul' 'salt' + 10 'adlerMod32'", "password", "soul", (uint)10 };

            // Long string for getting into catch statement
            yield return new object[] { "longpassword 'password' + '' 'salt' + null 'adlerMod32'", longstring, "", null };
            yield return new object[] { "longpassword 'password' + '' 'salt' + 10 'adlerMod32'", longstring, "", (uint)10 };
            yield return new object[] { "longpassword 'password' + 'soul' 'salt' + null 'adlerMod32'", longstring, "soul", null };
            yield return new object[] { "longpassword 'password' + 'soul' 'salt' + 10 'adlerMod32'", longstring, "soul", (uint)10 };
        }

        [Theory]
        [MemberData(nameof(GetDataHash))]
        public void TestGetHash(string description, string password, string salt, uint? adlerMod32)
        {
            output.WriteLine("+=======================+");
            output.WriteLine(description);
            output.WriteLine("+=======================+");

            Assert.Equal(
                PasswordHasher.GetHash(password, salt, adlerMod32),
                PasswordHasher.GetHash(password, salt, adlerMod32)
            );
            Assert.NotEqual(
                PasswordHasher.GetHash(password, salt, adlerMod32),
                PasswordHasher.GetHash(password+"1", salt, adlerMod32)
            );
        }


        public static IEnumerable<object[]> GetDataInit()
        {
            yield return new object[] { "null 'salt', 0 'adlerMod32'", "password", null, 0 };
            yield return new object[] { "", "", "password", (uint)10 };
            yield return new object[] { "", "soul", "password", null };
            yield return new object[] { "", "soul", "password", (uint)10 };

        }

        [Theory]
        [MemberData(nameof(GetDataInit))]
        public void TestInit(string description, string password, string salt, uint adlerMod32)
        {
            output.WriteLine("+=======================+");
            output.WriteLine(description);
            output.WriteLine("+=======================+");
            PasswordHasher.Init(salt, adlerMod32);
            Assert.Equal(PasswordHasher.GetHash(password), PasswordHasher.GetHash(password));
            Assert.NotEqual(PasswordHasher.GetHash(password), PasswordHasher.GetHash(password + "1"));

        }
    }
}
