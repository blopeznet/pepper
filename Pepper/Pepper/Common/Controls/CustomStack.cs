using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pepper.Common.Controls
{
    /// <summary>
    /// CustomStack for list items into scrollviewer
    /// </summary>
    public class CustomStack:StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
   nameof(ItemsSource),
   typeof(IEnumerable),
   typeof(CustomStack),
   null,
   BindingMode.OneWay,
   propertyChanged: (bindable, oldValue, newValue) => OnItemsSourceChanged(bindable, oldValue, newValue));

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {

            if (oldValue==null && newValue != null)
            {
                var stacklayout = bindable as CustomStack;
                foreach (var item in (IEnumerable)newValue)                
                    stacklayout.Children.Add(CustomStack.GenerateItem(stacklayout, item));                
            }

            if (oldValue!= null && newValue != oldValue && newValue!=null)
            {
                var stacklayout = bindable as CustomStack;
                foreach (var item in (IEnumerable)newValue)
                    stacklayout.Children.Add(CustomStack.GenerateItem(stacklayout, item));
            }

            void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                var stacklayout = bindable as CustomStack;

                if (e.OldItems != null)
                    foreach (var item in e.OldItems)
                        stacklayout.Children.Remove(CustomStack.GenerateItem(stacklayout,item));

                if (e.NewItems != null)
                    foreach (var item in e.NewItems)
                        stacklayout.Children.Add(CustomStack.GenerateItem(stacklayout, item));
            }            

            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if (null != oldValueINotifyCollectionChanged)
            {
                oldValueINotifyCollectionChanged.CollectionChanged -= newValueINotifyCollectionChanged_CollectionChanged;
            }

            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;

            if (null != newValueINotifyCollectionChanged)
            {
                newValueINotifyCollectionChanged.CollectionChanged += newValueINotifyCollectionChanged_CollectionChanged;
            }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty =
               BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(CustomStack), default(DataTemplate));

        public static readonly BindableProperty CommandProperty = 
                BindableProperty.Create("Command", typeof(ICommand), typeof(CustomStack), null);

     
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
               
        
        #region Internal methods
        private static View GenerateItem(CustomStack stack, object item)
        {            
            var viewCell = stack.ItemTemplate.CreateContent() as ViewCell;            
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (sender, e) =>
            {
                if (stack.Command == null)                
                    return;

                Xamanimation.AnimationExtension.Animate(stack, new Xamanimation.ShakeAnimation());

                if (stack.Command.CanExecute(item))                
                    stack.Command.Execute(item);                
            };
            viewCell.View.GestureRecognizers.Add(tapGestureRecognizer);
            viewCell.View.BindingContext = item;
            return viewCell.View;            
        }
        #endregion
    }
}
