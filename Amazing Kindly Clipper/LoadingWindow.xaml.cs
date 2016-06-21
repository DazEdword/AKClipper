using System.Windows;
using System.ComponentModel;
using System.Threading.Tasks;

namespace ClippingManager
{
   
    public partial class LoadingWindow : Window
    {
        /// <summary>
        /// LoadingWindow is a simple, indeterminate Progress Bar in a Window. These methods below can be called whenever a heavy method 
        /// potentially taking a good time is going to be called, Open to create and show the window, and Close to close it. Remember to call
        /// the heavy method with  await Task.Run(() => Method()); to avoid blocking the UI thread (or other non-UI blocking solutions). Methods
        /// are public so that they can be reused across the program. 
        /// </summary>

        public bool Closable = false;

        public LoadingWindow()
        {    
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            this.Topmost = true;

            //this.DelayedShow(); //Comment just one of these in order to use the delayed show that removes flickering on short calculations. 
            this.Show();
        }

    
        public void CloseLoadingWindow()
        {
            if (this != null)
            {
                this.Closable = true;
                this.Close();
            }
        }

        private void setName(string title)
        {
            this.Title = title;
        }

        private async void DelayedShow()
        {
            await Task.Delay(150); //0,15 seconds of delay to avoid flickering on short actions. Current implementation might be problematic.  
            if (this.Closable == false)
            {
                this.Show();
            }         
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Closable)
            {
                e.Cancel = true;  // Overriding OnClosing to cancel the close request.
                base.OnClosing(e);
            }

        }
    }
}
