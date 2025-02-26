﻿using ast_visual_studio_extension.CxExtension.Panels;
using EnvDTE;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ast_visual_studio_extension.CxExtension.Utils
{
    internal class SolutionExplorerUtils
    {
        public static AsyncPackage AsyncPackage { private get; set; }
        /// <summary>
        /// Get current EnvDTE
        /// </summary>
        /// <returns></returns>
        private static DTE GetDTE()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            return Package.GetGlobalService(typeof(SDTE)) as DTE;
        }

        /// <summary>
        /// Open a file when it exists in the solution
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal static void OpenFile(object sender, RoutedEventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            FileNode node = (((sender as Hyperlink).Parent as TextBlock).Parent as ListViewItem).Tag as FileNode;

            List<string> files = FindFilesInSolutionExplorerProjects(node.FileName);

            if (files.Count == 0)
            {
                CxUtils.DisplayMessageInInfoBar(AsyncPackage, string.Format(CxConstants.NOTIFY_FILE_NOT_FOUND, node.FileName), KnownMonikers.StatusWarning);
            }

            var dte = GetDTE();

            foreach (string file in files)
            {
                // Open the file itself
                _ = dte.ItemOperations.OpenFile(file, EnvDTE.Constants.vsViewKindTextView);

                try
                {
                    // move the cursor for the specific line and column
                    EnvDTE.TextSelection textSelection = dte.ActiveDocument.Selection as EnvDTE.TextSelection;
                    textSelection.MoveToLineAndOffset(node.Line, node.Column);
                }
                catch (Exception)
                {
                    // Avoid Visual Studio to crash if something goes wrong when moving cursor to the specific line
                    continue;
                }
                
            }
        }

        /// <summary>
        /// Find files with the provided name
        /// </summary>
        /// <param name="partialName"></param>
        /// <returns></returns>
        private static List<string> FindFilesInSolutionExplorerProjects(string fileName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string fileNamePath = fileName.Replace("/", "\\");

            fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);

            var dte = GetDTE();

            // Get all projects in the solution
            var projects = dte.Solution.Projects;

            List<string> allFiles = new List<string>();

            foreach (Project project in projects)
            {
                bool projectIsUnloadedInSolution = string.Compare(EnvDTE.Constants.vsProjectKindUnmodeled, project.Kind, System.StringComparison.OrdinalIgnoreCase) == 0;
                
                if (projectIsUnloadedInSolution || string.IsNullOrEmpty(project.FullName)) continue;

                FileInfo projectFileInfo = new FileInfo(project.FullName);
                string projectPath = Directory.GetParent(projectFileInfo.Directory.FullName).FullName;

                // Ignore bin folder
                var foldersToIgnore = Directory.GetDirectories(projectPath, "bin", SearchOption.AllDirectories);

                IEnumerable<string> files;

                try
                {
                    files = Directory.GetFiles(projectPath, fileName, SearchOption.AllDirectories).Where(x => !foldersToIgnore.Any(c => x.StartsWith(c)) && x.Contains(fileNamePath));
                }
                catch(Exception)
                {
                    continue;
                }
                

                allFiles.AddRange(files);
            }

            return allFiles;
        }
    }
}
