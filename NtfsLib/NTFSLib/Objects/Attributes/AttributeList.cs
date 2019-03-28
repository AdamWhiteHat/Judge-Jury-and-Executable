﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using NTFSLib.NTFS;
using NTFSLib.Objects.Enums;
using NTFSLib.Utilities;

namespace NTFSLib.Objects.Attributes
{
    public class AttributeList : Attribute
    {
        public AttributeListItem[] Items { get; set; }

        public override AttributeResidentAllow AllowedResidentStates
        {
            get
            {
                return AttributeResidentAllow.Resident | AttributeResidentAllow.NonResident;
            }
        }

        internal override void ParseAttributeResidentBody(byte[] data, int maxLength, int offset)
        {
            base.ParseAttributeResidentBody(data, maxLength, offset);

            Debug.Assert(maxLength >= ResidentHeader.ContentLength);

            List<AttributeListItem> results = new List<AttributeListItem>();

            int pointer = offset;
            while (pointer + 26 <= offset + maxLength)      // 26 is the smallest possible MFTAttributeListItem
            {
                AttributeListItem item = AttributeListItem.ParseListItem(data, Math.Min(data.Length - pointer, maxLength), pointer);

                if (item.Type == AttributeType.EndOfAttributes)
                    break;

                results.Add(item);

                pointer += item.Length;
            }

            Items = results.ToArray();
        }

        internal override void ParseAttributeNonResidentBody(INTFSInfo ntfsInfo)
        {
            base.ParseAttributeNonResidentBody(ntfsInfo);

            // Get all chunks
            byte[] data = NtfsUtils.ReadFragments(ntfsInfo, NonResidentHeader.Fragments);

            // Parse
            List<AttributeListItem> results = new List<AttributeListItem>();

            int pointer = 0;
            int contentSize = (int) NonResidentHeader.ContentSize;
            while (pointer + 26 <= contentSize)     // 26 is the smallest possible MFTAttributeListItem
            {
                AttributeListItem item = AttributeListItem.ParseListItem(data, data.Length - pointer, pointer);

                if (item.Type == AttributeType.EndOfAttributes)
                    break;

                if (item.Length == 0)
                    break;

                results.Add(item);

                pointer += item.Length;
            }

            Debug.Assert(pointer == contentSize);

            Items = results.ToArray();
        }
    }
}