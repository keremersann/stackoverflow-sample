using StackOverFlowClone.ServiceLayer;
using System.Web.Http;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace StackOverFlowClone
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<IQuestionService, QuestionService>();
            container.RegisterType<IUsersService, UsersService>();
            container.RegisterType<ICategoriesService, CategoriesService>();
            container.RegisterType<IAnswersService, AnswersService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}