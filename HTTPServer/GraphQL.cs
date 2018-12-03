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
        public Task<int> AddChange(BudgetChangeBody change)
        {
            return this.changes.AddChange(change);
        }
        
        [GraphQLMetadata("saveChange")]
        public Task<int> SaveChange(int id, BudgetChangeBody change)
        {
            return this.changes.SaveChange(id, change);
        }
        
        [GraphQLMetadata("deleteChange")]
        public Task<int> DeleteChange(int id)
        {
            return this.changes.DeleteChange(id);
        }
    }
}