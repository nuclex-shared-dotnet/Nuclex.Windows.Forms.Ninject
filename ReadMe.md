  - **Status:** Stable and mature. Several projects are using this library,
    and it has received extensive testing on Linux and Windows.

  - **Platforms:** Cross-platform, developed on Linux but also tested and
    working without any known issues on Windows.

Nuclex.Windows.Forms.Ninject
============================

Nuclex.Windows.Forms is a standalone, lightweight MVVM library that lets you "display"
view models by automatically picking a default view, creating an instance of it and
of your view model.

This library provides a module for Ninject by which view models can not just be
constructed via a default constructor, byt can use full constructor injection to require
services such as the `IMessageService` (to display message boxes), the `IWindowManager`
to open modal and modeless child windows and anything else you bind via Ninject.
