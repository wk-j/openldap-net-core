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
            var attr = new LdapAttributeSet
            {
                new LdapAttribute("uid", "wk"),
                new LdapAttribute("sn", "Beer"),
                new LdapAttribute("cn", "Karlsberg"),
                new LdapAttribute("objectClass", "inetOrgPerson"),
                new LdapAttribute("objectClass", "person"),
                new LdapAttribute("objectClass", "top"),
                new LdapAttribute("userPassword", "admin"),
            };

            var dn = $"cn=Karlsberg,{baseDn}";

            Console.WriteLine($"-- dn | {dn}");

            return new LdapEntry(dn, attr);
        }

        static void Main(string[] args)
        {
            var connection = new LdapConnection();
            connection.SecureSocketLayer = false;
            connection.Connect("192.168.0.20", 389);
            connection.Bind("cn=ldapadm,dc=itzgeek,dc=local", "admin");

            void searchAll()
            {
                var searchBase = "cn=itzgeek,dc=local";
                var filter = "(objectClass=*)";
                var search = connection.Search(searchBase, LdapConnection.SCOPE_SUB, filter, null, false);
                Console.WriteLine($"-- count | {search.Count}");
            }

            void addEntry()
            {
                try
                {
                    var entry = NewLdapEntry(baseDn: "cn=ldapadm,cn=itzgeek,dc=local");
                    connection.Add(entry);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Demystify());
                }
            }

            addEntry();
        }
    }
}
