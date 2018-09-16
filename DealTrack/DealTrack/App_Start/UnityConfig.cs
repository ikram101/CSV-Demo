using DealTrack.IServices;
using DealTrack.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace DealTrack
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<ICsvFileService, CsvFileService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}