using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace NuGet.Build
{
    public class ResolvePackageReference : Task
    {
        [Required]
        public ITaskItem[] PackageReferences { get; set; }

        [Required]
        public string ProjectDirectory { get; set; }

        [Required]
        public string TargetFramework { get; set; }

        [Required]
        public ITaskItem[] CurrentReferences { get; set; }

        [Output]
        public ITaskItem[] References { get; set; }

        public override bool Execute()
        {
            // Get the Id and version
            //Debugger.Launch();
            References = PackageReferences.SelectMany(ResolvePackage).ToArray();
            return true;
        }

        private IEnumerable<ITaskItem> ResolvePackage(ITaskItem package)
        {
            string id = package.ItemSpec;
            string version = package.GetMetadata("Version");

            Log.LogMessage(MessageImportance.Normal, "Resolving Package Reference {0} {1}...", id, version);

            // Initial version just searches a machine-level repository

            var localFs = new PhysicalFileSystem(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NuGet", "Lib"));
            var defaultResolver = new DefaultPackagePathResolver(localFs);
            var machineRepo = new LocalPackageRepository(defaultResolver, localFs);
            var buildRepo = new BuildPackageRepository();
            var remoteRepo = new DataServicePackageRepository(new Uri("https://nuget.org/api/v2"));
            var project = new BuildProjectSystem(ProjectDirectory, new FrameworkName(TargetFramework), CurrentReferences);
            var manager = new PackageManager(remoteRepo, defaultResolver, localFs, machineRepo);
            var projectManager = new ProjectManager(remoteRepo, defaultResolver, project, buildRepo);

            // Install the package
            var ver = new SemanticVersion(version);
            manager.PackageInstalling += manager_PackageInstalling;
            manager.InstallPackage(id, ver);
            projectManager.AddPackageReference(id, ver);


            return project.OutputReferences.Select(item =>
            {
                var name = AssemblyName.GetAssemblyName(item);
                return new TaskItem(name.FullName, new Dictionary<string, string>() {
                    {"HintPath", item },
                    {"Private", "true"}
                });
            });
        }

        void manager_PackageInstalling(object sender, PackageOperationEventArgs e)
        {
            Log.LogMessage(MessageImportance.High, "Installing {0} {1} ...", e.Package.Id, e.Package.Version);
        }
    }
}
