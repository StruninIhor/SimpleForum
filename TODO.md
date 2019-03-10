#Restructuring 
1. Divide contracts and implementations into two separate class libraries:
	1.1. DataContract and DataAccessServices (Not the DAC that already exist)
	1.2. BusinessContract and BusinessServices (From old DAC) 
	1.3. Move Resolving Dependencies into separate class Library



## Web


###Configure Comment form post and handling
	1. Remove other attributes except HttpPostAttribute and HttpGetAttribute in CommentController and rewrite models and rename actions in CommentController

### Add Replies Count in Comment Model (maybe in BusinessServices.Models.CommentModel too)



### Add manage(admin) Controller

### Add user profile view - maybe configure routes from /Account/GetUserProfile/id to /User/id

###Edit Navigation Menu Test
