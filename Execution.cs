using System;
using System.Collections.Generic;
using VultrManager.Model;
using System.Linq;

namespace VultrManager
{
    public class Execution
    {
        public Execution()
        {
            Console.WriteLine("Welcome using Vultr-Manger-Cli tool, please wait while the data is loading...");
            Load();
        }

        public List<Instance> Instances { get; set; }
        public List<Snapshot> Snapshots { get; set; }
        public List<SSHKey> SSHKeys { get; set; }

        private void Load()
        {
            Instances = VultrAPI.GetInstances().ToList();
            Snapshots = VultrAPI.GetSnapshots().ToList();
            SSHKeys = VultrAPI.GetSSHKeys().ToList();
        }

        public string GetMenus()
        {
            string menus = "===== Vultr Cli Manager =====\r\n\r\n";
            menus += "Please select one choice: \r\n";
            menus += "  - 1. List instances\r\n";
            menus += "  - 2. Delete instance\r\n";
            menus += "  - 3. Create instance\r\n";
            menus += "  - 4. List snapshots\r\n";
            menus += "  - 5. Delete snapshot\r\n";
            menus += "  - 6. Create snapshot\r\n";
            menus += "  - 7. List ssh-keys\r\n";
            menus += "  - 8. Delete ssh-key\r\n";
            menus += "  - 9. Create ssh-key\r\n";
            menus += "  - 0. Exit\r\n\r\n";
            menus += "Input your choice: ";
            return menus;
        }

        public void Execute(int choice)
        {
            switch (choice)
            {
                case 0:
                    Environment.Exit(-1);
                    break;
                case 1:
                    ListInstances();
                    break;
                case 2:
                    DeleteInstance();
                    break;
                case 3:
                    CreateInstance();
                    break;
                case 4:
                    ListSnapshots();
                    break;
                case 5:
                    DeleteSnapshot();
                    break;
                case 6:
                    CreateSnapshot();
                    break;
                case 7:
                    ListSSHKeys();
                    break;
                case 8:
                    DeleteSSHKey();
                    break;
                case 9:
                    CreateSSHKey();
                    break;
                default:
                    Console.WriteLine($"Choice {choice} does not exist.");
                    break;
            }
        }

        public void ListInstances()
        {
            Console.WriteLine("\r\n===== Execute List Instance =====");
            string header = string.Format("{0,-3}  {1,-36}  {2,-16}  {3,-10}  {4,-10}", "NO", "ID", "IP", "Label", "Status");
            Console.WriteLine(header);
            int no = 0;
            foreach (var instance in Instances)
            {
                Console.WriteLine(instance.ToShortString(no++));
            }
            System.Console.WriteLine();
        }

        public void DeleteInstance()
        {
            Console.WriteLine("\r\n===== Execute Delete Instance =====");
            if (Instances?.Any() != true)
            {
                Console.WriteLine("There are no instances, may forgot to list?");
                return;
            }
            Console.Write("Please enter the NO or ID of the instance to delete: ");
            string line = Console.ReadLine();
            if (int.TryParse(line, out int no))
            {
                if (no < Instances.Count)
                {
                    string result = VultrAPI.DeleteInstance(Instances[no]);
                    Console.WriteLine($"Execution result: \r\n{result}");
                    Instances.RemoveAt(no);
                }
                else
                {
                    Console.WriteLine($"There is no instance whose NO is {no}");
                    ListInstances();
                }
            }
            else
            {
                if (Instances.Any(ins => ins.ID == line))
                {
                    var toRemove = Instances.Find(instance => instance.ID == line);
                    string result = VultrAPI.DeleteInstance(toRemove);
                    Console.WriteLine($"Execution result: \r\n{result}");
                    Instances.Remove(toRemove);
                }
                else
                {
                    Console.WriteLine($"There is no instance whose ID is {no}");
                    ListInstances();
                }
            }
            System.Console.WriteLine();
        }

        public void CreateInstance()
        {
            Console.WriteLine("\r\n===== Execute Create Instance =====");
            string line;
            Console.Write("Please enter instance label to create: ");
            string label = "";
            while ((line = Console.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("The input name must be a invalid string");
                    continue;
                }
                label = line;
                break;
            }
            Console.Write("Enter the snapshot ID: ");
            Snapshot selectedSnapshot = null;
            while ((line = Console.ReadLine()) != null)
            {
                if (int.TryParse(line, out int no))
                {
                    if (no < Snapshots.Count)
                    {
                        selectedSnapshot = Snapshots[no];
                        break;
                    }
                }
                System.Console.WriteLine("Invalid input, please check.");
                Console.Write("Enter the snapshot ID: ");
            }
            Console.Write("Enter the ssh-key NOs if used(use ',' to seperate, and defalut for select all):");
            List<SSHKey> sshkeys = new();
            if ((line = Console.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    sshkeys.AddRange(SSHKeys);
                }
                else
                {
                    var keyNos = line.Split(',').Select(v => int.Parse(v));
                    foreach (var keyNo in keyNos)
                    {
                        sshkeys.Add(SSHKeys[keyNo]);
                    }
                }
            }
            string result = VultrAPI.CreateInstance(label, selectedSnapshot, sshkeys);
            Console.WriteLine($"Execution result: \r\n{result}");
            System.Console.WriteLine();
            Instances = VultrAPI.GetInstances().ToList();
        }

