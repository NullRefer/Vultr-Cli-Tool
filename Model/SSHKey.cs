using System;
using System.Collections.Generic;
using System.Linq;

namespace VultrManager.Model
{
    public class SSHKey
    {
        public string ID { get; set; }

        public DateTime DateCreated { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string ToShortString()
        {
            return string.Format("{0,-36}  {1,-20}  {2,-16}  {3}", ID, DateCreated, Name, Key[..17] + "...");
        }

        public string ToShortString(int no)
        {
            return string.Format(string.Format("{0,-3}  {1}", no, ToShortString()));
        }

        public static IEnumerable<SSHKey> StringToSSHKeys(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }
            List<SSHKey> sshkeys = new List<SSHKey>();
            var rows = str.Split('\n');
            for (int i = 1; i < rows.Count(); i++)
            {
                var items = rows[i].Split('\t').Where(v => !string.IsNullOrEmpty(v))
                                           .ToList();
                if (items.Count != 4) continue;
                var sshkey = new SSHKey();
                sshkey.ID = items[0];
                sshkey.DateCreated = DateTime.Parse(items[1]);
                sshkey.Name = items[2];
                sshkey.Key = items[3];
                sshkeys.Add(sshkey);
            }
            return sshkeys;
        }
    }
}