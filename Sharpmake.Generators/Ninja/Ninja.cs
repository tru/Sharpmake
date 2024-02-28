using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sharpmake.Generators.Ninja
{
    /// <summary>
    ///
    /// </summary>
    public partial class NinjaGenerator : IProjectGenerator, ISolutionGenerator
    {
        private const string NinjaExtension = ".ninja";
        private const string ObjectExtension = ".obj";

        private string Escape(string instr)
        {
            return instr.Replace(" ", "$ ");
        }

        private class NinjaFile
        {
            public string FileName;

            public NinjaFile(string fileName)
            {
                FileName = fileName;
            }
        }

        private class NinjaContext
        {
            Builder Builder;
            Project Project;
            string ProjectDir;
            public IReadOnlyList<Project.Configuration> ProjectConfigurations { get; }

            public NinjaContext(Builder builder, Project project, string projectDir, IEnumerable<Project.Configuration> projectConfigurations)
            {
                Builder = builder;
                Project = project;
                ProjectDir = projectDir;
                ProjectConfigurations = projectConfigurations as IReadOnlyList<Project.Configuration>;
            }
        }


        private List<NinjaFile> GetSourceFiles(NinjaContext context)
        {
            List<NinjaFile> files = new();


            return files;
        }

        public void Generate(Builder builder, Project project, List<Project.Configuration> configurations, string projectFile, List<string> generatedFiles, List<string> skipFiles)
        {
            if (!NinjaSettings.NinjaSupportEnabled)
                return;

            //To make sure that all the projects are fastbuild
            configurations = configurations.Where(x => x.IsNinja && !x.DoNotGenerateNinja).OrderBy(x => x.Platform).ToList();
            if (!configurations.Any())
                return;

            var projectFileInfo = new FileInfo(Util.GetCapitalizedPath(projectFile + NinjaExtension));
            string projectPath = new FileInfo(projectFile).Directory.FullName;
            NinjaContext context = new(builder, project, projectPath, configurations);
            var sourceFiles = GetSourceFiles(context);

            var fileGen = new FileGenerator();
            {
                fileGen.Write(Template.Header);
            }
            builder.Context.WriteGeneratedFile(project.GetType(), projectFileInfo, fileGen);
        }

        public void Generate(Builder builder, Solution solution, List<Solution.Configuration> configurations, string solutionFile, List<string> generatedFiles, List<string> skipFiles)
        {
            var fileInfo = new FileInfo(solutionFile);
            string solutionPath = fileInfo.Directory.FullName;
            var solutionFileInfo = new FileInfo(Util.GetCapitalizedPath(solutionPath + Path.DirectorySeparatorChar + "build" + NinjaExtension));
            Console.WriteLine($"Ninja solution: {solutionFileInfo.FullName}");

            bool projectsWereFiltered = false;
            List<Solution.ResolvedProject> solutionProjects = solution.GetResolvedProjects(configurations, out projectsWereFiltered).ToList();
            solutionProjects.Sort((a, b) => string.Compare(a.ProjectName, b.ProjectName)); // Ensure all projects are always in the same order to avoid random shuffles

            if (solutionProjects.Count == 0)
            {
                // Erase solution file if solution has no projects.
                /*updated = solutionFileInfo.Exists;
                if (updated)
                    Util.TryDeleteFile(solutionFileInfo.FullName);
                return solutionFileInfo.FullName;*/
            }

            var fileGen = new FileGenerator();
            {
                fileGen.Write(Template.Header);

                foreach (Solution.ResolvedProject resolvedProject in solutionProjects)
                {
                    using (fileGen.Declare("projectFile", Escape(resolvedProject.ProjectFile)))
                    {
                        fileGen.Write(Template.Include);
                    }
                }
            }

            builder.Context.WriteGeneratedFile(solution.GetType(), solutionFileInfo, fileGen);
        }

        #region Utils

        private static string PathMakeUnix(string path, bool escapeSpaces = true)
        {
            string clean = path.Replace(Util.WindowsSeparator, Util.UnixSeparator).TrimEnd(Util.UnixSeparator);
            if (escapeSpaces)
                return clean.Replace(" ", @"\ ");

            return clean;
        }

        private static void PathMakeUnix(IList<string> paths)
        {
            for (int i = 0; i < paths.Count; ++i)
                paths[i] = PathMakeUnix(paths[i]);
        }

        private void SelectOption(Project.Configuration conf, params Options.OptionAction[] options)
        {
            Options.SelectOption(conf, options);
        }

        #endregion

    }
}
