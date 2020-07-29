using Autofac;
using Autofac.Integration.Mvc;
using IComponent;
using IManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace OrderPlatForm.App_Start
{
    /// <summary>
    /// 依赖注入类   
    /// </summary>
    public class AutoFacConfig
    {
        /// <summary>
        /// 依赖注入方法
        /// </summary>
        public static void Register()
        {
            //实例化一个创建aotufac的创建容器
            var builder = new ContainerBuilder();
            //告诉Autofac框架，将来要创建的控制器类存放在哪个程序集 (AutoFacMvcDemo)
            Assembly controllerAss = Assembly.Load("OrderPlatForm");
            //注册MVC控制器，并且允许属性注入
            builder.RegisterControllers(Assembly.GetCallingAssembly())//注册mvc的Controller
                .PropertiesAutowired();//属性注入
            //一次性注册所有实现了IDependency接口的类
            Type baseType = typeof(IDependency);
            Assembly[] assemblies = Directory.GetFiles(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll").Select(Assembly.LoadFrom).ToArray();
            builder.RegisterAssemblyTypes(assemblies)
                   .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract)
                   .AsSelf().AsImplementedInterfaces()
                   .PropertiesAutowired().InstancePerLifetimeScope();
            //一次性注册所有实现了IDependencys接口的类
            Type baseTypes = typeof(IDependencys);
            Assembly[] assembliess = Directory.GetFiles(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll").Select(Assembly.LoadFrom).ToArray();
            builder.RegisterAssemblyTypes(assembliess)
                   .Where(type => baseTypes.IsAssignableFrom(type) && !type.IsAbstract)
                   .AsSelf().AsImplementedInterfaces()
                   .PropertiesAutowired().InstancePerLifetimeScope();
            //创建一个Autofac容器
            IContainer container = builder.Build();
            //将mvc的控制器对象实例 交由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}