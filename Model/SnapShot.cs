using System;
using System.Collections.Generic;
using System.Linq;

namespace VultrManager.Model
{
    public class Snapshot
    {
        public string ID { get; set; }

        public DateTime DateCreated { get; set; }

        public double Size { get; set; }

        public string Status { get; set; }

        public int OSID { get; set; }

        public int APPID { get; set; }

        public string Description { get; set; }

        public string ToShortString()
        {
            return string.Format("{0,-36}  {1,-20}  {2,-4}  {3,-10}  {4}", ID, DateCreated, Size, Status, Description);
        }

        public string ToShortString(int no)
        {
            return string.Format(string.Format("{0,-3}  {1}", no, ToShortString()));
        }

        public static IEnumerable<Snapshot> StringToSnapshots(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(nameof(str));
            }
            List<Snapshot> snapshots = new List<Snapshot>();
            var rows = str.Split('\n');
            for (int i = 1; i < rows.Count(); i++)
            {
                var items = rows[i].Split('\t').Where(v => !string.IsNullOrEmpty(v))
                                           .ToList();
                if (items.Count != 7) continue;
                var snapshot = new Snapshot();
                snapshot.ID = items[0];
                snapshot.DateCreated = DateTime.Parse(items[1]);
                snapshot.Size = long.Parse(items[2]) / (Math.Pow(1024, 3));
                snapshot.Status = items[3];
                snapshot.OSID = int.Parse(items[4]);
                snapshot.APPID = int.Parse(items[5]);
                snapshot.Description = items[6];
                snapshots.Add(snapshot);
            }
            return snapshots;
        }
    }
}