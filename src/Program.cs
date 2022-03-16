using MailService.Services;
using MailService.Controller;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Select a valid endpoint, to see the endpoints check: {value}");

app.MapPost("/receive", (User user) => {
    var mail = new Mail();
    mail.SetInboxConf(builder.Configuration.GetValue<string>("mailuser"),
     builder.Configuration.GetValue<string>("mailpassword"),
     builder.Configuration.GetValue<string>("hostimap"),
     builder.Configuration.GetValue<int>("imapport"));
    if(user.user == builder.Configuration.GetValue<string>("mailuser") && user.password == builder.Configuration.GetValue<string>("mailpassword"))
    {
        return mail.ReadMail();
    }else
    {
        return "Invalid endpoint!";
    }
});

app.MapPost("/send", (Destinatary destinatary) => {
    try
    {
        var mail = new Mail();
        mail.SetMailConf(builder.Configuration.GetValue<string>("hostsmtp"), builder.Configuration.GetValue<int>("portsmtp"), $"Olá {destinatary.destinatary.Split('@')[0]}", "<div><nav style='background-color: #1F1E27;'></nav><p>Olá, Recebemos a sua menssagem, retornaremos ela em breve</p></div>", builder.Configuration.GetValue<string>("mailuser"), builder.Configuration.GetValue<string>("mailpassword"));
        mail.SendMail(true, "UTF-8", $"{destinatary.destinatary}");
    }catch(Exception e)
    {
        Log.AppendLog($" \n ! An error has occured:\n\n {e} ! \n ");
    }
});

app.Run();

record Destinatary(string destinatary);

record User(string user, string password);