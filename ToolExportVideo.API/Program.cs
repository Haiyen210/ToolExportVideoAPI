using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ToolExportVideo.API;
using ToolExportVideo.Common;
using ToolExportVideo.Library;

var builder = WebApplication.CreateBuilder(args);

var AllowDomainPolicy = "AllowDomainPolicy";

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowDomainPolicy,
                      policy =>
                      {
                          policy.WithOrigins("*");
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                      });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(opt =>
{
    opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    DateTimeZoneHandling = DateTimeZoneHandling.Local
};
// Build a config object, using env vars and JSON providers.
IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
//builder.Services.AddControllers().AddJsonOptions(oprion => { oprion.JsonSerializerOptions.});
builder.Services.AddControllersWithViews().
                AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Constant.SecretSercurityKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
builder.Services.AddAuthorization();

//builder.Services.AddScoped<BLMember>();
//builder.Services.AddScoped<BLEmployee>();
//builder.Services.AddScoped<BLActiveCode>();
//builder.Services.AddScoped<BLCard>();
//builder.Services.AddScoped<BLPayReward>();
//builder.Services.AddScoped<BLPointTransfer>();
//builder.Services.AddScoped<BLRewardHistory>();

var app = builder.Build();
app.UseCors(AllowDomainPolicy);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

ConfigUtil.SetConfigGlobal(config);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<HandleMiddleware>();

app.Run();
