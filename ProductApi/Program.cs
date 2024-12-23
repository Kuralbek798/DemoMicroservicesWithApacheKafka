using Confluent.Kafka;
using ProductApi.ProductServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Создает объект конфигурации для Kafka продюсера с указанным bootstrap сервером.
// Это необходимо для указания адреса Kafka брокера, к которому будет подключаться продюсер.
// Соединение с Kafka брокером необходимо для того, чтобы отправлять сообщения в Kafka топики.
// В данном случае, Kafka продюсер используется для отправки сообщений о добавлении продуктов в топик "add-product".
// Это позволяет реализовать асинхронную обработку данных и интеграцию с другими системами, которые могут подписываться на эти сообщения и обрабатывать их.
var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
// Регистрирует singleton сервис для Kafka продюсера, используя конфигурацию.
// Это гарантирует, что один и тот же экземпляр продюсера будет использоваться на протяжении всего времени работы приложения.
builder.Services.AddSingleton<IProducer<Null, string>>(str => new ProducerBuilder<Null, string>(config).Build());

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
