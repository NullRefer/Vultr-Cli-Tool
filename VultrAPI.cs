using System;
using System.Collections.Generic;
using System.Linq;
using VultrManager.Model;

namespace VultrManager
{
    public static class VultrAPI
    {
        public static IEnumerable<Instance> GetInstances()
        {
            string command = "vultr-cli instance list";
            string result = CmdHelper.Execute(command);
            result = result[result.IndexOf("ID")..];
            var intances = Instance.StringToInstances(result);
            return intances;
        }

        public static string DeleteInstance(Instance instance)
        {
            string command = $"vultr-cli instance delete {instance.ID}";
            return CmdHelper.Execute(command);
        }

        public static string CreateInstance(string label, Snapshot snapshot = null, IEnumerable<SSHKey> sshkeys = null)
        {
            string command = $"vultr-cli instance create --label \"{label}\" --region nrt --plan vc2-1c-1gb";
            if (snapshot != null)
            {
                command += $" --snapshot {snapshot.ID}";
            }
            if (sshkeys?.Any() == true)
            {
                command += $" --ssh-keys";
                foreach (var sshkey in sshkeys)
                {
                    command += $" {sshkey.Key}";
                }
            }
            return CmdHelper.Execute(command);
        }

        public static IEnumerable<Snapshot> GetSnapshots()
        {
            string command = "vultr-cli snapshot list";
            string result = CmdHelper.Execute(command);
            result = result[result.IndexOf("ID")..];
            var snapshots = Snapshot.StringToSnapshots(result);
            return snapshots;
        }

        public static string DeleteSnapshot(Snapshot snapshot)
        {
            string command = $"vultr-cli snapshot delete {snapshot.ID}";
            return CmdHelper.Execute(command);
        }

        public static string CreateSanpShot(Instance instance, string label)
        {
            string command = $"vultr-cli snapshot create --id {instance.ID} --description \"{label}\"";
            return CmdHelper.Execute(command);
        }

        public static IEnumerable<SSHKey> GetSSHKeys()
        {
            string command = "vultr-cli ssh-key list";
            string result = CmdHelper.Execute(command);
            result = result[result.IndexOf("ID")..];
            var sshkeys = SSHKey.StringToSSHKeys(result);
            return sshkeys;
        }

        public static string DeleteSSHKey(SSHKey sshkey)
        {
            string command = $"vultr-cli ssh-key delete {sshkey.ID}";
            return CmdHelper.Execute(command);
        }

        public static string CreateSSHKey(string name, string keyvalue)
        {
            string command = $"vultr-cli ssh-key create --name \"{name}\" --key \"{keyvalue}\"";
            return CmdHelper.Execute(command);
        }
    }
}
