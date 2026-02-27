using Microsoft.AspNetCore.Components;

namespace BlazorApp1.Render
{
    public partial class WizardStep
    {
        [CascadingParameter]
        protected internal Wizard Parent { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected override void OnInitialized()
        {
            Parent?.AddStep(this);
        }
    }
}
