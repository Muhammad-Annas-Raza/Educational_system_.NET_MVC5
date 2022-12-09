using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Project_eStudiez.Interface;
using Project_eStudiez.Models;
using Project_eStudiez.OtherMethods;

namespace Project_eStudiez
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers


            container.RegisterType<IRepository<tbl_user>,RepositoryClass<tbl_user>>();
            container.RegisterType<IRepository<tbl_standard>, RepositoryClass<tbl_standard>>();
            container.RegisterType<IRepository<tbl_role>, RepositoryClass<tbl_role>>();
            container.RegisterType<IRepository<tbl_resources>, RepositoryClass<tbl_resources>>();
            container.RegisterType<IRepository<tbl_pdf>, RepositoryClass<tbl_pdf>>();
            container.RegisterType<IRepository<tbl_mark>, RepositoryClass<tbl_mark>>();
            container.RegisterType<IRepository<tbl_link>, RepositoryClass<tbl_link>>();
            container.RegisterType<IRepository<tbl_feedback>, RepositoryClass<tbl_feedback>>();
            container.RegisterType<IRepository<tbl_extraclass>, RepositoryClass<tbl_extraclass>>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}