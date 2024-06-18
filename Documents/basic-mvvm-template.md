Basic MVVM Template (with Ninject)
==================================


`Program` Class
---------------

Your program entry point should be changed to the following code snippet. This is more complex
than the default one for .NET Windows Forms projects, but you'll have a fully usable Ninject
kernel, emergency error handling on missing dependencies and cleanup of any services that
implement `IDisposable`.

```csharp
/// <summary>Setup code and main entry point for the application</summary>
internal static class Program {

  /// <summary>The application's main entry point</summary>
  [STAThread]
  static void Main() {
    try {

      // Wrapping the application code in a method lets .NET's "fusion" linker to compile
      // and run the Main() method, even when there are missing dependencies. This enables
      // us to at least display some kind of error message rather than just fizzling out.
      runApplication();

    }
    catch(Exception error) {
      MessageBox.Show(
        "Application failed to launch: " + error.Message,
        "Error",
        MessageBoxButtons.OK,
        MessageBoxIcon.Error
      );
    }
  }

  /// <summary>
  ///   Initializes services, creates the main window and keeps the application running
  ///   until the main window closes.
  /// </summary>
  private static void runApplication() {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    // Set up our dependency injector and all its bindings
    using(var kernel = new StandardKernel()) {
      setupBindings(kernel);

      // Then let the window manager create the view model and show the window
      var windowManager = kernel.Get<IWindowManager>();
      Application.Run(windowManager.OpenRoot<MainViewModel>());
    }
  }

  /// <summary>Setups up dependency bindings for the application</summary>
  /// <param name="kernel">Dependency injector the bindings will be set up in</param>
  private static void setupBindings(IKernel kernel) {
    kernel.Load<MvvmModule>();
    kernel.Bind<IServiceProvider>().ToConstant(kernel);
  }

}
```

MainForm
--------

The main window should inherit from `ViewForm`. It won't change anything regarding how you use
Windows Forms designer and such, but it gives your main window a new property: `DataContext`.

The name `DataContext` comes from WPF and UWP and is used for data binding. For example, a list
control would have a `DataContext` property to which you can assign any kind of list object to
make the list control display its contents.

Similarly, the `DataContext` of your main window will be its view model. The `WindowManager` of
`Nuclex.Windows.Forms` checks if your Form inherits from `ViewModel` and automatically assigns
the view model to the `DataContext` if it can.

```csharp
/// <summary>Main window for the application</summary>
public partial class MainForm : ViewForm {

  /// <summary>Initializes a new main window</summary>
  public MainForm() {
    InitializeComponent();
  }

  /// <summary>The view model for the main window under its proper type</summary>
  private MainViewModel ViewModel {
    get { return DataContext as MainViewModel; }
  }

  // ...
}
```

MainViewModel
-------------

As you could already see in the `Program` class at the top, rather than create and display
a form itself, the new `Program` class just asks the `WindowManager` to "open"
the `MainViewModel` and create its default view, which is discovered by name. Names it will
try are: `MainView`, `MainPage`, `MainForm`, `MainWindow`, `MainDialog` and `MainControl`.

```csharp
var windowManager = kernel.Get<IWindowManager>();
Application.Run(windowManager.OpenRoot<MainViewModel>());
```

Thanks to Ninject, our `MainViewModel` can have constructor parameters and Ninject will
try to satisfy them by looking in its bound services:

```csharp
/// <summary>View model for the application's main window</summary>
public class MainViewModel : ViewModel {

  /// <summary>Initializes a new view model for the main view</summary>
  /// <param name="windowManager">Window manager used to display child windows</param>
  /// <param name="messageService">Service to display messages to the user</param>
  public MainViewModel(
    IWindowManager windowManager,
    IMessageService messageService,
  ) : base(windowManager) {
    this.messageService = messageService;
  }

  /// <summary>Displays an example message to the user</summary>
  public void ShowExampleMessage() {
    this.messageService.Inform(
      new MessageText() {
        Caption = "Example",
        Message = "This is an example message."
      }
    );
  }

  /// <summary>Reports an error to the user</summary>
  /// <param name="exception">Error that will be reported</param>
  protected override void ReportError(Exception exception) {
    this.messageService.ReportError(
      new MessageText() {
        Caption = "Error",
        Message = "An error has occurred: " + exception.Message
      }
    );
  }

  /// <summary>Displays messages to the user</summary>
  private IMessageService messageService;

}
```
