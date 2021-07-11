using Microsoft.AspNetCore.Components;

namespace ScrumPoker.UI.Components.Tabs
{
    public interface ITab
    {
        RenderFragment ChildContent { get; }
    }
}
