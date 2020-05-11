# AzureOnTheCheap
Azure on the Cheap posts focus on the architecture, techniques and technology in Azure that add considerable value without breaking the bank. This value is being measured in both the amount of effort taken to develop, provision, and maintain the resources, as well as the monthly charge for the resource.

This project is provided to provide a sample project of how the technology highlighted in the posts were put together. It is not meant as a best practice guide but it is encouraged that viewers contribute to the quality of the examples.

If you see the value in this project, then please comment and contribute!

# Implementation Notes
The current project contains:
## Static website containing 
* landing page (index.html) containing a Datatables.net grid that uses an Azure Function API for source data
* Three galleries to illustrate content from local content, Azure Storage blob container and Azure CDN
## GitHub Actions
Illustrates using GitHub Actions for deploying the Azure Functions and static website to multiple regions
## Azure Function
Illustrates an webhttp API for returning source data
