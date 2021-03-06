﻿using Blazor.Gitter.Library;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Gitter.Core.Components.Shared
{
    public class RoomTitleBase : ComponentBase, IDisposable
    {
        [Inject] IAppState State { get; set; }

        [Parameter] public IChatRoom ChatRoom { get; set; }
        [Parameter] public string OuterClassList { get; set; }

        bool IsPaused = false;

        private bool OuterClassListIsEmpty => string.IsNullOrWhiteSpace(OuterClassList);
        internal string OuterClass => new BlazorComponentUtilities.CssBuilder()
            .AddClass("blg-top-center",OuterClassListIsEmpty)
            .AddClass("d-flex", OuterClassListIsEmpty)
            .AddClass("justify-content-start", OuterClassListIsEmpty)
            .AddClass("align-items-center", OuterClassListIsEmpty)
            .AddClass("text-muted",OuterClassListIsEmpty)
            .AddClass("text-truncate",OuterClassListIsEmpty)
            .AddClass("paused",IsPaused)
            .AddClass(OuterClassList,!OuterClassListIsEmpty)
            .Build();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            State.ActivityTimeout += ActivityTimeout;
            State.ActivityResumed += ActivityResumed;
        }

        private void ActivityResumed(object sender, EventArgs e)
        {
            IsPaused = false;
            InvokeAsync(StateHasChanged);
            Task.Delay(1);
        }

        private void ActivityTimeout(object sender, EventArgs e)
        {
            IsPaused = true;
            InvokeAsync(StateHasChanged);
            Task.Delay(1);
        }

        public void Dispose()
        {
            State.ActivityTimeout -= ActivityTimeout;
            State.ActivityResumed -= ActivityResumed;
        }
    }
}
