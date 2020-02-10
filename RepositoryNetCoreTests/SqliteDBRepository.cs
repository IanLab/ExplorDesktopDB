using DBCommon;
using System;
using System.Linq;
using AutoMapper;
using AutoMapper.Configuration;

namespace NetCorDBTests
{
    public class SqliteDBRepository : IRepository
    {
        private readonly static IMapper _mapper = CreateMapper();

        private static IMapper CreateMapper()
        {
            var cfge = new MapperConfigurationExpression();
            cfge.CreateMap<ICommandAble, Entity1>().Include<Entity1, Entity1>();
            cfge.CreateMap<Entity1, Entity1>();
            var cfg = new MapperConfiguration(cfge);
            return new Mapper(cfg);
        }

        private readonly string _filePath;
        public SqliteDBRepository(string filePath)
        {
            _filePath = filePath;
        }

        public void Save(ICommandAble entity)
        {
            using var dbContext = new NetCoreSqliteDB.SqliteDBContext(_filePath);
            if (entity.BasedOnUpdatedDateTime == default(DateTime))
            {
                dbContext.Table1.Add(entity as Entity1);
            }
            else
            {
                var preEntity = dbContext.Table1.SingleOrDefault(e => e.Id == entity.Id);
                if(preEntity.UpdatedDateTime > entity.BasedOnUpdatedDateTime)
                {
                    throw new Exception();
                }
                else
                {
                    _mapper.Map(entity, preEntity);
                }                    
            }
            dbContext.SaveChanges();
        }
    }
}
