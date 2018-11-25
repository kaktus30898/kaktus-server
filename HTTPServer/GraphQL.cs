using System.Threading.Tasks;
using GraphQL;
using HTTPServer.Model;

namespace HTTPServer
{
    /**
     * Класс, описывающий тип данных Query из GraphQL
     */
    public class Query
    {
        private readonly BudgetChangeManager changes;

        public Query(BudgetChangeManager changes)
        {
            this.changes = changes;
        }

        // Привязка метода к свойству changes из GraphQL
        [GraphQLMetadata("changes")]
        public BudgetChangeManager GetChanges()
        {
            return changes;
        }
    }
    
    /**
     * Класс, описывающий тип данных Mutation из GraphQL
     */
    public class Mutation
    {
        private readonly BudgetChangeManager changes;

        public Mutation(BudgetChangeManager changes)
        {
            this.changes = changes;
        }

        // Привязка метода к функции addChange из GraphQL
        [GraphQLMetadata("addChange")]
        public Task<int> AddChange(NewBudgetChange change)
        {
            return this.changes.AddChange(change);
        }
    }
}