# README

These projects create some nuget packages that form the backbone of my own applications. They are not
intended for public use, however you are free to do so.

Be aware that versioning does not follow semantic versioning, and that all the nuget packages produced
by these projects are "together" in the sense that version ranges are not considered. If you upgrade/use
one of them, you *probably* need to upgrade them all. As I said, they're not intended for anyone but me.

By "not intended for public use" I simply mean that the framework lay out the
foundation for my own applications in a way I have found to be quite good. It
will almost certainly not be the way everybody else wants to do it, and there
may be decisions and methods used here that will clash with other peoples
requirements.

Some principles my applications follow:

* Everything is async
* I use DryIoc as a service container
* Almost everything is handled as services
* Configuration is done using appsettings.json (and some sister files)

The framework supplies a lot of features, here's a few of the core ones:

* Background services that run for as long as the application runs
* Support different types of applications
    * Tray icon application (on Windows only)
    * Console application (it does what it does then terminates)
    * Daemon application (console application that will never terminate except if asked to)
    * WebAPI application (ASP.NET Core MVC based)
* Full async handling of everything, including graceful shutdown of console applications
* Configuration via json files (and support for adding new sources if needed)
    * Including support for automatic reload of configuration while the program
      is running if the underlying files change
* Logging to console and file, all configurable (log levels, filenames, etc.)
* Platform-independent "data protection" api, can store secrets in
  public files, encryption key stored locally in environment
  variables or other sources
* Support for asking a daemon-type program to quit by creating a file with
  a specific name, also see that it is running by the presence of another
  such file

The basic guideline is that for my applications, simple things should be simple.

A daemon application consists of a one-liner Program class, as well as
the background service(s) that will run as part of the daemon, and
the service registrations for those background services. The overhead to
get the daemon application up and running is literally just a handful or two
of lines of code, and they're not complex.

**If you have questions about the use of any of the classes or packages, [feel free to ask](mailto:lasse@vkarlsen.no).**