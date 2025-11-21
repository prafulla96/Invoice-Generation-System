Invoice Generation System

The Invoice Generation System is a web-based application built using ASP.NET Core MVC, Entity Framework, SQL Server, HTML, CSS, JavaScript, and Bootstrap.
It allows users to create invoices, calculate GST, generate yearly summaries, search invoices by invoice number or date range, and export invoices as PDFs.

🔹 Invoice Management
#Create, view, edit, and delete invoices
#Automatic GST calculation
#Line-item based subtotal & tax calculation

🔹 Search & Filter
#Search invoices by invoice number
#Filter invoices by date range
#Generate filtered lists instantly

🔹 Yearly Summary Reports
#Calculate yearly totals (subtotal, GST, and grand total)
#Visual summary of yearly transactions
#Export yearly summary as PDF

🔹 PDF Generation
#Download individual invoice PDFs
#Clean, professional invoice layout
#Uses server-side PDF rendering
***************************The PDFs include:
Invoice number
Date
Customer details
Itemized list
GST applied
Final amount

📁 Project Structure
InvoiceGenerationSystem/
│── Controllers/
│── Models/
│── Views/
│── Services/
│── wwwroot/
│── Migrations/
│── InvoiceGenerationSystem.csproj
│── appsettings.json

Clone the Repository
git clone https://github.com/prafulla96/Invoice-Generation-System
cd InvoiceGenerationSystem