        public void ListSnapshots()
        {
            Console.WriteLine("\r\n===== Execute List Snapshots =====");
            string header = string.Format("{0,-3}  {1,-36}  {2,-20}  {3,-4}  {4,-10}  {5}", "NO", "ID", "DateCreated", "Size", "Status", "Description");
            Console.WriteLine(header);
            int no = 0;
            foreach (var snapshot in Snapshots)
            {
                Console.WriteLine(snapshot.ToShortString(no++));
            }
            System.Console.WriteLine();
        }

        public void DeleteSnapshot()
        {
            Console.WriteLine("\r\n===== Execute Delete Snapshot =====");
            if (Snapshots?.Any() != true)
            {
                Console.WriteLine("There are no snapshots, may forgot to list?");
                return;
            }
            Console.Write("Please enter the NO or ID of the snapshot to delete: ");
            string line = Console.ReadLine();
            if (int.TryParse(line, out int no))
            {
                if (no < Snapshots.Count)
                {
                    string result = VultrAPI.DeleteSnapshot(Snapshots[no]);
                    Console.WriteLine($"Execution result: \r\n{result}");
                    Snapshots.RemoveAt(no);
                }
                else
                {
                    Console.WriteLine($"There is no snapshot whose NO is {no}");
                    ListSnapshots();
                }
            }
            else
            {
                if (Snapshots.Any(snapshot => snapshot.ID == line))
                {
                    var toRemove = Snapshots.Find(snapshot => snapshot.ID == line);
                    string result = VultrAPI.DeleteSnapshot(toRemove);
                    Snapshots.Remove(toRemove);
                    Console.WriteLine($"Execution result: \r\n{result}");
                }
                else
                {
                    Console.WriteLine($"There is no snapshot whose ID is {no}");
                    ListSnapshots();
                }
            }
            System.Console.WriteLine();
        }

        public void CreateSnapshot()
        {
            Console.WriteLine("\r\n===== Execute Create Snapshot =====");
            Console.Write("Please select an instance for snapshot to create from: \r\n");
            ListInstances();
            Console.Write("Select an instance NO: ");
            string line;
            Instance selectedInstance = null;
            while ((line = Console.ReadLine()) != null)
            {
                if (int.TryParse(line, out int no))
                {
                    if (no < Instances.Count)
                    {
                        selectedInstance = Instances[no];
                        break;
                    }
                    else
                    {
                        System.Console.WriteLine($"Cannot find an instance with NO {no}, please check.");
                    }
                }
                else
                {
                    Console.WriteLine("Input is not valid.");
                }
                Console.Write("Select an instance NO: ");
            }
            System.Console.Write("Please input the snapshot description: ");
            if ((line = Console.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    line = "No description for this snapshot";
                }
                string result = VultrAPI.CreateSanpShot(selectedInstance, line);
                System.Console.WriteLine($"Execution result: \r\n{result}");
            }
            System.Console.WriteLine();
            Snapshots = VultrAPI.GetSnapshots().ToList();
        }

        public void ListSSHKeys()
        {
            Console.WriteLine("\r\n===== Execute List SSH-Keys =====");
            string header = string.Format("{0,-3}  {1,-36}  {2,-20}  {3,-16}  {4}", "NO", "ID", "DateCreated", "Name", "Keyvalue");
            Console.WriteLine(header);
            int no = 0;
            foreach (var sshkey in SSHKeys)
            {
                Console.WriteLine(sshkey.ToShortString(no++));
            }
            System.Console.WriteLine();
        }

        public void DeleteSSHKey()
        {
            Console.WriteLine("\r\n===== Execute Delete SSH-Key =====");
            if (SSHKeys?.Any() != true)
            {
                Console.WriteLine("There are no SSH-Keys, may forgot to list?");
                return;
            }
            Console.Write("Please enter the NO or ID of the SSH-Key to delete: ");
            string line = Console.ReadLine();
            if (int.TryParse(line, out int no))
            {
                if (no < SSHKeys.Count)
                {
                    string result = VultrAPI.DeleteSSHKey(SSHKeys[no]);
                    Console.WriteLine($"Execution result: \r\n{result}");
                    SSHKeys.RemoveAt(no);
                }
                else
                {
                    Console.WriteLine($"There is no SSH-Key whose NO is {no}");
                    ListSSHKeys();
                }
            }
            else
            {
                if (SSHKeys.Any(sshkey => sshkey.ID == line))
                {
                    var toRemove = SSHKeys.Find(item => item.ID == line);
                    string result = VultrAPI.DeleteSSHKey(toRemove);
                    SSHKeys.Remove(toRemove);
                    Console.WriteLine($"Execution result: \r\n{result}");
                }
                else
                {
                    Console.WriteLine($"There is no SSH-Key whose ID is {no}");
                    ListSSHKeys();
                }
            }
            System.Console.WriteLine();
        }

        public void CreateSSHKey()
        {
            Console.WriteLine("\r\n===== Execute Create SSH-Keys =====");
            string line;
            string name = "", key = "";
            Console.Write("Please input the created SSH-Key name: ");
            while ((line = Console.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("The input name must be a invalid string");
                    continue;
                }
                name = line;
                break;
            }
            Console.Write("Please input the created SSH-Key value(usually found in ~/.ssh/id_rsa.pub): ");
            while ((line = Console.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("The input name must be a invalid string");
                    continue;
                }
                key = line;
                break;
            }
            System.Console.WriteLine(VultrAPI.CreateSSHKey(name, key));
            System.Console.WriteLine();
            SSHKeys = VultrAPI.GetSSHKeys().ToList();
        }
    }
}