﻿using ast_visual_studio_extension.CxCLI.Models;
using ast_visual_studio_extension.CxExtension.Enums;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace ast_visual_studio_extension.CxExtension.Utils
{
    internal class ResultsFilteringAndGrouping
    {
        public static List<TreeViewItem> FilterAndGroupResults(AsyncPackage package, List<TreeViewItem> results)
        {

            HashSet<State> enabledStates = SettingsUtils.EnabledStates(package);
            HashSet<Severity> enabledSeverities = SettingsUtils.EnabledSeverities(package);
            List<GroupBy> enabledGroupBys = SettingsUtils.EnabledGroupByOptions(package);

            var treeResults = new List<TreeViewItem>();

            enabledGroupBys.Insert(0, GroupBy.ENGINE);
            foreach (TreeViewItem item in results)
            {
                var result = item.Tag as Result;
                Enum.TryParse(result.State, out State itemState);
                Enum.TryParse(result.Severity, out Severity itemSeverity);

                if (!enabledStates.Contains(itemState) || !enabledSeverities.Contains(itemSeverity))
                {
                    continue;
                }

                List<TreeViewItem> children = GetInsertLocation(enabledGroupBys, treeResults, result);

                children.Add(item);
            }

            return treeResults;
        }

        private static List<TreeViewItem> GetInsertLocation(List<GroupBy> enabledGroupBys, List<TreeViewItem> treeResults, Result result)
        {
            var children = treeResults;
            foreach (GroupBy groupBy in enabledGroupBys)
            {
                var generator = getGroupByTitleGenerator(groupBy);
                if (generator == null)
                {
                    continue;
                }

                var childNodeName = getGroupByTitleGenerator(groupBy).Invoke(result);
                if (childNodeName == null)
                {
                    continue;
                }

                // single underscore is used as mnemonic
                childNodeName = childNodeName.Replace("_", "__");

                TreeViewItem child = null;

                foreach (var childNode in children)
                {
                    if (childNodeName == (childNode.Header as TextBlock).Tag as string)
                    {
                        child = childNode;
                    }
                }
                if (child == null)
                {
                    child = UIUtils.CreateTreeViewItemWithItemsSource(childNodeName, new List<TreeViewItem> { new TreeViewItem() });
                    (child.ItemsSource as List<TreeViewItem>).Clear();
                    children.Add(child);
                }
                children = child.ItemsSource as List<TreeViewItem>;
            }

            return children;
        }

        private static Func<Result, string> getGroupByTitleGenerator(GroupBy groupBy)
        {
            switch (groupBy)
            {
                case GroupBy.ENGINE:
                    return (result) => result.Type;
                case GroupBy.FILE:
                    return (result) =>
                    {
                        if (result.Data.FileName != null)
                        {
                            return Path.GetFileName(result.Data.FileName);
                        }
                        if (result.Data.Nodes != null && result.Data.Nodes.Count > 0)
                        {
                            return Path.GetFileName(result.Data.Nodes[0].FileName);
                        }
                        return null;
                    };
                case GroupBy.SEVERITY:
                    return (result) => result.Severity;
                case GroupBy.STATE:
                    return (result) => result.State;
                case GroupBy.QUERY_NAME:
                    return (result) => result.Data.QueryName ?? result.Id;
                default:
                    return null;
            }
        }
    }
}
