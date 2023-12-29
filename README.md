# HomeStorage
Made as a project to keep track on what you have on storage. Whether it be food supply or attic storage.

## Goals
### Primary goal
Currently it is being developed with the primary focus of keeping track of colonial goods / dry storage / freezer overview. With the goal of reducing foodwaste, by keeping track of expiration dates, and what is on hand. As with everything, this tool is only as good as the data provided. So it's trial and error, to find what should be added to the system, as updating it could be trivial for some often used items.

### Secondary goal
For me to lern Vue, and an opportunity to play with technologies/features i usually don't or are inexperienced with.

## Development
Since this is a one man operation (for now), the development will be slow. But a lof of things are on the drawingboard.

### Under development
- Filter system
    - This has become quite extensite, and will probaly be separated into its own project. But the goal being a fully customizable filter framewore. That will be applied to the project.

### Future development
- Shared shopping list
    - Yet to be decided if this will be location based or separate and shared between users.
    - Multiple shoppingslists?
    - Update inventory directly from shoppinglist
- Low inventory
    - Be able to set a lower limit, for when a product should be restocked.
    - Automatic add to specified shoppinglist when lower limit is reached.
- Multi language support
- Proper use of the features a PWA provide
    - Caching of products for offline usage.
    - Notifications for products close to expiring.
    - Update
- Barcode support
    - Yet to try if a free available barcode database, could provide usefull data. To easier create new products and scan to open existing product.
    - Could be interesting to look at QR codes, as eg. GS1 Denmark are pushing for them to be mandatory as replacement for barcodes. And multiple contries already have taking them into use, to an extend.
- Login via 3rd party
- Styling and overall look / useability
    - Overall walkthrough of stylesheets to make a color scheme that gives it an unique look and feel.
    - Proper icon for the downloadabe PWA
    - Proper support for larger devices (Currently only mobile is taken into consideration)
    - Currently it's being developed with Android users in mind, using the back button at the bottom. Meaning it could be trivial for iOS users without a dedicated back button.

### Known bugs
- Can't delete a product while it's in a category
