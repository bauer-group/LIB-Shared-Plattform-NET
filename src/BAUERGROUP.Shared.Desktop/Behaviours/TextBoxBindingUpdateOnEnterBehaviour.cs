using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfTextBox = System.Windows.Controls.TextBox;

namespace BAUERGROUP.Shared.Desktop.Behaviours
{
    public class TextBoxBindingUpdateOnEnterBehaviour : Behavior<WpfTextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.KeyUp += OnTextBoxKeyUp;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyUp -= OnTextBoxKeyUp;
        }

        private void OnTextBoxKeyUp(Object oSender, System.Windows.Input.KeyEventArgs oArguments)
        {
            if (oArguments.Key == Key.Enter)
            {
                var oTextBox = oSender as WpfTextBox;
                oTextBox.GetBindingExpression(WpfTextBox.TextProperty).UpdateSource();
            }
        }
    }
}
