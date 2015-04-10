using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseType.Common
{
    public interface IEntityState
    {
    }

    public enum StateEntity
    {
        Added,// Сущности еще нет в базе. Метод SaveChanges инициирует выполнение запроса INSERT.
Unchanged,// При вызове SaveChanges ничего не происходит. Данное состояние у сущности при извлечении её из базы данных.
Modified,// Значения свойств сущности были изменены, SaveChanges выполняет запрос UPDATE.
Deleted,// Сущность помечена к удалению, SaveChanges выполняет запрос DELETE.
Detached //Состояние сущности не отслеживается контекстом базы данных. 
    }
}
