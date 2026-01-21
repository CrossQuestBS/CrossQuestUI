using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;

namespace CrossQuestUI.Services
{
    public class DesignResolve(Type Type) : MarkupExtension
    { 
        
        public override object ProvideValue(IServiceProvider serviceProvider) {
            if (!Design.IsDesignMode) {
                throw new InvalidOperationException();
            }

            return App.Current.Services.GetRequiredService(Type);
        }
    }
}