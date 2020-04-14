﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO.Filesystem.Ntfs;

namespace FilePropertiesEnumerator
{
    public static class MftHelper
    {
        public static IEnumerable<INode> EnumerateMft(string drive)
        {
            List<DriveInfo> ntfsDrives = DriveInfo.GetDrives().Where(d => d.DriveFormat == "NTFS").ToList();

            DriveInfo driveToAnalyze = ntfsDrives.Where(dr => dr.Name.Contains(drive)).Single();

            NtfsReader ntfsReader = new NtfsReader(driveToAnalyze, RetrieveMode.All);

            IEnumerable<INode> nodes =
                ntfsReader.GetNodes(driveToAnalyze.Name)
                    .Where(n => (n.Attributes &
                                 (Attributes.Hidden | Attributes.System |
                                  Attributes.Temporary | Attributes.Device |
                                  Attributes.Directory | Attributes.Offline |
                                  Attributes.ReparsePoint | Attributes.SparseFile)) == 0);
            //.OrderByDescending(n => n.Size);

            return nodes;
        }
    }
}
