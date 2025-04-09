# SportsWatcher
A way to make sports new gen 

For password verication here is a skel :

  var result = hasher.VerifyHashedPassword(new User(), hashedPassword, "MySecurePassword123!");
  
  if (result == PasswordVerificationResult.Success)
  {
      Console.WriteLine("Password is correct!");
  }
