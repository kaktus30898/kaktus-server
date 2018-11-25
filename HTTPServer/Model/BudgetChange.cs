using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using LinqToDB;
using LinqToDB.Mapping;

namespace HTTPServer.Model
{
    /**
     * Класс, описывающий тип данных BudgetChange из GraphQL и его привязку к MySQL
     */
    [Table(Name = "changes")]
    public class BudgetChange
    {
        [PrimaryKey, Identity, Column(Name = "id")]
        public int ID { get; set; }

        [Column(Name = "caption"), NotNull] public string Caption { get; set; }

        [Column(Name = "delta"), NotNull] public double Delta { get; set; }
    }
    
    /**
     * Класс, описывающий тип данных NewBudgetChange из GraphQL
     */
    public class NewBudgetChange
    {
        public string Caption { get; set; }

        public double Delta { get; set; }

        public BudgetChange ToChange()
        {
            return new BudgetChange()
            {
                ID = -1, 
                Caption = Caption,
                Delta = Delta,
            };
        }
    }

    /**
     * Класс, описывающий тип данных BudgetChangeManager из GraphQL
     */
    public class BudgetChangeManager
    {
        // Ссылка на базу данных, с которой будет происходить обмен
        private readonly Database db;

        // Свойство, которое предоставляет доступ к таблице изменений бюджета
        private ITable<BudgetChange> Changes => db.GetTable<BudgetChange>();

        public BudgetChangeManager(Database db)
        {
            this.db = db;
        }

        // Привязка метода к свойству list из GraphQL
        [GraphQLMetadata("list")]
        public Task<List<BudgetChange>> GetAll()
        {
            return (
                from c in Changes select c
            ).ToListAsync();
        }

        // Привязка метода к свойству rest из GraphQL
        [GraphQLMetadata("rest")]
        public Task<double> GetRest()
        {
            return (
                from c in Changes select c.Delta
            ).SumAsync();
        }

        // Метод записи нового изменения в базу
        public Task<int> AddChange(NewBudgetChange change)
        {
            return db.InsertAsync(change.ToChange());
        }
    }
}