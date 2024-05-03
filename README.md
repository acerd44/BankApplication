# Bank System Website Project

This is a comprehensive banking system administration tool written in ASP.NET and EF Core Razor Pages. This web application offers a range of features for managing customers, accounts, and users (both admins and cashiers), all designed with a user-friendly interface based on Bootstrap template. 
It also includes a Web API for checking customer info and transaction lists and a Console App checking for suspicious transactions.
This project was developed for my web development course in university.
## Features

- **Azure Hosting**: It is being hosted on Azure [here](https://hossenbank.azurewebsites.net/). 
- **Database**: This uses a database first principle, the .bak file can be downloaded [here](https://www.dropbox.com/scl/fi/3agsb6mo48u12kdviziom/BankAppDatav2-1.bak?rlkey=b4q7her90zenm8xn7ephh39ux&st=i1bzyi4i&dl=0).
- **Customer and Account Management**: Create, Read, Update, and Delete (CRUD) operations for customers and users.
- **User Roles**: Administrators have full access to manage users, customers, and accounts, while Cashiers can only handle customers and accounts.
- **View Information**: Get detailed insights into customer information, transactions, and account balances.
- **Search and Sort**: Convenient search system for customers and accounts, with sorting options based on ID, Country, City, and Balance.
- **Country Statistics**: See the statistics on how many accounts, customers and total balance in the four countries: Sweden, Norway, Denmark, and Finland.
- **Top Ten Customers**: See statistics for the top ten customers in four countries mentioned above.
- **Transactions**: Perform withdrawals and deposits from accounts, view transaction history.
- **Pagination**: Implemented for easy navigation and organization of data.
- **Class Library and Services**: Used alongside AutoMapper for efficient data mapping, Services, and Dependency Injection.
- **Web API**: Utilize the Web API to input an account ID and retrieve all transactions, or input a customer ID to get their information.
- **Console App**: Monitor suspicious transactions for potential money laundering in each country. Conditions include:
  - A single transaction above 15,000 SEK.
  - Total transactions in the last 72 hours above 23,000 SEK.
  After the search, a report is saved as a .txt file on your computer, detailing country, names, account IDs, and transaction IDs.

## Getting Started

### Prerequisites
- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Visual Studio](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/download)
- Bootstrap template for design (included)

### Installation
1. Clone this repository to your local machine with the command below or just press on the Code button and then Open with Visual Studio.
   ```bash
   git clone https://github.com/acerd44/BankApplication.git
2. Open the solution file in Visual Studo.

### Azure Hosting
- If you do not wish to install the database and the application, the website is also being hosted on Azure [here](https://hossenbank.azurewebsites.net/). 
- **Admin account (Email - Password)**: "admin.test@gmail.com" - "TestAccount123!"
- **Cashier account (Email - Password)**: "cashier.test@gmail.com" - "TestAccount123!"
