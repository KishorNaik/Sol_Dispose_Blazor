using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Sol_Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sol_Demo.Pages.Demo
{
    public partial class ComponentDisposeDemo : IAsyncDisposable
    {
        #region Declaration

        private Task<IJSObjectReference> _module = null;

        #endregion Declaration

        #region Constructor

        public ComponentDisposeDemo()
        {
            Data = new DataModel();
        }

        #endregion Constructor

        #region DI

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        #endregion DI

        #region Private Property

        private DataModel Data { get; set; }

        private Func<EditContext, Task> OnSubmitCommand { get; set; }

        #endregion Private Property

        #region Private Method

        private void LoadJsModule()
        {
            _module = _module ?? JSRuntime
                           .InvokeAsync<IJSObjectReference>("import", "./js/demo.js")
                           .AsTask();
        }

        private async Task CallDemoJSAsync()
        {
            await (await _module).InvokeVoidAsync(identifier: "displayConsole", Data.Value);
        }

        #endregion Private Method

        #region Handler

        private async Task OnSubmitHandlerAsync(EditContext editContext)
        {
            var flag = editContext.Validate();
            if (flag == false) return;

            await CallDemoJSAsync();
        }

        #endregion Handler

        #region Dispose Method

        /// <summary>
        /// Automatically dispose method call once the component removed from UI.
        /// If end user navigate from one component to another component then it will automatically call the dispose method of previous component.
        /// </summary>
        /// <returns></returns>
        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_module != null)
            {
                await (await _module).DisposeAsync();
            }
        }

        #endregion Dispose Method

        #region Life Cycle Events

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                OnSubmitCommand = (editContext) => this.OnSubmitHandlerAsync(editContext);

                this.LoadJsModule();
            }
        }

        #endregion Life Cycle Events
    }
}