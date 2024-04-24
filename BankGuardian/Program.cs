using BankLibrary.Services;
using System.Drawing.Text;
using Microsoft.Extensions.Hosting;
using Autofac;
namespace BankGuardian
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AutoFacModule>();
            var container = builder.Build();
            container.Resolve<Guardian>().Check();
        }
    }
}
