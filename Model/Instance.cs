using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace VultrManager.Model
{
    public class Instance
    {
        public string ID { get; set; }

        public IPAddress IP { get; set; }

        public string Label { get; set; }

        public string OS { get; set; }

        public string Status { get; set; }

        public string Region { get; set; }

        public int CPU { get; set; }

        public int RAM { get; set; }

        public int Disk { get; set; }

        public int BandWidth { get; set; }

        public string ToShortString()
        {
            return string.Format("{0,-36}  {1,-16}  {2,-10}  {3,-10}", ID, IP, Label, Status);
        }

        public string ToShortString(int no)
        {
            return string.Format("{0,-3}  {1}", no, ToShortString());
        }

        public static IEnumerable<Instance> StringToInstances(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }
            List<Instance> instances = new List<Instance>();
            var rows = str.Split('\n');
            for (int i = 1; i < rows.Count(); i++)
            {
                var items = rows[i].Split('\t').Where(v => !string.IsNullOrEmpty(v))
                                           .ToList();
                if (items.Count != 10) continue;
                var instance = new Instance();
                instance.ID = items[0];
                instance.IP = IPAddress.Parse(items[1]);
                instance.Label = items[2];
                instance.OS = items[3];
                instance.Status = items[4];
                instance.Region = items[5];
                instance.CPU = int.Parse(items[6]);
                instance.RAM = int.Parse(items[7]);
                instance.Disk = int.Parse(items[8]);
                instance.BandWidth = int.Parse(items[9]);
                instances.Add(instance);
            }
            return instances;
        }
    }
}