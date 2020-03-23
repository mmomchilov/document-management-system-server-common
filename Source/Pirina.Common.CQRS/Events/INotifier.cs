using System.Threading.Tasks;

namespace Glasswall.Common.CQRS.Events
{
    public interface INotifier
    {
        Task Notify();
        NotifierModel NotifierModel { get; set; }
    }
}