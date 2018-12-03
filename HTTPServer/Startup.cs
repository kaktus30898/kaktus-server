using GraphiQl;
using GraphQL.Server;
using GraphQL.Types;
using HTTPServer.Model;
using HTTPServer.Util;
using LinqToDB.DataProvider.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace HTTPServer
{
    /**
     * Класс, осуществляющий конфигурацию программы.
     */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Добавляем сервис GraphQL
            services.AddGraphQL(options => { options.ExposeExceptions = true; })
                .AddDataLoader();

            // Добавляет сервис базы данных
            services.AddSingleton<Database>(_ => new Database(
                    new MySqlDataProvider(),
                    new MySqlConnection(@"Database=kaktustest;Data source=localhost;User Id=mysql;Password=mysql;")
            ));
            
            // Описываем схему данных GraphQL
            const string schema = @"
                type BudgetChange {
                    id: Int!
                    caption: String!
                    delta: Float!
                }

                type BudgetChangeManager {
                    list: [BudgetChange!]!
                    rest: Float!
                }

                type Query {
                    changes: BudgetChangeManager!
                }

                input BudgetChangeBody {
                    caption: String!
                    delta: Float!
                }

                type Mutation {
                    addChange(change: BudgetChangeBody!): Int!
                    saveChange(id: Int!, change: BudgetChangeBody!): Int!
                    deleteChange(id: Int!): Int!
                }
            ";
            
            // Описываем схему GraphQL на уровне типов данных
            services.AddScoped<ISchema>(provider => Schema.For(schema, builder =>
            {
                // Описываем привязку типов из схемы к типам C#
                builder.Types.Include<Query>();
                builder.Types.Include<Mutation>();
                builder.Types.Include<BudgetChange>();
                builder.Types.Include<BudgetChangeManager>();
//                builder.Types.Include<BudgetChangeBody>();
                builder.Types.Include<BudgetChangeManager>("MutationBudgetChangeManager");
                
                // Предоставляем доступ GraphQL к контейнеру зависимостей приложения
                builder.DependencyResolver = new Resolver(provider);
            }));
            // Добавляем описанные в схеме сервисы к приложению
            services.AddScoped<BudgetChangeManager>();
            services.AddScoped<Query>();
            services.AddScoped<Mutation>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHostFiltering();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Конфигурируем сервер GrapgQL
            app.UseWebSockets();
            app.UseGraphQL<ISchema>("/graphql");
            app.UseGraphiQl("/graphiql", "/graphql");
        }
    }
}