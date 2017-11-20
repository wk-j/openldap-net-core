using System;
using Novell.Directory.Ldap;
using System.Linq;
using System.Diagnostics;

namespace OpenLDAP
{
    class Program
    {
        public const string DefaultPassword = "password";

        static LdapEntry NewLdapEntry(string baseDn)
        {

            var cn = Guid.NewGuid().ToString();
            var attributeSet = new LdapAttributeSet
            {
                new LdapAttribute("objectClass", "top"),
                new LdapAttribute("objectClass", "person"),
                new LdapAttribute("objectClass", "organizationPerson"),
                new LdapAttribute("objectClass", "inetOrgPerson"),

                new LdapAttribute("cn", cn),
                new LdapAttribute("givenName", "Lionel"),
                new LdapAttribute("sn", "Messi"),
                new LdapAttribute("mail", cn + "@gmail.com"),
                new LdapAttribute("userPassword", DefaultPassword)
            };

            var attr = new LdapAttributeSet
            {
               new LdapAttribute("objectClass", "top"),
               new LdapAttribute("objectClass", "account"),
               new LdapAttribute("objectClass", "posixAccount"),
               new LdapAttribute("objectClass", "shadowAccount"),
               new LdapAttribute("cn", "raj2"),
               new LdapAttribute("uid", "raj2"),
               new LdapAttribute("uidNumber", "9990"),
               new LdapAttribute("gidNumber", "100"),
               new LdapAttribute("homeDirectory", "/home/raj2"),
               new LdapAttribute("loginShell", "/bin/bash"),
               new LdapAttribute("gecos", "Raj[Admin(at) ITzGeek]"),
               new LdapAttribute("userPassword", "{crypt}x"),
               new LdapAttribute("shadowLastChange", "17058"),
               new LdapAttribute("shadowMin", "0"),
               new LdapAttribute("shadowMax", "99999"),
               new LdapAttribute("shadowWarning", "7")
            };

            //var dn = $"cn={cn}," + baseDn;
            var dn = $"uid=raj2," + baseDn;

            //return new LdapEntry(dn, attributeSet);
            Console.WriteLine($"-- dn | {dn}");

            return new LdapEntry(dn, attr);
        }

        static void Main(string[] args)
        {
            var connection = new LdapConnection();
            connection.SecureSocketLayer = false;
            connection.Connect("192.168.0.20", 389);
            connection.Bind("cn=ldapadm,dc=itzgeek,dc=local", "admin");

            var searchBase = "cn=itzgeek,dc=local";
            var filter = "(objectClass=*)";
            var search = connection.Search(searchBase, LdapConnection.SCOPE_SUB, filter, null, false);
            Console.WriteLine($"-- count | {search.Count}");

            try
            {
                var entry = NewLdapEntry(baseDn: "cn=itzgeek,dc=local");
                connection.Add(entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Demystify());
            }
        }
    }
}
