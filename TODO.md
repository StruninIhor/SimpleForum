## DataContract
### Add Models to Database:
1. UserProfile
2. Add UserProfile nav. property to user
3. Comment
4. Topic
5. Forum
6. Article
### Configure relationships between entities using Fluent API
## DataAccessServices
### Update UserService (and IUserService interface):
1. Add IUserService.GenerateConfirmationTokenAsync() method to user service
2. Add validation when creating a user
3. will be continued... 

## Web
### Add ViewModels for Account:
1. ProfileViewModel - after adding Profiles to database
2. EditProfileViewModel
### Add Actions to Controller:
(These tasks are more interesting because of dependencies on services which have to implement these tasks)
1. Login
2. ConfirmEmail
2. ForgotPassword
2. ForgotPasswordConfirmation
3. ResetPassword
4. Profile
5. EditProfile
6. ChangePassword
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
## DONE
1. Test if AspNet.Identity services are working properly with redefined IDs. (They were changed from default string ti int)

### Add ViewModels for Account:
1. ~RegisterViewModel~
2. ~LoginViewModel~
3. ~ConfirmEmailViewModel~ -> just string and code
3. ~ForgotPasswordViewModel~
4. ~ResetPasswordViewModel~
7. ~ChangePasswordViewModel~
