using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pepper.Common.Behavior
{
    /// <summary>
    /// Class for use item tapped as behaviour
    /// </summary>
    public class ListViewTappedItemBehavior : Behavior<ListView>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ListViewTappedItemBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(ListViewTappedItemBehavior), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        public ListView AssociatedObject { get; private set; }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += OnBindingContextChanged;
            bindable.SelectionMode = ListViewSelectionMode.None;
            bindable.ItemTapped += OnListViewTapped;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
            bindable.ItemTapped -= OnListViewTapped;
            AssociatedObject = null;
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
        

        async void OnListViewTapped(object sender, ItemTappedEventArgs e)
        {
            

            if (Command == null)
            {
                return;
            }
            
            object parameter = Converter.Convert(e, typeof(object), null, null);
            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
                        
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        private async Task ShakeElement(VisualElement element, CancellationToken cancellation)
        {
            uint timeout = 25;
            await element.TranslateTo(-10, 0, timeout);
            await element.TranslateTo(10, 0, timeout);
            await element.TranslateTo(-5, 0, timeout);
            await element.TranslateTo(5, 0, timeout);
            element.TranslationX = 0;
        }
    }
}
