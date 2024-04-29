using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;

namespace ScrumPoker.UI.Components.Tabs
{
    public abstract class TablerBaseComponent : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> UnmatchedParameters { get; set; }

        protected object GetUnmatchedParameter(string key)
        {
            if (UnmatchedParameters?.ContainsKey(key) ?? false)
            {
                var value = UnmatchedParameters[key];
                UnmatchedParameters.Remove(key);
                return value;
            }

            return null;
        }
    }
}
