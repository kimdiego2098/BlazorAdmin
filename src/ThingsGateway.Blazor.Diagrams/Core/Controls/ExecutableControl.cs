using ThingsGateway.Blazor.Diagrams.Core.Events;
using ThingsGateway.Blazor.Diagrams.Core.Models.Base;
using System.Threading.Tasks;

namespace ThingsGateway.Blazor.Diagrams.Core.Controls;

public abstract class ExecutableControl : Control
{
    public abstract ValueTask OnPointerDown(Diagram diagram, Model model, PointerEventArgs e);
}