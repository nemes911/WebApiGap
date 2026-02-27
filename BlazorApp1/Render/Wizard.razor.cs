using BlazorApp1.Components.Pages;
using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Render
{
    public partial class Wizard
    {
        protected internal List<WizardStep> Steps = new();

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal void AddStep(WizardStep step) { Steps.Add(step); } 
    }
}
