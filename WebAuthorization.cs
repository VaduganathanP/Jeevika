using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jeevika
{
    public class WebAuthorization : Base
    {
        internal void Create(string applicationPath, Application application)
        {
            CreateApplicationUserManager(applicationPath, application);
            CreateApplicationSignInManager(applicationPath, application);
            CreateEmailService(applicationPath, application);
            CreateSmsService(applicationPath, application);
            CreateStartup_Auth(applicationPath, application);
            CreateStartup_cs(applicationPath, application);

            //if (!Directory.Exists(Path.Combine(applicationPath, "bin", "App_Data")))
            //    Directory.CreateDirectory(Path.Combine(applicationPath, "bin", "App_Data"));
        }
        internal void CreateLoginModule(string applicationPath, Application application)
        {
            CreateLoginViewModel(applicationPath, application);
            CreateLoginController(applicationPath, application);
            CreateLoginView(applicationPath, application);
        }

        internal void CreateRegisterModule(string applicationPath, Application application)
        {
            CreateRegisterViewModel(applicationPath, application);
            CreateRegisterController(applicationPath, application);
            CreateRegisterView(applicationPath, application);
        }

        internal void CreateAccountModule(string applicationPath, Application application)
        {
            //CreateAccountViewModel(applicationPath, application);
            CreateAccountController(applicationPath, application);
            //CreateAccountView(applicationPath, application);
        }


        private void CreateApplicationUserManager(string applicationPath, Application application)
        {
            var appStartDirectory = Path.Combine(applicationPath, "App_Start");
            base.CreateDirectory(appStartDirectory);
            var applicationUserManagerFilePath = Path.Combine(appStartDirectory, "ApplicationUserManager.cs");
            StreamWriter f = base.CreateFile(applicationUserManagerFilePath);

            f.WriteLine("using {0}.Models;", application.Name);
            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("using Microsoft.AspNet.Identity.EntityFramework;", "");
            f.WriteLine("using Microsoft.AspNet.Identity.Owin;", "");
            f.WriteLine("using Microsoft.Owin;", "");
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class ApplicationUserManager : UserManager<ApplicationUser>", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public ApplicationUserManager(IUserStore<ApplicationUser> store)", "");
            f.WriteLine("            : base(store)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));", "");
            f.WriteLine("            // Configure validation logic for usernames", "");
            f.WriteLine("            manager.UserValidator = new UserValidator<ApplicationUser>(manager)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                AllowOnlyAlphanumericUserNames = false,", "");
            f.WriteLine("                RequireUniqueEmail = true", "");
            f.WriteLine("            }};", "");
            f.WriteLine("", "");
            f.WriteLine("            // Configure validation logic for passwords", "");
            f.WriteLine("            manager.PasswordValidator = new PasswordValidator", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                RequiredLength = 6,", "");
            f.WriteLine("                RequireNonLetterOrDigit = true,", "");
            f.WriteLine("                RequireDigit = true,", "");
            f.WriteLine("                RequireLowercase = true,", "");
            f.WriteLine("                RequireUppercase = true,", "");
            f.WriteLine("            }};", "");
            f.WriteLine("", "");
            f.WriteLine("            // Configure user lockout defaults", "");
            f.WriteLine("            manager.UserLockoutEnabledByDefault = true;", "");
            f.WriteLine("            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);", "");
            f.WriteLine("            manager.MaxFailedAccessAttemptsBeforeLockout = 5;", "");
            f.WriteLine("", "");
            f.WriteLine("            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user", "");
            f.WriteLine("            // You can write your own provider and plug it in here.", "");
            f.WriteLine("            manager.RegisterTwoFactorProvider(\"Phone Code\", new PhoneNumberTokenProvider<ApplicationUser>", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                MessageFormat = \"Your security code is {{0}}\"", "");
            f.WriteLine("            }});", "");
            f.WriteLine("            manager.RegisterTwoFactorProvider(\"Email Code\", new EmailTokenProvider<ApplicationUser>", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                Subject = \"Security Code\",", "");
            f.WriteLine("                BodyFormat = \"Your security code is {{0}}\"", "");
            f.WriteLine("            }});", "");
            f.WriteLine("            manager.EmailService = new EmailService();", "");
            f.WriteLine("            manager.SmsService = new SmsService();", "");
            f.WriteLine("            var dataProtectionProvider = options.DataProtectionProvider;", "");
            f.WriteLine("            if (dataProtectionProvider != null)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                manager.UserTokenProvider =", "");
            f.WriteLine("                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create(\"ASP.NET Identity\"));", "");
            f.WriteLine("            }}", "");
            f.WriteLine("            return manager;", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("App_Start", "ApplicationUserManager.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateApplicationSignInManager(string applicationPath, Application application)
        {
            var dir = Path.Combine(applicationPath, "App_Start");
            base.CreateDirectory(dir);
            var filePath = Path.Combine(dir, "ApplicationSignInManager.cs");
            StreamWriter f = base.CreateFile(filePath);

            f.WriteLine("using {0}.Models;", application.Name);
            f.WriteLine("using Microsoft.AspNet.Identity.Owin;", "");
            f.WriteLine("using Microsoft.Owin;", "");
            f.WriteLine("using Microsoft.Owin.Security;", "");
            f.WriteLine("using System.Security.Claims;", "");
            f.WriteLine("using System.Threading.Tasks;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)", "");
            f.WriteLine("            : base(userManager, authenticationManager)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("App_Start", "ApplicationSignInManager.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateEmailService(string applicationPath, Application application)
        {
            var dir = Path.Combine(applicationPath, "App_Start");
            base.CreateDirectory(dir);
            var filePath = Path.Combine(dir, "EmailService.cs");
            StreamWriter f = base.CreateFile(filePath);

            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("using System;", "");
            f.WriteLine("using System.Collections.Generic;", "");
            f.WriteLine("using System.Linq;", "");
            f.WriteLine("using System.Net.Mail;", "");
            f.WriteLine("using System.Threading.Tasks;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class EmailService : IIdentityMessageService", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public Task SendAsync(IdentityMessage message)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            SmtpClient client = new SmtpClient();", "");
            f.WriteLine("            var mailMessage = new MailMessage();", "");
            f.WriteLine("            mailMessage.Subject = message.Subject;", "");
            f.WriteLine("            mailMessage.Body = message.Body;", "");
            f.WriteLine("            mailMessage.IsBodyHtml = true;", "");
            f.WriteLine("            mailMessage.To.Add(message.Destination);", "");
            f.WriteLine("            return client.SendMailAsync(mailMessage);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("App_Start", "EmailService.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateSmsService(string applicationPath, Application application)
        {
            var dir = Path.Combine(applicationPath, "App_Start");
            base.CreateDirectory(dir);
            var filePath = Path.Combine(dir, "SmsService.cs");
            StreamWriter f = base.CreateFile(filePath);

            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("using System.Threading.Tasks;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class SmsService : IIdentityMessageService", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public Task SendAsync(IdentityMessage message)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            // Plug in your SMS service here to send a text message.", "");
            f.WriteLine("            return Task.FromResult(0);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("App_Start", "SmsService.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateStartup_Auth(string applicationPath, Application application)
        {
            var dir = Path.Combine(applicationPath, "App_Start");
            base.CreateDirectory(dir);
            var filePath = Path.Combine(dir, "Startup_Auth.cs");
            StreamWriter f = base.CreateFile(filePath);

            f.WriteLine("using {0}.Models;", application.Name);
            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("using Microsoft.AspNet.Identity.Owin;", "");
            f.WriteLine("using Microsoft.Owin;", "");
            f.WriteLine("using Microsoft.Owin.Security.Cookies;", "");
            f.WriteLine("using Owin;", "");
            f.WriteLine("using System;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public partial class Startup", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public void ConfigureAuth(IAppBuilder app)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            app.CreatePerOwinContext(ApplicationDbContext.Create);", "");
            f.WriteLine("            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);", "");
            f.WriteLine("            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);", "");
            f.WriteLine("", "");
            f.WriteLine("            app.UseCookieAuthentication(new CookieAuthenticationOptions", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,", "");
            f.WriteLine("                LoginPath = new PathString(\"/Login\"),", "");
            f.WriteLine("                Provider = new CookieAuthenticationProvider", "");
            f.WriteLine("                {{", "");
            f.WriteLine("                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(", "");
            f.WriteLine("                        validateInterval: TimeSpan.FromMinutes(30),", "");
            f.WriteLine("                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))", "");
            f.WriteLine("                }}", "");
            f.WriteLine("            }});", "");
            f.WriteLine("            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);", "");
            f.WriteLine("", "");
            f.WriteLine("            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));", "");
            f.WriteLine("", "");
            f.WriteLine("            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");


            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("App_Start", "Startup_Auth.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateStartup_cs(string applicationPath, Application application)
        {
            var filePath = Path.Combine(applicationPath, "Startup.cs");
            StreamWriter f = base.CreateFile(filePath);

            f.WriteLine("using Microsoft.Owin;", "");
            f.WriteLine("using Owin;", "");
            f.WriteLine("", "");
            f.WriteLine("[assembly: OwinStartupAttribute(typeof({0}.Startup))]", application.Name);
            f.WriteLine("namespace {0}", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public partial class Startup", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public void Configuration(IAppBuilder app)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            ConfigureAuth(app);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");
            f.WriteLine("", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("Startup.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        #region Login
        private void CreateLoginViewModel(string applicationPath, Application application)
        {
            var viewModelDirectory = Path.Combine(applicationPath, "ViewModels");
            base.CreateDirectory(viewModelDirectory);
            var loginViewModelFilePath = Path.Combine(viewModelDirectory, "LoginViewModel.cs");
            StreamWriter f = base.CreateFile(loginViewModelFilePath);

            f.WriteLine("using System.ComponentModel.DataAnnotations;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.ViewModels", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class LoginViewModel", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        [Required]", "");
            f.WriteLine("        [Display(Name = \"Email\")]", "");
            f.WriteLine("        [EmailAddress]", "");
            f.WriteLine("        public string Email {{ get; set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [Required]", "");
            f.WriteLine("        [DataType(DataType.Password)]", "");
            f.WriteLine("        [Display(Name = \"Password\")]", "");
            f.WriteLine("        public string Password {{ get; set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [Display(Name = \"Remember me?\")]", "");
            f.WriteLine("        public bool RememberMe {{ get; set; }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("ViewModels", "LoginViewModel.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateLoginController(string applicationPath, Application application)
        {
            var controllerDirectory = Path.Combine(applicationPath, "Controllers");
            base.CreateDirectory(controllerDirectory);
            var loginControllerFilePath = Path.Combine(controllerDirectory, "LoginController.cs");
            StreamWriter f = base.CreateFile(loginControllerFilePath);

            f.WriteLine("using {0}.ViewModels;", application.Name);
            f.WriteLine("using Microsoft.AspNet.Identity.Owin;", "");
            f.WriteLine("using System.Threading.Tasks;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Controllers", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class LoginController : Controller", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public ApplicationSignInManager SignInManager", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            get", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        public ApplicationUserManager UserManager", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            get", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        // GET: /Login", "");
            f.WriteLine("        [AllowAnonymous]", "");
            f.WriteLine("        public ActionResult Index(string returnUrl)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            ViewBag.ReturnUrl = returnUrl;", "");
            f.WriteLine("            return View();", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        // POST: /Login", "");
            f.WriteLine("        [HttpPost]", "");
            f.WriteLine("        [AllowAnonymous]", "");
            f.WriteLine("        [ValidateAntiForgeryToken]", "");
            f.WriteLine("        public async Task<ActionResult> Index(LoginViewModel model, string returnUrl)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            if (!ModelState.IsValid)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return View(model);", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);", "");
            f.WriteLine("            switch (result)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                case SignInStatus.Success:", "");
            f.WriteLine("                    return RedirectToLocal(returnUrl);", "");
            f.WriteLine("                case SignInStatus.LockedOut:", "");
            f.WriteLine("                    return View(\"Lockout\");", "");
            f.WriteLine("                case SignInStatus.RequiresVerification:", "");
            f.WriteLine("                    return RedirectToAction(\"SendCode\", new {{ ReturnUrl = returnUrl, RememberMe = model.RememberMe }});", "");
            f.WriteLine("                case SignInStatus.Failure:", "");
            f.WriteLine("                default:", "");
            f.WriteLine("                    ModelState.AddModelError(\"\", \"Invalid login attempt.\");", "");
            f.WriteLine("                    return View(model);", "");
            f.WriteLine("            }}", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        private ActionResult RedirectToLocal(string returnUrl)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            if (Url.IsLocalUrl(returnUrl))", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return Redirect(returnUrl);", "");
            f.WriteLine("            }}", "");
            f.WriteLine("            return RedirectToAction(\"Index\", \"Home\");", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("Controllers", "LoginController.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateLoginView(string applicationPath, Application application)
        {
            var loginViewDirectory = Path.Combine(applicationPath, "Views", "Login");
            base.CreateDirectory(loginViewDirectory);
            var loginIndexViewFilePath = Path.Combine(loginViewDirectory, "Index.cshtml");
            StreamWriter f = base.CreateFile(loginIndexViewFilePath);

            f.WriteLine("@using {0}.ViewModels", application.Name);
            f.WriteLine("@model LoginViewModel", "");
            f.WriteLine("@{{", "");
            f.WriteLine("    Layout = null;", "");
            f.WriteLine("    ViewBag.Title = \"Log in\";", "");
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("<!DOCTYPE html>", "");
            f.WriteLine("<html>", "");
            f.WriteLine("<head>", "");
            f.WriteLine("    <meta charset=\"utf-8\" />", "");
            f.WriteLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">", "");
            f.WriteLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\">", "");
            f.WriteLine("    <meta name=\"description\" content=\"\">", "");
            f.WriteLine("    <meta name=\"author\" content=\"\">", "");
            f.WriteLine("    <title>@ViewBag.Title - {0}</title>", application.DisplayName);
            f.WriteLine("    <link href=\"https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i\" rel=\"stylesheet\">", "");
            f.WriteLine("    @Styles.Render(\"~/Content/css\")", "");
            f.WriteLine("    <link href=\"~/Content/fontawesome-all.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("    <link href=\"~/Content/sb-admin-2.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("</head>", "");
            f.WriteLine("", "");
            f.WriteLine("<body class=\"bg-gradient-primary\">", "");
            f.WriteLine("    <div class=\"container\">", "");
            f.WriteLine("        <div class=\"row justify-content-center\">", "");
            f.WriteLine("            <div class=\"col-xl-6 col-lg-8 col-md-6\">", "");
            f.WriteLine("                <div class=\"card o-hidden border-0 shadow-lg my-5\">", "");
            f.WriteLine("                    <div class=\"card-body p-0\">", "");
            f.WriteLine("                        <div class=\"row\">", "");
            f.WriteLine("                            <div class=\"col-lg-12\">", "");
            f.WriteLine("                                <div class=\"p-5\">", "");
            f.WriteLine("", "");
            f.WriteLine("                                    <div class=\"text-center\">", "");
            f.WriteLine("                                        <img class=\"align-content-center mb-3\" src=\"~/Images/android-chrome-192x192.png\" height=\"80\" />", "");
            f.WriteLine("                                        <h1 class=\"h4 text-gray-900 mb-4\">{0}</h1>", application.DisplayName);
            f.WriteLine("                                        <h3 class=\"text-gray-900 mb-4\">Welcome Back!</h3>", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                    @using (Html.BeginForm(\"Index\", \"Login\", new {{ ReturnUrl = ViewBag.ReturnUrl }}, FormMethod.Post, new {{ @class = \"form-horizontal\", role = \"form\" }}))", "");
            f.WriteLine("                                    {{", "");
            f.WriteLine("                                        @Html.AntiForgeryToken()", "");
            f.WriteLine("                                        <hr />", "");
            f.WriteLine("                                        @Html.ValidationSummary(true, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                        <div class=\"form-group\">", "");
            f.WriteLine("                                            @Html.TextBoxFor(m => m.Email, new {{ @class = \"form-control form-control-user\", @placeholder = \"Enter Email Address...\" }})", "");
            f.WriteLine("                                            @Html.ValidationMessageFor(m => m.Email, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                        </div>", "");
            f.WriteLine("                                        <div class=\"form-group\">", "");
            f.WriteLine("                                            @Html.PasswordFor(m => m.Password, new {{ @class = \"form-control form-control-user\", @placeholder = \"Password\" }})", "");
            f.WriteLine("                                            @Html.ValidationMessageFor(m => m.Password, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                        </div>", "");
            f.WriteLine("                                        <div class=\"form-group\">", "");
            f.WriteLine("                                            <div class=\"custom-control custom-checkbox small\">", "");
            f.WriteLine("                                                @Html.CheckBoxFor(m => m.RememberMe, new {{ @class = \"custom-control-input\" }})", "");
            f.WriteLine("                                                <label class=\"custom-control-label\" for=\"RememberMe\">Remember Me</label>", "");
            f.WriteLine("                                            </div>", "");
            f.WriteLine("                                        </div>", "");
            f.WriteLine("                                        <input type=\"submit\" value=\"Login\" class=\"btn btn-primary btn-user btn-block\" />", "");
            f.WriteLine("                                    }}", "");
            f.WriteLine("                                    <hr>", "");
            f.WriteLine("                                    <div class=\"text-center\">", "");
            f.WriteLine("                                       <a class=\"small\" href=\"@Url.Action(\"Index\",\"ForgotPassword\")\">Forgot Password?</a>", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                    <div class=\"text-center\">", "");
            f.WriteLine("                                       <a class=\"small\" href=\"@Url.Action(\"Index\",\"Register\")\">Register as a new user.</a>", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                </div>", "");
            f.WriteLine("                            </div>", "");
            f.WriteLine("                        </div>", "");
            f.WriteLine("                    </div>", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jquery\")", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/bootstrap\")", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jqueryval\")", "");
            f.WriteLine("    <script src=\"~/Scripts/jquery.easing.min.js\"></script>", "");
            f.WriteLine("    <script src=\"~/Scripts/sb-admin-2.min.js\"></script>", "");
            f.WriteLine("</body>", "");
            f.WriteLine("</html>", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projEightNode = projRootElement.ChildNodes[8];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("Views", "Login", "Index.cshtml"));
            projEightNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        #endregion


        #region Account
        private void CreateAccountViewModel(string applicationPath, Application application)
        {
            var viewModelDirectory = Path.Combine(applicationPath, "ViewModels");
            base.CreateDirectory(viewModelDirectory);
            var loginViewModelFilePath = Path.Combine(viewModelDirectory, "LoginViewModel.cs");
            StreamWriter f = base.CreateFile(loginViewModelFilePath);

            f.WriteLine("using System.ComponentModel.DataAnnotations;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.ViewModels", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class LoginViewModel", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        [Required]", "");
            f.WriteLine("        [Display(Name = \"Email\")]", "");
            f.WriteLine("        [EmailAddress]", "");
            f.WriteLine("        public string Email {{ get; set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [Required]", "");
            f.WriteLine("        [DataType(DataType.Password)]", "");
            f.WriteLine("        [Display(Name = \"Password\")]", "");
            f.WriteLine("        public string Password {{ get; set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [Display(Name = \"Remember me?\")]", "");
            f.WriteLine("        public bool RememberMe {{ get; set; }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("ViewModels", "LoginViewModel.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateAccountController(string applicationPath, Application application)
        {
            var controllerDirectory = Path.Combine(applicationPath, "Controllers");
            base.CreateDirectory(controllerDirectory);
            var loginControllerFilePath = Path.Combine(controllerDirectory, "AccountController.cs");
            StreamWriter f = base.CreateFile(loginControllerFilePath);

            f.WriteLine("using {0}.ViewModels;", application.Name);
            f.WriteLine("using Microsoft.AspNet.Identity.Owin;", "");
            f.WriteLine("using System.Threading.Tasks;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("using Microsoft.Owin.Security;", "");
            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Controllers", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class AccountController : BaseController", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public ApplicationSignInManager SignInManager", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            get", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        public ApplicationUserManager UserManager", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            get", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        private IAuthenticationManager AuthenticationManager", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            get", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return HttpContext.GetOwinContext().Authentication;", "");
            f.WriteLine("            }}", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        // POST: /Account/LogOff", "");
            f.WriteLine("        [HttpPost]", "");
            f.WriteLine("        [ValidateAntiForgeryToken]", "");
            f.WriteLine("        public ActionResult LogOff()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);", "");
            f.WriteLine("            return RedirectToAction(\"Index\", \"Home\");", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("Controllers", "AccountController.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateAccountView(string applicationPath, Application application)
        {
            var loginViewDirectory = Path.Combine(applicationPath, "Views", "Login");
            base.CreateDirectory(loginViewDirectory);
            var loginIndexViewFilePath = Path.Combine(loginViewDirectory, "Index.cshtml");
            StreamWriter f = base.CreateFile(loginIndexViewFilePath);

            f.WriteLine("@using {0}.ViewModels", application.Name);
            f.WriteLine("@model LoginViewModel", "");
            f.WriteLine("@{{", "");
            f.WriteLine("    Layout = null;", "");
            f.WriteLine("    ViewBag.Title = \"Log in\";", "");
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("<!DOCTYPE html>", "");
            f.WriteLine("<html>", "");
            f.WriteLine("<head>", "");
            f.WriteLine("    <meta charset=\"utf-8\" />", "");
            f.WriteLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">", "");
            f.WriteLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\">", "");
            f.WriteLine("    <meta name=\"description\" content=\"\">", "");
            f.WriteLine("    <meta name=\"author\" content=\"\">", "");
            f.WriteLine("    <title>@ViewBag.Title - {0}</title>", application.DisplayName);
            f.WriteLine("    <link href=\"https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i\" rel=\"stylesheet\">", "");
            f.WriteLine("    @Styles.Render(\"~/Content/css\")", "");
            f.WriteLine("    <link href=\"~/Content/fontawesome-all.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("    <link href=\"~/Content/sb-admin-2.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("", "");
            f.WriteLine("", "");
            f.WriteLine("</head>", "");
            f.WriteLine("", "");
            f.WriteLine("<body class=\"bg-gradient-primary\">", "");
            f.WriteLine("    <div class=\"container\">", "");
            f.WriteLine("        <div class=\"row justify-content-center\">", "");
            f.WriteLine("            <div class=\"col-xl-6 col-lg-8 col-md-6\">", "");
            f.WriteLine("                <div class=\"card o-hidden border-0 shadow-lg my-5\">", "");
            f.WriteLine("                    <div class=\"card-body p-0\">", "");
            f.WriteLine("                        <div class=\"row\">", "");
            f.WriteLine("                            <div class=\"col-lg-12\">", "");
            f.WriteLine("                                <div class=\"p-5\">", "");
            f.WriteLine("", "");
            f.WriteLine("                                    <div class=\"text-center\">", "");
            f.WriteLine("                                        <img class=\"align-content-center mb-3\" src=\"~/Images/android-chrome-192x192.png\" height=\"80\" />", "");
            f.WriteLine("                                        <h1 class=\"h4 text-gray-900 mb-4\">{0}</h1>", application.DisplayName);
            f.WriteLine("                                        <h3 class=\"text-gray-900 mb-4\">Welcome Back!</h3>", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                    @using (Html.BeginForm(\"Index\", \"Login\", new {{ ReturnUrl = ViewBag.ReturnUrl }}, FormMethod.Post, new {{ @class = \"form-horizontal\", role = \"form\" }}))", "");
            f.WriteLine("                                    {{", "");
            f.WriteLine("                                        @Html.AntiForgeryToken()", "");
            f.WriteLine("                                        <hr />", "");
            f.WriteLine("                                        @Html.ValidationSummary(true, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                        <div class=\"form-group\">", "");
            f.WriteLine("                                            @Html.TextBoxFor(m => m.Email, new {{ @class = \"form-control form-control-user\", @placeholder = \"Enter Email Address...\" }})", "");
            f.WriteLine("                                            @Html.ValidationMessageFor(m => m.Email, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                        </div>", "");
            f.WriteLine("                                        <div class=\"form-group\">", "");
            f.WriteLine("                                            @Html.PasswordFor(m => m.Password, new {{ @class = \"form-control form-control-user\", @placeholder = \"Password\" }})", "");
            f.WriteLine("                                            @Html.ValidationMessageFor(m => m.Password, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                        </div>", "");
            f.WriteLine("                                        <div class=\"form-group\">", "");
            f.WriteLine("                                            <div class=\"custom-control custom-checkbox small\">", "");
            f.WriteLine("                                                @Html.CheckBoxFor(m => m.RememberMe, new {{ @class = \"custom-control-input\" }})", "");
            f.WriteLine("                                                <label class=\"custom-control-label\" for=\"RememberMe\">Remember Me</label>", "");
            f.WriteLine("                                            </div>", "");
            f.WriteLine("                                        </div>", "");
            f.WriteLine("                                        <input type=\"submit\" value=\"Login\" class=\"btn btn-primary btn-user btn-block\" />", "");
            f.WriteLine("                                    }}", "");
            f.WriteLine("                                    <hr>", "");
            f.WriteLine("                                    <div class=\"text-center\">", "");
            f.WriteLine("                                       <a class=\"small\" href=\"@Url.Action(\"Index\",\"ForgotPassword\")\">Forgot Password?</a>", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                    <div class=\"text-center\">", "");
            f.WriteLine("                                       <a class=\"small\" href=\"@Url.Action(\"Index\",\"Register\")\">Register as a new user.</a>", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                </div>", "");
            f.WriteLine("                            </div>", "");
            f.WriteLine("                        </div>", "");
            f.WriteLine("                    </div>", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jquery\")", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/bootstrap\")", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jqueryval\")", "");
            f.WriteLine("    <script src=\"~/Scripts/jquery.easing.min.js\"></script>", "");
            f.WriteLine("    <script src=\"~/Scripts/sb-admin-2.min.js\"></script>", "");
            f.WriteLine("</body>", "");
            f.WriteLine("</html>", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projEightNode = projRootElement.ChildNodes[8];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("Views", "Login", "Index.cshtml"));
            projEightNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        #endregion

        #region Register
        private void CreateRegisterViewModel(string applicationPath, Application application)
        {
            var viewModelDirectory = Path.Combine(applicationPath, "ViewModels");
            base.CreateDirectory(viewModelDirectory);
            var viewModelFilePath = Path.Combine(viewModelDirectory, "RegisterViewModel.cs");
            StreamWriter f = base.CreateFile(viewModelFilePath);

            f.WriteLine("using System.ComponentModel.DataAnnotations;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.ViewModels", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class RegisterViewModel", "");
            f.WriteLine("    {{", "");
            f.WriteLine("		 [Required]", "");
            f.WriteLine("        [StringLength(100, ErrorMessage = \"The {{0}} must be at least {{2}} characters long.\", MinimumLength = 1)]", "");
            f.WriteLine("        [DataType(DataType.Password)]", "");
            f.WriteLine("        [Display(Name = \"First Name\")]", "");
            f.WriteLine("        public string FirstName {{ get; set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [Required]", "");
            f.WriteLine("        [StringLength(100, ErrorMessage = \"The {{0}} must be at least {{2}} characters long.\", MinimumLength = 1)]", "");
            f.WriteLine("        [Display(Name = \"Last Name\")]", "");
            f.WriteLine("        public string LastName {{ get; set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [Required]", "");
            f.WriteLine("        [EmailAddress]", "");
            f.WriteLine("        [Display(Name = \"Email\")]", "");
            f.WriteLine("        public string Email {{ get; set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [Required]", "");
            f.WriteLine("        [StringLength(100, ErrorMessage = \"The {{0}} must be at least {{2}} characters long.\", MinimumLength = 6)]", "");
            f.WriteLine("        [DataType(DataType.Password)]", "");
            f.WriteLine("        [Display(Name = \"Password\")]", "");
            f.WriteLine("        public string Password {{ get; set; }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [DataType(DataType.Password)]", "");
            f.WriteLine("        [Display(Name = \"Confirm password\")]", "");
            f.WriteLine("        [Compare(\"Password\", ErrorMessage = \"The password and confirmation password do not match.\")]", "");
            f.WriteLine("        public string ConfirmPassword {{ get; set; }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");


            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("ViewModels", "RegisterViewModel.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateRegisterController(string applicationPath, Application application)
        {
            var controllerDirectory = Path.Combine(applicationPath, "Controllers");
            base.CreateDirectory(controllerDirectory);
            var controllerFilePath = Path.Combine(controllerDirectory, "RegisterController.cs");
            StreamWriter f = base.CreateFile(controllerFilePath);

            f.WriteLine("using {0}.Models;", application.Name);
            f.WriteLine("using {0}.ViewModels;", application.Name);
            f.WriteLine("using Microsoft.AspNet.Identity;", "");
            f.WriteLine("using Microsoft.AspNet.Identity.Owin;", "");
            f.WriteLine("using System.Threading.Tasks;", "");
            f.WriteLine("using System.Web;", "");
            f.WriteLine("using System.Web.Mvc;", "");
            f.WriteLine("", "");
            f.WriteLine("namespace {0}.Controllers", application.Name);
            f.WriteLine("{{", "");
            f.WriteLine("    public class RegisterController : BaseController", "");
            f.WriteLine("    {{", "");
            f.WriteLine("        public ApplicationSignInManager SignInManager", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            get", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        public ApplicationUserManager UserManager", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            get", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();", "");
            f.WriteLine("            }}", "");
            f.WriteLine("", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [AllowAnonymous]", "");
            f.WriteLine("        public ActionResult Index()", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            return View();", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        [HttpPost]", "");
            f.WriteLine("        [AllowAnonymous]", "");
            f.WriteLine("        [ValidateAntiForgeryToken]", "");
            f.WriteLine("        public async Task<ActionResult> Index(RegisterViewModel model)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            if (ModelState.IsValid)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                var user = new ApplicationUser {{ UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName }};", "");
            f.WriteLine("                var result = await UserManager.CreateAsync(user, model.Password);", "");
            f.WriteLine("                if (result.Succeeded)", "");
            f.WriteLine("                {{", "");
            f.WriteLine("                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);", "");
            //f.WriteLine("                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);", "");
            //f.WriteLine("                    var callbackUrl = Url.Action(\"ConfirmEmail\", \"Account\", new {{ userId = user.Id, code = code }}, protocol: Request.Url.Scheme);", "");
            //f.WriteLine("                    var body = \"<!DOCTYPE html><html><head> <meta charset=\\\"utf-8\\\"> <meta http-equiv=\\\"x-ua-compatible\\\" content=\\\"ie=edge\\\"> <title>Email Confirmation</title> <meta name=\\\"viewport\\\" content=\\\"width=device-width, initial-scale=1\\\"> <style type=\\\"text/css\\\"> /** * Google webfonts. Recommended to include the .woff version for cross-client compatibility. */ @media screen {{{{ @font-face {{{{ font-family: 'Source Sans Pro'; font-style: normal; font-weight: 400; src: local('Source Sans Pro Regular'), local('SourceSansPro-Regular'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/ODelI1aHBYDBqgeIAH2zlBM0YzuT7MdOe03otPbuUS0.woff) format('woff'); }}}} @font-face {{{{ font-family: 'Source Sans Pro'; font-style: normal; font-weight: 700; src: local('Source Sans Pro Bold'), local('SourceSansPro-Bold'), url(https://fonts.gstatic.com/s/sourcesanspro/v10/toadOcfmlt9b38dHJxOBGFkQc6VGVFSmCnC_l7QZG60.woff) format('woff'); }}}} }}}} /** * Avoid browser level font resizing. * 1. Windows Mobile * 2. iOS / OSX */ body, table, td, a {{{{ -ms-text-size-adjust: 100%; /* 1 */ -webkit-text-size-adjust: 100%; /* 2 */ }}}} /** * Remove extra space added to tables and cells in Outlook. */ table, td {{{{ mso-table-rspace: 0pt; mso-table-lspace: 0pt; }}}} /** * Better fluid images in Internet Explorer. */ img {{{{ -ms-interpolation-mode: bicubic; }}}} /** * Remove blue links for iOS devices. */ a[x-apple-data-detectors] {{{{ font-family: inherit !important; font-size: inherit !important; font-weight: inherit !important; line-height: inherit !important; color: inherit !important; text-decoration: none !important; }}}} /** * Fix centering issues in Android 4.4. */ div[style*=\\\"margin: 16px 0;\\\"] {{{{ margin: 0 !important; }}}} body {{{{ width: 100% !important; height: 100% !important; padding: 0 !important; margin: 0 !important; }}}} /** * Collapse table borders to avoid space between cells. */ table {{{{ border-collapse: collapse !important; }}}} a {{{{ color: #1a82e2; }}}} img {{{{ height: auto; line-height: 100%; text-decoration: none; border: 0; outline: none; }}}} </style></head><body style=\\\"background-color: #e9ecef;\\\"> <div class=\\\"preheader\\\" style=\\\"display: none; max-width: 0; max-height: 0; overflow: hidden; font-size: 1px; line-height: 1px; color: #fff; opacity: 0;\\\"> Confirm your email address. </div> <table border=\\\"0\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\" width=\\\"100%\\\"> <tr> <td align=\\\"center\\\" bgcolor=\\\"#e9ecef\\\"> <table border=\\\"0\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\" width=\\\"100%\\\" style=\\\"max-width: 600px;\\\"> <tr> <td align=\\\"left\\\" bgcolor=\\\"#ffffff\\\" style=\\\"padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;\\\"> <h1 style=\\\"margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;\\\">Confirm Your Email Address</h1> </td> </tr> </table> </td> </tr> <tr> <td align=\\\"center\\\" bgcolor=\\\"#e9ecef\\\"> <table border=\\\"0\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\" width=\\\"100%\\\" style=\\\"max-width: 600px;\\\"><tr> <td align=\\\"left\\\" bgcolor=\\\"#ffffff\\\" style=\\\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\\\"> <p style=\\\"margin: 0;\\\">Tap the button below to confirm your email address. If you didn't create an account with <b>###AppName###</b>, you can safely delete this email.</p> </td> </tr> <tr> <td align=\\\"left\\\" bgcolor=\\\"#ffffff\\\"> <table border=\\\"0\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\" width=\\\"100%\\\"> <tr> <td align=\\\"center\\\" bgcolor=\\\"#ffffff\\\" style=\\\"padding: 12px;\\\"> <table border=\\\"0\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\"> <tr> <td align=\\\"center\\\" bgcolor=\\\"#1a82e2\\\" style=\\\"border-radius: 6px;\\\"> <a href=\\\"###ConfirmationLink###\\\" target=\\\"_blank\\\" style=\\\"display: inline-block; padding: 16px 36px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;\\\">Confirm</a> </td> </tr> </table> </td> </tr> </table> </td> </tr> <tr> <td align=\\\"left\\\" bgcolor=\\\"#ffffff\\\" style=\\\"padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;\\\"> <p style=\\\"margin: 0;\\\">If that doesn't work, copy and paste the following link in your browser:</p> <p style=\\\"margin: 0;\\\"><a href=\\\"###ConfirmationLink###\\\" target=\\\"_blank\\\">###ConfirmationLink###</a></p> </td> </tr> </table> </td> </tr> </table></body></html>\";", "");
            //f.WriteLine("                    body = body.Replace(\"###ConfirmationLink###\", callbackUrl);", "");
            //f.WriteLine("                    body = body.Replace(\"###AppName###\", \"{0}\");", application.DisplayName);
            //f.WriteLine("                    await UserManager.SendEmailAsync(user.Id, \"Confirm your account\", body);", "");
            f.WriteLine("                    return RedirectToAction(\"Index\", \"Home\");", "");
            f.WriteLine("                }}", "");
            f.WriteLine("                AddErrors(result);", "");
            f.WriteLine("            }}", "");
            f.WriteLine("            return View(model);", "");
            f.WriteLine("        }}", "");
            f.WriteLine("", "");
            f.WriteLine("        private void AddErrors(IdentityResult result)", "");
            f.WriteLine("        {{", "");
            f.WriteLine("            foreach (var error in result.Errors)", "");
            f.WriteLine("            {{", "");
            f.WriteLine("                ModelState.AddModelError(\"\", error);", "");
            f.WriteLine("            }}", "");
            f.WriteLine("        }}", "");
            f.WriteLine("    }}", "");
            f.WriteLine("}}", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projSeventhNode = projRootElement.ChildNodes[7];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Compile", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("Controllers", "RegisterController.cs"));
            projSeventhNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        private void CreateRegisterView(string applicationPath, Application application)
        {
            var loginViewDirectory = Path.Combine(applicationPath, "Views", "Register");
            base.CreateDirectory(loginViewDirectory);
            var loginIndexViewFilePath = Path.Combine(loginViewDirectory, "Index.cshtml");
            StreamWriter f = base.CreateFile(loginIndexViewFilePath);

            f.WriteLine("@using {0}.ViewModels", application.Name);
            f.WriteLine("@model RegisterViewModel", "");
            f.WriteLine("@{{", "");
            f.WriteLine("    Layout = null;", "");
            f.WriteLine("    ViewBag.Title = \"Register\";", "");
            f.WriteLine("}}", "");
            f.WriteLine("", "");
            f.WriteLine("<!DOCTYPE html>", "");
            f.WriteLine("<html>", "");
            f.WriteLine("<head>", "");
            f.WriteLine("    <meta charset=\"utf-8\" />", "");
            f.WriteLine("    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">", "");
            f.WriteLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, shrink-to-fit=no\">", "");
            f.WriteLine("    <meta name=\"description\" content=\"\">", "");
            f.WriteLine("    <meta name=\"author\" content=\"\">", "");
            f.WriteLine("    <title>@ViewBag.Title - {0}</title>", application.DisplayName);
            f.WriteLine("    <link href=\"https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i\" rel=\"stylesheet\">", "");
            f.WriteLine("    @Styles.Render(\"~/Content/css\")", "");
            f.WriteLine("    <link href=\"~/Content/fontawesome-all.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("    <link href=\"~/Content/sb-admin-2.min.css\" rel=\"stylesheet\" />", "");
            f.WriteLine("</head>", "");
            f.WriteLine("<body class=\"bg-gradient-primary\">", "");
            f.WriteLine("    <div class=\"container\">", "");
            f.WriteLine("        <div class=\"card o-hidden border-0 shadow-lg my-5\">", "");
            f.WriteLine("            <div class=\"card-body p-0\">", "");
            f.WriteLine("                <div class=\"row\">", "");
            f.WriteLine("                    <div class=\"col-lg-12\">", "");
            f.WriteLine("                        <div class=\"p-5\">", "");
            f.WriteLine("                            <div class=\"text-center\">", "");
            f.WriteLine("                                <img class=\"align-content-center mb-3\" src=\"~/Images/android-chrome-192x192.png\" height=\"80\" />", "");
            f.WriteLine("                                <h1 class=\"h4 text-gray-900 mb-4\">{0}</h1>", application.DisplayName);
            f.WriteLine("                                <h3 class=\"text-gray-900 mb-4\">Create an Account!</h3>", "");
            f.WriteLine("                            </div>", "");
            f.WriteLine("                            @using (Html.BeginForm(\"Index\", \"Register\", new {{ ReturnUrl = ViewBag.ReturnUrl }}, FormMethod.Post, new {{ @class = \"form-horizontal\", role = \"form\" }}))", "");
            f.WriteLine("                            {{", "");
            f.WriteLine("                                @Html.AntiForgeryToken()", "");
            f.WriteLine("                                <hr />", "");
            f.WriteLine("                                @Html.ValidationSummary(true, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                <div class=\"form-group row\">", "");
            f.WriteLine("                                    <div class=\"col-sm-6 mb-3 mb-sm-0\">", "");
            f.WriteLine("                                        @Html.TextBoxFor(m => m.FirstName, new {{ @class = \"form-control form-control-user\", @placeholder = \"First Name\" }})", "");
            f.WriteLine("                                        @Html.ValidationMessageFor(m => m.FirstName, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                    <div class=\"col-sm-6\">", "");
            f.WriteLine("                                        @Html.TextBoxFor(m => m.LastName, new {{ @class = \"form-control form-control-user\", @placeholder = \"Last Name\" }})", "");
            f.WriteLine("                                        @Html.ValidationMessageFor(m => m.LastName, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                </div>", "");
            f.WriteLine("                                <div class=\"form-group\">", "");
            f.WriteLine("                                    @Html.TextBoxFor(m => m.Email, new {{ @class = \"form-control form-control-user\", @placeholder = \"Email Address\" }})", "");
            f.WriteLine("                                    @Html.ValidationMessageFor(m => m.Email, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                </div>", "");
            f.WriteLine("                                <div class=\"form-group row\">", "");
            f.WriteLine("                                    <div class=\"col-sm-6 mb-3 mb-sm-0\">", "");
            f.WriteLine("                                        @Html.PasswordFor(m => m.Password, new {{ @class = \"form-control form-control-user\", @placeholder = \"Password\" }})", "");
            f.WriteLine("                                        @Html.ValidationMessageFor(m => m.Password, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                    <div class=\"col-sm-6\">", "");
            f.WriteLine("                                        @Html.PasswordFor(m => m.ConfirmPassword, new {{ @class = \"form-control form-control-user\", @placeholder = \"Repeat Password\" }})", "");
            f.WriteLine("                                        @Html.ValidationMessageFor(m => m.ConfirmPassword, \"\", new {{ @class = \"text-danger\" }})", "");
            f.WriteLine("                                    </div>", "");
            f.WriteLine("                                </div>", "");
            f.WriteLine("                                <input type=\"submit\" value=\"Register Account\" class=\"btn btn-primary btn-user btn-block\" />", "");
            f.WriteLine("                            }}", "");
            f.WriteLine("                            <hr>", "");
            f.WriteLine("                            <div class=\"text-center\">", "");
            f.WriteLine("                               <a class=\"small\" href=\"@Url.Action(\"Index\",\"ForgotPassword\")\">Forgot Password?</a>", "");
            f.WriteLine("                            </div>", "");
            f.WriteLine("                            <div class=\"text-center\">", "");
            f.WriteLine("                               <a class=\"small\" href=\"@Url.Action(\"Index\",\"Login\")\">Already have an account? Login!</a>", "");
            f.WriteLine("                            </div>", "");
            f.WriteLine("                        </div>", "");
            f.WriteLine("                    </div>", "");
            f.WriteLine("                </div>", "");
            f.WriteLine("            </div>", "");
            f.WriteLine("        </div>", "");
            f.WriteLine("    </div>", "");
            f.WriteLine("", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jquery\")", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/bootstrap\")", "");
            f.WriteLine("    @Scripts.Render(\"~/bundles/jqueryval\")", "");
            f.WriteLine("    <script src=\"~/Scripts/jquery.easing.min.js\"></script>", "");
            f.WriteLine("    <script src=\"~/Scripts/sb-admin-2.min.js\"></script>", "");
            f.WriteLine("</body>", "");
            f.WriteLine("</html>", "");

            XmlDocument projectFile = new XmlDocument();
            projectFile.Load(Path.Combine(applicationPath, application.Name + ".csproj"));
            XmlNode projRootElement = projectFile.DocumentElement;
            XmlNode projEightNode = projRootElement.ChildNodes[8];

            XmlElement xmlBaseModelElement = projectFile.CreateElement("Content", projectFile.DocumentElement.NamespaceURI);
            xmlBaseModelElement.SetAttribute("Include", Path.Combine("Views", "Register", "Index.cshtml"));
            projEightNode.AppendChild(xmlBaseModelElement);
            projectFile.Save(Path.Combine(applicationPath, application.Name + ".csproj"));

            base.Dispose();
        }

        #endregion
    }
}
