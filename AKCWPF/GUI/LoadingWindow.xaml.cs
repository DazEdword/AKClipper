using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace AKCWPF {

    public partial class LoadingWindow : Window {
        /// <summary>
        /// LoadingWindow is a simple, indeterminate Progress Bar in a Window. These methods below can be called whenever a heavy method
        /// potentially taking some time to return is going to be called. Remember to call the performance-heavy method with
        /// await Task.Run(() => Method()); to avoid blocking the UI thread (or use other non-UI blocking solutions). Methods
        /// are public so that they can be reused across the program.
        /// </summary>

        public bool Closable = false;

        public LoadingWindow() {
            InitializeComponent();

            this.Owner = Application.Current.MainWindow;
            this.Topmost = true;

            //Comment just one of these in order to use the delayed show that removes flickering on short calculations.
            //this.DelayedShow(); 
            this.Show();
        }

        public void CloseLoadingWindow() {
            if (this != null) {
                this.Closable = true;
                this.Close();
            }
        }

        private void setName(string title) {
            this.Title = title;
        }

        private async void DelayedShow() {
            await Task.Delay(150); //0,15 seconds of delay to avoid flickering on short actions. Current implementation might be problematic.
            if (this.Closable == false) {
                this.Show();
            }
        }

        protected override void OnClosing(CancelEventArgs e) {
            if (!Closable) {
                // Overriding OnClosing to cancel the close request.
                e.Cancel = true;  
                base.OnClosing(e);
            }
        }
    }
}