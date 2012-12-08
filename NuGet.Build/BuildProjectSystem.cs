using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;

namespace NuGet.Build
{
    public class BuildProjectSystem : PhysicalFileSystem, IProjectSystem
    {
        private FrameworkName _targetFramework;
        private ITaskItem[] _currentReferences;

        public IList<string> OutputReferences { get; private set; }

        public BuildProjectSystem(string root, FrameworkName targetFramework, ITaskItem[] currentReferences)
            : base(root)
        {
            _targetFramework = targetFramework;
            _currentReferences = currentReferences;
            OutputReferences = new List<string>();
        }

        public void AddFrameworkReference(string name)
        {
            throw new NotImplementedException();
        }

        public void AddReference(string referencePath, Stream stream)
        {
            OutputReferences.Add(referencePath);
        }

        public bool IsBindingRedirectSupported
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSupportedFile(string path)
        {
            throw new NotImplementedException();
        }

        public string ProjectName
        {
            get { return "<Dummy>"; }
        }

        public bool ReferenceExists(string name)
        {
            return _currentReferences.Any(item => String.Equals(item.ItemSpec, name));
        }

        public void RemoveReference(string name)
        {
            throw new NotImplementedException();
        }

        public string ResolvePath(string path)
        {
            throw new NotImplementedException();
        }

        public FrameworkName TargetFramework
        {
            get { return _targetFramework; }
        }

        public dynamic GetPropertyValue(string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
