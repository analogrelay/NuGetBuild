using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGet.Build
{
    public class BuildPackageRepository : IPackageRepository
    {
        private IList<IPackage> _processed = new List<IPackage>();
        
        public BuildPackageRepository()
        {
        }

        public void AddPackage(IPackage package)
        {
            _processed.Add(package);
        }

        public IQueryable<IPackage> GetPackages()
        {
            // Only return packages we've "processed"
            return _processed.AsQueryable();
        }

        public void RemovePackage(IPackage package)
        {
            _processed.Remove(package);
        }

        public string Source
        {
            get { return "<Project>"; }
        }

        public bool SupportsPrereleasePackages
        {
            get { return true; }
        }
    }
}
