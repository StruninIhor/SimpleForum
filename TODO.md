## DataContract
### Add avatar field to AppUser (postponed)

## DataAccessServices


## Web



## DONE
1. Test if AspNet.Identity services are working properly with redefined IDs. (They were changed from default 'string' to 'int')

### Add ViewModels for Account:
1. ~RegisterViewModel~
2. ~LoginViewModel~
3. ~ConfirmEmailViewModel~ -> just string and code
3. ~ForgotPasswordViewModel~
4. ~ResetPasswordViewModel~
7. ~ChangePasswordViewModel~

### Add Models to Database:
1. UserProfile
2. Add UserProfile nav. property to user
3. Comment
4. Topic
5. Forum
6. Article
### Configure relationships between entities using Fluent API
### Add EmailService Class and find the way to pass login and password into it (I suppose using Web.Config file and NinjectModule)
### Update UserService (and IUserService interface):
1. ~Configure profile creating~
2. Add validation when creating a user
3. Implement methods from IUserService

### Add ViewModels for Account:
1. ProfileViewModel - after adding Profiles to database
2. EditProfileViewModel
### Add Actions to Controller:
(These tasks are more interesting because of dependencies on services which have to implement these tasks)
1. Login
2. ConfirmEmail
3. ForgotPassword
4. ForgotPasswordConfirmation
5. ResetPassword
6. Profile = Index
7. EditProfile
8. ChangePassword

### Add View Pages for Account:
  1. Register
  2. Login
  3. RegisterSuccessful
  4. ConfirmEmailSuccessful
  5. ResetPasswordRequest
  6. ResetPassword
  7. ResetPasswordSucessful
  8. Profile
  9. EditProfile
  10. ChangePassword