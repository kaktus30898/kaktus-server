using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using Ninject;
using Ninject.Modules;

namespace DataServer
{
    internal class Greeter
    {
        [GraphQLMetadata("forMe")]
        public string ForMe(string name)
        {
            return $"Hello, {name}!";
        }
    }

    internal class Query
    {
        [Inject]
        private readonly Greeter greeter;

        public Query(Greeter greeter)
        {
            this.greeter = greeter;
        }

        [GraphQLMetadata("hello")]
        public string GetHello()
        {
            return "Hello World!";
        }

        [GraphQLMetadata("greeter")]
        public Greeter GetGreeter()
        {
            return greeter;
        }
    }

    internal class Container : NinjectModule
    {
        public override void Load()
        {
        }
    }

    internal static class Program
    {
        public static void Main(string[] args)
        {
            var schema = Schema.For(@"
                type Greeter {
                    forMe(name: String!): String!
                }

                type Query {
                    hello: String!
                    greeter: Greeter!
                }
            ", _ =>
            {
                _.Types.Include<Greeter>();
                _.Types.Include<Query>();
            });

            var kernel = new StandardKernel(new Container());

            var json = schema.Execute(_ =>
            {
                _.Query = @"query Mix($name: String!) {
                    hello,
                    greeter {
                        forMe(name: $name)
                    }
                }";
                _.Inputs = new Inputs(new Dictionary<string, object> {{"name", "John"}});
                _.Root = kernel.Get<Query>();
            });

            Console.WriteLine(json);
        }
    }
}