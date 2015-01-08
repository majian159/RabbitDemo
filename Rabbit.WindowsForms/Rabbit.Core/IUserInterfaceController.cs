using Rabbit.Kernel;
using System.Windows.Forms;

namespace Rabbit.Core
{
    public interface IUserInterfaceController : ISingletonDependency
    {
        void ShowContent(string title, Control control);
    }
}