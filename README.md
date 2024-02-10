# Ako Maori

This repository contains the code for the Ako Māori Full-Stack web application, which is aimed at providing a platform for learning te reo Māori.

## Back-End

### A2Controller.cs
Description: This controller manages various endpoints for the website's API.
### Endpoints:
/webapi/GetVersion: Retrieves the version of the API.  
/webapi/Logo: Retrieves the website's logo.
/webapi/AllItems: Retrieves all items available on the website.
/webapi/Items/{term}: Retrieves items based on a search term.
/webapi/ItemImage/{id}: Retrieves the image of an item by its ID.
/webapi/GetComment/{id}: Retrieves a comment by its ID.
/webapi/WriteComment: Writes a new comment.
/webapi/Comments/{num}: Retrieves a specified number of comments.
/webapi/Register: Registers a new user.
/webapi/PurchaseItem/{id}: Initiates the purchase of an item.
/webapi/AddEvent: Adds a new event to the system.
/webapi/EventCount: Retrieves the count of events.
/webapi/Event/{id}: Retrieves details of a specific event.

### Database
The Back-End uses a SQLite database with 5 tables- Comments, Events, Organizers, Products, Users- as described in the Models folder.

### Authorization
The controller includes authorization attributes to restrict access to certain endpoints:
Authorize(Policy = "UserOnly"): Allows access only to authenticated users.
Authorize(Policy = "AdminOnly"): Allows access only to authenticated users with admin privileges.
Authorize(Policy = "AdminAndUser"): Allows access to both authenticated users and admins.
### Usage
To use the backend API:

Ensure the backend server is running.
Send HTTP requests to the appropriate endpoints mentioned above.
Handle the responses returned by the API.
### Dependencies
The backend relies on external dependencies for various functionalities such as logging, authorization, and file operations. .NET7 must be installed to run APIs in Swagger simulator or to implement in the Front-End.
### Notes
Ensure proper authentication and authorization mechanisms are in place to secure sensitive endpoints.
Validate input data to prevent security vulnerabilities such as SQL injection and cross-site scripting (XSS).
Handle errors gracefully and provide informative responses to clients.

## Front-End

### AkoMaori.js

This file contains the JavaScript code that provides the functionality for the Ako Māori web application. It includes functions for tab navigation, displaying version information, dynamically creating events tables, dynamically making shop tables, handling user registration and login, uploading comments, and implementing a learning game.

### AkoMaori.html

This HTML file serves as the structure for the Ako Māori web application. It defines the layout of the application using HTML elements and includes references to the JavaScript and CSS files. It also contains tab content sections for different features of the application, such as the home page, guest book, shop, user registration and login, events, and learning te reo.

### AkoMaori.css

The CSS file contains styles for the Ako Māori web application, including background colors, tab styles, headers, paragraphs, form elements, and table formatting.

### Getting Started

To run the Ako Māori web application locally, simply clone this repository and open the `AkoMaori.html` file in a web browser.

### Usage

Once the application is running, you can navigate through the different tabs to explore various features such as the guest book, shop, events, and learning te reo.

## Project Status

The professor instructed us to use a separate back-end for our website that he designed so students can still build the front-end even if they did not complete the first part of the course. If we got 100%, which I did, then the back-end's are identical, aside from the matching pairs which the professor gave us. I will look to perfect the final bugs during the Summer.

## Bugs

Learning Te Reo does not accurately calculate the user score and some media do not display properly.
