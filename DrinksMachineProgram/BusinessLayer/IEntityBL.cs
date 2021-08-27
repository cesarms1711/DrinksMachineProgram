using System.Collections.Generic;

namespace DrinksMachineProgram.BusinessLayer
{

    public interface IEntityBL<TEntity, in TKey>
    {

        List<TEntity> List();

        TEntity Detail(TKey id);

        void Create(TEntity entity);

        void Edit(TEntity entity);

        void Delete(TKey id);

    }

}
