# Contacts-Manager

MyContacts is a simple yet functional contacts manager application built with **C#.Net WinForms** and **SQLite** , **Ado.net**. It allows you to manage your personal or professional contacts efficiently with a clean and intuitive interface.

## Features

* **Add New Contact:** Store personal information, including name, email, phone, address, date of birth, country, and an optional image.
* **Edit Contact:** Modify existing contacts easily.
* **Delete Contact:** Remove contacts you no longer need.
* **View Contacts:** Load all contacts in a DataGridView for quick browsing.
* **Country Selection:** Choose the contact's country from a dropdown populated from the database.
* **Image Support:** Attach profile pictures for each contact.

## How to Use

* **Run the Executable:** Navigate to `Contacts/bin/Release` and run `Contacts.exe`.
* **Run from Visual Studio:** Open the `.sln` file in Visual Studio and press F5 to run in Debug mode.

## Notes

* Make sure the SQLite database (`Contacts.db`) is in the correct folder (`bin/Release` or `bin/Debug`) when running the executable.
* The application uses a simple three-layer architecture: Presentation (UI), Business Logic, and Data Access (SQLite).

This is a simple, lightweight app designed for personal use or as a portfolio project, showcasing OOP principles, ADO.NET database operations, and full CRUD functionality in a well-structured WinForms application.

